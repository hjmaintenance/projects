using DevExpress.Blazor;
using DevExpress.ClipboardSource.SpreadsheetML;
//using DevExpress.Pdf.Native.BouncyCastle.Asn1.Cms;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.NativeBricks;
using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Grid;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.Razor.Pages.Component.TreeView;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.UI.SETTINGS
{
    public class SettingProgramRoleManagerBase : CommonUIComponentBase
    {
        
        protected CommonGrid? Grd1 { get; set; }
        protected CommonGrid? Grd2 { get; set; }
        protected CommonTreeView? TreeView { get; set; }

        protected override void OnInitialized()
        {
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                await btn_Search();
            }
        }

        #region [ 공통 버튼 기능 ]

        protected override async Task Btn_Common_Search_Click()
        {
            await btn_Search();
        }

        protected override async Task Btn_Common_Save_Click()
        {
            await btn_Save();
        }

        #endregion [ 공통 버튼 기능 ]

        #region [ 사용자 버튼 이벤트 ]

        protected async Task btn_Search()
        {
            await Search();
        }

        protected async Task btn_Save()
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

            MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
        }

        protected async Task SaveCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await Save())
            {
                MessageBoxService?.Show("저장하였습니다.");
                await btn_Search();
            }
            else
            {
                MessageBoxService?.Show("저장에 실패하였습니다.");
            }
        }

        #endregion [ 사용자 버튼 이벤트 ]

        #region [ 사용자 이벤트 함수 ]

        protected async Task OnFocusedRowChangedAsync(GridFocusedRowChangedEventArgs e)
        {
            await SearchRoleGroupMenu();
        }

        protected async Task OnSelectionChangedAsync(TreeViewNodeEventArgs e)
        {
            await SearchRoleGroupRoleButton();
        }

        #endregion [ 사용자 이벤트 함수 ]

        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            try
            {
                await SearchRoleGroup();
                await SearchRoleGroupMenu();
                await SearchRoleGroupRoleButton();

                StateHasChanged();
            }
            catch (Exception ex)
            {
                MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
            }
        }

        private async Task SearchRoleGroup()
        {
            if (Grd1 == null)
                return;

            try
            {
                ShowLoadingPanel();

                var datatable = await DBSearchRoleGroup();
                Grd1.DataSource = datatable;

                await Grd1.PostEditorAsync();
            }
            catch(Exception ex)
            {
                CloseLoadingPanel();
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            { 
                CloseLoadingPanel(); 
            }
        }

        private async Task SearchRoleGroupMenu()
        {
            if (Grd1 == null || TreeView == null)
                return;

            try
            {
                ShowLoadingPanel();

                var ROLE_GRP_ID = Grd1.GetFocusedRowCellValue("ROLE_GRP_ID");
                
                if (ROLE_GRP_ID != null)
                {
                    var datatable = await DBSearchRoleGroupMenu(ROLE_GRP_ID.ToStringTrim());

                    TreeView.DataSource = datatable;
                }
                else
                {
                    TreeView.DataSource = null;
                }    
                

                StateHasChanged();

                TreeView.ExpandAll();
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        private async Task SearchRoleGroupRoleButton()
        {
            if (Grd1 == null || Grd2 == null || TreeView == null)
                return;

            try
            {
                ShowLoadingPanel();

                var ROLE_GRP_ID = Grd1.GetFocusedRowCellValue("ROLE_GRP_ID");
                var MENU_ID = TreeView.SelectedNodeData?.Row?["MENU_ID"];

                var datatable = await DBSearchRoleGroupRoleButton(ROLE_GRP_ID.ToStringTrim(), MENU_ID.ToStringTrim());
                Grd2.DataSource = datatable;
            }
            catch (Exception ex)
            {
                CloseLoadingPanel();
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                CloseLoadingPanel();
            }
        }

        private async Task<bool> Save()
        {
            if (Grd2 == null)
                return false;

            try
            {
                ShowLoadingPanel();

                await Grd2.PostEditorAsync();

                var checkedRows = Grd2.GetCheckedRows();
                if (checkedRows == null || checkedRows.Length == 0)
                    return false;

                await DBSave(checkedRows.CopyToDataTable());

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

        private async Task<DataTable?> DBSearchRoleGroup()
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("SP_SETTING_PROGRAM_ROLE_GRP_ROLE_GRP_SELECT", new Dictionary<string, object?>());

            return datatable;
        }

        private async Task<DataTable?> DBSearchRoleGroupMenu(string ROLE_GRP_ID)
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("SP_SETTING_PROGRAM_ROLE_GRP_ROLE_GRP_MENU_SELECT", new Dictionary<string, object?>() {
                { "ROLE_GRP_ID", ROLE_GRP_ID }
            });

            if (datatable != null)
            {
                if (!datatable.Columns.Contains("ICON_URI"))
                    datatable.Columns.Add("ICON_URI", typeof(string));
                if (!datatable.Columns.Contains("ICON_CSS_CLASS"))
                    datatable.Columns.Add("ICON_CSS_CLASS", typeof(string));
                if (!datatable.Columns.Contains("CSS_CLASS"))
                    datatable.Columns.Add("CSS_CLASS", typeof(string));

                foreach (DataRow row in datatable.Rows)
                {
                    row["ICON_URI"] = row["MENU_TYPE"].ToStringTrim().Equals("UI") ? "images/main/setting_menu_ui.png" : "images/main/setting_menu_folder.png";
                    row["ICON_CSS_CLASS"] = "setting-menu-treelist-icon";
                    row["CSS_CLASS"] = row["BTN_YN"].ToStringTrim().Equals("Y") ? "tree-color-text-blue" : "tree-color-text-black";
                }
            }

            return datatable;
        }

        private async Task<DataTable?> DBSearchRoleGroupRoleButton(string ROLE_GRP_ID, string MENU_ID)
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("SP_SETTING_PROGRAM_ROLE_GRP_ROLE_GRP_MENU_BTN_SELECT", new Dictionary<string, object?>() {
                { "ROLE_GRP_ID", ROLE_GRP_ID },
                { "MENU_ID", MENU_ID },
            });

            return datatable;
        }

        private async Task DBSave(DataTable datatable)
        {
            if (QueryService == null)
                return;

            await QueryService.ExecuteNonQuery("SP_SETTING_PROGRAM_ROLE_GRP_SAVE", datatable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
