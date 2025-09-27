/*
* 작성자명 : 김지수
* 작성일자 : 25-02-27
* 최종수정 : 25-02-27
* 화면명 : 계측기별 Peak값 조회
* 프로시저명 : : P_HMI_DAY_USEQTY_PEAK, P_HMI_TIME_USEQTY_PEAK, P_HMI_MIN_UEEQTY_PEAK, P_HMI_PEAK_VALUE_RST
*/
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
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
using System.Data;
using System.Timers;

namespace MIT.UI.UMS;

public class UM3130QA1_E2Base : CommonUIComponentBase {

  protected CommonDateEdit? SYYYYMM { get; set; }
  protected CommonDateEdit? EYYYYMM { get; set; }


  public DateTime StartValue { get; set; } = DateTime.Now;
  public DateTime EndValue { get; set; } = DateTime.Now.AddMonths(1);

  public DateTime StartValueC { get; set; } = DateTime.ParseExact("2025-03-13", "yyyy-MM-dd", null);
  public DateTime EndValueC { get; set; } = DateTime.ParseExact("2025-03-15", "yyyy-MM-dd", null);

  public DateTime StartValueF { get; set; } = DateTime.ParseExact("2025-03-13", "yyyy-MM-dd", null);
  public DateTime EndValueF { get; set; } = DateTime.ParseExact("2025-03-15", "yyyy-MM-dd", null);

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


  public bool IsVerticalSizeAuto { get; set; } = true;


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





  public List<DPoint> gardData { get; set; } = new List<DPoint>();



  protected async Task OnValueChanged(RangeSelectorValueChangedEventArgs info) {


    if (info.CurrentRange == null || info.CurrentRange[0] == null || info.CurrentRange[1] == null) {
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

  protected IEnumerable<KeyValuePair<string, string>> FDR_Tag_Value { get; set; } = new List<KeyValuePair<string, string>>();

  
  public string FDR_ID {
    get {
      if (FDR_Tag_Value == null || FDR_Tag_Value.ToList().Count <= 0 ) { return ""; }
      return string.Join(",", FDR_Tag_Value?.AsEnumerable().Select(r => r.Key));
    }
  }


  protected MitCombo? ComObj { get; set; }

  protected List<DataPoints> ChartData = new List<DataPoints>();
  //protected List<DataPoints> ChartDataMain = new List<DataPoints>();



  public int _currentCount;
  private System.Timers.Timer _timer;

  protected override void OnInitialized() {
    _timer = new();
    _timer.Interval = 1000;
    _timer.Elapsed += async (object? sender, ElapsedEventArgs e) =>    {
      if (IsRealTime) {
        _currentCount++;
        await GetRealTimeData();
        await InvokeAsync(StateHasChanged);
      }
    };
    _timer.Enabled = true;
  }


  public void Dispose() {
    IsRealTime = false;
    _timer.Enabled = false;
  }






  async Task GetRealTimeData() {




    DateTime tmpdt = DateTime.MinValue;
    foreach (var cd in ChartData) {

     DPoint dp =  cd.DPoints.LastOrDefault();
     if(tmpdt < dp.Xtime) {
        tmpdt = (DateTime)dp.Xtime;
      }

    }

    tmpdt = tmpdt.AddSeconds(1);

    DateTime tmpdt2 = tmpdt.AddSeconds(400);

    EndValue = tmpdt2;

    double minVal = StartValueY;
    double maxVal = EndValueY;


    foreach (var mcs in FDR_Tag_Value) {

      DataPoints dps = new DataPoints() { RName = mcs.Key };


      var rawData = await QueryService.ExecuteDatatableAsync("P_HMI_USEQTY_PEAK_FROM_TO_NEW3", new Dictionary<string, object?>()        {
                  {"SDATETM", tmpdt.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"EDATETM", tmpdt2.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"COM_ID", SelComp },
                  {"MC_ID", mcs.Key.ToStringTrim() },
              });

      foreach (DataRow d in rawData.Rows) {
        DateTime dttmp = DateTime.ParseExact(d["GAUGE_DATETIME"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
        double qty = double.Parse("0" + d["AVG_R_USEQTY"].ToString());


        if (IsVerticalSizeAuto) {
          if (minVal > qty) {
            minVal = qty;
          }
          if (maxVal < qty) {
            maxVal = qty;
          }
        }


        DPoint dp = new DPoint(dttmp, qty);
        dps.DPoints.Add(dp);
      }

      foreach (var cd in ChartData) {
        if(cd.RName == mcs.Key) {

          cd.DPoints.AddRange(dps.DPoints);
          break;
        }

      }

    }


    // 적용 하면 안된다. 적용 하려면 기존 데이터 까지 포함 해서 다루어라.
    if (IsVerticalSizeAuto) {
      StartValueX = EndValueY2 - (minVal + 10);
      EndValueY = maxVal + 10;
    }



    foreach (var cd in ChartData) {
      if (cd.DPoints.Count > 200) {
        cd.DPoints.RemoveRange(0, cd.DPoints.Count - 150 );
      }
    }



    _chart.RefreshData();
    await InvokeAsync(StateHasChanged);
    StateHasChanged();
  }







  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {
      if (!await IsAuthenticatedCheck()) return;

      await InitControls();

      StateHasChanged();

    }
  }

  private async Task InitControls() {



  StartValueC = DateTime.ParseExact("2025-03-13", "yyyy-MM-dd", null); // DateTime.Now.AddDays(-3);
          EndValueC = DateTime.ParseExact("2025-03-15", "yyyy-MM-dd", null); // DateTime.Now.AddDays(-2);

    List<DPoint> dTemp = new List<DPoint>();
    for (DateTime i = StartValueF; i <= EndValueF; i = i.AddMinutes(1)) {

      dTemp.Add(new DPoint() { Xtime = i, Qty1= 0});

    }
    gardData = dTemp;




    StartValue = DateTime.ParseExact("2025-03-13", "yyyy-MM-dd", null); // DateTime.Now.AddDays(-3);
          EndValue = DateTime.ParseExact("2025-03-15", "yyyy-MM-dd", null); // DateTime.Now.AddDays(-2);


          SYYYYMM.EditValue = StartValue.ToString("yyyy-MM-dd");
          EYYYYMM.EditValue = EndValue.ToString("yyyy-MM-dd");

   // });


  }

  protected override async Task Btn_Common_Search_Click() {
    await btn_Search();
  }

  // 조회 버튼 클릭
  protected async Task btn_Search() {



    try {
      ShowLoadingPanel();
      await ChartDataSelector();
      CloseLoadingPanel();
    }
    catch (Exception ee) {
      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "중요",
        Text = "ChartDataSelector Check :" + ee.Message
      });
    }
    finally {
      CloseLoadingPanel();
    }

  }


  public string _selComp = "hcc_US1";
  public string _p_selComp = "";
  public string SelComp {
    get { return _selComp; }
    set {
      _selComp = value;


      SetFdrData(_selComp);
    }
  }


  public async Task FDR_Change(IEnumerable<KeyValuePair<string, string>> values) {


    FDR_Tag_Value = values;
    // StateHasChanged();
   await MainChartData();
  }



  public async Task SetFdrData(string etc0) {
_p_selComp = etc0;
    var dt = await GetCommonCode("fdr", "", "", "", etc0);
    List<KeyValuePair<string, string>> dic = new List<KeyValuePair<string, string>>();
    foreach (DataRow d in dt.Rows) {


      dic.Add(new KeyValuePair<string, string>(d["CM_ID"] + "", d["CM_name"] + ""));

    }
    FdrData = dic;

    StateHasChanged();

  }



  public async void COM_Change(Razor.Pages.CommCode args) {
    SelComp = args.Name;
  }



  async Task MainChartData() {



    if (string.IsNullOrWhiteSpace(SelComp)) {

      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "알림",
        Text = "수용가 필수 선택 항목 입니다."
      });

      return;
    }

    List<DataPoints> _tmpData = await GetData(StartValue, EndValue        );

    ChartData = _tmpData;

    _chart.RefreshData();
    StateHasChanged();
  }

  async Task ChartDataSelector() {



    if (string.IsNullOrWhiteSpace(SelComp)) {

      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "알림",
        Text = "수용가 필수 선택 항목 입니다."
      });

      return;
    }

    StartValue = DateTime.ParseExact(SYYYYMM.EditValue, "yyyy-MM-dd", null);
    EndValue = DateTime.ParseExact(SYYYYMM.EditValue, "yyyy-MM-dd", null).AddMonths(1);
    StartValueC = StartValue; 
    EndValueC = EndValue;

    List<DataPoints> _tmpData = await GetData(StartValue, EndValue );

    List<DataPoints> main = _tmpData.ToList();
    

    //ChartDataMain = main;

    ChartData = _tmpData;

    _chart.RefreshData();
    StateHasChanged();
  }





  public DxChart _chart { get; set; }


  async Task<List<DataPoints>> GetData( DateTime sdt, DateTime edt) {

    double minVal = 9999999;
    double maxVal = -100;
    List<DataPoints> _tmpData = new List<DataPoints>();
    foreach (var mcs in FDR_Tag_Value) {

      DataPoints dps = new DataPoints() { RName = mcs.Key };


      var rawData = await QueryService.ExecuteDatatableAsync("P_HMI_USEQTY_PEAK_FROM_TO_NEW3", new Dictionary<string, object?>()        {
                  //{"SDATETM", DateTime.ParseExact( SYYYYMM.EditValue, "yyyy-MM-dd", null).ToString("yyyy-MM-dd HH:mm:ss") },
                  //{"EDATETM", DateTime.ParseExact( EYYYYMM.EditValue, "yyyy-MM-dd", null).AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss") },
                  {"SDATETM", sdt.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"EDATETM", edt.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"COM_ID", SelComp },
                  {"MC_ID", mcs.Key.ToStringTrim() },
                 // {"MC_ID", "PE1" },
              });

      foreach (DataRow d in rawData.Rows) {
        DateTime dttmp = DateTime.ParseExact(d["GAUGE_DATETIME"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
        double qty = double.Parse("0" + d["AVG_R_USEQTY"].ToString());


        if (IsVerticalSizeAuto) {
          if (minVal > qty) {
            minVal = qty;
          }
          if (maxVal < qty) {
            maxVal = qty;
          }
        }

        DPoint dp = new DPoint(dttmp, qty);
        dps.DPoints.Add(dp);
      }

      _tmpData.Add(dps);
    }


    if (IsVerticalSizeAuto) {
      StartValueX = EndValueY2 - (minVal + 10);
      EndValueY = maxVal + 10;
    }
    return _tmpData; 
  }
 

}
