     
using DevExpress.Blazor;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
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
* 작성일자 : 25-02-05
* 최종수정 : 25-02-18
* 프로시저 : P_HMI_EVENT_SELECT02
*/
namespace MIT.UI.UMS;
public partial class UH2220QA1Base : CommonUIComponentBase {
  protected CommonDateEdit? SDATE { get; set; }
  protected CommonTimeEdit? STIME { get; set; }
  protected CommonDateEdit? EDATE { get; set; }
  protected CommonTimeEdit? ETIME { get; set; }

  protected string? SDTTM { get; set; }
  protected string? EDTTM { get; set; }


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
    if (Grd1 == null || Grd1.Grid == null)      return;

    //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
    Grd1.SetCellStyle("COM_NAME", "합 계");

    Grd1.DataCellCss.Add("SST_PRI", "grid-col-left-line");
    Grd1.DataCellCss.Add("SDWR", "grid-col-left-line");
    Grd1.DataCellCss.Add("COM_NAME1", "grid-col-left-line");
    Grd1.DataCellCss.Add("SPW_PW1_B", "grid-col-left-line");
    Grd1.DataCellCss.Add("SST_PRI_B", "grid-col-left-line");
    Grd1.DataCellCss.Add("SDWR_B", "grid-col-left-line");
    Grd1.DataCellCss.Add("SPW_PW1_CHA", "grid-col-left-line");
    Grd1.DataCellCss.Add("SST_PRI_CHA", "grid-col-left-line");
    Grd1.DataCellCss.Add("SDWR_CHA", "grid-col-left-line");
    Grd1.DataCellCss.Add("REMARK", "grid-col-left-line");
    Grd1.DataCellCss.Add("SPW_PW1", "grid-col-left-line");


    


  }

  #endregion [ 컨트롤 초기 세팅 ]
}

