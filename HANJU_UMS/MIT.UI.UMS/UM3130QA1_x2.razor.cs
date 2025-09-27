/*
* 작성자명 : 김지수
* 작성일자 : 25-02-27
* 최종수정 : 25-02-27
* 화면명 : 계측기별 Peak값 조회
* 프로시저명 : : P_HMI_DAY_USEQTY_PEAK, P_HMI_TIME_USEQTY_PEAK, P_HMI_MIN_UEEQTY_PEAK, P_HMI_PEAK_VALUE_RST
*/
using DevExpress.Blazor;
using MIT.DataUtil.Common;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.ServiceModel;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using static System.Net.WebRequestMethods;


namespace MIT.UI.UMS;

public class UM3130QA1_x2Base : CommonUIComponentBase {

  protected string FormatDate(DateTime date) => date.ToString("yyyy-MM-dd HH:mm:ss");

  protected CommonDateEdit SYYYYMM { get; set; }
  protected CommonDateEdit EYYYYMM { get; set; }
  //protected MitCombo FDR_CB { get; set; }




  protected MitTagBox FDR_Tag { get; set; }
  protected IEnumerable<CommCode> FDR_Tag_Value { get; set; } = new List<CommCode>();



  protected string? COM_ID { get; set; } = "hcc_US1";
  

  public string FDR_ID {
    get {
      return string.Join(",", FDR_Tag_Value?.AsEnumerable().Select(r => r.Name));
    }
    set {

    }
  }















  protected string? COM_NAME { get; set; }

  //protected string? Fmt_id { get; set; }
  //protected string? Fmt_nm { get; set; }





  protected MitCombo ComObj { get; set; }
  protected MitCombo FmtObj { get; set; }


  protected CommonGrid? Grd1 { get; set; }

  protected List<DataPoints> ChartDataMain = new List<DataPoints>();
  protected List<DataPoints> ChartData = new List<DataPoints>();



  public DateTime StartValue = DateTime.Now;
  public DateTime EndValue = DateTime.Now.AddMonths(1);

  public double StartValueY = 0;
  public double EndValueY = 9999;



  public DateTime StartValueC { get; set; }// = DateTime.Now;
  public DateTime EndValueC { get; set; }// = DateTime.Now.AddMonths(1);




  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {
      if (!await IsAuthenticatedCheck()) return;

      await InitControls();

      StateHasChanged();



      //await btn_Search();
    }
  }


  private async Task InitControls() {
    if (Grd1 == null || Grd1.Grid == null) return;


    await Task.Run(async () => {

      bool isOk = false;
      for (int i = 0; i < 10; i++) {

        await Task.Delay(500);
        try {

          SYYYYMM.EditValue = DateTime.Now.ToString("yyyy-MM");
          EYYYYMM.EditValue = DateTime.Now.ToString("yyyy-MM");

          StartValueC = DateTime.ParseExact(SYYYYMM.EditValue + "-01", "yyyy-MM-dd", null);
          EndValueC = DateTime.ParseExact(EYYYYMM.EditValue + "-01", "yyyy-MM-dd", null).AddMonths(1);

          StartValue = DateTime.ParseExact(SYYYYMM.EditValue + "-01", "yyyy-MM-dd", null);
          EndValue = DateTime.ParseExact(EYYYYMM.EditValue + "-01", "yyyy-MM-dd", null).AddMonths(1);


          if (USER_TYPE == "H") {

            //CommCode cc = await ComObj.GetFirstRowSelect();

            //COM_ID = cc.Name;
            //COM_NAME = cc.Value;
          }

          //CommCode ccf = await FmtObj.GetFirstRowSelect();

          //Fmt_id = ccf.Name;
          //Fmt_nm = ccf.Value;

          //Fmt_id = "1";
          //Fmt_nm = "일";

          isOk = true;

        }
        catch (Exception ee) {
        }

        if (isOk) {
          break;
        }

      }

      StateHasChanged();

    });



  }



  protected override async Task Btn_Common_Search_Click() {
    await btn_Search();
  }


  // 조회 버튼 클릭
  protected async Task btn_Search() {

    try {
      ShowLoadingPanel();
      await MainChartData();
      CloseLoadingPanel();
    }
    catch (Exception ee) {
      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "중요",
        Text = "MainChartData Check :" + ee.Message
      });
    }
    finally {
      CloseLoadingPanel();
    }

  }


  // 중앙 차트 데이터 호출
  public async Task Search_Run() {
    if (Grd1 == null) return;

    //StartValueY = 0;
    //EndValueY = 9999;

    await DBSearchMaxPeakValues();
    isCLoading = false;
    StateHasChanged();
  }



  public DateTime chkSdt { get; set; }
  public DateTime chkEdt { get; set; }
  public bool isCLoading = true;

  public async void COM_Change(Razor.Pages.CommCode args) {


    //FDR_CB.Etc0 = args.Name;
    FDR_Tag.Etc0 = args.Name;
    //StateHasChanged();
  }

  public DxChart _chart { get; set; }

  public string choiceFmt { get; set; }

  //public async Task FmtItemChanged(Razor.Pages.CommCode cc) {
  //  ShowLoadingPanel();



  //  CloseLoadingPanel();
  //}

  public List<PeakPoint> _peakList = new List<PeakPoint>();

  //public List<DataPoint> _lstAll = new List<DataPoint>();

  public async Task DBSearchMaxPeakValues() {
    if (QueryService == null) return;
    //List<DataPoints> _lstAll = new List<DataPoints>();
    //ChartData = _lstAll;
    ChartData.Clear();
    //StateHasChanged();

    try {

      var rawData = await DBSearchUseQTYSec();
      if (rawData == null) return;


      DataPoints ddp = null;
      foreach (DataRow d in rawData.Rows) {

        bool isExist = false;
        foreach (var cd in ChartData) {
          if (cd.RName == d["MC_ID"].ToStringTrim()) { // 같은 이름이 있으면
            ddp = cd;
            isExist = true;
            break;
          }
        }

        if (!isExist) { // 같은 이름이 없으면
          ddp = new DataPoints() { RName = d["MC_ID"].ToStringTrim() };
          ChartData.Add(ddp);
        }

        DateTime dttmp = DateTime.ParseExact(d["GAUGE_DATETIME"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
        DPoint dp = new DPoint(dttmp, double.Parse("0" + d["AVG_R_USEQTY"].ToString()));
        ddp.DPoints.Add(dp);
      }

      //StartValueY = minVal - 100;
      //EndValueY = maxVal + 100;

      //ChartData = _lstAll;

    }
    catch (Exception ee) {
      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "중요",
        Text = "Data Check :"+ ee.Message
      });
    }
    finally {
      isCLoading = false;
    }
    StateHasChanged();
  }

  public bool isFirstLoadComplete = false;

  async Task MainChartData() {

    if (Grd1 == null) return;


    if (string.IsNullOrWhiteSpace(COM_ID)) {

      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "알림",
        Text = "수용가 필수 선택 항목 입니다."
      });
      isCLoading = false;

      return;
    }


    // 데이터 초기화
    ChartDataMain.Clear();
    ChartData.Clear();
    Grd1.DataSource = null;


    StartValueC = DateTime.ParseExact(SYYYYMM.EditValue + "-01", "yyyy-MM-dd", null);
    EndValueC = DateTime.ParseExact(EYYYYMM.EditValue + "-01", "yyyy-MM-dd", null).AddMonths(1);

    StartValue = DateTime.ParseExact(SYYYYMM.EditValue + "-01", "yyyy-MM-dd", null);
    EndValue = DateTime.ParseExact(EYYYYMM.EditValue + "-01", "yyyy-MM-dd", null).AddMonths(1);

    try {
    }
    catch (Exception ee) {
    }
    finally {
    }

      // 가이드 차트 데이터
      var rawData = await QueryService.ExecuteDatatableAsync("P_HMI_USEQTY_PEAK_FROM_TO_NEW2", new Dictionary<string, object?>()        {
                  {"SDATETM", StartValueC.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"EDATETM", EndValueC.ToString("yyyy-MM-dd HH:mm:ss") },
                  //{"TYPE", "2" },
                  {"COM_ID", COM_ID },
                  {"MC_ID", FDR_ID.ToStringTrim() },
                  //{"VALUE_TP", "1" },
              });


   // List<DataPoints> dpp = new List<DataPoints>();
   // List<DataPoints> dpp2 = new List<DataPoints>();
   // dpp2.Add(new DPoint(StartValue, 0));

    double minVal = 9999;
    double maxVal = 0;
    bool isFirst = true;
    DataPoints ddp = null;
    foreach (DataRow d in rawData.Rows) {


      bool isExist = false;
      foreach (var cd in ChartDataMain) {
        if (cd.RName == d["MC_ID"].ToStringTrim()) { // 같은 이름이 있으면
          ddp = cd;
          isExist = true;
          break;
        }
      }

      if (!isExist) { // 같은 이름이 없으면
        ddp = new DataPoints() { RName = d["MC_ID"].ToStringTrim() };
        ChartDataMain.Add(ddp);
      }



      DateTime dttmp = DateTime.ParseExact(d["GAUGE_DATETIME"].ToString(), "yyyy-MM-dd HH:mm:ss", null);

      double qty = double.Parse("0" + d["AVG_R_USEQTY"].ToString());

      if (minVal > qty) {
        minVal = qty;
      }
      if (maxVal < qty) {
        maxVal = qty;
      }
      DPoint dp = new DPoint(dttmp, qty);
      ddp.DPoints.Add(dp);

      if(isFirst) {
        //dpp2.Add(new DPoint(dttmp, 0));
        isFirst = false;
      }
      //dpp2.Add(dp);

    }

    StartValueY = minVal - 1000;
    EndValueY = maxVal + 1000;


    //ChartData = dpp;

    //dpp2.Add(new DataPoint(dpp2.Last().GAUGE_DATETIME, 0, false));

    //dpp2.Add(new DataPoint(EndValue, 0, false));

    //ChartDataMain = dpp2;

    StateHasChanged();

    // peak 차트 데이터
    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_PEAK_VALUE_RST_FROM_TO", new Dictionary<string, object?>()    {
        {"SYYYYMM", SYYYYMM?.EditValue },
        {"EYYYYMM", EYYYYMM?.EditValue },
        {"MC_ID", FDR_ID.ToStringTrim() },
    });

    Grd1.DataSource = datatable;
    await Grd1.PostEditorAsync();

    foreach (DataRow d in datatable.Rows) {
      break;
      _peakList.Add(new PeakPoint(
          DateTime.ParseExact(d["PEAK_SDATE"] + "" + d["PEAK_STIME"], "yyyy-MM-ddHH:mm", null),
          DateTime.ParseExact(d["PEAK_EDATE"] + "" + d["PEAK_ETIME"], "yyyy-MM-ddHH:mm", null),
          double.Parse("0" + d["PEAK_VAL"].ToString())
      ));

    }
    Task.Run(() => {

      Task.Delay(5000);

      isCLoading = false;
      isFirstLoadComplete = true;

    });

    StateHasChanged();

  }


  // 중앙 차트 데이터
  private async Task<DataTable?> DBSearchUseQTYSec() {
    if (QueryService == null) return null;

    var dt = await QueryService.ExecuteDatatableAsync("P_HMI_USEQTY_PEAK_FROM_TO_NEW2", new Dictionary<string, object?>()        {
                  {"SDATETM", StartValue.ToString("yyyy-MM-dd HH:mm:ss") },
                  {"EDATETM", EndValue.ToString("yyyy-MM-dd HH:mm:ss") },
                  //{"TYPE", level },
                  {"COM_ID", COM_ID },
                  {"MC_ID", FDR_ID.ToStringTrim() },
                  {"Zoom", "4" },
              });



    return dt;
  }

  public double c1_gap_total = 0;
  public double c2_gap_total = 0;
  public double c1_c2_gap_total = 0;

  protected async Task OnValueChanged(RangeSelectorValueChangedEventArgs info) {


    if ( !isFirstLoadComplete )   return;


    if (info.CurrentRange == null || info.CurrentRange[0] == null || info.CurrentRange[1] == null) {
      isCLoading = false;
      return;
    }
    ;


    DateTime c1 = (DateTime)info.CurrentRange[0];
    DateTime c2 = (DateTime)info.CurrentRange[1];

    TimeSpan c1_gap = (chkSdt - c1).Duration();
    TimeSpan c2_gap = (chkEdt - c2).Duration();

    if (c1_gap.TotalMinutes > 1 || c2_gap.TotalMinutes > 1) {
    }
    else {
      isCLoading = false;


      return;
    }
    ;

    c1_gap_total = c1_gap.TotalMinutes;
    c2_gap_total = c2_gap.TotalMinutes;
   
    c1_c2_gap_total = Math.Abs((c1 - c2).TotalMinutes);


    StartValue = (DateTime)info.CurrentRange[0];
    EndValue = (DateTime)info.CurrentRange[1];
    //eval = int.Parse((info.CurrentRange[1] + "").Split('.')[0]);


    isCLoading = true;

    chkSdt = (DateTime)info.CurrentRange[0];
    chkEdt = (DateTime)info.CurrentRange[1];

    await Search_Run();

    StateHasChanged();



  }





















}
