/*
* 작성자명 : 김지수
* 작성일자 : 25-03-14
* 최종수정 : 25-03-14
* 프로시저 : P_HMI_CHECK_LIST_RST_SELECT
*/
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MIT.DataUtil.Common;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.ServiceModel;
using Newtonsoft.Json;
using System.Data;

namespace MIT.UI.UMS;
    
public class UM5100QA1Base : CommonPopupComponentBase
{


  [Inject] public IJSRuntime JS { get; set; }

  protected CommonDateEdit? YYMM { get; set; }
  protected string? MeterReadingId { get; set; } = "1";
  protected string? MeterReadingText { get; set; } = "중간검침";




  // protected string? COM_ID { get; set; } 
  protected CommCode CSTINTCDE { get; set; }

  protected override async Task OnInitializedAsync() {
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

  protected override async Task Btn_Common_Print_Click() {
    await Print();
  }

  #endregion [ 공통 버튼 기능 ]

  #region [ 사용자 버튼 기능 ]

  protected async Task btn_Search() {
    await Search();
  }


  #endregion [ 사용자 버튼 기능 ]

  #region [ 사용자 이벤트 함수 ]

  protected void OnInputSearchParameter(Dictionary<string, object?> parameters) {
    //parameters.Add("P_YYYY", P_YYYY.ToStringTrim());
  }

  #endregion [ 사용자 이벤트 함수 ]

  #region [ 사용자 정의 메소드 ]


   async Task<string> makeRpt( ) {

    var datasource = await DBSearch();

    RptInfo ri = new RptInfo() { Columns = null, Data = datasource };

    DateTime adt = DateTime.Parse(YYMM.EditValue);

    ri.Title = adt.ToString("MM") + "월 유틸리티 " + MeterReadingText + "표";
    if (MeterReadingId == "1") {
      ri.SubTitle = adt.ToString("yyyy-MM") + "-01 ~ " + adt.ToString("yyyy-MM") + "-13";
    }
    else {
      ri.SubTitle = adt.ToString("yyyy-MM") + "-01 ~ " + DateTime.ParseExact(adt.ToString("yyyy-MM") + "-01", "yyyy-MM-dd", null).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
    }
    ri.UName = "test U";
    ri.FileName = "test_" + DateTime.Now.Ticks.ToString();
    ri.Landscape = "Y";
    ri.Date1 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    ri.ExportType = "pdf";

    var serialize = JsonConvert.SerializeObject(ri);
    return serialize;
  }

  private async Task Print() {
    try {
      ShowLoadingPanel();
      var serialize = await makeRpt();
      await JSRuntime.InvokeVoidAsync("printFromStream", "testfile", "documentviewer2", serialize);
    }
    catch (Exception ex) {
      CloseLoadingPanel();
      MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
    }
    finally {
      CloseLoadingPanel();
    }
  }
  
    
    
    private async Task Search() {

    if (string.IsNullOrEmpty(CSTINTCDE.Value)) {

      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "알림",
        Text = "수용가를 선택해 주세요."
      });

      return;
    }

    try {
      ShowLoadingPanel();
      var serialize = await makeRpt();
      await JSRuntime.InvokeVoidAsync("printViewFromStream", "testfile", serialize);
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
    if (QueryService == null)      return null;

    var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_CHECK_LIST_RST_SELECT", new Dictionary<string, object?>()        {
                    {"YYYY", YYMM.EditValue.Substring(0,4) },
                    {"MM", YYMM.EditValue.Substring(5, 2) },
                    {"KIND", MeterReadingId.ToStringTrim() },
                    {"COM_ID", CSTINTCDE.Desc.ToStringTrim() },
                    {"CSTINTCDE", CSTINTCDE.Name.ToStringTrim() },
                });

    return datatable;
  }


  private async Task DBSave(DataTable datatable) {
    if (QueryService == null)
      return;

    await QueryService.ExecuteNonQuery("P_HMI_CHECKLIST01_SAVE", datatable, new Dictionary<string, object?>()    {
                { "REG_ID", USER_ID },
            });
  }

  #endregion [ 데이터 정의 메소드 ]
}