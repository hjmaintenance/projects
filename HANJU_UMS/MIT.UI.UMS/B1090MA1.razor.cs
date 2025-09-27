     
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
* 최종수정 : 25-02-26
* 프로시저 : P_HMI_MC_INSP_RST_SELECT
*/
namespace MIT.UI.UMS {
  public partial class B1090MA1Base : CommonUIComponentBase {
    protected string? Mail_Addr { get; set; }
    protected string? Mail_Name { get; set; }

    protected override void OnInitialized() {

    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender) {
        if (!await IsAuthenticatedCheck())          return;

        InitControls();

        await Search();
      }
    }

    // 컨트롤 초기 세팅 

    private void InitControls() {
      //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
    }

    // 컨트롤 초기 세팅 end
  }
}