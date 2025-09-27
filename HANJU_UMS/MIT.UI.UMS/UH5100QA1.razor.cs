/*
* 작성자명 : 김병만
* 작성일자 : 25-02-19
* 최종수정 : 25-02-19
* 화면명 : 동력팀 주간 주요 업무
* 프로시저 : P_HMI_PWR_)MAIN_DRV_RST_SELECT
*            P_HMI_PWR_)MAIN_DRV_RST_SAVE
*/
using DevExpress.Blazor;
using Microsoft.JSInterop;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.ServiceModel;
using Newtonsoft.Json;
using System.Data;
using System.Xml;

namespace MIT.UI.UMS;

public class UH5100QA1Base : CommonUIComponentBase {
  protected CommonDateEdit? SDATE { get; set; }
  protected CommonDateEdit? EDATE { get; set; }

  protected DataRow? currentRow;



  protected DxSpinEdit<float?>? SILO_VAL { get; set; }
  protected DxSpinEdit<float?>? PORT_VAL { get; set; }
  protected DxSpinEdit<float?>? ASH_SILO_6VAL { get; set; }
  protected DxSpinEdit<float?>? ASH_SILO_8VAL { get; set; }
  protected DxSpinEdit<float?>? LNG_DAY_USE_QTY { get; set; }
  protected DxSpinEdit<float?>? LNG_MON_USE_QTY { get; set; }
  protected DxSpinEdit<float?>? LPG_DAY_USE_QTY { get; set; }
  protected DxSpinEdit<float?>? LPG_MON_USE_QTY { get; set; }
  protected DxSpinEdit<float?>? PURE_WAT_TANK_QT107 { get; set; }
  protected DxSpinEdit<float?>? PURE_WAT_TANK_QT207 { get; set; }
  protected DxSpinEdit<float?>? PURE_WAT_TANK_QT307 { get; set; }
  protected DxSpinEdit<float?>? PURE_WAT_TANK_QT407 { get; set; }



  override protected async Task OnInitializedAsync() {
    await base.OnInitializedAsync();

    await btn_Search();
  }

  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {
      if (!await IsAuthenticatedCheck())
        return;

      InitControls();

      //await btn_Search();
    }
  }

  #region [ 컨트롤 초기 세팅 ]

  private void InitControls() {

  }

  #endregion [ 컨트롤 초기 세팅 ]

  #region [ 공통 버튼 기능 ]

  protected override async Task Btn_Common_Search_Click() {
    await btn_Search();
  }

  protected override async Task Btn_Common_Save_Click() {
    await btn_Save();
  }

    protected override async Task Btn_Common_Print_Click()
    {
        await Print();
    }


    /// <summary> jskim 의견 </summary>
    public async Task ShowPrint(string filename, string docUrl, string data)
    {
        await JSRuntime.InvokeVoidAsync("printFromStream", filename, docUrl, data);
    }

    private async Task Print()
    {
        try
        {
            ShowLoadingPanel();
            var serialize = await makeRpt();

            await ShowPrint("testfile", "opviewer", serialize);

        }
        catch (Exception ex)
        {
            CloseLoadingPanel();
            MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
        }
        finally
        {
            CloseLoadingPanel();
        }
    }

    async Task<string> makeRpt()
    {
        SyncCurrentRow();

        //var datasource = await DBSearch();
        var datasource = currentRow.Table;

        RptInfo ri = new RptInfo() { Columns = null, Data = datasource };

        DateTime searchDate = DateTime.Parse(SDATE.EditValue);
        DateTime compDate = DateTime.Parse(EDATE.EditValue);

        ri.Title = "동력팀 주요운전현황";
        ri.SubTitle = $"조회일자: {searchDate.ToString("yyyy.MM.dd")}\n비교일자: {compDate.ToString("yyyy.MM.dd")}";
        ri.UName = "test U";
        ri.FileName = "test_" + DateTime.Now.Ticks.ToString();
        ri.Landscape = "Y";
        ri.Date1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ri.ExportType = "pdf";

        var serialize = JsonConvert.SerializeObject(ri);
        return serialize;
    }


    #endregion [ 공통 버튼 기능 ]

    #region [ 사용자 버튼 기능 ]

    protected async Task btn_Search() {
    await Search();
  }

  protected async Task btn_Save() {
    if (currentRow == null)      return;

    MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback, Header: "저장");
  }
  #endregion [ 사용자 버튼 기능 ]

  #region [ 사용자 이벤트 함수 ]

  #endregion [ 사용자 이벤트 함수 ]

  #region [ 사용자 정의 메소드 ]

  private async Task Search() {


    try {
      ShowLoadingPanel();

      var datasource = await DBSearch();

      currentRow = datasource?.Rows.Count > 0 ? datasource.Rows[0] : null;

      CUST_COMMNET1 = currentRow["CUST_COMMNET1"].ToString();
      CUST_COMMNET2= currentRow["CUST_COMMNET2"].ToString();
      CUST_COMMNET3 = currentRow["CUST_COMMNET3"].ToString();
      SST_STM_CUST_REMARK1 = currentRow["SST_STM_CUST_REMARK1"].ToString();
      SST_STM_CUST_REMARK2 = currentRow["SST_STM_CUST_REMARK2"].ToString();
      SST_STM_CUST_REMARK3 = currentRow["SST_STM_CUST_REMARK3"].ToString();

    }
    catch (Exception ex) {
      CloseLoadingPanel();

      MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
    }
    finally {
      CloseLoadingPanel();
    }
  }

  protected async Task SaveCallback(CommonMsgResult result) {
    if (result != CommonMsgResult.Yes)
      return;

    if (await Save()) {
      MessageBoxService?.Show("저장하였습니다.", Header: "저장");
      await btn_Search();
    }
    else {
      MessageBoxService?.Show("저장에 실패하였습니다.", Header: "저장");
    }
  }


  public string CUST_COMMNET1 { get; set; }
  public string CUST_COMMNET2 { get; set; }
  public string CUST_COMMNET3 { get; set; }
  public string SST_STM_CUST_REMARK1 { get; set; }
  public string SST_STM_CUST_REMARK2 { get; set; }
  public string SST_STM_CUST_REMARK3 { get; set; }

  private void SyncCurrentRow()
    {
        if (currentRow == null) return;

        currentRow["SILO_VAL"] = SILO_VAL?.Value;
        currentRow["PORT_VAL"] = PORT_VAL?.Value;
        currentRow["ASH_SILO_6VAL"] = ASH_SILO_6VAL?.Value;
        currentRow["ASH_SILO_8VAL"] = ASH_SILO_8VAL?.Value;
        currentRow["LNG_DAY_USE_QTY"] = LNG_DAY_USE_QTY?.Value;
        currentRow["LNG_MON_USE_QTY"] = LNG_MON_USE_QTY?.Value;
        currentRow["LPG_DAY_USE_QTY"] = LPG_DAY_USE_QTY?.Value;
        currentRow["LPG_MON_USE_QTY"] = LPG_MON_USE_QTY?.Value;
        currentRow["PURE_WAT_TANK_QT107"] = PURE_WAT_TANK_QT107?.Value;
        currentRow["PURE_WAT_TANK_QT207"] = PURE_WAT_TANK_QT207?.Value;
        currentRow["PURE_WAT_TANK_QT307"] = PURE_WAT_TANK_QT307?.Value;
        currentRow["PURE_WAT_TANK_QT407"] = PURE_WAT_TANK_QT407?.Value;


        currentRow["CUST_COMMNET1"] = CUST_COMMNET1;


        currentRow["CUST_COMMNET2"] = CUST_COMMNET2;
        currentRow["CUST_COMMNET3"] = CUST_COMMNET3;
        currentRow["SST_STM_CUST_REMARK1"] = SST_STM_CUST_REMARK1;
        currentRow["SST_STM_CUST_REMARK2"] = SST_STM_CUST_REMARK2;
        currentRow["SST_STM_CUST_REMARK3"] = SST_STM_CUST_REMARK3;
    }

 

  private async Task<bool> Save() {
    try {
      ShowLoadingPanel();

        if(currentRow == null) return false;

        SyncCurrentRow();


      await DBSave(currentRow.Table);

      return true;
    }
    catch (Exception ex) {
      CloseLoadingPanel();
      MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
      return false;
    }
    finally {
      CloseLoadingPanel();
    }
  }
  #endregion [ 사용자 정의 메소드 ]

  #region [ 데이터 정의 메소드 ]

  private async Task<DataTable?> DBSearch() {
    if (QueryService == null)      return null;

    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_PWR_)MAIN_DRV_RST_SELECT", new Dictionary<string, object?>()              {
                  {"DRV_DATE", SDATE?.EditValue },
                  {"CMP_DATE", EDATE?.EditValue }
              });

    return datatable;
  }


  private async Task DBSave(DataTable datatable) {
    if (QueryService == null)      return;

    await QueryService.ExecuteNonQuery("P_HMI_PWR_)MAIN_DRV_RST_SAVE", datatable, null);
  }

  #endregion [ 데이터 정의 메소드 ]
}
