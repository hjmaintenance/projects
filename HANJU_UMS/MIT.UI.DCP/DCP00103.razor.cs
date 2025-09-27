using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.DataEdits.Data;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Grid.Data;
using MIT.Razor.Pages.Component.Grid.RepositoryItem;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.UI.LIB.DataEdits;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.DCP
{
    public class DCP00103Base : CommonUIComponentBase
    {
        protected CommonTextBox? txt_MSG_ID { get; set; }
        protected CommonTextBox? txt_TB_NAME { get; set; }

        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                await InitControls();

                await Search();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        protected async Task InitControls()
        {
            // 그리드 필수 컬럼 셋팅
            Grd1?.SetNeedColumns(new string[] {
                "MSG_ID", "TB_NAME", "TB_HIS_NAME", "TB_PRC_NAME"
            });

            Grd2?.SetNeedColumns(new string[] {
                "MSG_ID", "COL_NAME", "DATA_TYPE"
            });

            await InvokeAsync(StateHasChanged);
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await Search();
        }

        protected override async Task Btn_Common_Save_Click()
        {
            if (Grd1 == null || Grd2 == null)
                return;

            await Grd1.PostEditorAsync();
            await Grd2.PostEditorAsync();

            if (!Grd1.IsCheckedRows() && !Grd2.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            if (!Grd1.IsCheckedNeedColumns() && !Grd2.IsCheckedNeedColumns())
            {
                MessageBoxService?.Show("필수 입력값이 누락되었습니다.");
                return;
            }

            MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
        }

        protected async Task SaveCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await Save())
            {
                MessageBoxService?.Show("저장하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("저장에 실패하였습니다.");
            }
        }

        protected override async Task Btn_Common_Delete_Click()
        {
            if (Grd1 == null || Grd2 == null)
                return;

            await Grd1.PostEditorAsync();
            await Grd2.PostEditorAsync();

            if (!Grd1.IsCheckedRows() && !Grd2.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("삭제하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: DeleteCallback);
        }

        protected async Task DeleteCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await Delete())
            {
                MessageBoxService?.Show("삭제하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("삭제에 실패하였습니다.");
            }
        }

        #endregion [ 공통 버튼 기능 ]

        #region [사용자 정의 버튼 ]

        /// <summary>
        /// 툴바 조회 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnPrimaryKeyUpdateButtonClick(ToolbarItemClickEventArgs e)
        {
            if (Grd2 == null)
                return;

            await Grd2.PostEditorAsync();

            if (!Grd2.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            if (!Grd2.IsCheckedNeedColumns())
            {
                MessageBoxService?.Show("필수 입력값이 누락되었습니다.");
                return;
            }

            if (!CheckValidationGrd2PrimaryKeyNotNull())
            {
                MessageBoxService?.Show("기본키 지정 컬럼은 Not Null인 값만 지정 가능합니다.");
                return;
            }

            MessageBoxService?.Show("기본키를 지정하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SavePrimaryKeyUpdateCallback);
        }

        protected async Task SavePrimaryKeyUpdateCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await SavePrimaryKey())
            {
                MessageBoxService?.Show("기본키를 지정하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("기본키 지정에 실패하였습니다.");
            }
        }

        /// <summary>
        /// 툴바 조회 버튼 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task OnPrimaryKeyDeleteButtonClick(ToolbarItemClickEventArgs e)
        {
            if (Grd2 == null)
                return;

            await Grd2.PostEditorAsync();

            if (!Grd2.IsRows())
            {
                MessageBoxService?.Show("데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("기본키를 삭제하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: DeletePrimaryKeyUpdateCallback);
        }

        protected async Task DeletePrimaryKeyUpdateCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await DeletePrimaryKey())
            {
                MessageBoxService?.Show("기본키를 삭제하였습니다.");
                await Search();
            }
            else
            {
                MessageBoxService?.Show("기본키 삭제에 실패하였습니다.");
            }
        }

        #endregion [사용자 정의 버튼 ]

        #region [Validation]

        private bool CheckValidationGrd2PrimaryKeyNotNull()
        {
            if (Grd2 == null)
                return false;

            var checkedRows = Grd2.GetCheckedRows();

            if (checkedRows == null)
                return false;

            foreach (DataRow row in checkedRows)
            {
                if (row["NOTNULL_YN"].ToStringTrim().Equals("N"))
                    return false;
            }

            return true;
        }

        #endregion [Validation]

        #region [사용자 정의 이벤트]

        /// <summary>
        /// 그리드 클릭시 포커스 이벤트
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected async Task Grd1_FocusedRowChanged(GridFocusedRowChangedEventArgs e)
        {
            await SearchDetail();
        }

        /// <summary>
        /// 그리드에 새로운 행 추가전에 값 셋팅 추가 또는 새로운행 추가 취소
        /// 취소는 Cancel 에 true 값 셋팅
        /// </summary>
        /// <param name="e"></param>
        protected void Grd2_InitNewRowChanging(CommonGridInitNewRowEventArgs e)
        {
            if (e.Row == null || Grd1 == null)
            {
                e.Cancel = true;
                return;
            }

            if (Grd1.GetFocusedRowCellValue("SAVE_YN").ToStringTrim().Equals("N"))
            {
                MessageBoxService?.Show("신규 테이블 정보 저장 후 상세정보 추가해주세요.");
                e.Cancel = true;
                return;
            }

            e.Row["MSG_ID"] = Grd1.GetFocusedRowCellValue("MSG_ID").ToStringTrim();
            e.Row["DATA_TYPE"] = "VARCHAR";
            e.Row["DATA_LEN"] = 50;
            e.Row["PK_YN"] = "N";
            e.Row["MODIFY_YN"] = "Y";
            e.Row["NOTNULL_YN"] = "N";
            e.Row["JOB_YN"] = "Y";
        }

        protected void reptxt_TB_NAME_1_TextChanged(string text)
        {
            Grd1?.SetFocusedRowCellValue("TB_HIS_NAME", $"TB_CP_HIS_{text}");
            Grd1?.SetFocusedRowCellValue("TB_PRC_NAME", $"TB_CP_PRC_{text}");

            Grd1?.PostEditor();
        }

        #endregion [사용자 정의 이벤트]


        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            if (Grd1 == null || Grd2 == null)
                return;

            try
            {
                ShowLoadingPanel();
                var datatable = await DBSearch();

                Grd1.DataSource = datatable;

                await Grd1.PostEditorAsync();

                var datatable2 = await DBSearchDetail(Grd1.GetFocusedRowCellValue("MSG_ID").ToStringTrim());

                Grd2.DataSource = datatable2;

                await Grd2.PostEditorAsync();
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        private async Task SearchDetail()
        {
            if (Grd2 == null || Grd1 == null)
                return;

            try
            {
                ShowLoadingPanel();
                var datatable = await DBSearchDetail(Grd1.GetFocusedRowCellValue("MSG_ID").ToStringTrim());

                Grd2.DataSource = datatable;

                await Grd2.PostEditorAsync();
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        private async Task<bool> Save()
        {
            if (Grd1 == null || Grd2 == null)
                return false;

            await Grd1.PostEditorAsync();
            await Grd2.PostEditorAsync();

            try
            {
                ShowLoadingPanel();

                var checkedRows1 = Grd1.GetCheckedRows();

                if (checkedRows1 != null && checkedRows1.Length > 0)
                    await DBSave(checkedRows1.CopyToDataTable());

                var checkedRows2 = Grd2.GetCheckedRows();

                if (checkedRows2 != null && checkedRows2.Length > 0)
                    await DBSaveDetail(checkedRows2.CopyToDataTable());

                return true;
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        private async Task<bool> Delete()
        {
            if (Grd1 == null || Grd2 == null)
                return false;

            await Grd1.PostEditorAsync();

            try
            {
                ShowLoadingPanel();

                var checkedRows1 = Grd1.GetCheckedRows();

                if (checkedRows1 != null && checkedRows1.Length > 0)
                    await DBDelete(checkedRows1.CopyToDataTable());

                var checkedRows2 = Grd2.GetCheckedRows();

                if (checkedRows2 != null && checkedRows2.Length > 0)
                    await DBDeleteDetail(checkedRows2.CopyToDataTable());

                return true;
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        private async Task<bool> SavePrimaryKey()
        {
            if (Grd2 == null)
                return false;

            await Grd2.PostEditorAsync();

            try
            {
                ShowLoadingPanel();

                var checkedRows = Grd2.GetCheckedRows();

                if (checkedRows != null && checkedRows.Length > 0)
                {
                    string MSG_ID = checkedRows[0]["MSG_ID"].ToStringTrim();
                    string COL_IDS = checkedRows.Select(s => s["COL_ID"].ToStringTrim()).Aggregate((i, j) => i + "," + j);

                    await DBSavePrimaryKey(MSG_ID, COL_IDS);
                }
                return true;
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        private async Task<bool> DeletePrimaryKey()
        {
            if (Grd2 == null)
                return false;

            await Grd2.PostEditorAsync();

            try
            {
                ShowLoadingPanel();

                await DBDeletePrimaryKey(Grd2.GetRowCellValue(0, "MSG_ID").ToStringTrim());

                return true;
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseLoadingPanel();
            }
        }


        #endregion [ 사용자 정의 메소드 ]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearch()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_DCP00103_GRD1_SELECT", new Dictionary<string, object?>()
                {
                    { "MSG_ID", txt_MSG_ID?.Text },
                    { "TB_NAME", txt_TB_NAME?.Text },
                });
            return datatable;
        }

        private async Task DBSave(DataTable dataTable)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00103_GRD1_SAVE", dataTable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        private async Task DBDelete(DataTable dataTable)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00103_GRD1_DELETE", dataTable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        private async Task<DataTable?> DBSearchDetail(string MSG_ID)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_DCP00103_GRD2_SELECT", new Dictionary<string, object?>()
                {
                    { "MSG_ID", MSG_ID },
                });
            return datatable;
        }

        private async Task DBSaveDetail(DataTable dataTable)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00103_GRD2_SAVE", dataTable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        private async Task DBDeleteDetail(DataTable dataTable)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00103_GRD2_DELETE", dataTable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        private async Task DBSavePrimaryKey(string MSG_ID, string COL_IDS)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00103_GRD2_PK_SAVE", new Dictionary<string, object?>()
            {
                { "MSG_ID", MSG_ID },
                { "COL_IDS", COL_IDS },
                { "REG_ID", USER_ID },
            });
        }

        private async Task DBDeletePrimaryKey(string MSG_ID)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_DCP00103_GRD2_PK_DELETE", new Dictionary<string, object?>()
            {
                { "MSG_ID", MSG_ID },
                { "REG_ID", USER_ID },
            });
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}

