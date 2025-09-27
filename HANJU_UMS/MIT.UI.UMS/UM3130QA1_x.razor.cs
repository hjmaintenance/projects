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


namespace MIT.UI.UMS;
public class UM3130QA1_xBase : CommonUIComponentBase {
  protected CommonDateEdit SYYYYMM { get; set; }
  protected CommonDateEdit EYYYYMM { get; set; }
  protected MitCombo FDR_CB { get; set; }
  protected string? COM_ID { get; set; }
  protected string? FDR_ID { get; set; }

  protected string? COM_NAME { get; set; }

  protected string? Fmt_id { get; set; }
  protected string? Fmt_nm { get; set; }





  
    



  protected MitCombo ComObj { get; set; }
  protected MitCombo FmtObj { get; set; }


  protected CommonGrid? Grd1 { get; set; }

  protected List<DataPoint> ChartData = new List<DataPoint>();





  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {
      if (!await IsAuthenticatedCheck())        return;

     await InitControls();

      StateHasChanged();



      //await btn_Search();
    }
  }

  #region [ 컨트롤 초기 세팅 ]

  private async Task InitControls() {
    if (Grd1 == null || Grd1.Grid == null)      return;













    await Task.Run(async () => {

      bool isOk = false;
      for (int i = 0; i < 10; i++) {

        await Task.Delay(500);
        try {



          SYYYYMM.EditValue = DateTime.Now.ToString("yyyy-MM");
          EYYYYMM.EditValue = DateTime.Now.ToString("yyyy-MM");

          //COM_ID = "ka_AA";// { get; set; }
          //FDR_ID = "AA_FR_DW";


          Sdate = SYYYYMM.EditValue + "-01";
          Edate = DateTime.ParseExact(EYYYYMM.EditValue + "-01", "yyyy-MM-dd", null).AddMonths(1).ToString("yyyy-MM-dd");


          if (USER_TYPE == "H") {

            CommCode cc = await ComObj.GetFirstRowSelect();

            COM_ID = cc.Name;
            COM_NAME = cc.Value;
          }

          //CommCode ccf = await FmtObj.GetFirstRowSelect();

          //Fmt_id = ccf.Name;
          //Fmt_nm = ccf.Value;

          Fmt_id = "yyyyMMddHH";
          Fmt_nm = "정시";


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
  #endregion [ 컨트롤 초기 세팅 ]

  #region [ 공통 버튼 기능 ]

  protected override async Task Btn_Common_Search_Click() {
    await btn_Search();
  }

  #endregion [ 공통 버튼 기능 ]

  #region [ 사용자 버튼 기능 ]

  protected async Task btn_Search() {
    await Search();
  }

  #endregion [ 사용자 버튼 기능 ]

  #region [ 사용자 이벤트 함수 ]

  protected void OnInputSearchParameter(Dictionary<string, object?> parameters) {

  }

  public string Sdate { get; set; } = "2025-02-01";
  public string Edate { get; set; } = "2025-04-01";


  #endregion [ 사용자 이벤트 함수 ]

  #region [ 사용자 정의 메소드 ]

  public List<DataPoint> _lstAll = new List<DataPoint>();
  public List<DataPoint> _lstDay = new List<DataPoint>();
  public List<DataPoint> _lstHour = new List<DataPoint>();
  public List<DataPoint> _lstMin = new List<DataPoint>();
  public List<DataPoint> _lstSec = new List<DataPoint>();

  private async Task Search() {
    if (Grd1 == null)      return;


    if (string.IsNullOrWhiteSpace(FDR_ID)) {

      MessageBoxService?.Show("FDR 은 필수 선택 항목 입니다.");
      return;
    }

    try {
      ShowLoadingPanel();

      Sdate = DateTime.ParseExact(SYYYYMM.EditValue + "-01", "yyyy-MM-dd", null).ToString("yyyy-MM-dd");
      Edate = DateTime.ParseExact( EYYYYMM.EditValue + "-01", "yyyy-MM-dd", null).AddMonths(1).ToString("yyyy-MM-dd");



      StateHasChanged();

      // quristyle 그리드 데이터 가져오기.
      await DBSearchMaxPeakValues();


    } catch (Exception ex) {
      CloseLoadingPanel();

      MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
    } finally {
      CloseLoadingPanel();
    }
  }




  public async void COM_Change(Razor.Pages.CommCode args) {
    FDR_CB.Etc0 = args.Name;
  }



  public DxChart _chart { get; set; }


  public string choiceFmt { get; set; }


  public async Task CheckData(string fmat) {


    switch (fmat) {
      case "yyyyMMdd": ChartData = _lstDay; break;
      case "yyyyMMddHH": ChartData = _lstHour; break;
      case "yyyyMMddHHmm": ChartData = _lstMin; break;
      case "yyyyMMddHHmms": ChartData = _lstSec; break;
      //case "yyyyMMddHHmmss": ChartData = _lstAll; break;
      default: ChartData = _lstDay; break;
    }
  }

  public async Task FmtItemChanged(Razor.Pages.CommCode cc) {
    ShowLoadingPanel();
    await CheckData(cc.Name);
    CloseLoadingPanel();
  }


  #endregion [ 사용자 정의 메소드 ]

  #region [ 데이터 정의 메소드 ]


  public List<PeakPoint> _peakList = new List<PeakPoint>();


  public async Task DBSearchMaxPeakValues_fffffffffffffffffffffffffffffffffffffffffffffffffffff () {
    if (QueryService == null)      return ;

    _peakList.Clear();


    _lstAll.Clear();
    _lstDay.Clear();
    _lstHour.Clear();
    _lstMin.Clear();
    _lstSec.Clear();
    ChartData = null;
    Grd1.DataSource = null;

    StateHasChanged();

    await Task.Run( async () => { 
    
    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_PEAK_VALUE_RST_FROM_TO", new Dictionary<string, object?>()        {
                  {"SYYYYMM", SYYYYMM?.EditValue },
                  {"EYYYYMM", EYYYYMM?.EditValue },
                  {"MC_ID", FDR_ID.ToStringTrim() },
              });


    Grd1.DataSource = datatable;
    await Grd1.PostEditorAsync();


    foreach (DataRow d in datatable.Rows) {
      _peakList.Add(
      new PeakPoint(DateTime.ParseExact(d["PEAK_SDATE"] + "" + d["PEAK_STIME"] + "", "yyyy-MM-ddHH:mm", null)
        , DateTime.ParseExact(d["PEAK_EDATE"] + "" + d["PEAK_ETIME"] + "", "yyyy-MM-ddHH:mm", null)
        , double.Parse(d["PEAK_VAL"] + ""))
      );

    }


    var rawData = await DBSearchUseQTYSec();
    //ChartData.Clear();

    DateTime rdt = DateTime.MinValue; // rowdata 건건
    DateTime tdtd = DateTime.MinValue; // day 비교용.
    DateTime tdth = DateTime.MinValue; // hour 비교용.
    DateTime tdtm = DateTime.MinValue; // min 비교용.
    DateTime tdts = DateTime.MinValue; // 10초 비교용.

      DateTime dttmp = DateTime.MinValue;
    bool isPeak = false; 
    foreach (DataRow d in rawData.Rows) {

      dttmp = DateTime.ParseExact(d["GAUGE_DATETIME"].ToString(), "yyyy-MM-dd HH:mm:ss", null);

      // 첫 시작시 비교 data 동일 출발.
      if (rdt == DateTime.MinValue) { tdtd = dttmp; tdth = dttmp; tdtm = dttmp; tdts = dttmp; }
      // 첫 시작시 rowdata 동일 출발.
      rdt = dttmp;

      isPeak = false;
      foreach ( var p in _peakList) {
        if( p.Sdt <= dttmp && p.Edt >= dttmp  ) { isPeak = true; break; }
      }


      DataPoint dp = new DataPoint(dttmp, double.Parse(d["R_USEQTY"].ToString()), isPeak);

      if ((rdt.ToString("yyyyMMdd") == tdtd.ToString("yyyyMMdd"))) { // 일자가 같은한놈.
        _lstDay.Add(new DataPoint(dttmp, double.Parse(d["R_USEQTY"].ToString()), isPeak));
      }
      if ((rdt.ToString("yyyyMMdd") == tdtd.ToString("yyyyMMdd")) 
         || (rdt.ToString("yyyyMMddHH") == tdth.ToString("yyyyMMddHH"))
         ) { // 일시가 같은한놈.
        _lstHour.Add(new DataPoint(dttmp, double.Parse(d["R_USEQTY"].ToString()), isPeak));
      }
      if ((rdt.ToString("yyyyMMdd") == tdtd.ToString("yyyyMMdd"))
         || (rdt.ToString("yyyyMMddHH") == tdth.ToString("yyyyMMddHH"))
         || (rdt.ToString("yyyyMMddHHmm") == tdtm.ToString("yyyyMMddHHmm"))
         ) { // 일시분이 같은한놈.
        _lstMin.Add(new DataPoint(dttmp, double.Parse(d["R_USEQTY"].ToString()), isPeak));
      }
      if ((rdt.ToString("yyyyMMdd") == tdtd.ToString("yyyyMMdd"))
         || (rdt.ToString("yyyyMMddHH") == tdth.ToString("yyyyMMddHH"))
         || (rdt.ToString("yyyyMMddHHmm") == tdtm.ToString("yyyyMMddHHmm"))
         || (rdt.ToString("yyyyMMddHHmms") == tdts.ToString("yyyyMMddHHmms"))) { // 일시분이 같은한놈.
        _lstSec.Add(new DataPoint(dttmp, double.Parse(d["R_USEQTY"].ToString()), isPeak));
      }


      if ((rdt.ToString("yyyyMMdd") == tdtd.ToString("yyyyMMdd"))) { // 일자가 같은한놈.
        tdtd = rdt.AddDays(1);
      }
      if ((rdt.ToString("yyyyMMdd") == tdtd.ToString("yyyyMMdd"))
         || (rdt.ToString("yyyyMMddHH") == tdth.ToString("yyyyMMddHH"))
         ) { // 일시가 같은한놈.
        tdth = rdt.AddHours(1);
      }
      if ((rdt.ToString("yyyyMMdd") == tdtd.ToString("yyyyMMdd"))
         || (rdt.ToString("yyyyMMddHH") == tdth.ToString("yyyyMMddHH"))
         || (rdt.ToString("yyyyMMddHHmm") == tdtm.ToString("yyyyMMddHHmm"))
         ) { // 일시분이 같은한놈.
        tdtm = rdt.AddMinutes(1);
      }
      if ((rdt.ToString("yyyyMMdd") == tdtd.ToString("yyyyMMdd"))
         || (rdt.ToString("yyyyMMddHH") == tdth.ToString("yyyyMMddHH"))
         || (rdt.ToString("yyyyMMddHHmm") == tdtm.ToString("yyyyMMddHHmm"))
         || (rdt.ToString("yyyyMMddHHmms") == tdts.ToString("yyyyMMddHHmms"))) { // 일시분이 같은한놈.
        tdts = rdt.AddSeconds(10);
      }
      _lstAll.Add(dp);
    }




    await CheckData(Fmt_id);


    });

    //return datatable;
  }



  public async Task DBSearchMaxPeakValues() {
    if (QueryService == null) return;

    _peakList.Clear();
    _lstAll.Clear();
    _lstDay.Clear();
    _lstHour.Clear();
    _lstMin.Clear();
    _lstSec.Clear();
    ChartData = null;
    Grd1.DataSource = null;

    StateHasChanged();

    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_PEAK_VALUE_RST_FROM_TO", new Dictionary<string, object?>()
    {
        {"SYYYYMM", SYYYYMM?.EditValue },
        {"EYYYYMM", EYYYYMM?.EditValue },
        {"MC_ID", FDR_ID.ToStringTrim() },
    });

    Grd1.DataSource = datatable;
    await Grd1.PostEditorAsync();

    foreach (DataRow d in datatable.Rows) {
      _peakList.Add(new PeakPoint(
          DateTime.ParseExact(d["PEAK_SDATE"] + "" + d["PEAK_STIME"], "yyyy-MM-ddHH:mm", null),
          DateTime.ParseExact(d["PEAK_EDATE"] + "" + d["PEAK_ETIME"], "yyyy-MM-ddHH:mm", null),
          double.Parse(d["PEAK_VAL"].ToString())
      ));
    }

    var rawData = await DBSearchUseQTYSec();
    if (rawData == null) return;

    DateTime tdtd = DateTime.MinValue;
    DateTime tdth = DateTime.MinValue;
    DateTime tdtm = DateTime.MinValue;
    DateTime tdts = DateTime.MinValue;

    foreach (DataRow d in rawData.Rows) {
      DateTime dttmp = DateTime.ParseExact(d["GAUGE_DATETIME"].ToString(), "yyyy-MM-dd HH:mm:ss", null);
      bool isPeak = _peakList.Any(p => p.Sdt <= dttmp && p.Edt >= dttmp);

      DataPoint dp = new DataPoint(dttmp, double.Parse(d["R_USEQTY"].ToString()), isPeak);

      if (dttmp.Date == tdtd.Date) _lstDay.Add(dp);
      if (dttmp.Date == tdtd.Date || dttmp.Hour == tdth.Hour) _lstHour.Add(dp);
      if (dttmp.Date == tdtd.Date || dttmp.Hour == tdth.Hour || dttmp.Minute == tdtm.Minute) _lstMin.Add(dp);
      if (dttmp.Date == tdtd.Date || dttmp.Hour == tdth.Hour || dttmp.Minute == tdtm.Minute || dttmp.Second / 10 == tdts.Second / 10) _lstSec.Add(dp);

      tdtd = dttmp.AddDays(1);
      tdth = dttmp.AddHours(1);
      tdtm = dttmp.AddMinutes(1);
      tdts = dttmp.AddSeconds(10);

      _lstAll.Add(dp);
    }

    await CheckData(Fmt_id);

  }




  private async Task<DataTable?> DBSearchUseQTYSec() {
    if (QueryService == null)      return null;
    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_MIN_USEQTY_PEAK_FROM_TO", new Dictionary<string, object?>()        {
                  {"SYYYYMM", SYYYYMM?.EditValue },
                  {"EYYYYMM", EYYYYMM?.EditValue },
                  {"MC_ID", FDR_ID.ToStringTrim() },
              });

    return datatable;
  }

  //private async Task<DataTable?> DBSearchUseQTYHour()
  //{
  //    if (QueryService == null)
  //        return null;

  //    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_TIME_USEQTY_PEAK", new Dictionary<string, object?>()
  //        {
  //            {"YYYYMM", YYYYMM?.EditValue },
  //            {"MC_ID", FDR_ID.ToStringTrim() },
  //        });

  //    return datatable;
  //}

  //private async Task<DataTable?> DBSearchUseQTYDay()
  //{
  //    if (QueryService == null)
  //        return null;

  //    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_DAY_USEQTY_PEAK", new Dictionary<string, object?>()
  //        {
  //            {"YYYYMM", YYYYMM?.EditValue },
  //            {"MC_ID", FDR_ID.ToStringTrim() },
  //        });

  //    return datatable;
  //}

  #endregion [ 데이터 정의 메소드 ]
}

