/*
* 작성자명 : 김지수
* 작성일자 : 25-02-06
* 최종수정 : 25-02-06
* 화면명 : 증기 공급량 현황 (수용가별,일별)
* 프로시저 : P_HMI_USE_DAYSTMCOMDAY_SELECT01
*/
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;

namespace MIT.UI.UMS;

public class UM2240QA1Base : CommonUIComponentBase {
  protected CommonDateEdit? DATE { get; set; }
  protected string? COM_ID { get; set; }
  protected CommonGrid? Grd1 { get; set; }

  protected override void OnInitialized() {

  }

  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {
      if (!await IsAuthenticatedCheck())
        return;

      InitControls();

      await btn_Search();
    }
  }

  #region [ 컨트롤 초기 세팅 ]

  private void InitControls() {
    if (Grd1 == null || Grd1.Grid == null)
      return;
    Grd1.SetCellStyle("COM_NAME", "소 계,합 계,잉여증기 합계");
    //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });

    //Default 전일을 선택한 날짜로
    DATE.EditValue = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");

    StateHasChanged();
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

  #endregion [ 사용자 이벤트 함수 ]

  #region [ 사용자 정의 메소드 ]

  private async Task Search() {
    if (Grd1 == null)
      return;

    try {
      ShowLoadingPanel();

      var datasource = await DBSearch();

      Grd1.DataSource = datasource;

      await Grd1.PostEditorAsync();
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

  private async Task<DataTable?> DBSearch() {
    if (QueryService == null)
      return null;

    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_DAYSTMCOMDAY_SELECT01", new Dictionary<string, object?>()
        {
                  {"DATE", DATE?.EditValue },
                  {"COM_ID", COM_ID.ToStringTrim()},
              });

    return datatable;
  }

  #endregion [ 데이터 정의 메소드 ]
}
