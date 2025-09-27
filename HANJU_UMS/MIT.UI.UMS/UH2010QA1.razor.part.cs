using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-05
* 최종수정 : 25-02-05
* 프로시저 : P_HMI_LOG_ELEC
*/
namespace MIT.UI.UMS {
  public partial class UH2010QA1Base : CommonUIComponentBase {

    protected override async Task Btn_Common_Search_Click() {
      await btn_Search();
    }


    protected async Task btn_Search() {
      await Search();
    }


    public Dictionary<int, string> cols = new Dictionary<int, string>();

    private async Task Search() {
      if (Grd1 == null)
        return;

      try {
        ShowLoadingPanel();

        var datasource = await DBSearch();

        string ttl = "MC_NAME";
        if (P_GAUGE_TYPE.Value == "WA") ttl = "COM_NAME";

        cols.Clear();
        int cidx = 0;
        for (int i = 0; i < datasource.Rows.Count; i++) {
          DataRow dr = datasource.Rows[i];
          if (dr["GAUGE_MIN"].ToString() == "01:00") {
            cols.Add(cidx, dr[ttl].ToString());
            cidx++;
          }
        }

        DataTable dt = new DataTable();
        dt.Columns.Add("GAUGE_MIN");
        for (int i = 0; i < cols.Count; i++) {
          if (P_GAUGE_TYPE.Value == "WA") {

            dt.Columns.Add("R_USEQTY_" + ReVal("", i));
            dt.Columns.Add("AC_TON_" + ReVal("", i));
          }
          else if (P_GAUGE_TYPE.Value == "ST") {

            dt.Columns.Add("R_USEQTY_" + ReVal("", i));
            dt.Columns.Add("AV_TEMPER_" + ReVal("", i));
            dt.Columns.Add("AV_PRESS_" + ReVal("", i));
            dt.Columns.Add("AC_TON_" + ReVal("", i));


          }
          else {
            dt.Columns.Add("REC_AC_KWH_C_" + ReVal("", i));
            dt.Columns.Add("REC_AC_KWH_A_" + ReVal("", i));
            dt.Columns.Add("REC_AC_KWH_B_" + ReVal("", i));
            dt.Columns.Add("REC_AC_KVARH_A_" + ReVal("", i));
            dt.Columns.Add("REC_AC_KVARH_B_" + ReVal("", i));

          }
        }


        foreach (DataRow dr in datasource.Rows) {

          for (int i = 0; i < cols.Count; i++) {
            if (dr[ttl].ToString() == cols[i]) {

              DataRow ndr = findDR(dt, "GAUGE_MIN", dr["GAUGE_MIN"].ToString());// dt.NewRow();

              if (P_GAUGE_TYPE.Value == "WA") {
                ndr["R_USEQTY_" + ReVal("", i)] = dr["R_USEQTY"].ToString();
                ndr["AC_TON_" + ReVal("", i)] = dr["AC_TON"].ToString();
              }
              else if (P_GAUGE_TYPE.Value == "ST") {
                ndr["R_USEQTY_" + ReVal("", i)] = dr["R_USEQTY"].ToString();
                ndr["AV_TEMPER_" + ReVal("", i)] = dr["AV_TEMPER"].ToString();
                ndr["AV_PRESS_" + ReVal("", i)] = dr["AV_PRESS"].ToString();
                ndr["AC_TON_" + ReVal("", i)] = dr["AC_TON"].ToString();
              }
              else {


                ndr["REC_AC_KWH_C_" + ReVal("", i)] = dr["REC_AC_KWH_C"].ToString();
                ndr["REC_AC_KWH_A_" + ReVal("", i)] = dr["REC_AC_KWH_A"].ToString();
                ndr["REC_AC_KWH_B_" + ReVal("", i)] = dr["REC_AC_KWH_B"].ToString();
                ndr["REC_AC_KVARH_A_" + ReVal("", i)] = dr["REC_AC_KVARH_A"].ToString();
                ndr["REC_AC_KVARH_B_" + ReVal("", i)] = dr["REC_AC_KVARH_B"].ToString();

              }



              break;
            }
          }

        }

        //Grd1.DataSource = datasource;
        Grd1.DataSource = dt;

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


    private DataRow findDR(DataTable dt, string colName, string colval) {
      DataRow dr = null;

      foreach (DataRow d in dt.Rows) {
        if (d[colName].ToString() == colval) {
          dr = d;
          break;
        }
      }

      if (dr == null) {
        dr = dt.NewRow();
        dr[colName] = colval;
        dt.Rows.Add(dr);
      }
      return dr;
    }


    private async Task<DataTable?> DBSearch() {
      if (QueryService == null) return null;

      string proc = "P_HMI_LOG_ELEC";
      if (P_GAUGE_TYPE.Value == "ST") proc = "P_HMI_LOG_STEAM"; // 증기
      else if (P_GAUGE_TYPE.Value == "WA") proc = "P_HMI_LOG_WATER"; // 용수

      var datatable = await QueryService.ExecuteDatatableAsync(proc, new Dictionary<string, object?>()          {
                    {"P_DATE", P_DATE.EditValue.ToStringTrim() },
                    {"GAUGE_TYPE", P_GAUGE_TYPE.Value.ToStringTrim() },
                });
            subtitle = $"대상일 : {P_DATE.EditValue.ToStringTrim()}";
            return datatable;
    }

  }
}
