    
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
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-19
* 최종수정 : 25-02-19
* 프로시저 : P_HMI_ELEC_ACTUAL_VAL_ALL
*/
namespace MIT.UI.UMS {
  public partial class UH2020QA1Base : CommonUIComponentBase {

    // 공통 버튼 기능 

    protected override async Task Btn_Common_Search_Click() {
      await btn_Search();
    }


    // 공통 버튼 기능 end

    // 사용자 버튼 기능 ]

    protected async Task btn_Search() {



      await Search();
    }


    // 사용자 버튼 기능 end

    // 사용자 이벤트 함수 

    // 사용자 이벤트 함수 end

    // 사용자 정의 메소드 

    private async Task Search() {
      if (Grd1 == null)        return;

      try {
        ShowLoadingPanel();

        var datasource = await DBSearch();
        if(datasource.Rows.Count > 0)
        {
            int i = 1;
            for (; i <= 50; i++)
            {
                // 모든 행을 순회해서 i번째 MC_ID 추출, 없으면 null
                var mc_id = datasource.AsEnumerable().Select(row => row[ReVal("MC_ID_", i)] +"")
                                                     .FirstOrDefault(val => !string.IsNullOrEmpty(val));
                // 빈값이 나올때까지 장비명 캡션 저장
                if (string.IsNullOrEmpty(mc_id))
                {
                    // 빈값은 장비 갯수에서 제외
                    mc_num = i - 1;
                    break;
                }
                BandCaption[i - 1] = mc_id;
            }
            if(i >= 50)
            {
                mc_num = 50;
            }
            sval = 1;
            eval = mc_num < 3? mc_num : 3;
        }


        Grd1.DataSource = datasource;

        await Grd1.PostEditorAsync();
      }
      catch (Exception ex) {
        CloseLoadingPanel();

        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
      }
      finally {
        CloseLoadingPanel();
        
        StateHasChanged();
      }
    }

    // 사용자 정의 메소드 end

    // 데이터 정의 메소드 

    private async Task<DataTable?> DBSearch() {
      if (QueryService == null)         return null;

      string proc_nm = "P_HMI_ELEC_ACTUAL_CHOICE";  //"P_HMI_ELEC_ACTUAL_VAL_ALL";
      /*
      string st_grp = P_ST_GRP.ToStringTrim();
      switch (st_grp) {
        case "ALL_YEAR": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        case "ALL_MONTH": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        case "ALL_DAY": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        case "ALL_HOUR": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        case "ALL_TIME": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        case "COM_TIME": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        case "COM_YEAR": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        case "COM_MONTH": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        case "MIN15": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        case "PEAK": proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL"; break;
        default:          proc_nm = "P_HMI_ELEC_ACTUAL_VAL_ALL";          break;  
      }
      */

      

      var datatable = await QueryService.ExecuteDatatableAsync_fix(proc_nm, new Dictionary<string, object?>()                {
                    {"SDATE", P_SDATE.EditValue.ToStringTrim() },
                    {"EDATE", P_EDATE.EditValue.ToStringTrim() },
                    {"COM_ID", P_COM_ID.Value.ToStringTrim() },
                    {"MC_ID", P_MC_ID.Value.ToStringTrim() },
                    {"ST_GRP", P_ST_GRP?.Name .ToStringTrim() },
                }, "P_");

      //ChartDt1 = datatable;

      //DataTable ndt = new DataTable();
      ChartDt1.Clear();
      if (ChartDt1.Columns.Count == 0) {
        ChartDt1.Columns.Add("RUM");
        ChartDt1.Columns.Add("VAL1");
        ChartDt1.Columns.Add("VAL2");
        ChartDt1.Columns.Add("VAL3");
        ChartDt1.Columns.Add("VAL4");
        ChartDt1.Columns.Add("VAL5");
      }

      
      foreach(DataRow dr in datatable.Rows) {
        for (int i = 1; i <= 50; i++) {

          DataRow ndr = ChartDt1.NewRow();
          ndr["RUM"] = i;
          ndr["VAL1"] = dr[ReVal("REC_AC_KWH_C_", i)];
          ndr["VAL2"] = dr[ReVal("REC_AC_KWH_A_", i)];
          ndr["VAL3"] = dr[ReVal("REC_AC_KWH_B_", i)];
          ndr["VAL4"] = dr[ReVal("REC_AC_KVARH_A_", i)];
          ndr["VAL5"] = dr[ReVal("REC_AC_KVARH_B_", i)];
          ChartDt1.Rows.Add(ndr);
        }
        break;
      }

      

      return datatable;
    }

    // 데이터 정의 메소드 end
  }
}
