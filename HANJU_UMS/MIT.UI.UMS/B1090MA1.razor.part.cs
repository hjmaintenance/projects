    
using DevExpress.Blazor;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Charts;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
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
* 최종수정 : 25-02-26
* 프로시저 : P_HMI_MC_INSP_RST_SELECT
*/
namespace MIT.UI.UMS {
  public partial class B1090MA1Base : CommonUIComponentBase {

    protected override async Task Btn_Common_Save_Click() {
      await btn_Save();
    }


    protected async Task btn_Save() {

      MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
    }

    protected async Task SaveCallback(CommonMsgResult result) {
      if (result != CommonMsgResult.Yes) return;

      if (await Save()) {
        MessageBoxService?.Show("저장하였습니다.");
        await Search();
      }
    }

    bool isRrrr = true;
    private async Task Search() {
      try {
        ShowLoadingPanel();

        var dt = await DBSearch();

        if (dt != null) {
          // 가져온거 뿌리자 quri
          Mail_Addr = dt.Rows[0]["MAIL_ADDR"].ToString();
          Mail_Name = dt.Rows[0]["MAIL_NAME"].ToString();
        }
      }
      catch (Exception ex) {
        CloseLoadingPanel();

        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
      }
      finally {
        if(isRrrr && string.IsNullOrEmpty(Mail_Addr)) {
          Thread.Sleep(3000);
          isRrrr = false;
          await Search();
        }
        CloseLoadingPanel();
        
      }

      StateHasChanged();

    }

    private async Task<bool> Save() {
      try {
        ShowLoadingPanel();

        await DBSave();

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


    private async Task<DataTable?> DBSearch() {
      if (QueryService == null) return null;

      var datatable = await QueryService.ExecuteDatatableAsync("P_SYS_PROP_SELECT", new Dictionary<string, object?>()                {
                    {"Mail_Addr", Mail_Addr.ToStringTrim() },
                    {"Mail_Name", Mail_Name.ToStringTrim() },
                });

      return datatable;
    }

    private async Task DBSave() {
      if (QueryService == null) return;


      await QueryService.ExecuteDatatableAsync("P_SYS_PROP_SAVE", new Dictionary<string, object?>()                {
                    {"KEY", "MAIL_ADDR" },
                    {"VAL", Mail_Addr.ToStringTrim() },
                });
      await QueryService.ExecuteDatatableAsync("P_SYS_PROP_SAVE", new Dictionary<string, object?>()                {
                    {"KEY",  "MAIL_NAME" },
                    {"VAL", Mail_Name.ToStringTrim() },
                });

    }

  }
}