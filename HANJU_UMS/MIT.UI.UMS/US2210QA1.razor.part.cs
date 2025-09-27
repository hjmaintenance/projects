    
using DevExpress.Blazor;
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
* 작성자명 : quristyle
* 작성일자 : 25-02-05
* 최종수정 : 25-02-05
* 프로시저 : P_HMI_USE_DAYWATTIME_SELECT01
*/
namespace MIT.UI.UMS {
  public partial class US2210QA1Base : CommonUIComponentBase {

    #region [ 공통 버튼 기능 ]

    protected override async Task Btn_Common_Search_Click() {
      await btn_Search();
    }

    #endregion [ 공통 버튼 기능 ]

    #region [ 사용자 버튼 기능 ]

    protected async Task btn_Search() {
      await Search();
    }

    #endregion [ 사용자 버튼 기능 ]

    #region [ 사용자 이벤트 함수 ]

    #endregion [ 사용자 이벤트 함수 ]

    #region [ 사용자 정의 메소드 ]

    private async Task Search() {
      if (Grd1 == null)
        return;

      try {
        ShowLoadingPanel();

        var datasource = await DBSearch();

        Grd1.DataSource = datasource;

        if (datasource != null && datasource.Rows.Count > 0)
        {
            DataRow sumRow = datasource.Rows[datasource.Rows.Count - 1];

            // 응축수 순수, 여과수 숨김여부
            VisibleFRDW = !isZero(sumRow, "SOL_05") || !isZero(sumRow, "SOL_06");
            VisibleFRFW = !isZero(sumRow, "SOL_07") || !isZero(sumRow, "SOL_08");
        }

        // 마지막 행은 합계라서 제외하고 차트 데이터로 사용
        //if (datasource != null && datasource.Rows.Count > 0)
        //{
        //    var filteredDataSource = datasource.Clone();
        //    for (int i = 0; i < datasource.Rows.Count - 1; i++)
        //    {
        //        filteredDataSource.ImportRow(datasource.Rows[i]);
        //    }
        //    Chart1.DataSource = filteredDataSource;
        //}
        await Grd1.PostEditorAsync();
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
    // 합계 행에서 응축수(순수)칼럼 SOL05,06 응축수(여과수)칼럼 SOL07,08 값이 0이면 각 컬럼을 숨김
    private bool isZero(DataRow dr, string colName) {
        return dr[colName].ToDecimal() <= 0m;
    }

    #endregion [ 사용자 정의 메소드 ]

    #region [ 데이터 정의 메소드 ]

    private async Task<DataTable?> DBSearch() {
      if (QueryService == null)
        return null;
            var comId = isHanju ? COM_ID : USER_ID;
      var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_USE_DAYWATTIME_SELECT01", new Dictionary<string, object?>()
          {
                    {"DATE", DATE?.EditValue },
                    {"COM_ID", comId.ToStringTrim() },
                    //{"GAUGE_TYPE", "" },
                });

      return datatable;
    }

    #endregion [ 데이터 정의 메소드 ]
  }
}