     
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
* 작성일자 : 25-02-18
* 최종수정 : 25-02-18
* 프로시저 : P_POWER_FAC_BASE_SELECT
*/
namespace MIT.UI.UMS {
  public partial class B9010MA1Base : CommonUIComponentBase {
    protected string? P_COM_ID { get; set; }
    protected string? COM_ID { get; set; }
    protected string? COM_NAME { get; set; }
    protected string? START_DT { get; set; }
    protected string? PF_BASE_VAL { get; set; }
    protected string? END_DT { get; set; }


    protected CommonGrid? Grd1 { get; set; }
    protected CommonGrid? Grd2 { get; set; }
    protected DataTable ChartDt1 { get; set; } = new DataTable();

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

    // 컨트롤 초기 세팅 

    private void InitControls() {
      if (Grd1 == null || Grd1.Grid == null)
        return;

      //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
    }

    // 컨트롤 초기 세팅 end
  }
}