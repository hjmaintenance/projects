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

public class UM3130QA1_FBase : CommonUIComponentBase {

  //protected CommonDateEdit? SYYYYMM { get; set; }
  //protected CommonDateEdit? EYYYYMM { get; set; }

  public DateTimeOffset StartOffsetDt { get; set; } = DateTimeOffset.Now;
  public DateTimeOffset EndOffsetDt { get; set; } = DateTimeOffset.Now;

  protected CommCode GAUGE_TYPE_Code { get; set; }

  public DateTime StartValue { get; set; } = DateTime.Now;
  public DateTime EndValue { get; set; } = DateTime.Now.AddMonths(1);

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
  //public string LabelFormatDate = "yy-MM-dd HH:mm";

  public bool IsChartDataSelectorLoad = false;
 
  public List<KeyValuePair<string, string>> FdrData { get; set; } = new List<KeyValuePair<string, string>>();

  protected MitCombo2? ComObj { get; set; }

  protected MitCombo2? UtilObj { get; set; }



  private DotNetObjectReference<UM3130QA1_FBase>? objRef;




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


  protected int _preStartZoom = -1;
  protected int _preEndZoom = -1;

  [JSInvokable("OnChartZoomed")]
  public void OnChartZoomed(string start, string end) {

    int curStartZoom = Convert.ToInt32(start);
    int curEndZoom = Convert.ToInt32(end);

    TargetInfo ti = targetInfos.FirstOrDefault();
    if( ti != null) {


      //if(_preStartZoom >= curStartZoom ) {  // 축소

      //}
      //else { // 확대
        string _zoomStart = ti.Labels[curStartZoom];
        string _zoomEnd = ti.Labels[curEndZoom];
        //Console.WriteLine($"!!!!!!! :  {_preStartZoom} {curStartZoom} : {_preEndZoom} {curEndZoom} , {_zoomStart} : {_zoomEnd} ");
        // 여기에서 정의한 범위의 데이터를 가져오고 가져온 범위의 데이터를 기존의 데이터범위에서 교체한다.

        DateTime StartValueC = DateTime.ParseExact(_zoomStart, "yyyy-MM-dd HH:mm:ss", null);
        DateTime EndValueC = DateTime.ParseExact(_zoomEnd, "yyyy-MM-dd HH:mm:ss", null);

        GetData(StartValueC, EndValueC, true, curStartZoom, curEndZoom);


      //}

    }
    _preStartZoom = curStartZoom;
    _preEndZoom = curEndZoom;

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

    StartValue = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", null); 
    EndValue = DateTime.Now;

    StartOffsetDt = StartValue;
    EndOffsetDt = EndValue;


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


  protected async Task ChartDataSelector() {
    IsChartDataSelectorLoad = true;

    StartValue = StartOffsetDt.ToDateTime();
    EndValue = EndOffsetDt.ToDateTime();


    await GetData(StartValue, EndValue, false, 0, 0);

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

  async Task GetData( DateTime sdt, DateTime edt, bool idChange, int s, int e) {

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

      //Console.WriteLine($"dps.DPoints : {mcs.DPoints.Count}");

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

      if (idChange) { // 범위데이터 교체



        // 배열을 List로 변환
        List<string> labelList = aaa.Labels.ToList();
        List<double?> qtyList = aaa.Qtys.ToList();


        // 156번째부터 249번째까지 삭제 (249 - 156 + 1 = 94개 삭제)
        labelList.RemoveRange(s, (e-s));
        qtyList.RemoveRange(s, (e - s));

        // 156번째 위치에 새 데이터 삽입
        labelList.InsertRange(s, result);
        qtyList.InsertRange(s, qtyArr);

        // 다시 배열로 변환 (원하면)
        aaa.Labels = labelList.ToArray();
        aaa.Qtys = qtyList.ToArray();



        await JSRuntime.InvokeVoidAsync("chartAddSetData", targetInfos);
      }
      else { // 통체로 변경
        aaa.Labels = result;
        aaa.Qtys = qtyArr;
        _preStartZoom = 0;
        _preEndZoom = result.Length - 1;
        await JSRuntime.InvokeVoidAsync("chartSetData", targetInfos);
      }
    }

    //await JSRuntime.InvokeVoidAsync("chartSetData", targetInfos);

  }


  public List<TargetInfo> targetInfos = new List<TargetInfo>();

}

