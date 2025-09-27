
using DevExpress.Blazor;
using DevExpress.Drawing.Internal.Fonts;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.Grid;
using Newtonsoft.Json.Linq;
using System.Data;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-05
* 최종수정 : 25-02-14
* 화면명 : 실적외 데이터 관리
* 프로시저 : P_EXCL_RST_SELECT, P_EXCEL_RST_SAVE, P_EXCL_RST_DEL
*/
namespace MIT.UI.UMS;
  public partial class UH5120QA1_POPBase : CommonPopupComponentBase {
    protected CommonDateEdit? SDATE { get; set; }
    protected CommonDateEdit? EDATE { get; set; }
    protected MitCombo MC_ID { get; set; }



    public string CmId { get; set; }

    public string CmName { get; set; }






    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string McText { get; set; }
    public string ComName { get; set; }




    public bool IsSideView
    {
        get
        {
            return (this.RecieveParameter == null);
        }
    }

    protected CommonGrid? Grd1 { get; set; }



    public bool isLoading { get; set; } = false;

    protected DataTable FDR_DATA { get; set; } = new DataTable();
    protected DataTable COM_DATA { get; set; } = new DataTable();
    protected override void OnInitialized() {

    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender) {
        if (!await IsAuthenticatedCheck())          return;

            FDR_DATA = await GetCommonCode("FDR");
            COM_DATA = await GetCommonCode("COMPANY");
            await InitControls();

        //await btn_Search();
      }
    }


    private async Task InitControls() {
      if (Grd1 == null || Grd1.Grid == null) return;

      //SDATE.EditValue = "2025-01-01";
      //MC_ID.Value = "AA";
      //MC_ID.Text = "adfaf";




      await Task.Run(async () => {
          if (this.RecieveParameter != null)
          {
              PerformExcludeDataCode pedc = this.RecieveParameter as PerformExcludeDataCode;
              SDATE.EditValue = pedc.StartDate.ToString("yyyy-MM-dd");
              EDATE.EditValue = pedc.EndDate.ToString("yyyy-MM-dd");
              CmId = pedc.FdrId;



              // 상단 표기 처리..


              StartDate = pedc.StartDate.ToString("yyyy-MM-dd");
              EndDate = pedc.EndDate.ToString("yyyy-MM-dd");

              DataRow mcRow = FDR_DATA.AsEnumerable().FirstOrDefault(r => r.Field<string>("CM_ID") == pedc.FdrId);

              McText = mcRow?.Field<string>("CM_NAME") ?? string.Empty;

              if (mcRow != null)
              {
                  var comRow = COM_DATA.AsEnumerable().FirstOrDefault(r => r.Field<string>("CM_ID") == mcRow.Field<string>("CM_DESC"));
                  ComName = comRow?.Field<string>("CM_NAME") ?? string.Empty;
              }
              else
              {
                  ComName = string.Empty;
              }

              StateHasChanged();

              await Search();
          }
      });
    





      StateHasChanged();
      //Grd1.SetNeedColumns(new string[]{"KEY_COLUMN" });
    }

}

public class PerformExcludeDataCode
{
    public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-1.0);
    public DateTime EndDate { get; set; } = DateTime.Now;
    public string FdrId { get; set; } = "";
    //public string FdrName { get; set; } = "";
    //public string ComName { get; set; } = "";
}