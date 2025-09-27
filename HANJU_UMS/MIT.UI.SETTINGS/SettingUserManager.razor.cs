using DevExpress.XtraPrinting.Native.MarkupText;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.Grid.RepositoryItem;
using MIT.Razor.Pages.Component.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.SETTINGS
{
    public class SettingUserManagerBase : CommonUIComponentBase
    {
        protected string SearchUserID { get; set; } = string.Empty;
        protected string SearchUserName { get; set; } = string.Empty;

        protected DataTable? RoleGroupDataTable { get; set; }
        protected CommonGrid? Grd1 { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                InitControls();

                //await btn_Search();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        private async void InitControls()
        {
            Grd1?.SetNeedColumns(new string[]
            {
                "USER_ID"
                , "PASSWORD"
                , "ROLE_GRP_ID"
                , "USER_NAME"
            });

            RoleGroupDataTable = await DBSearchRoleGroupDataComboBoxAsync();
        }

        #endregion [ 컨트롤 초기 세팅 ]

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await btn_Search();
        }

        protected override async Task Btn_Common_Save_Click()
        {
            await btn_Save();
        }

        protected override async Task Btn_Common_Delete_Click()
        {
            await btn_Delete();
        }

        #endregion [ 공통 버튼 기능 ]

        #region [ 사용자 버튼 이벤트 ]

        protected async Task btn_Search()
        {
            await SearchAsync();
        }

        protected async Task btn_Save()
        {
            if (Grd1 == null)
                return;

            await Grd1.PostEditorAsync();

            if (!Grd1.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            if (!Grd1.IsCheckedNeedColumns())
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

            if (await SaveAsync())
            {
                MessageBoxService?.Show("저장하였습니다.");
                await btn_Search();
            }
            else
            {
                MessageBoxService?.Show("저장에 실패하였습니다.");
            }
        }

        protected async Task btn_Delete()
        {
            if (Grd1 == null)
                return;

            await Grd1.PostEditorAsync();

            if (!Grd1.IsCheckedRows())
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

            if (await DeleteAsync())
            {
                MessageBoxService?.Show("삭제하였습니다.");
                await btn_Search();
            }
            else
            {
                MessageBoxService?.Show("삭제에 실패하였습니다.");
            }
        }

        protected async Task btn_Grd1_Row_Add()
        {
            if (Grd1 == null)
                return;

            Grd1.AddNewRow();

            await Grd1.PostEditorAsync();
        }

        #endregion [ 사용자 버튼 이벤트 ]

        #region [ 사용자 정의 메소드 ]

        private async Task SearchAsync()
        {
            if (Grd1 == null)
                return;

            try
            {
                ShowLoadingPanel();
                var dataList = await DBSearchAsync();

                Grd1.DataSource = dataList;

                await Grd1.PostEditorAsync();
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

        private async Task<bool> SaveAsync()
        {
            if (Grd1 == null)
                return false;

            try
            {
                ShowLoadingPanel();
                await Grd1.PostEditorAsync();

                var checkedRows = Grd1.GetCheckedRows();

                if (checkedRows == null || checkedRows.Length == 0)
                    return false;

                await DBSaveAsync(checkedRows.CopyToDataTable());

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

        private async Task<bool> DeleteAsync()
        {
            if (Grd1 == null)
                return false;

            try
            {
                ShowLoadingPanel();

                await Grd1.PostEditorAsync();

                var checkedRows = Grd1.GetCheckedRows();

                if (checkedRows == null || checkedRows.Length == 0)
                    return false;

                await DBDeleteAsync(checkedRows.CopyToDataTable());
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

        #endregion [ 사용자 정의 메소드]

        #region [ 데이터 정의 메소드 ]

        private async Task<DataTable?> DBSearchAsync()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_SETTING_USER_SELECT", new Dictionary<string, object?>()
                {
                    { "USER_ID", SearchUserID },
                    { "USER_NAME", SearchUserName },
                });
            return datatable;
        }

        private async Task DBSaveAsync(DataTable datatable)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_SETTING_USER_SAVE", datatable, new Dictionary<string, object?>()
                {
                    { "REG_ID", USER_ID },
                });
        }

        private async Task DBDeleteAsync(DataTable datatable)
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            await QueryService.ExecuteNonQuery("SP_SETTING_USER_DELETE", datatable, new Dictionary<string, object?>()
                {
                    { "REG_ID", USER_ID },
                });
        }

        protected async Task<DataTable?> DBSearchRoleGroupDataComboBoxAsync()
        {
            if (QueryService == null)
                throw new Exception("QueryService가 null 입니다.");

            var datatable = await QueryService.ExecuteDatatableAsync("SP_COMBO_ROLE_GRP_SELECT", new Dictionary<string, object?>());

            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
