/*
* 작성자명 : quristyle
* 작성일자 : 25-02-03
* 최종수정 : 25-02-03
* 프로시저 : P_HMI_COMMON_COMINFO_SELECT
*/     
using DevExpress.Blazor;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Native;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component.Grid.Data;
using MIT.Razor.Pages.Component.DataEdits;

namespace MIT.UI.UMS {
  public partial class B4015MA1Base : CommonUIComponentBase {

   //public MitCombo Txt_COM_ID { get; set; }
    protected string? P_COM_ID { get; set; }
    protected string? P_COM_NAME { get; set; }
        protected string? P_USE_YN { get; set; } = "Y";


    protected CommonGrid? Grd1 { get; set; }


    [Parameter]    public List<CommCode> comm_dt { get; set; } = new List<CommCode>();


    protected void OnSetting(GridRowClickEventArgs e) {
      if (CommonPopupService != null){

        if ( e.Column.Caption == "비고") {
          return;
        }


        var sel_data = e.Grid.GetDataItem(e.VisibleIndex);
        DataRow sel_dr = (sel_data as DataRowView).Row;
        ShowPop(sel_dr);
      }
    }


    protected void NewRowEventCall(CommonGridInitNewRowEventArgs e) {

     


      ShowPop(e.Row);
    }

    void ShowPop(DataRow sel_dr) {

      CommonPopupService.Show(typeof(B4015MA1_POP),
        "수용가 관리",
        Width: 1080,
        Height: 740,
      CloseCallback: OnCloseCallback,
      Parameter: sel_dr
      );
    }






    protected async Task OnCloseCallback(PopupResult result) {
      if (result.PopupResultType != PopupResultType.OK)
        return;

      await Task.Delay(1);

    }


    protected async Task comKeyDown(KeyboardEventArgs e) {
      if (e.Key == "Enter") {
        await btn_Search();
      }
    }





    protected override async Task OnAfterRenderAsync(bool firstRender) {
      await base.OnAfterRenderAsync(firstRender);

      if (firstRender) {
        if (!await IsAuthenticatedCheck())
          return;

        InitControls();

        await btn_Search();
      }
    }




    protected override async Task OnInitializedAsync() {


    }





    #region [ 컨트롤 초기 세팅 ]

    private void InitControls() {
      if (Grd1 == null || Grd1.Grid == null)
        return;

      Grd1.SetNeedColumns(new string[] { "COM_ID" });


    }

    #endregion [ 컨트롤 초기 세팅 ]

    #region [ 공통 버튼 기능 ]

    protected override async Task Btn_Common_Search_Click() {
      await btn_Search();
    }

    protected override async Task Btn_Common_Save_Click() {
      await btn_Save();
    }

    protected override async Task Btn_Common_Delete_Click() {
      await btn_Delete();
    }

    #endregion [ 공통 버튼 기능 ]

    #region [ 사용자 버튼 기능 ]

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
      //  MessageBoxService?.Show("저장에 실패하였습니다.");
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
        //MessageBoxService?.Show("삭제에 실패하였습니다.");
      }
    }






    #endregion [ 사용자 버튼 기능 ]
    #region [ 사용자 정의 메소드 ]
    private async Task Search() {




      if (Grd1 == null)
        return;

      try {
        ShowLoadingPanel();

        var datasource = await DBSearch();

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

    #endregion [ 사용자 정의 메소드 ]

    #region [ 데이터 정의 메소드 ]

    private async Task<DataTable?> DBSearch() {

      if(QueryService == null) {
        return null;
      }

      var datatable = await QueryService.ExecuteDatatableAsync_fix("P_HMI_COMMON_COMINFO_SELECT", new Dictionary<string, object?>()          {
                    {"COM_ID", P_COM_ID.ToStringTrim() },
                    {"COM_NAME", P_COM_NAME.ToStringTrim() },
                    {"USE_YN", P_USE_YN.ToStringTrim() },
                }, "P_");

      return datatable;
    }

    private async Task DBSave(DataTable datatable) {
      if (QueryService == null)
        return;

      await QueryService.ExecuteNonQuery_fix("P_HMI_COMMON_COMINFO_SAVE", datatable, new Dictionary<string, object?>()      {            }, "P_");
    }

    private async Task DBDelete(DataTable datatable) {
      if (QueryService == null)
        return;

      await QueryService.ExecuteNonQuery_fix("P_HMI_COMMON_COMINFO_DELETE", datatable, new Dictionary<string, object?>()      {            }, "P_");
    }

    #endregion [ 데이터 정의 메소드 ]






  }





}
