
using DevExpress.Drawing.Internal.Fonts;
using Microsoft.AspNetCore.Authorization;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using System.Data;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-05
* 최종수정 : 25-02-14
* 화면명 : 실적외 데이터 관리
* 프로시저 : P_EXCL_RST_SELECT, P_EXCEL_RST_SAVE, P_EXCL_RST_DEL
*/
namespace MIT.UI.UMS {
  public partial class UH5120QA1Base : CommonPopupComponentBase {
    protected CommonDateEdit? SDATE { get; set; }
    protected CommonDateEdit? EDATE { get; set; }
    protected MitCombo MC_ID { get; set; }



    public string CmId { get; set; }

    public string CmName { get; set; }




    protected CommonGrid? Grd1 { get; set; }

    protected override void OnInitialized() {

    }



        protected DataTable FDR_DATA { get; set; } = new DataTable();

        protected override async Task OnAfterRenderAsync(bool firstRender) {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender) {
        if (!await IsAuthenticatedCheck())          return;

        await InitControls();


                FDR_DATA =   await GetCommonCode("FDR");


        await btn_Search();
      }
    }


    private async Task InitControls() {
      if (Grd1 == null || Grd1.Grid == null) return;

      SDATE.EditValue = "2025-01-01";
      //MC_ID.Value = "AA";
      //MC_ID.Text = "adfaf";




      await Task.Run(async () => {

        bool isOk = false;
        for (int i = 0; i < 10; i++) {

          await Task.Delay(1000);
          try {


            CommCode cc = await MC_ID.GetFirstRowSelect();

            CmId = cc.Name;
            CmName = cc.Value;
            isOk = true;

          } catch(Exception ee) {

          }

          if (isOk) {
            break;
          }

        }
      });
    





      StateHasChanged();
      //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
    }

  }
}
