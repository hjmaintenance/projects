using MIT.DataUtil.Common;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;
/*
* 작성자명 : quristyle
* 작성일자 : 25-01-22
* 최종수정 : 25-02-13
* 프로시저 : P_HMI_WATER_HOUR_SRV_RST_SELECT02
*/
namespace MIT.UI.UMS {
  public class UH2200QA1Base : CommonUIComponentBase {
    protected CommonDateEdit? Dte_P_DATE { get; set; }

    protected IEnumerable<CommCode> ComValues { get; set; } = new List<CommCode>();
    public string COM_ID
    {
        get
        {
            return string.Join(",", ComValues?.AsEnumerable().Select(r => r.Name));
        }
        set
        {

        }
    }
        protected string? seq { get; set; }
    protected string? SEQ2 { get; set; }
    protected string? com_name { get; set; }
    //protected string? mc_id { get; set; }
    protected string? GAUGE_TYPE { get; set; }


    protected CommonGrid? Grd1 { get; set; }

    protected override void OnInitialized() { }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender) {
        if (!await IsAuthenticatedCheck()) return;

        InitControls();

        await btn_Search();

      }
    }

    #region [ 컨트롤 초기 세팅 ]

    private void InitControls() {
      if (Grd1 == null || Grd1.Grid == null) return;
      Dte_P_DATE?.SetOffsetDayDate(0);
      Grd1.SetCellStyle("com_name", "순수계,여과수계");
    }


    #endregion [ 컨트롤 초기 세팅 ]

    #region [ 공통 버튼 기능 ]

    protected override async Task Btn_Common_Search_Click() { await btn_Search(); }
    #endregion [ 공통 버튼 기능 ]

    #region [ 사용자 버튼 기능 ]
    protected async Task btn_Search() { await Search();   }
    #endregion [ 사용자 버튼 기능 ]

    #region [ 사용자 이벤트 함수 ]



    #endregion [ 사용자 이벤트 함수 ]

    #region [ 사용자 정의 메소드 ]

    private async Task Search() {
      if (Grd1 == null) return;

      try {
        ShowLoadingPanel();

        var datasource = await DBSearch();

        Grd1.DataSource = datasource;

        //await Grd1.PostEditorAsync();
      }
      catch (Exception ex) {
        CloseLoadingPanel();

        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
      }
      finally {
        CloseLoadingPanel();
      }
    }


        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch()
        {
            if (QueryService == null) return null;

            //string proc_name = "P_HMI_WATER_HOUR_SRV_RST_SELECT02";

            //if (string.IsNullOrWhiteSpace(P_COM_ID.ToStringTrim())) { proc_name = "P_HMI_WATER_HOUR_SRV_RST"; }
            //var datatable = await QueryService.ExecuteDatatableAsync_fix(proc_name, new Dictionary<string, object?>()          {
            //              { "DATE", Dte_P_DATE?.EditValue },
            //              { "COM_ID", P_COM_ID.ToStringTrim() },
            //    }, "P_");

      string procedureName = string.IsNullOrWhiteSpace(COM_ID) ? "P_HMI_WATER_HOUR_SRV_RST" : "P_HMI_WATER_HOUR_SRV_RST_SELECT02"; // 업체 선택 안한경우 : 업체 선택한 경우

      var datatable = await QueryService.ExecuteDatatableAsync_fix(procedureName, new Dictionary<string, object?>()
                {
                    { "DATE", Dte_P_DATE?.EditValue },
                    { "COM_ID", COM_ID.ToStringTrim() },
                }, "P_");



      return datatable;
        }

    #endregion [ 데이터 정의 메소드 ]
  }
}