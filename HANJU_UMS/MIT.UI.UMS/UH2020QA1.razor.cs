     
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
using DevExpress.ClipboardSource.SpreadsheetML;
using System.ServiceModel.Channels;
using MIT.Razor.Pages;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-19
* 최종수정 : 25-02-19
* 프로시저 : P_HMI_ELEC_ACTUAL_VAL_ALL
*/
namespace MIT.UI.UMS {
  public partial class UH2020QA1Base : CommonUIComponentBase {
    protected CommonDateEdit? P_SDATE { get; set; }
    protected CommonDateEdit? P_EDATE { get; set; }

    protected MitCombo P_COM_ID { get; set; }
    protected MitCombo? P_MC_ID { get; set; }


    protected CommCode P_ST_GRP { get; set; }


    public string P_COM_VAL { get; set; }

    public int gapval { get; set; } = 2;
    public int sval { get; set; } = 1;
    public int eval { get; set; } = 3;
    public int mc_num { get; set; } = 0;

        protected CommonGrid? Grd1 { get; set; }
    protected string[] BandCaption { get; set; } = Enumerable.Repeat<string>("", 50).ToArray();
        protected DataTable ChartDt1 { get; set; } = new DataTable();



    public DateTime Date { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender) {
        if (!await IsAuthenticatedCheck())          return;

        InitControls();



        //await btn_Search();
      }
    }

    // 컨트롤 초기 세팅 

    private void InitControls() {
      if (Grd1 == null || Grd1.Grid == null) return;

      //P_SDATE.EditValue = "2020-01-01";
      // P_EDATE.EditValue = "2020-01-01";


      Task.Run(async () => {

          InvokeAsync(StateHasChanged);

        await Task.Delay(500);

        eval = sval + gapval;
        //P_ST_GRP = "ALL_TIME";
        P_COM_VAL = "GLB";

        InvokeAsync(StateHasChanged);
        await Task.Delay(10);
        await btn_Search();

      });


      //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
    }




    // 컨트롤 초기 세팅 end

  }
}
