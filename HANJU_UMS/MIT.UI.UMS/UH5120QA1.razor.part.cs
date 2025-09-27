    
using MIT.DataUtil.Common;
using MIT.Razor.Pages;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;
/*
* 작성자명 : quristyle
* 작성일자 : 25-02-05
* 최종수정 : 25-02-14
* 화면명 : 실적외 데이터 관리
* 프로시저 : P_EXCL_RST_SELECT, P_EXCEL_RST_SAVE, P_EXCL_RST_DEL
*/
namespace MIT.UI.UMS {
  public partial class UH5120QA1Base : CommonPopupComponentBase {

    #region [ 공통 버튼 기능 ]

    protected override async Task Btn_Common_Search_Click() {




      //MC_ID.Value = "AA";
      //MC_ID.Text = "CCCCCCCCC";




      /*
      foreach (var d in dt) {
        MC_ID.Value = d.Name;
        MC_ID.Text = d.Value;
        break;
      }
      */

      //StateHasChanged();

      // bool chk = await MC_ID.SetFirstRowSelect();
      // Console.WriteLine("chk : " + chk);

      //if (!chk) {
      //  await Task.Run(async () => {

      //    for (int i = 0; i < 10; i++) {

      //      await Task.Delay(1000);
      //      try {
      //        bool isCheck = await MC_ID.SetFirstRowSelect();
      //        Console.WriteLine("Task chk2 : " + isCheck);
      //        if (isCheck) {
      //          break;
      //        }

      //      } catch {

      //        Console.WriteLine("Task exxxxxxxxxxxxxxxxxxxxxxxx: ");
      //      }

      //    }
      //  });
      //}

     // StateHasChanged();


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
      if (Grd1 == null || Grd1.Grid == null)        return;

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


    protected async Task btn_Grd1_Row_Add() {
      if (Grd1 == null || Grd1.Grid == null)
        return;

      Grd1.AddNewRow();

      await Grd1.PostEditorAsync();
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

        await Grd1.PostEditorAsync();
      } catch (Exception ex) {
        CloseLoadingPanel();

        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
      } finally {
        CloseLoadingPanel();
      }
    }

    private async Task<bool> Save() {
      try {
        ShowLoadingPanel();

        var checkedRows = Grd1?.GetCheckedRows();
        if (checkedRows == null || checkedRows.Length == 0)
          return false;




        //foreach (DataRow dr in checkedRows) {
        //  dr["EXCL_STIME"] = ((DateTime)dr["EXCL_SDATE"]).ToString("HH:mm");
        //  dr["EXCL_ETIME"] = ((DateTime)dr["EXCL_EDATE"]).ToString("HH:mm");
        //  //dr["CHK_END_TM"] = ((DateTime)dr["CHK_END_DT"]).ToString("HH:mm");
        //}









        await DBSave(checkedRows.CopyToDataTable());
                DBPeakUpdate(checkedRows.CopyToDataTable());
                return true;
      } catch (Exception ex) {
        CloseLoadingPanel();
        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
        return false;
      } finally {
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
                DBPeakUpdate(checkedRows.CopyToDataTable());
                return true;
      } catch (Exception ex) {
        CloseLoadingPanel();
        MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
        return false;
      } finally {
        CloseLoadingPanel();
      }
    }


        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch() {
      if (QueryService == null)
        return null;

      var datatable = await QueryService.ExecuteDatatableAsync("P_EXCL_RST_SELECT", new Dictionary<string, object?>()                {
                    {"SDATE", SDATE?.EditValue ?? DateTime.Today.ToString() }, // SDATE?.EditValue ??
                    {"EDATE", EDATE?.EditValue ?? DateTime.Today.ToString() },
                    {"MC_ID", MC_ID.Value.ToStringTrim() },
                });

      return datatable;
    }

    private async Task DBSave(DataTable datatable) {
      if (QueryService == null)        return;





      await QueryService.ExecuteNonQuery("P_EXCEL_RST_SAVE", datatable, new Dictionary<string, object?>(){});


      /*
       
	@IN_MC_ID			VARCHAR(30),
	@IN_EXCL_SDATE		VARCHAR(10),
	@IN_EXCL_STIME		VARCHAR(10),
	@IN_EXCL_EDATE		VARCHAR(10),
	@IN_EXCL_ETIME		VARCHAR(10),
	@IN_EXCL_REMARK		VARCHAR(1000)
       
        
       */











    }

    private async Task DBDelete(DataTable datatable) {
      if (QueryService == null)
        return;

      await QueryService.ExecuteNonQuery("P_EXCL_RST_DEL", datatable, new Dictionary<string, object?>()
      {
                { "REG_ID", USER_ID },
            });
    }
    private async Task DBPeakUpdate(DataTable datatable)
    {
        if (QueryService == null)
            return;

            // datatable에서 MC_ID, EXCL_SDATE,EXCL_EDATE 데이터들을 추출해서 
            // newDatatable에 추가한다.
            // newDatatable은 MC_ID, PEAK_DATE를 칼럼으로 가진다.
            // MC_ID는 그대로 가져가고,
            // EXCL_SDATE,EXCL_EDATE 사이의 날짜들을 PEAK_DATE로 추가한다.


            DataTable newDatatable = new DataTable();
            newDatatable.Columns.Add("MC_ID", typeof(string));
            newDatatable.Columns.Add("PEAK_DATE", typeof(string));
            foreach (DataRow row in datatable.Rows)
            {
                string mcId = row["MC_ID"].ToString();
                DateTime startDate = DateTime.Parse(row["EXCL_SDATE"].ToString());
                DateTime endDate = DateTime.Parse(row["EXCL_EDATE"].ToString());
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    DataRow newRow = newDatatable.NewRow();
                    newRow["MC_ID"] = mcId;
                    newRow["PEAK_DATE"] = date.ToString("yyyy-MM-dd");
                    newDatatable.Rows.Add(newRow);
                }
            }


            await QueryService.ExecuteNonQuery("p_update_daily_peak", newDatatable, new Dictionary<string, object?>(){});
    }
        #endregion [ 데이터 정의 메소드 ]
    }
}