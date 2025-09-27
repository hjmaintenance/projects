     
using DevExpress.Blazor;
using DevExpress.Data.Helpers;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-05
* 최종수정 : 25-02-05
* 프로시저 : P_HMI_LOG_ELEC
*/
namespace MIT.UI.UMS {
  public partial class UH2010QA1Base : CommonUIComponentBase {
    protected CommonDateEdit P_DATE { get; set; }
    protected MitCombo P_GAUGE_TYPE { get; set; }
    protected string subtitle { get; set; }

    protected CommonGrid? Grd1 { get; set; }

    protected override void OnInitialized() {

    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender) {
        if (!await IsAuthenticatedCheck())           return;

        await InitControls();

        //await btn_Search();
      }
    }


    protected string? P_GAUGE_Val { get; set; }
    protected string? P_GAUGE_Text { get; set; }

    private async Task InitControls() {
      if (Grd1 == null || Grd1.Grid == null)         return;

    }

  }
}
