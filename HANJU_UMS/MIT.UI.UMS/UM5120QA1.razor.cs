     
using DevExpress.Blazor;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Charts;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.Razor.Pages.Component.DataEdits;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-26
* 최종수정 : 25-02-26MIT.UI.UMS
* 프로시저 : P_HMI_MC_INSP_RST_SELECT
*/
namespace MIT.UI.UMS;
public partial class UM5120QA1Base : CommonUIComponentBase {

  [Parameter] public string PreferredLanguage { get; set; } = "활성화";
  protected CommonDateEdit SendingMailDate { get; set; }
  protected IEnumerable<string> Languages = new[] { "활성화", "비활성화" };

  protected MitCombo   SD_MAIL_T { get; set; }


  protected override void OnInitialized() { }

  protected override async Task OnAfterRenderAsync(bool firstRender) {
    await base.OnAfterRenderAsync(firstRender);

    if (firstRender) {
      if (!await IsAuthenticatedCheck()) return;
      InitControls();
      await LoadData();
    }
  }

  private void InitControls() {
  }

  public override async Task SetParametersAsync(ParameterView parameters) {
    await base.SetParametersAsync(parameters);
  }

  //public DxRadioGroup LanuageObj { get; set; }



  async Task LoadData() {
    if (QueryService == null) return;
    var dt = await QueryService.ExecuteDatatableAsync("p_change_menu_activation", new Dictionary<string, object?>() { { "call_tp", "load" }, { "pgm_id", "UM5100QA1" } });
    if (dt != null && dt.Rows.Count > 0) {
      PreferredLanguage = dt.Rows[0][0].ToString();

      StateHasChanged();
      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "알림",
        Text = "현재 "+dt.Rows[0][0].ToString()+" 상태 입니다."
      });


      await Task.Run(async () => {
        Task.Delay(300);
        PreferredLanguage = dt.Rows[0][0].ToString();
      });




    }

    StateHasChanged();
  }


  protected async Task SaveData(MouseEventArgs args) {
    if (QueryService == null) return;


    if (string.IsNullOrEmpty(PreferredLanguage)) {
      MessageBoxService?.Show("메뉴 활성 여부를 선택 하세요.");
      return;
    }


    var dt = await QueryService.ExecuteDatatableAsync("p_change_menu_activation", new Dictionary<string, object?>() { 
      { "pl", PreferredLanguage }
    , { "pgm_id", "UM5100QA1" }
    , { "call_tp", "save" }
    });


    if (dt != null && dt.Rows.Count > 0) {
      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "알림",
        Text = dt.Rows[0][0].ToString()
      });
    }

  }

  protected async Task SendMail(MouseEventArgs args) {
    if (QueryService == null) return;

    if (string.IsNullOrEmpty(SD_MAIL_T.Value)) {
      MessageBoxService?.Show("검침표 종류를 선택하세요.");
      return;
    }

    MessageBoxService?.Show("알림 발송을 하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
  }


  protected async Task SaveCallback(CommonMsgResult result) {
    if (result != CommonMsgResult.Yes) return;
    var dt = await QueryService.ExecuteDatatableAsync("p_change_menu_activation", new Dictionary<string, object?>() {
      { "pgm_id", "UM5100QA1" }
    , { "call_tp", "sendmail" }
    , { "sd_mail_t", SD_MAIL_T.Text }
    , { "date", SendingMailDate.EditValue?.Substring(0,7) } 
    });
    if (dt != null && dt.Rows.Count > 0) {
      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "알림",
        Text = dt.Rows[0][0].ToString()
      });
    }

  }


}