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
    
public class UM5100QA1_POPBase : CommonPopupComponentBase
{
  protected string YYMM { get; set; } = string.Empty;
  protected string KIND_ID { get; set; } = string.Empty; // 1: 중간검침, 2: 정기검침
    protected string KIND_TEXT { get; set; } = string.Empty; // "중간검침", "정기검침"
    protected string CSTINTCDE { get; set; } = string.Empty; // 수용가 코드(View에서 선택된 값)
  protected string COM_ID { get; set; } = string.Empty;
  protected string Confirmer { get; set; } = string.Empty; // 확인자명


  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {
      if (!await IsAuthenticatedCheck())
        return;

      await InitControls();

      //await btn_Search();
    }
  }

  #region [ 컨트롤 초기 세팅 ]

  private async Task InitControls() {
        await Task.Run(async () =>
        {
            if (this.RecieveParameter != null)
            {
                //Console.WriteLine($"ReeceiveParameter is not null, initializing... : {this.RecieveParameter is CheckListConfirmParameter}");
                CheckListConfirmParameter parameter = this.RecieveParameter as CheckListConfirmParameter;
                if (parameter != null)
                {
                    YYMM = parameter.YYMM;
                    KIND_ID = parameter.KIND;
                    KIND_TEXT = (KIND_ID == "1") ? "중간검침" : "정기검침";
                    CSTINTCDE = parameter.CSTINTCDE;
                    COM_ID = parameter.COM_ID;
                }
                //Console.WriteLine($"Initialized with parameters: YYMM={YYMM}, KIND_ID={KIND_ID}, CSTINTCDE={CSTINTCDE}, COM_ID={COM_ID}");
                StateHasChanged();
            }
        });
    }

  #endregion [ 컨트롤 초기 세팅 ]

  #region [ 공통 버튼 기능 ]


  #endregion [ 공통 버튼 기능 ]

  #region [ 사용자 버튼 기능 ]

  protected async Task btn_Save() {
    MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
  }
    protected async Task SaveCallback(CommonMsgResult result)
    {
        if (result != CommonMsgResult.Yes)
            return;

        if (await Save())
        {
            MessageBoxService?.Show("저장하였습니다.");
        }
        else
        {
            MessageBoxService?.Show("저장에 실패하였습니다.");
        }
    }



    #endregion [ 사용자 버튼 기능 ]

    #region [ 사용자 이벤트 함수 ]

    #endregion [ 사용자 이벤트 함수 ]

    #region [ 사용자 정의 메소드
    private async Task<bool> Save()
    {
        try
        {
            ShowLoadingPanel();

            await DBSave();

            return true;
        }
        catch (Exception ex)
        {
            CloseLoadingPanel();
            MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            return false;
        }
        finally
        {
            CloseLoadingPanel();
        }
    }

    #endregion [ 사용자 정의 메소드 ]

    #region [ 데이터 정의 메소드 ]

    /*private async Task<DataTable?> DBSearch() {
      if (QueryService == null)      return null;

      var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_CHECK_LIST_RST_SELECT", new Dictionary<string, object?>()        {
                      {"YYYY", YYMM.EditValue.Substring(0,4) },
                      {"MM", YYMM.EditValue.Substring(5, 2) },
                      {"KIND", MeterReadingId.ToStringTrim() },
                      {"COM_ID", CSTINTCDE.Desc.ToStringTrim() },
                      {"CSTINTCDE", CSTINTCDE.Name.ToStringTrim() },
                  });

      return datatable;
    }*/


    private async Task DBSave() {
    if (QueryService == null)
      return;

    await QueryService.ExecuteNonQuery("P_HMI_CHECKLIST_SAVE", new Dictionary<string, object?>()    {
                { "YYMM", YYMM },
                { "KIND", KIND_ID },
                { "CSTINTCDE", CSTINTCDE },
                { "COM_ID", COM_ID },
                { "CUST_NAME", Confirmer },
    });
  }

  #endregion [ 데이터 정의 메소드 ]
}

// 팝업에 넘길 파라미터 클래스
public class CheckListConfirmParameter
{
    public string YYMM { get; set; } = string.Empty;
    public string KIND { get; set; } = string.Empty;      // 1: 중간검침, 2: 정기검침
    public string CSTINTCDE { get; set; } = string.Empty; // 수용가 코드(View에서 선택된 값)
    public string COM_ID { get; set; } = string.Empty;    // 수용가 ID(HMI에서 선택된 값)
}