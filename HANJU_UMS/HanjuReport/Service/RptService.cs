
using DevExpress.Pdf.Xmp;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.Native;
using MIT.ServiceModel;
using System.Data;
using System.Linq;

namespace HanjuReport.Service {
  public class RptService {

        public string FormData { get; set; }

        public Task<RptInfo> GetRptAsync(string format) {

      DataTable dt = new DataTable("sample report");
      for (int i = 0; i < 20; i++) {
        string colstring = "COL" + i;
        dt.Columns.Add(colstring);
      }


      for (int i = 0; i < 100; i++) {
        DataRow dr = dt.NewRow();
        foreach (DataColumn col in dt.Columns) {
          dr[col.ColumnName] = "cont " + (i + Random.Shared.Next(1, 5000));
        }
        dt.Rows.Add(dr);
      }

      RptInfo rpt = new RptInfo();
      rpt.Title = "NULL DATA";
      rpt.SubTitle = "sub title";
      rpt.Date1 = DateTime.Now.ToString("yyyy-MM-dd HH:MI:ss");
      rpt.Data = dt;
      rpt.UName = "김아무개";
      rpt.ExportType = format;
      rpt.FileName = "sample"+format;

      Dictionary<string, string> dic = new Dictionary<string, string>();
      Dictionary<string, RptCol> dic2 = new Dictionary<string, RptCol>();
      for (int i = 0; i < dt.Columns.Count;++i) {
        DataColumn col = dt.Columns[i];
        string fieldName = col.ColumnName;
        string id;
        string kind;
        int rowSpan;
        int colSpan;
        int depth;
        int order;
        RptCol rc0 = null;
        RptCol rc1 = null;
        if ((i+1) % 10 == 0)
        {
          id = $"B_{3+(i + 1) / 10}";
          kind = "Band";
          rowSpan = 1;
          colSpan = 10;
          depth = 0;
          order = 23 + ((i + 1) / 10);
          rc0 = new RptCol(id, $"COL{order}", kind, null, order, depth, colSpan, rowSpan);
          dic2.Add(id, rc0);
        }
        

        if ((i+1) % 5 == 0)
        {
            id = $"B_{(i+1)/5 - 1}";
            kind = "Band";
            rowSpan = 1;
            colSpan = 5;
            depth = 1;
            order = 19 + ((i + 1) / 5);
            rc1 = new RptCol(id, $"COL{order}", kind, $"B_{4+(i/10)}", order, depth, colSpan, rowSpan);
            dic2.Add(id, rc1);
        }
        
        RptCol rptCol = new RptCol(col.ColumnName, col.ColumnName, "Data", $"B_{i/5}", i, 2, 1, 1); 
        dic.Add(fieldName, fieldName);
        dic2.Add(fieldName, rptCol);
      }
      rpt.Columns = dic;
      rpt.Columns2 = dic2;
      rpt.Landscape = "Y";
        //rpt.Landscape = "N";

            return Task.FromResult(rpt);
    }
  }


}
