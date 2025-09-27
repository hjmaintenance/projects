/*
* 작성자명 : 김지수
* 작성일자 : 25-02-27
* 최종수정 : 25-02-27
* 화면명 : 계측기별 Peak값 조회
* 프로시저명 : : P_HMI_DAY_USEQTY_PEAK, P_HMI_TIME_USEQTY_PEAK, P_HMI_MIN_UEEQTY_PEAK, P_HMI_PEAK_VALUE_RST
*/
using DevExpress.Blazor;
using DevExpress.Blazor.Popup.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MIT.DataUtil.Common;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.ServiceModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace MIT.UI.UMS;

public class UM3130QA1_Base : CommonUIComponentBase {

  public DateTimeOffset StartOffsetDt { get; set; } = DateTimeOffset.Now.AddDays(-1);
  public DateTimeOffset EndOffsetDt { get; set; } = DateTimeOffset.Now;

  protected CommCode GAUGE_TYPE_Code { get; set; } // = new CommCode() { Name="PW", Value="전기" };

  public DateTime StartValue { get; set; } = DateTime.Now;
  public DateTime EndValue { get; set; } = DateTime.Now.AddMonths(1);

  public DateTime StartValueC { get; set; } = DateTime.ParseExact(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
  public DateTime EndValueC { get; set; } = DateTime.Now;

  public DateTime StartValueF { get; set; } = DateTime.ParseExact(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
  public DateTime EndValueF { get; set; } = DateTime.Now;

  public double StartValueY { get; set; } = -10;
  public double EndValueY { get; set; }  = 33000;

  public double StartValueY2 { get; set; } = -10;
  public double EndValueY2 { get; set; } = 33000;




  //public bool IsVerticalSizeAuto { get; set; } = true;


  public double MiddleValueY2 { get {
      return EndValueY2 / 2;
    }
 }




  public double _startValueX =33000;
  public double StartValueX {
    get {
      return _startValueX;
    }
    set {
      _startValueX = value;
      StartValueY = EndValueY2 - _startValueX;
    }
  }




  public bool GridVisible = false;
  public bool PeakWindowVisible = false;

  protected CommonGrid? Grd1 { get; set; }


  //string LabelFormat = ",##0";
  public string LabelFormatDate = "yy-MM-dd HH:mm";



  public List<DPoint> gardData { get; set; } = new List<DPoint>();



  protected async Task OnValueChanged(RangeSelectorValueChangedEventArgs info) {

    if (IsChartDataSelectorLoad ||  info.CurrentRange == null || info.CurrentRange[0] == null || info.CurrentRange[1] == null) {
      return;
    }
  ;

    StartValue = (DateTime)info.CurrentRange[0];
    EndValue = (DateTime)info.CurrentRange[1];

    await MainChartData();



  }



  protected async Task OnValueChanged_V(RangeSelectorValueChangedEventArgs info) {



    if (info.CurrentRange == null || info.CurrentRange[0] == null || info.CurrentRange[1] == null) {
      return; 
    }  ;

    StartValueY = (double)info.CurrentRange[0];
    EndValueY = (double)info.CurrentRange[1];

    StateHasChanged();

  }



  public List<KeyValuePair<string, string>> FdrData { get; set; } = new List<KeyValuePair<string, string>>();

  private DotNetObjectReference<UM3130QA1_Base>? objRef;


  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {

     objRef = DotNetObjectReference.Create(this);
     await  JSRuntime.InvokeVoidAsync("registerBlazorCallback", objRef);


      if (!await IsAuthenticatedCheck()) return;

      await InitControls();

      StateHasChanged();

    }
  }

  [JSInvokable("OnChartZoomed")]
  public void OnChartZoomed(string start, string end) {


    TargetInfo ti = targetInfos.FirstOrDefault();
    if( ti != null) {
      string _zoomStart = ti.Labels[int.Parse(start)];
      string _zoomEnd = ti.Labels[int.Parse(end)];


      if(start == "0") {
        
        StartValueC = StartValueF;
        EndValueC = EndValueF;
      }
      else {
        StartValueC = DateTime.ParseExact(_zoomStart, "yyyy-MM-dd HH:mm:ss", null);
        EndValueC = DateTime.ParseExact(_zoomEnd, "yyyy-MM-dd HH:mm:ss", null);
      }

      StateHasChanged();

    }


  }




  protected async Task GetTargetInfo() {

    foreach (var mcs in targetInfos) {
      if (mcs.McType2 == "QTY") { // 증기의 유량만
        mcs.Peak = "loading";
        mcs.Peak2 = "loading";
      }
    }


    StateHasChanged();

    /*
    foreach (var mcs in targetInfos) {

      if (mcs.McType2 == "QTY") { // 증기의 유량만

        var rawData = await QueryService.ExecuteDatatableAsync("sp_get_peak", new Dictionary<string, object?>()        {
                  {"mc_id", mcs.McId },
                  {"startdt", StartOffsetDt.ToString("yyyy-MM-dd HH:mm:ss")  }, //  SYYYYMM.EditValue+" 00:00:00"
                  //{"enddt", EYYYYMM.EditValue+" 00:00:00" },
                  {"enddt", EndOffsetDt.ToString("yyyy-MM-dd HH:mm:ss") },
              });


        mcs.Peak = Math.Round( double.Parse("0"+ rawData.Rows[0]["peak"].ToString()), 1).ToString();

    StateHasChanged();


        var rawData2 = await QueryService.ExecuteDatatableAsync("sp_get_peak", new Dictionary<string, object?>()        {
                  {"mc_id", mcs.McId },
                  {"startdt", StartValue.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"enddt",  EndValue.ToString("yyyy-MM-dd HH:mm:ss") },
              });


        mcs.Peak2 = Math.Round(double.Parse("0" + rawData2.Rows[0]["peak"].ToString()), 1).ToString();
      }
    }
    
    StateHasChanged();
    */


    var tasks = new List<Task>();

    foreach (var mcs in targetInfos) {
      if (mcs.McType2 == "QTY") {
        tasks.Add(Task.Run(async () => {
          // 첫 번째 쿼리
          var rawData = await QueryService.ExecuteDatatableAsync("sp_get_peak", new Dictionary<string, object?> {
                {"mc_id", mcs.McId },
                {"startdt", StartOffsetDt.ToString("yyyy-MM-dd HH:mm:ss") },
                {"enddt", EndOffsetDt.ToString("yyyy-MM-dd HH:mm:ss") },
            });

          mcs.Peak = Math.Round(double.Parse("0" + rawData.Rows[0]["peak"].ToString()), 1).ToString();

          // UI 스레드에서 StateHasChanged 호출
          await InvokeAsync(StateHasChanged);




          // 두 번째 쿼리
          var rawData2 = await QueryService.ExecuteDatatableAsync("sp_get_peak", new Dictionary<string, object?> {
                {"mc_id", mcs.McId },
                {"startdt", StartValue.ToString("yyyy-MM-dd HH:mm:ss") },
                {"enddt", EndValue.ToString("yyyy-MM-dd HH:mm:ss") },
            });

          mcs.Peak2 = Math.Round(double.Parse("0" + rawData2.Rows[0]["peak"].ToString()), 1).ToString();

          // UI 스레드에서 StateHasChanged 호출
          await InvokeAsync(StateHasChanged);
        }));
      }
    }

    await Task.WhenAll(tasks);


  }





  private async Task InitControls() {

    StartValueC = DateTime.ParseExact(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "yyyy-MM-dd", null); // DateTime.Now.AddDays(-3);
    EndValueC = DateTime.Now;// DateTime.ParseExact("2025-03-15", "yyyy-MM-dd", null); // DateTime.Now.AddDays(-2);

    List<DPoint> dTemp = new List<DPoint>();
    for (DateTime i = StartValueF; i <= EndValueF; i = i.AddMinutes(1)) {
      dTemp.Add(new DPoint() { Xtime = i, Qty1= 0});
    }
    gardData = dTemp;
    StartValue = DateTime.ParseExact(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "yyyy-MM-dd", null); // DateTime.Now.AddDays(-3);
    EndValue = DateTime.Now;// DateTime.ParseExact("2025-03-15 23:59:59", "yyyy-MM-dd HH:mm:ss", null); // DateTime.Now.AddDays(-2);

    StartOffsetDt = StartValue;
    EndOffsetDt = EndValue;

    StartValueF = StartValue;
    EndValueF = EndValue;//.AddDays(1).AddSeconds(-1);

    StartValueC = StartValueF;
    EndValueC = EndValueF;
    LoadMcIds("PW");
  }


  protected async Task LoadMcIds(string mctype) {

 
    targetInfos.Clear();

    var dt = await GetCommonCode("MC", USER_ID, "", "", mctype);


    if (dt != null) {
      foreach (DataRow dr in dt.Rows) {

        string mc_id = dr["MC_ID"] + "";
        string mc_name = dr["DESCRIPT"] + "";
        string mc_desc = dr["DESCRIPT2"] + "";
        string gs = dr["GAUGE_SECTION"] + "";
        string crmin = dr["chart_range_min"] + "";
        string crmax = dr["chart_range_max"] + "";


        if (gs == "ST") {

          AddTarger(USER_ID, USER_NAME, mc_id, mc_name, gs, gs, "QTY", "유량", mc_desc, crmin, crmax);

          crmin = dr["chart_range_min_press"] + "";
          crmax = dr["chart_range_max_press"] + "";

          AddTarger(USER_ID, USER_NAME, mc_id, mc_name, gs, gs, "PRESS", "압력", mc_desc, crmin, crmax);

          crmin = dr["chart_range_min_temper"] + "";
          crmax = dr["chart_range_max_temper"] + "";

          AddTarger(USER_ID, USER_NAME, mc_id, mc_name, gs, gs, "TEMPER", "온도", mc_desc, crmin, crmax);

        }
        else {
          AddTarger(USER_ID, USER_NAME, mc_id, mc_name, gs, gs, "", "", mc_desc, crmin, crmax);
        }

      }
    }

    //GetTargetInfo();
    DisplayReLoad();
    StateHasChanged();

  }


  public async Task DisplayReLoad() {


    //await JS.InvokeVoidAsync("chartSetData", targetInfos);
    await JSRuntime.InvokeVoidAsync("chartDeleyLoad", targetInfos);

    //chartDeleyLoad

  }



  public async void COM_Change_G(Razor.Pages.CommCode args) {
    if(string.IsNullOrEmpty(USER_ID)) {
      return;
    }
    await LoadMcIds(args.Name);


    //foreach ( TargetInfo ti in targetInfos) {
    //  if( ti.McType == args.Name) {
    //    ti.IsLineView = true;
    //  }
    //  else {
    //    ti.IsLineView = false;
    //  }
    //}

    //StateHasChanged();
  }






  protected override async Task Btn_Common_Search_Click() {
    await btn_Search();
  }


  

  // 조회 버튼 클릭
  protected async Task btn_Search() {

    try {
      ShowLoadingPanel();

      StartValue = StartOffsetDt.ToDateTime() ;// DateTime.ParseExact(SYYYYMM.EditValue, "yyyy-MM-dd", null); // DateTime.Now.AddDays(-3);
      EndValue = EndOffsetDt.ToDateTime();// DateTime.ParseExact(EYYYYMM.EditValue, "yyyy-MM-dd", null); // DateTime.Now.AddDays(-2);

      //StartValue = StartValueC;
      //EndValue = EndValueC;

      StartValueF = StartValue;
      EndValueF = EndValue;//.AddDays(1).AddSeconds(-1);

      StartValueC = StartValueF;
      EndValueC = EndValueF;

      await ChartDataSelector();
    }
    catch (Exception ee) {
    }
    finally {
      CloseLoadingPanel();
    }

  }



  private readonly Random random = new Random();

  public string GetRandomColor() {
    const string letters = "0123456789ABCDEF";
    char[] colorChars = new char[6];

    for (int i = 0; i < 6; i++) {
      int index = random.Next(letters.Length);
      colorChars[i] = letters[index];
    }

    return "#" + new string(colorChars);
  }



  void AddTarger(string comid, string comname, string mcid, string mcname, string mctype, string mctypeName
    ,  string mcType2, string mcType2Name, string descript2
    , string crmin, string crmax
    ) {

    foreach(var mcs in targetInfos) {
      if (mcs.McId == mcid &&  mcs.McType2 == mcType2) {
        return;
      }
    }

    TargetInfo ti = new TargetInfo();
    ti.ComId = comid;
    ti.ComName = comname;
    ti.McId = mcid;
    ti.McName = mcname;
    ti.McType = mctype;
    ti.McType2 = mcType2;
    ti.McTypeName = mctypeName;
    ti.Descript2 = descript2;
    ti.McType2Name = mcType2Name;
    // ti.RQtyColor = ti.bg_color[targetInfos.Count % 7];

    ti.RQtyColor = GetRandomColor();
    ti.RQtyColor2 = ColorTranslator.FromHtml(ti.RQtyColor);

    ti.MoveMin = Convert.ToDecimal( crmin);
    ti.MoveMax = Convert.ToDecimal(crmax);

    /*
    switch (mctype) {
      case "ST":
        switch(mcType2) {
          case "QTY":
            ti.MoveMin = qmin;
            ti.MoveMax = qmax;
            break;
          case "PRESS":
            ti.MoveMin = pmin;
            ti.MoveMax = pmax;
            break;
          case "TEMPER":
            ti.MoveMin = tmin;
            ti.MoveMax = tmax;
            break;
          default:
            ti.MoveMin = qmin;
            ti.MoveMax = qmax;
            break;
        }
        break;
      case "PW":
        ti.MoveMin = pwmin;
        ti.MoveMin = pwmax;
        break;
      case "WA":
        ti.MoveMin = wamin;
        ti.MoveMin = wamax;
        break;
      default:
        ti.MoveMin = pwmin;
        ti.MoveMin = pwmax;
        break;
    }
    */

    targetInfos.Add(ti);

  }








  


  async Task MainChartData() {


    GetTargetInfo();
    await GetData(StartValue, EndValue);
    _chart.RefreshData();
    StateHasChanged();
  }

  public bool IsChartDataSelectorLoad = false;
  protected async Task ChartDataSelector() {
    IsChartDataSelectorLoad = true;

    StartValue = StartOffsetDt.ToDateTime();
    EndValue = EndOffsetDt.ToDateTime();
    StartValueC = StartValue; 
    EndValueC = EndValue;

    GetTargetInfo();
    await GetData(StartValue, EndValue);

    if (_chart != null) {
      _chart.RefreshData();
    }
    StateHasChanged();

    IsChartDataSelectorLoad = false;
  }





  public DxChart _chart { get; set; }




  protected async Task btn_PeakList( string mc_id ) {
    PeakWindowVisible = true;
    //DateTime sdt = DateTime.ParseExact(SYYYYMM.EditValue, "yyyy-MM-dd", null);
    //DateTime edt = DateTime.ParseExact(EYYYYMM.EditValue, "yyyy-MM-dd", null).AddDays(1);

    var rawData = await QueryService.ExecuteDatatableAsync("sp_get_peaklist", new Dictionary<string, object?>()        {
                  //{"startdt", sdt.ToString("yyyy-MM-dd HH:mm:ss") },
                  //{"enddt", edt.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"startdt", StartOffsetDt.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"enddt", EndOffsetDt.ToString("yyyy-MM-dd HH:mm:ss")  },
                  {"MC_ID", mc_id },
              });

    Grd1.DataSource = rawData;
    StateHasChanged();
  }



  async Task GetData( DateTime sdt, DateTime edt) {



    // 각 targetInfo에 대해 비동기 데이터 로딩 작업 생성
    var loadTasks = targetInfos.Select(async mcs => {
      mcs.DPoints.Clear();

      var rawData = await QueryService.ExecuteDatatableAsync("P_HMI_USEQTY_PEAK_FROM_TO_NEW3", new Dictionary<string, object?>() {
          {"SDATETM", sdt.ToString("yyyy-MM-dd HH:mm:ss") },
          {"EDATETM", edt.ToString("yyyy-MM-dd HH:mm:ss") },
          {"COM_ID", mcs.ComId },
          {"MC_ID", mcs.McId.ToStringTrim() },
      });

      foreach (DataRow d in rawData.Rows) {
        DateTime dttmp = DateTime.ParseExact(d["GAUGE_DATETIME"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
        double qty = double.Parse("0" + d["AVG_R_USEQTY"].ToString());
        double press = double.Parse("0" + d["AV_PRESS"].ToString());
        double temper = double.Parse("0" + d["AV_TEMPER"].ToString());

        DPoint dp = new DPoint(dttmp, qty, press, temper, ((mcs.McType == "ST") ? ((mcs.McType2 == "QTY") ? 0 : ((mcs.McType2 == "PRESS") ? 1 : 2)) : 0));
        mcs.DPoints.Add(dp);
      }


      mcs.Min = mcs.DPoints.Where(dp => dp.ViewQty.HasValue).Min(dp => dp.ViewQty).GetValueOrDefault();
      mcs.Max = mcs.DPoints.Where(dp => dp.ViewQty.HasValue).Max(dp => dp.ViewQty).GetValueOrDefault();
      mcs.Avg = Math.Round(mcs.DPoints.Where(dp => dp.ViewQty.HasValue).Average(dp => dp.ViewQty).GetValueOrDefault(), 1);
    }).ToList();

    // 모든 비동기 작업이 끝날 때까지 대기
    await Task.WhenAll(loadTasks);

    // 날짜 수집 및 정렬
    var allDates = targetInfos
             .SelectMany(a => a.DPoints)
             .Where(d => d.Xtime.HasValue)
             .Select(d => d.Xtime.Value)
             .Distinct()
             .OrderBy(d => d)
             .ToList();

    // 날짜
    string[] result = allDates.Select(d => d.ToString("yyyy-MM-dd HH:mm:ss")).ToArray();

    // AAA마다 qty 배열 생성

    foreach (var aaa in targetInfos) {
   
      var map = aaa.DPoints
                .Where(d => d.Xtime.HasValue)
                .ToDictionary(d => d.Xtime.Value, d => d.ViewQty);

      double?[] qtyArr = allDates
               .Select(date => map.ContainsKey(date) ? map[date] : null)
               .ToArray();

      aaa.Labels = result;
      aaa.Qtys = qtyArr;
    }

    await JSRuntime.InvokeVoidAsync("chartSetData", targetInfos);

  }


  public List<TargetInfo> targetInfos = new List<TargetInfo>();

  //public bool IsCheckedYgagde { get; set; } = false; // 범례

  //public bool IsCheckedMonoton { get; set; } = true; // 부드럽게




}

