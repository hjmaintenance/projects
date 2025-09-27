     
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
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component.Grid.Data;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-26
* 최종수정 : 25-02-26
* 프로시저 : P_HMI_COMMUNITY_BOARDINFO_SELECT01
*/
namespace MIT.UI.UMS {
  public partial class Z9010MA1Base : CommonUIComponentBase {
    protected string? BOARD_ID { get; set; }
    protected string? BOARD_NUM { get; set; }
    protected string? THREAD_NUM { get; set; }
    protected string? REF_LEVEL { get; set; }
    protected string? USER_ID { get; set; }
    protected string? USER_NAME { get; set; }
    protected string? BOARD_TITLE { get; set; }
    protected string? BOARD_CONTENT { get; set; }
    protected string? FILE_SIZE { get; set; }
    protected string? FILE_NAME { get; set; }
    protected string? HTML_YN { get; set; }
    protected string? USER_EMAIL { get; set; }
    protected string? READ_NUM { get; set; }
    protected string? DATE_WRITE { get; set; }
    protected string? PWD { get; set; }
    protected string? IP { get; set; }


    protected CommonGrid? Grd1 { get; set; }
    protected DataTable ChartDt1 { get; set; } = new DataTable();

    protected override void OnInitialized() {

    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender) {
        if (!await IsAuthenticatedCheck())          return;

        InitControls();

        await btn_Search();
      }
    }

    // 컨트롤 초기 세팅 

    private void InitControls() {
      if (Grd1 == null || Grd1.Grid == null)        return;

    }

    // 컨트롤 초기 세팅 end


    protected void NewRowEventCall(CommonGridInitNewRowEventArgs e) {
      ShowPop(e.Row);
    }

    protected void DBLRowEventCall(GridRowClickEventArgs e) {
      DataRow dr = Grd1.GetFocusedRowValue();
      ShowPop(dr);
    }




    void ShowPop(DataRow sel_dr) {
      CommonPopupService.Show(typeof(Z9010MA1_POP),
        "공지 관리",
        Width: 1080,
        Height: 750,
        CloseCallback: OnCloseCallback,
        Parameter: sel_dr
      );
    }

    protected async Task OnCloseCallback(PopupResult result) {
      if (result.PopupResultType != PopupResultType.OK) return;

      await Task.Delay(1);
      await btn_Search();

    }


  }
}