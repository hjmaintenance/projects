    
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
using Microsoft.Extensions.Primitives;
using MIT.ServiceModel;
/*
* 작성자명 : quristyle
* 작성일자 : 25-03-14
* 최종수정 : 25-03-14
* 프로시저 : P_HMI_TREND_SELECT
*/
namespace MIT.UI.UMS;

    public partial class UH1070QA1Base : CommonPopupComponentBase {

  // 공통 버튼 기능 

  protected override async Task Btn_Common_Search_Click() {
    await btn_Search();
  }





  public List< DataPoints> ChartData = new List<DataPoints>();


  protected async Task btn_Search() {
    await Search();
  }



  private async Task Search() {


    string mc_ids = "";
    foreach (var v in Values) {
      if (!string.IsNullOrEmpty(mc_ids)) {
        mc_ids += ",";
      }
      mc_ids += v.Key;

    }

    if (string.IsNullOrEmpty(GAUGE_TYPE_Code)) { MessageBoxService.Show("need GAUGE_TYPE_Code"); return; }
    if (string.IsNullOrEmpty(SelComp)) { MessageBoxService.Show("need SelComp"); return; }
    if (string.IsNullOrEmpty(mc_ids)) { MessageBoxService.Show("need mc_ids"); return; }
    if (string.IsNullOrEmpty(PreferredLanguage)) { MessageBoxService.Show("need PreferredLanguage"); return; }



    await Search2();

    return;







    try {
      ShowLoadingPanel();


      //EXEC  P_HMI_TREND_SELECT 'PW', '2025-03-02', 'ka_AA', 'AA' 

      var dt = await DBSearch();

      ChartData.Clear();
      //_chart.RefreshData();
      DataPoints dp = null;
      foreach (DataRow dr in dt.Rows) {
        bool isExist = false;
        foreach (var cd in ChartData) {
          if (cd.RName == dr["MC_ID"].ToStringTrim()) { // 같은 이름이 있으면
            dp = cd;
            isExist = true;
            break;
          }
        }

        if (!isExist) { // 같은 이름이 없으면
          dp = new DataPoints() { RName = dr["MC_ID"].ToStringTrim() };
          ChartData.Add(dp);
        }

        dp.DPoints.Add(new DPoint() { Xtime = DateTime.ParseExact(dr["GAUGE_DATE"] + "" + dr["GAUGE_MIN"], "yyyy-MM-ddHH:mm:ss", null) 
          , Qty1 = double.Parse( dr["R_USEQTY"].ToStringTrim())
          , Qty2 = double.Parse(dr["R_USEQTY2"].ToStringTrim())

        });

      }



      StateHasChanged();

    }
    catch (Exception ex) {
      CloseLoadingPanel();

      MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
    }
    finally {
      CloseLoadingPanel();
    }
  }



  public double StartValue = 0;
  public double EndValue = 99999;
  bool isStart = false;

  public async Task Search2() {

    if (isStart) return;
    isStart = true;

    string mc_ids = "";
    if (Values != null) {
      foreach (var v in Values) {
        if (!string.IsNullOrEmpty(mc_ids)) {
          mc_ids += ",";
        }
        mc_ids += v.Key;

      }
    }

    if (string.IsNullOrEmpty(GAUGE_TYPE_Code)
      || string.IsNullOrEmpty(SelComp)
      || string.IsNullOrEmpty(mc_ids)
      || string.IsNullOrEmpty(PreferredLanguage)) {

      StartValue = 0;
      EndValue = 99999;
      isStart = false;
      ChartData.Clear();
      _chart.RefreshData();

      ToastService.ShowToast(new ToastOptions {
        ProviderName = "Positioning",
        Title = "알림",
        Text = "필수항목 선택 필요."
      });
      isStart = false;
      return; }

    //"실사용량(KW)", "일사용량(KW)", "월사용량(KW)", "당월PEAK(KW)", "당월역율(%)"

    string frist_time = "GAUGE_DATE";
    string secd_time = "GAUGE_MIN";
    string qty1 = "R_USEQTY";
    string qty2 = "R_USEQTY2";
    string format = "yyyy-MM-ddHH:mm:ss";

    if (PreferredLanguage == "실사용량(KW)") {
      frist_time = "GAUGE_DATE";
      secd_time = "GAUGE_MIN";
       qty1 = "R_USEQTY";
       qty2 = "R_USEQTY2";
      format = "yyyy-MM-ddHH:mm:ss";
    }
    else if (PreferredLanguage == "일사용량(KW)") {
      frist_time = "GAUGE_DATE";
      secd_time = "GAUGE_HOUR";
      qty1 = "H_USEQTY";
      qty2 = "H_USEQTY2";
      format = "yyyy-MM-ddHH:mm";
    }

    else if (PreferredLanguage == "월사용량(KW)"
      ) {
      frist_time = "GAUGE_YEAR";
      secd_time = "GAUGE_YYMM";
      qty1 = "M_USEQTY";
      qty2 = "M_USEQTY2";
      format = "yyyy-MM";
    }


    else if ( PreferredLanguage == "당월PEAK(KW)"
      ) {
      frist_time = "GAUGE_YEAR";
      secd_time = "GAUGE_YYMM";
      qty1 = "M_PEAK";
      qty2 = "M_PEAK2";
      format = "yyyy-MM";
    }

    else if ( PreferredLanguage == "당월역율(%)"
      ) {
      frist_time = "GAUGE_YEAR";
      secd_time = "GAUGE_YYMM";
      qty1 = "M_INTV";
      qty2 = "M_INTV2";
      format = "yyyy-MM";
    }






    try {
      ShowLoadingPanel();

      var dt = await DBSearch();

      ChartData.Clear();
      //_chart.RefreshData();
      DataPoints dp = null;
      double minValue = 99999;
      double maxValue = 0;
      foreach (DataRow dr in dt.Rows) {
        bool isExist = false;
        foreach (var cd in ChartData) {
          if (cd.RName == dr["MC_ID"].ToStringTrim()) { // 같은 이름이 있으면
            dp = cd;
            isExist = true;
            break;
          }
        }

        if (!isExist) { // 같은 이름이 없으면
          dp = new DataPoints() { RName = dr["MC_ID"].ToStringTrim() };
          ChartData.Add(dp);
        }

        if (minValue > double.Parse("0" + dr[qty1].ToStringTrim())) {
          minValue = double.Parse("0" + dr[qty1].ToStringTrim());
        }


        if (maxValue < double.Parse("0" + dr[qty1].ToStringTrim())) {
          maxValue = double.Parse("0" + dr[qty1].ToStringTrim());
        }

        dp.DPoints.Add(new DPoint() {
          Xtime = DateTime.ParseExact( ( (frist_time== "GAUGE_YEAR") ?"":( dr[frist_time]   + "") ) + dr[secd_time], format, null)
          ,
          Qty1 = double.Parse("0"+ dr[qty1].ToStringTrim())
          ,
          Qty2 = double.Parse("0" + dr[qty2].ToStringTrim())

        });

      }

      if (minValue < 99999) {
        StartValue = minValue-100;
      }
      if (maxValue > 0) {
        EndValue = maxValue+100;
      }

      StateHasChanged();

    }
    catch (Exception ex) {
      CloseLoadingPanel();

      MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
    }
    finally {
      isStart = false;
      CloseLoadingPanel();
    }
    isStart = false;
  }




  // 사용자 정의 메소드 end

  // 데이터 정의 메소드 

  private async Task<DataTable?> DBSearch() {
    if (QueryService == null)       return null;

    string test = "";
    string mc_ids = "";
    foreach (var v in Values) {
      if (!string.IsNullOrEmpty(mc_ids)) {
        mc_ids += ",";
        test = v.Key;
      }
      mc_ids  +=      v.Key;

    }

    //P_HMI_TREND_SELECT 'PW', '2025-03-02', 'ka_AA', 'AA'


    var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_TREND_CHOICE", new Dictionary<string, object?>()        {
                    //{"P_UTILITY_TYP", P_UTILITY_TYP.ToStringTrim() },
                    //{"P_DATE", P_DATE.ToStringTrim() },
                    //{"P_COM_ID", P_COM_ID.ToStringTrim() },
                    //{"P_MC_ID", P_MC_ID.ToStringTrim() },
                    //{"P_GUBUN", "실시간" }
                    {"UTILITY_TYP", GAUGE_TYPE_Code },
                    {"DATE", CompareDate?.EditValue.ToStringTrim() },
                    {"COM_ID", SelComp },
                    {"MC_ID", mc_ids },
                    {"GUBUN", PreferredLanguage }
                }, "P_");

    //ChartDt1 = datatable;
    return datatable;
  }


  // 데이터 정의 메소드 end
}
