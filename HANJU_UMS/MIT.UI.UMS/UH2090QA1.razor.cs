
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
* 작성자명 : jskim
* 작성일자 : 25-05-22
* 최종수정 : 25-05-22
* 프로시저 : 
*/
namespace MIT.UI.UMS {
  public partial class UH2090QA1Base : CommonUIComponentBase {

    protected CommonDateEdit? P_SDATE { get; set; }
    protected CommonDateEdit? P_EDATE { get; set; }

    protected MitCombo P_COM_ID { get; set; }
    protected MitCombo? P_MC_ID { get; set; }


    protected string? P_WA_GRP { get; set; } = "ALL_HOUR_W";
    protected string searched_grp { get; set; } = "ALL_HOUR_W";


    public string P_COM_VAL { get; set; }


    public int gapval { get; set; } = 4;
    public int sval { get; set; } = 1;
    public int eval { get; set; } = 5;
    public int mc_num { get; set; } = 1;

    public string[] ExcFieldName = Array.Empty<string>();



    protected CommonGrid? Grd1 { get; set; }
    //protected DataTable ChartDt1 { get; set; } = new DataTable();

    protected override void OnInitialized() {

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

    // 컨트롤 초기 세팅 ]

    private void InitControls() {
      
      //test전 visible 처리 되지 않는 경우 아래 테스트 후 제외할 컬럼명 추가 필
      //var k = 0;
      //for(int i = 1; i <= 30; i++) {
      //  ExcFieldName[k++] = ReVal("USEQTY_", i);  //평균
      //  ExcFieldName[k++] = ReVal("I_USEQTY_", i);  //사용량
      //  ExcFieldName[k++] = ReVal("MQ_TON_", i);  //정산량
      //  ExcFieldName[k++] = ReVal("AC_TON_", i);  //적산
      //}

      if (Grd1 == null || Grd1.Grid == null) return;
      Grd1.SetCellStyle("GAUGE_DATE","집계");
      //P_SDATE.EditValue = "2020-01-01";
      //P_EDATE.EditValue = "2020-01-01";


      Task.Run(async () => {

        InvokeAsync(StateHasChanged);

        await Task.Delay(500);

          //eval = sval + gapval;
          P_WA_GRP = "ALL_HOUR_W";
        //P_COM_VAL = "GLB";

        InvokeAsync(StateHasChanged);
        await Task.Delay(10);
        await btn_Search();

      });




    }

    // 컨트롤 초기 세팅 ]
  }
}