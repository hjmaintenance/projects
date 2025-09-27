using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.Razor.Pages.Component.DataEdits {
  public static class MitCommData {

    [Inject]
    static IQueryService? QueryService { get; set; }


    public static async Task<DataTable> GetCommonCode(string code, params string[] cm_ids) {
      DataTable result = null;
      result = await QueryService.ExecuteDatatableAsync("P_COMMON_CODE", new Dictionary<string, object?>()
        {
                { "CODE", code.ToUpper() },
                { "CM_ID", "" },
                { "CM_NAME", "" },
                { "CM_DESC", "" },

            });

      return result;
    }




    public static async Task<List<CommCode>> SetData(string code_id, bool isAll) {

      DataTable dt = await GetCommonCode(code_id);
      List<CommCode> comm_dt = new List<CommCode>();

      if (dt == null)
        return null;

      if (isAll) {
        comm_dt.Add(new CommCode() { Name = "", Value = "ALL" });
      }

      foreach (DataRow row in dt.Rows) {
        comm_dt.Add(new CommCode() { Name = row["CM_ID"] + "", Value = row["CM_NAME"] + "" });
      }
      return comm_dt;
      //StateHasChanged();
    }






  }



}
