    
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
using MIT.Razor.Pages.Component.Grid.Data;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-24
* 최종수정 : 25-02-24
* 프로시저 : P_HMI_MC_INSP_RST_SELECT
*/
namespace MIT.UI.UMS {
  public partial class US3140QA1Base : CommonUIComponentBase {

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
        MessageBoxService?.Show("저장하였습니다.");
        await btn_Search();
      }
      else {
        MessageBoxService?.Show("저장에 실패하였습니다.");
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
      else {
        MessageBoxService?.Show("삭제에 실패하였습니다.");
      }
    }



    protected async void NewRowEventCall(CommonGridInitNewRowEventArgs e) {
     
      DataRow dr = e.Row;
      DateTime dttm = DateTime.Now;
      string dt_str = dttm.ToString("yyyy-MM-dd");
      string tm_str = dttm.ToString("HH:mm");
      dr["CHK_STRT_DT"] = dt_str;
      dr["CHK_STRT_TM"] = tm_str;
      dr["CHK_END_DT"] = dt_str;
      dr["CHK_END_TM"] = tm_str;

      await Grd1.PostEditorAsync();
    }




    // 사용자 버튼 기능 end

    // 사용자 이벤트 함수 

    // 사용자 이벤트 함수 end

    // 사용자 정의 메소드 

    private async Task Search() {
      if (Grd1 == null)
        return;

      try {
        ShowLoadingPanel();

        var datasource = await DBSearch();
        //CompanyList = await GetCommonCode("mc", "", "", "", COM_ID);
        CompanyList = await GetCommonCode("mc", COM_ID, "", "", "");


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

    private async Task<bool> Save() {
      try {
        ShowLoadingPanel();

        var checkedRows = Grd1?.GetCheckedRows();
        if (checkedRows == null || checkedRows.Length == 0)
          return false;


        //foreach( DataRow dr in checkedRows) {
        //  dr["CHK_STRT_TM"] = ((DateTime)dr["CHK_STRT_DT"]).ToString("HH:mm");
        //  dr["CHK_END_TM"] = ((DateTime)dr["CHK_END_DT"]).ToString("HH:mm");
        //}

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
      if (QueryService == null)
        return null;

      var datatable = await QueryService.ExecuteDatatableAsync("P_HMI_MC_INSP_RST_SELECT", new Dictionary<string, object?>()          {
                    {"COM_ID", COM_ID.ToStringTrim() },
                    {"SDATE", SDATE.EditValue.ToStringTrim() },
                    {"EDATE", EDATE.EditValue.ToStringTrim() },
                });

      //ChartDt1 = datatable;
      return datatable;
    }

    private async Task DBSave(DataTable datatable) {
      if (QueryService == null)        return;

      await QueryService.ExecuteNonQuery("P_HMI_MC_INSP_RST_SAVE", datatable, new Dictionary<string, object?>()      {
            });
    }

    private async Task DBDelete(DataTable datatable) {
      if (QueryService == null)        return;

      await QueryService.ExecuteNonQuery("P_HMI_MC_INSP_RST_DELETE", datatable, new Dictionary<string, object?>()      {
            });
    }

    // 데이터 정의 메소드 end
  }
}