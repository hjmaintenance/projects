    
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
* 작성일자 : 25-02-18
* 최종수정 : 25-02-18
* 프로시저 : P_POWER_FAC_BASE_SELECT
*/
namespace MIT.UI.UMS {
  public partial class B9010MA1Base : CommonUIComponentBase {

    // 공통 버튼 기능 

    protected override async Task Btn_Common_Search_Click() {
      await btn_Search();
    }

    protected override async Task Btn_Common_Save_Click() {
      await btn_Save();
    }

    protected override async Task Btn_Common_Delete_Click() {
      await btn_Delete();
    }

    // 공통 버튼 기능 end

    // 사용자 버튼 기능 ]

    protected async Task btn_Search() {
      await Search();
    }

    protected async Task btn_Save() {
      if (Grd1 == null || Grd1.Grid == null)
        return;

      await Grd1.PostEditorAsync();

      if (!Grd1.IsCheckedRows()) {
        MessageBoxService?.Show("선택된 데이터가 없습니다.");
        return;
      }

      if (!Grd1.IsCheckedNeedColumns()) {
        MessageBoxService?.Show("필수 입력값이 누락되었습니다.");
        return;
      }

      MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
    }

    protected async Task SaveCallback(CommonMsgResult result) {
      if (result != CommonMsgResult.Yes)
        return;

      if (await Save()) {
        //MessageBoxService?.Show("저장하였습니다.");
        await btn_Search();
      }
    }

    protected async Task btn_Delete() {
      if (Grd1 == null || Grd1.Grid == null)
        return;

      await Grd1.PostEditorAsync();

      if (!Grd1.IsCheckedRows()) {
        MessageBoxService?.Show("선택된 데이터가 없습니다.");
        return;
      }

      MessageBoxService?.Show("삭제하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: DeleteCallback);
    }

    protected async Task DeleteCallback(CommonMsgResult result) {
      if (result != CommonMsgResult.Yes)
        return;

      if (await Delete()) {
        MessageBoxService?.Show("삭제하였습니다.");
        await btn_Search();
      }
    }


    // 사용자 버튼 기능 end

    // 사용자 정의 메소드 

    private async Task Search() {
      if (Grd1 == null)
        return;

      try {
        ShowLoadingPanel();

        var datasource = await DBSearch();

        Grd1.DataSource = datasource;
        if (datasource != null && datasource.Rows.Count > 0) {
          await SearchDetail(datasource.Rows[0]["COM_ID"] + "");
        }

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


    public async Task SearchDetail(string com_id) {
      if (Grd1 == null)
        return;

      try {
        ShowLoadingPanel();

        var datasource = await DBSearchDetail(com_id);

        Grd2.DataSource = datasource;

        await Grd2.PostEditorAsync();
      }
      catch (Exception ex) {
        CloseLoadingPanel();

        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
      }
      finally {
        CloseLoadingPanel();
      }
    }



    private async Task<bool> Save() {
      try {
        ShowLoadingPanel();

        var checkedRows = Grd1?.GetCheckedRows();
        if (checkedRows == null || checkedRows.Length == 0)
          return false;

        await DBSave(checkedRows.CopyToDataTable());

        return true;
      }
      catch (Exception ex) {
        CloseLoadingPanel();
        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
        return false;
      }
      finally {
        CloseLoadingPanel();
      }
    }

    private async Task<bool> Delete() {
      try {
        ShowLoadingPanel();

        var checkedRows = Grd1?.GetCheckedRows();

        if (checkedRows == null || checkedRows.Length == 0)
          return false;

        await DBDelete(checkedRows.CopyToDataTable());

        return true;
      }
      catch (Exception ex) {
        CloseLoadingPanel();
        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
        return false;
      }
      finally {
        CloseLoadingPanel();
      }
    }

    // 사용자 정의 메소드 end

    // 데이터 정의 메소드 

    private async Task<DataTable?> DBSearch() {
      if (QueryService == null)        return null;

      var datatable = await QueryService.ExecuteDatatableAsync_fix("P_POWER_FAC_BASE_SELECT", new Dictionary<string, object?>()          {
                    {"COM_ID", COM_ID.ToStringTrim() },
                    {"COM_NAME", COM_NAME.ToStringTrim() },
                    {"START_DT", START_DT.ToStringTrim() },
                    {"PF_BASE_VAL", PF_BASE_VAL.ToStringTrim() },
                    {"END_DT", END_DT.ToStringTrim() },
                },"P_");

      ChartDt1 = datatable;
      return datatable;
    }
    private async Task<DataTable?> DBSearchDetail(string com_id) {
      if (QueryService == null) return null;

      var datatable = await QueryService.ExecuteDatatableAsync_fix("P_POWER_FAC_BASE_HST_SELECT", new Dictionary<string, object?>()          {
                    {"COM_ID", com_id.ToStringTrim() },
                }, "P_");

      return datatable;
    }

    private async Task DBSave(DataTable datatable) {
      if (QueryService == null)
        return;

      await QueryService.ExecuteNonQuery_fix("P_POWER_FAC_BASE_SAVE", datatable, new Dictionary<string, object?>()      {
            }, "P_");
    }

    private async Task DBDelete(DataTable datatable) {
      if (QueryService == null)
        return;

      await QueryService.ExecuteNonQuery_fix("P_POWER_FAC_BASE_DELETE", datatable, new Dictionary<string, object?>()      {
            }, "P_");
    }

    // 데이터 정의 메소드 end
  }
}