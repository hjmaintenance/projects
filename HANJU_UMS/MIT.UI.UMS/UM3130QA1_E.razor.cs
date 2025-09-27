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
using System.Timers;

namespace MIT.UI.UMS;

public class UM3130QA1_EBase : CommonUIComponentBase {

  //protected CommonDateEdit? SYYYYMM { get; set; }
  //protected CommonDateEdit? EYYYYMM { get; set; }

  public DateTimeOffset StartOffsetDt { get; set; } = DateTimeOffset.Now;
  public DateTimeOffset EndOffsetDt { get; set; } = DateTimeOffset.Now;

  protected CommCode GAUGE_TYPE_Code { get; set; }

  public DateTime StartValue { get; set; } = DateTime.Now;
  public DateTime EndValue { get; set; } = DateTime.Now.AddMonths(1);

  public DateTime StartValueC { get; set; } = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
  public DateTime EndValueC { get; set; } = DateTime.Now;

  public DateTime StartValueF { get; set; } = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null);
  public DateTime EndValueF { get; set; } = DateTime.Now;

  public double StartValueY { get; set; } = -10;
  public double EndValueY { get; set; }  = 33000;

  public double StartValueY2 { get; set; } = -10;
  public double EndValueY2 { get; set; } = 33000;



  public bool _isRealTime = false;
  public bool IsRealTime { get { return _isRealTime; } 
    set {
      _isRealTime = value;
      if( value) {

        ToastService.ShowToast(new ToastOptions {
          ProviderName = "Positioning",
          Title = "알림",
          Text = "실시간 처리시 조회시작일자를 최소화 하여 가져옵니다."
        });

      }
      else {
        ToastService.ShowToast(new ToastOptions {
          ProviderName = "Positioning",
          Title = "알림",
          Text = "기준조회 일자의 데이터로 새로 호출합니다."
        });
        btn_Search();

      }
    } 
  }


  //public bool IsVerticalSizeAuto { get; set; } = true;


  public double MiddleValueY2 { get {
      return EndValueY2 / 2;
    }
 }


  public bool IsCheckedQty { get; set; }
  public bool IsCheckedPress { get; set; }
  public bool IsCheckedTemp { get; set; }


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



  public string IsSTChoiceStr {
    get {
      if (IsSTChoice) {
        return "block";
      }
      else {
        return "none";
      }
    }
  }


  public bool GridVisible = true;
  public bool PeakWindowVisible = false;

  protected CommonGrid? Grd1 { get; set; }


  //string LabelFormat = ",##0";
  public string LabelFormatDate = "yy-MM-dd HH:mm";



  public List<DPoint> gardData { get; set; } = new List<DPoint>();



  public bool IsChartDataSelectorLoad = false;
  protected async Task OnValueChanged(RangeSelectorValueChangedEventArgs info) {



    if (IsChartDataSelectorLoad || info.CurrentRange == null || info.CurrentRange[0] == null || info.CurrentRange[1] == null) {
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



  protected MitCombo2? ComObj { get; set; }

  protected MitCombo2? UtilObj { get; set; }



  private DotNetObjectReference<UM3130QA1_EBase>? objRef;




  //protected List<DataPoints> ChartData = new List<DataPoints>();
  //protected List<DataPoints> ChartDataMain = new List<DataPoints>();




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
    //dbo.fn_getPeak('PP_LP', '2025-03-13 00:00:00', '2025-03-15 00:00:00')

    foreach (var mcs in targetInfos) {

      if (mcs.McType2 == "QTY") { // 증기의 유량만
        mcs.Peak = "loading";
        mcs.Peak2 = "loading";
      }
    }


    StateHasChanged();


    foreach (var mcs in targetInfos) {

      if (mcs.McType2 == "QTY") { // 증기의 유량만

        var rawData = await QueryService.ExecuteDatatableAsync("sp_get_peak", new Dictionary<string, object?>()        {
                  {"mc_id", mcs.McId },
                  {"startdt", StartOffsetDt.ToString("yyyy-MM-dd HH:mm:ss")  }, //  SYYYYMM.EditValue+" 00:00:00"
                  //{"enddt", EYYYYMM.EditValue+" 00:00:00" },
                  {"enddt", EndOffsetDt.ToString("yyyy-MM-dd HH:mm:ss") },
              });


        mcs.Peak = Math.Round( double.Parse("0"+ rawData.Rows[0]["peak"].ToString()), 1).ToString();
      }
    }

    StateHasChanged();

    foreach (var mcs in targetInfos) {

      if (mcs.McType2 == "QTY") {

        var rawData = await QueryService.ExecuteDatatableAsync("sp_get_peak", new Dictionary<string, object?>()        {
                  {"mc_id", mcs.McId },
                  {"startdt", StartValue.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"enddt",  EndValue.ToString("yyyy-MM-dd HH:mm:ss") },
              });


        mcs.Peak2 = Math.Round(double.Parse("0" + rawData.Rows[0]["peak"].ToString()), 1).ToString();
      }
    }
    
    StateHasChanged();

}


  private async Task InitControls() {

    StartValueC = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null); 
    EndValueC = DateTime.Now;

    List<DPoint> dTemp = new List<DPoint>();
    for (DateTime i = StartValueF; i <= EndValueF; i = i.AddMinutes(1)) {
      dTemp.Add(new DPoint() { Xtime = i, Qty1= 0});
    }
    gardData = dTemp;


    StartValue = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null); 
    EndValue = DateTime.Now;

    StartOffsetDt = StartValue;
    EndOffsetDt = EndValue;

    StartValueF = StartValue;
    EndValueF = EndValue;

    StartValueC = StartValueF;
    EndValueC = EndValueF;

  }


  protected override async Task Btn_Common_Search_Click() {
    await btn_Search();
  }

  // 조회 버튼 클릭
  protected async Task btn_Search() {
    try {
      ShowLoadingPanel();

      StartValue = StartOffsetDt.ToDateTime() ;
      EndValue = EndOffsetDt.ToDateTime();

      StartValueF = StartValue;
      EndValueF = EndValue;

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

  public CommCode SelComp { get; set; }

  public CommCode SelUtil { get; set; }

  // st 선택일 경우만 활성
  public bool IsSTChoice { get; set; } = true;

  public async void COM_Change_G(Razor.Pages.CommCode args) {
    ComObj.Etc0 = args.Name;
    if( args.Name == "ST") {
      IsSTChoice = true;
    }
    else {
      IsSTChoice = false;
    }
    StateHasChanged();
  }

  public async void COM_Change(Razor.Pages.CommCode args) {

    UtilObj.Etc0 = args.Name;
    UtilObj.Etc1 = GAUGE_TYPE_Code.Name;

    StateHasChanged();

  }

  public async void COM_ADD_U() {

    GridVisible = false;
    if(SelUtil == null) {
      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "알림",
        Text = "계측기 선택 필수 항목 입니다."
      });
      return;
    }
    string gs = SelUtil.Others["GAUGE_SECTION"];
    string crmin = SelUtil.Others["chart_range_min"];
    string crmax = SelUtil.Others["chart_range_max"];

    if (gs == "ST") {
      if (IsCheckedQty) {
        AddTarger(SelComp.Name, SelComp.Value, SelUtil.Name, SelUtil.Value, gs, gs, "QTY", "유량", SelUtil.Others["DESCRIPT2"], crmin, crmax);
      }
      if (IsCheckedPress) {
        crmin = SelUtil.Others["chart_range_min_press"];
        crmax = SelUtil.Others["chart_range_max_press"];
        AddTarger(SelComp.Name, SelComp.Value, SelUtil.Name, SelUtil.Value, gs, gs, "PRESS", "압력", SelUtil.Others["DESCRIPT2"], crmin, crmax);
      }
      if (IsCheckedTemp) {
        crmin = SelUtil.Others["chart_range_min_temper"];
        crmax = SelUtil.Others["chart_range_max_temper"];
        AddTarger(SelComp.Name, SelComp.Value, SelUtil.Name, SelUtil.Value, gs, gs, "TEMPER", "온도", SelUtil.Others["DESCRIPT2"], crmin, crmax);
      }
    }
    else {
      AddTarger(SelComp.Name, SelComp.Value, SelUtil.Name, SelUtil.Value, gs, gs, "", "", SelUtil.Others["DESCRIPT2"], crmin, crmax);
    }
    GetTargetInfo();

    IsCheckedQty = false;
    IsCheckedPress = false;
    IsCheckedTemp = false;
    StateHasChanged();

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

    ti.RQtyColor = GetRandomColor();
    ti.RQtyColor2 = ColorTranslator.FromHtml(ti.RQtyColor);

    ti.MoveMin = Convert.ToDecimal( crmin);
    ti.MoveMax = Convert.ToDecimal(crmax);

    targetInfos.Add(ti);

  }

  async Task MainChartData() {

    GetTargetInfo();
    await GetData(StartValue, EndValue);

    _chart.RefreshData();
    StateHasChanged();
  }

  protected async Task ChartDataSelector() {
    IsChartDataSelectorLoad = true;

    StartValue = StartOffsetDt.ToDateTime();
    EndValue = EndOffsetDt.ToDateTime();
    StartValueC = StartValue; 
    EndValueC = EndValue;

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

    var rawData = await QueryService.ExecuteDatatableAsync("sp_get_peaklist", new Dictionary<string, object?>()        {
                  {"startdt", StartOffsetDt.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"enddt", EndOffsetDt.ToString("yyyy-MM-dd HH:mm:ss")  },
                  {"MC_ID", mc_id },
              });

    Grd1.DataSource = rawData;
    StateHasChanged();
  }

  public async Task DisplayReLoad() {
    await JSRuntime.InvokeVoidAsync("chartDeleyLoad", targetInfos);
  }

  async Task GetData( DateTime sdt, DateTime edt) {

    // 각 targetInfo에 대해 비동기 데이터 로딩 작업 생성
    var loadTasks = targetInfos.Select(async mcs => {
      mcs.DPoints.Clear();

      var rawData = await QueryService.ExecuteDatatableAsync("P_HMI_USEQTY_PEAK_FROM_TO_NEW3", new Dictionary<string, object?>()
      {
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

}


public class TargetInfo {

  public string Id {
    get {
      return $"{McId}_{McType}_{McType2}";
    }
  }

  public string ComId { get; set; }
  public string ComName { get; set; }
  public string McId { get; set; }
  public string McName { get; set; }
  public string McType { get; set; }
  public string McTypeName { get; set; }
  public string McType2 { get; set; }
  public string McType2Name { get; set; }
  public string McType2Name2 {
    get {
      if (McType2 == "QTY") {
        return "유량";
      }
      else if (McType2 == "PRESS") {
        return "압력";
      }
      else if (McType2 == "TEMPER") {
        return "온도";
      }
      return "";
    }
  }

  public string RQtyColor { get; set; }
  public Color RQtyColor2 { get; set; }
  public string RQty { get; set; } = "0";

  public string PressColor { get; set; }
  public string Press { get; set; } = "0";

  public string TempColor { get; set; }
  public string Temp { get; set; } = "0";

  public string Title {
    get {
      return $"{McId} : {McType} {McType2Name}";
    }
  }

  public double Min { get; set; } = 0;
  public double Max { get; set; } = 0;
  public double Avg { get; set; } = 0;
  public string Peak { get; set; } = "-";
  public string Peak2 { get; set; } = "-";

  public string RName { get; set; }
  public string Descript2 { get; set; }


  public double StartValueY { get; set; }
  public double EndValueY { get; set; }

  public string[] Labels { get; set; }

  public double?[] Qtys { get; set; }
  public bool IsYView { get; set; } = false;
  public bool IsLineView { get; set; } = true;
  public bool IsXGrid { get; set; } = false;
  public bool IsYGrid { get; set; } = false;

  public decimal MoveMin { get; set; } = 0;
  public decimal MoveMax { get; set; } = 0;
  public decimal MoveGap { get; set; } = 10;
  public decimal MoveBar { get; set; } = 0;

  public List<DPoint> DPoints = new List<DPoint>();

  public override string ToString() {
    return $"{McId} : {McType} {McType2Name}";
  }

}