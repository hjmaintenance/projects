
using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
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

    protected override async Task Btn_Common_Search_Click() {
      await btn_Search();
    }

    protected async Task btn_Search() {
      await Search();
    }

    private async Task Search() {
      if (Grd1 == null)
        return;

      try {
        ShowLoadingPanel();

        var datasource = await DBSearch();

        if (datasource.Rows.Count > 0) {
          int i = 1;
          for (i = 1; i <= 31; i++) {
            // 모든 행을 순회해서 i번째 MC_ID 와 칼럼명 추출, 없으면 null
            var mcInfo = datasource.AsEnumerable().Select(row => (ReVal("MC_ID_", i), row[ReVal("MC_ID_", i)] + ""))
                                                    .FirstOrDefault((val) => !string.IsNullOrEmpty(val.Item2));
            string mc_col = mcInfo.Item1;
            string mc_id = mcInfo.Item2;
            // 빈값이 나올때까지 장비명 캡션 저장
            if (string.IsNullOrEmpty(mc_id)) {
              break;
            }
            Colnames[mc_col] = mc_id;
          }
          // i값은 빈값이 나온위치거나, 합계 행 인덱스
          mc_num = i - 1;
          sval = 1;
          // 검색된 그룹이 적산이면, 컬럼이 1개라서 전부 보여줘도 성능에 문제가 생기지 않는다.
          eval = searched_grp.Split("_")[0] == "ALL" || mc_num < 5 ? mc_num : 5;
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
      }
    }


    //사용자 정의 메소드 ]

    // 데이터 정의 메소드 ]

    private async Task<DataTable?> DBSearch() {
      if (QueryService == null) return null;

      string proc_nm = "P_HMI_WAT_ACTUAL_VAL_CHOICE"; 
      DataTable datatable = await QueryService.ExecuteDatatableAsync_fix(proc_nm, new Dictionary<string, object?>()                {
                    {"SDATE", P_SDATE.EditValue.ToStringTrim() },
                    {"EDATE", P_EDATE.EditValue.ToStringTrim() },
                    {"COM_ID", P_COM_ID.Value.ToStringTrim() },
                    {"MC_ID", P_MC_ID.Value.ToStringTrim() },
                    {"ST_GRP", P_WA_GRP.ToStringTrim() },
                }, "P_");


        

      // 시보때만 화면에 마추는 로직 처리
      /*if (P_WA_GRP == "SIGNAL_HOUR_W") {

        datatable = new DataTable();
        datatable.Columns.Add("GAUGE_DATE");
        datatable.Columns.Add("GAUGE_MIN");
        for (int i = 1; i <= 55; i++) {
          datatable.Columns.Add($"MC_ID_{i:00}");
          datatable.Columns.Add($"USEQTY_{i:00}");
          datatable.Columns.Add($"AV_TEMPER_{i:00}");
          datatable.Columns.Add($"AV_PRESS_{i:00}");
          datatable.Columns.Add($"I_USEQTY_{i:00}");
          datatable.Columns.Add($"AC_TON_{i:00}");
        }

        var groupedRows = new Dictionary<string, DataRow>();

        foreach (DataRow dr in abc.Rows) {
          string key = $"{dr["GAUGE_DATE"]}_{dr["GAUGE_MIN"]}";

          if (!groupedRows.TryGetValue(key, out DataRow targetRow)) {
            targetRow = datatable.NewRow();
            targetRow["GAUGE_DATE"] = dr["GAUGE_DATE"];
            targetRow["GAUGE_MIN"] = dr["GAUGE_MIN"];
            groupedRows[key] = targetRow;
            datatable.Rows.Add(targetRow);
          }

          int nextIndex = 1;
          while (nextIndex <= 55) {
            string mcCol = $"MC_ID_{nextIndex:00}";
            if (targetRow[mcCol] == DBNull.Value || string.IsNullOrEmpty(targetRow[mcCol].ToString())) {


              targetRow[mcCol] = dr["MC_ID"];
              targetRow[$"USEQTY_{nextIndex:00}"] = dr["R_USEQTY"];
              targetRow[$"AV_TEMPER_{nextIndex:00}"] = dr["AV_TEMPER"];
              targetRow[$"AV_PRESS_{nextIndex:00}"] = dr["AV_PRESS"];
              targetRow[$"I_USEQTY_{nextIndex:00}"] = dr["NI_USEQTY"];
              targetRow[$"AC_TON_{nextIndex:00}"] = dr["AC_TON"];
              break;
            }
            else if (targetRow[mcCol].ToString() == dr["MC_ID"].ToString()) {
              break;
            }

            nextIndex++;
          }
        }

















      }
      else {
        datatable = abc;
      }*/


      //var datatable = PivotByTime(dt, "GAUGE_DATE", "GAUGE_MIN", "MC_ID", "R_USEQTY", "AV_TEMPER", "AV_PRESS", "NI_USEQTY", "AC_TON");

      //ChartDt1.Clear();
      //if (ChartDt1.Columns.Count == 0) {
      //  ChartDt1.Columns.Add("RUM");
      //  ChartDt1.Columns.Add("VAL1");
      //  ChartDt1.Columns.Add("VAL2");
      //}
      //Colnames.Clear();
      //Colnames.Add("");
      //foreach (DataRow dr in datatable.Rows) {
      //  for (int i = 1; i <= 55; i++) {

      //    //DataRow ndr = ChartDt1.NewRow();
      //    //string mc_id = ReVal("MC_ID_", i);

      //    //ndr["RUM"] = i;
      //    //ndr["VAL1"] = dr[mc_id];
      //    //ndr["VAL2"] = dr[ReVal("AC_TON_", i)];
      //    //ChartDt1.Rows.Add(ndr);

      //    // 컬럼명이 비었고, 가져온 값이 비어있지 않으면 컬럼명에 값을 넣는다.
      //    //if (string.IsNullOrEmpty(Colnames[mc_id]) 
      //    //              && !string.IsNullOrEmpty(dr[mc_id] +""))
      //    //{
      //      //Colnames[mc_id] = dr[mc_id] + "";
      //    //}
      //  }
      //  break;
      //}

      searched_grp = P_WA_GRP;

      return datatable;
    }
/*
    private DataTable PivotByTime(DataTable dt, string time_col, string time_col2, string mc_id, params IEnumerable<string>? value_cols) {
      return null; // ........................ 하다 말았나?????????????
      //---------------------------------------------------------
      // 0) 준비
      //---------------------------------------------------------
      var measures = value_cols?.ToList()
                      ?? throw new ArgumentNullException(nameof(value_cols));

      var mcidList = dt.AsEnumerable()
                          .Select(r => r.Field<string>(mc_id))
                          .Distinct()
                          .OrderBy(c => c)
                          .ToList();

      //---------------------------------------------------------
      // 1) Pivot 스키마 생성
      //---------------------------------------------------------
      var pivot = new DataTable("Pivot");
      pivot.Columns.Add(time_col, typeof(string));
      pivot.Columns.Add(time_col2, typeof(string));

      for (int i = 1; i <= mcidList.Count(); ++i)
        foreach (string mv in measures)
          pivot.Columns.Add($"{mv}_{i.ToString("00")}", typeof(double));

      //---------------------------------------------------------
      // 2) (time1,time2) 단위 그룹핑 → 행 채우기
      //---------------------------------------------------------
      var groups =
          from r in dt.AsEnumerable()
          group r by new {
            D = r.Field<string>(time_col),
            T = r.Field<string>(time_col2)
          } into g
          orderby g.Key.D, g.Key.T
          select g;

      foreach (var g in groups) {
        var row = pivot.NewRow();
        row[time_col] = g.Key.D;
        row[time_col2] = g.Key.T;

        // (Category,Measure)별 합계
        var agg =
            from r in g
            from mv in measures
            group r.Field<double>(mv)     // double 형식 가정
            by new {
              McId = r.Field<string>(mc_id),
              M = mv
            } into gm
            select new {
              gm.Key.McId,
              gm.Key.M,
              Sum = gm.Sum()
            };
        int i = 1;
        foreach (var a in agg) {
          row[$"{a.M}_{i.ToString("00")}"] = a.Sum;
          i++;
        }
        pivot.Rows.Add(row);
      }
      return pivot;
    }

*/
    public Dictionary<string, string> Colnames = new Dictionary<string, string>() {

{"MC_ID_01","" },{"MC_ID_11","" },{"MC_ID_21","" },{"MC_ID_31","" },{"MC_ID_41","" },
{"MC_ID_02","" },{"MC_ID_12","" },{"MC_ID_22","" },{"MC_ID_32","" },{"MC_ID_42","" },
{"MC_ID_03","" },{"MC_ID_13","" },{"MC_ID_23","" },{"MC_ID_33","" },{"MC_ID_43","" },
{"MC_ID_04","" },{"MC_ID_14","" },{"MC_ID_24","" },{"MC_ID_34","" },{"MC_ID_44","" },
{"MC_ID_05","" },{"MC_ID_15","" },{"MC_ID_25","" },{"MC_ID_35","" },{"MC_ID_45","" },
{"MC_ID_06","" },{"MC_ID_16","" },{"MC_ID_26","" },{"MC_ID_36","" },{"MC_ID_46","" },
{"MC_ID_07","" },{"MC_ID_17","" },{"MC_ID_27","" },{"MC_ID_37","" },{"MC_ID_47","" },
{"MC_ID_08","" },{"MC_ID_18","" },{"MC_ID_28","" },{"MC_ID_38","" },{"MC_ID_48","" },
{"MC_ID_09","" },{"MC_ID_19","" },{"MC_ID_29","" },{"MC_ID_39","" },{"MC_ID_49","" },
{"MC_ID_10","" },{"MC_ID_20","" },{"MC_ID_30","" },{"MC_ID_40","" },{"MC_ID_50","" },
{"MC_ID_51","" },{"MC_ID_52","" },{"MC_ID_53","" },{"MC_ID_54","" },{"MC_ID_55","" },

    };

    //데이터 정의 메소드 ]
  }
}
