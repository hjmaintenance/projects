using System.Linq;
using System.Threading.Tasks;
using HanjuReport;
using DevExpress.XtraReports.UI;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Data;
using Newtonsoft.Json;
using HanjuReport.Service;
using MIT.ServiceModel;
using DevExpress.Drawing;
using DevExpress.XtraRichEdit.Import.Doc;
using DevExpress.XtraCharts;
using DevExpress.XtraPrinting;
using DevExpress.Export;

namespace HanjuReport.Helpers;
public class ExportMiddleware : IMiddleware {
  readonly RptService rptService;
  private readonly List<string> excludedUrls = new List<string> { "/documentviewer", "/documentviewer2", "/opviewer" };




  public ExportMiddleware(RptService _rptService) {
    rptService = _rptService;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next) {

    try {
      //string rpt = string.Empty;
      string reqPath = context.Request.Path.ToString();

      // 참조 헤더 로그 기록
      var referrer = context.Request.Headers["Referer"].ToString();


      string rpt = string.Empty;

      // 특정 URL에 대해서는 ExportMiddleware를 사용하지 않도록 설정
      if (excludedUrls.Contains(reqPath, StringComparer.OrdinalIgnoreCase)) {
        // 폼 데이터를 세션에 저장
        //if (context.Request.HasFormContentType) {

        StringValues tmp;
        //var rptstr = context.Request.Form.TryGetValue(rpt, out tmp);
        string rptData = "";
        try {
          var formData = context.Request.Form.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());
          rptData = formData["rpt"];
          context.Session.SetString("FormData", rptData);
        }
        catch {
        }
        //}
        rptService.FormData = rptData;

        //Console.WriteLine($"111reqPath{reqPath}");
        if (reqPath.IndexOf("documentviewer2") >= 0) {

                    //Console.WriteLine($"0000reqPath{reqPath}");
                    context.Response.Redirect("/docviewer2");
        }
        else if (reqPath.IndexOf("documentviewer") >= 0) {

                    //Console.WriteLine($"2222reqPath{reqPath}");
                    context.Response.Redirect("/docviewer");
        }
        else if (reqPath.IndexOf("opviewer") >= 0)
                {
                    //Console.WriteLine($"3333reqPath{reqPath}");
                    context.Response.Redirect("/operationstatusviewer");
        }

        return;
      }

      if (reqPath.StartsWith("/exportPdf")) { rpt = pdf; }
      else if (reqPath.StartsWith("/exportXls")) { rpt = xlsx; }
      else if (reqPath.StartsWith("/exportDoc")) { rpt = docx; }
      if (!string.IsNullOrEmpty(rpt)) {
        await ExportResult(rpt, GetOptionsFromQuery(context.Request.QueryString.ToString()), context);
        return;
      }

      await next(context);
    }
    catch (Exception ex) {
      // 예외 처리 로직 추가
      context.Response.StatusCode = 500;
      await context.Response.WriteAsync("An error occurred: " + ex.Message);
    }
  }
  private DataSourceLoadOptionsBase GetOptionsFromQuery(string query) {
    DataSourceLoadOptionsBase options = new DataSourceLoadOptionsBase();
    IDictionary<string, StringValues> myQUeryString = QueryHelpers.ParseQuery(query);
    DataSourceLoadOptionsParser.Parse(options, key => {
      if (myQUeryString.ContainsKey(key))
        return myQUeryString[key];
      return null;
    });
    return options;
  }
  private readonly string pdf = "pdf";
  private readonly string xlsx = "xlsx";
  private readonly string docx = "docx";
  private readonly string image = "png";
  private readonly string rpt = "rpt";

  private async Task ExportResult(string format, DataSourceLoadOptionsBase dataOptions, HttpContext context) {
    XtraReport report = new XtraReport();
    dataOptions.Skip = 0;
    dataOptions.Take = 0;
    //var loadedData = DataSourceLoader.Load(await rptService.GetRptAsync(), dataOptions);
    //var rpt = loadedData.data.Cast<RptInfo>();

    StringValues tmp;

    bool isExsit = false;
    try {
      var form = await context.Request.ReadFormAsync();
      isExsit = form.ContainsKey(rpt);
    }
    catch (Exception e) { }

    RptInfo ri;
    if (isExsit) {
      context.Request.Form.TryGetValue(rpt, out tmp);
      ri = JsonConvert.DeserializeObject<RptInfo>(tmp);
    }
    else {
      //var loadedData = DataSourceLoader.Load(await rptService.GetRptAsync(), dataOptions);
      ri = await rptService.GetRptAsync(format);
      //ri = ris[0];
    }

    //var tmp = context.Request.Form["rpt"];

    //var tmpstr = tmp.ToString();

    //RptInfo ri = JsonConvert.DeserializeObject<RptInfo>(tmp);



    format = ri.ExportType;
    //foreach (DataColumn dc in dt.Columns) {
    //  colfname += dc.ColumnName + ",";
    //}
    if (format == xlsx) {
      ReportHelper.CreateReportXlsx(report, ri);
    }
    else {
      ReportHelper.CreateReport(report, ri);
    }

    string fname = ri.FileName;
    if (string.IsNullOrEmpty(fname)) {
      fname = DateTime.Now.ToString("yyyymmdd") + "_" + DateTime.Now.ToString("HHmmss");  //fname = DateTime.Now.ToString("yyyymmdd") + "_" + DateTime.Now.Ticks.ToString();
    }


    report.CreateDocument();
    using (MemoryStream fs = new MemoryStream()) {
    if (format == pdf) report.ExportToPdf(fs);
    else if (format == xlsx) report.ExportToXlsx(fs);
    else if (format == docx) report.ExportToDocx(fs);
    else if (format == image) report.ExportToImage(fs, DXImageFormat.Png);

      context.Response.Clear();
      context.Response.Headers.Append("Content-Type", "application/" + format);
      context.Response.Headers.Append("Content-Transfer-Encoding", "binary");


      var path = fname + "." + format;
      path = String.Join(
          "/",
          path.Split("/").Select(s => System.Net.WebUtility.UrlEncode(s))
      );


      context.Response.Headers.Append("Content-Disposition", "attachment; filename=" + path);
      await context.Response.Body.WriteAsync(fs.ToArray(), 0, fs.ToArray().Length);
      await context.Response.CompleteAsync();
    }
  }

}