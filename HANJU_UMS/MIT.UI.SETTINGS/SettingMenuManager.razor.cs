using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
//using DevExpress.Pdf.Native.BouncyCastle.Asn1.Cms;
using DevExpress.Blazor.Scheduler.Internal;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.TreeView;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component.MessageBox;
using System.Drawing.Drawing2D;
using MIT.Razor.Pages.Component.DataEdits;
using MIT.Razor.Pages.Component.DataEdits.Data;
using DevExpress.DirectX.Common.Direct2D;
using MIT.Razor.Pages.Component.TreeView.Data;

namespace MIT.UI.SETTINGS
{
    public class SettingMenuManagerBase : CommonUIComponentBase
    {
        protected CommonTreeView? TreeView { get; set; }
        protected DxContextMenu? ContextMenu { get; set; }

        protected TreeViewNodeData? SelectedNodeData { get; set; }

        protected CommonImageComboBox? Cbo_MenuType { get; set; }

        protected override void OnInitialized()
        {
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                InitControls();

                await btn_Search();
            }
        }

        #region [ 컨트롤 초기 세팅 ]

        private void InitControls()
        {
            if (Cbo_MenuType != null)
            {
                Cbo_MenuType.DataSource = GetMenuTypeData();
            }
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

        #region [ 사용자 버튼 기능 ]

        protected async Task btn_Search()
        {
            await Search();
        }

        protected async Task btn_Save()
        {
            if (TreeView == null)
                return;

            if (!TreeView.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: SaveCallback);
            await Task.Delay(1);
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

        protected async Task btn_Delete()
        {
            if(TreeView == null)
                return;

            if (!TreeView.IsCheckedRows())
            {
                MessageBoxService?.Show("선택된 데이터가 없습니다.");
                return;
            }

            MessageBoxService?.Show("저장하시겠습니까?", buttons: CommonMsgButtons.YesNo, CloseCallBack: DeleteCallback);
            await Task.Delay(1);
        }

        protected async Task DeleteCallback(CommonMsgResult result)
        {
            if (result != CommonMsgResult.Yes)
                return;

            if (await Delete())
            {
                MessageBoxService?.Show("삭제하였습니다.");
                await btn_Search();
            }
            else
            {
                MessageBoxService?.Show("삭제에 실패하였습니다.");
            }
        }

        #endregion [ 사용자 버튼 기능 ]

        #region 사용자 이벤트 함수

        protected void OnRootNodeClick()
        {
            TreeView?.AddNewRootNode();
        }

        protected void OnChildNodeClick()
        {
            TreeView?.AddNewSelectedChildNode();
        }

        protected void OnSelectionChanged(TreeViewNodeEventArgs e)
        {
            SelectedNodeData = e.NodeInfo.DataItem as TreeViewNodeData;
            if (Cbo_MenuType != null)
                Cbo_MenuType.EditValue = SelectedNodeData?.Row?["MENU_TYPE"].ToStringTrim();
        }

        protected async Task OnContextMenu(MouseEventArgs e)
        {
            if (ContextMenu == null)
                return;

            await ContextMenu.ShowAsync(e);
        }

        protected void OnTextChanged(object? value, string gubun)
        {
            if (SelectedNodeData == null || SelectedNodeData.Row == null) 
                return;

            SelectedNodeData.Checked = true;

            switch (gubun)
            {
                case "MENU_ID": SelectedNodeData.Row["MENU_ID"] = value.ToStringTrim(); break;
                case "PARENT_MENU_ID": SelectedNodeData.Row["PARENT_MENU_ID"] = value.ToStringTrim(); break;
                case "MENU_NAME": SelectedNodeData.Row["MENU_NAME"] = value.ToStringTrim(); break;
                case "SORT": SelectedNodeData.Row["SORT"] = value.ToInt(); break;
                case "MENU_NAME_ENG": SelectedNodeData.Row["MENU_NAME_ENG"] = value.ToStringTrim(); break;
                case "PGM_ID": SelectedNodeData.Row["PGM_ID"] = value.ToStringTrim(); break;
                case "MENU_TYPE":
                    var menuType = value == null ? string.Empty : ((ImageComboBoxData)value).Value.ToStringTrim();
                    SelectedNodeData.Row["MENU_TYPE"] = menuType;
                    if (menuType.Equals("UI"))
                    {
                        SelectedNodeData.Row["ICON_URI"] = "images/main/setting_menu_ui.png";
                    }
                    else if (menuType.Equals("FOLDER"))
                    {
                        SelectedNodeData.Row["ICON_URI"] = "images/main/setting_menu_folder.png";
                    }
                    else
                    {
                        SelectedNodeData.Row["ICON_URI"] = string.Empty;
                    }
                    break;
                case "USE_YN": SelectedNodeData.Row["USE_YN"] = value.ToStringTrim(); break;
                case "REMARK": SelectedNodeData.Row["REMARK"] = value.ToStringTrim(); break;
            }
            //StateHasChanged();
        }

        #endregion 사용자 이벤트 함수

        #region [ 사용자 정의 메소드 ]

        private async Task Search()
        {
            if (TreeView == null)
                return;

            try
            {
                ShowLoadingPanel();

                var datatable = await DBSearch();

                TreeView.DataSource = datatable;

                SelectedNodeData = null;

                if (Cbo_MenuType != null)
                    Cbo_MenuType.EditValue = null;

                StateHasChanged();

                TreeView.ExpandAll();
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
            try
            {
                ShowLoadingPanel();

                var checkedRows = TreeView?.GetCheckedRows();

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

        private async Task<bool> Delete()
        {
            try
            {
                ShowLoadingPanel();

                var checkedRows = TreeView?.GetCheckedRows();

                if (checkedRows == null || checkedRows.Length == 0)
                    return false;

                await DBDelete(checkedRows.CopyToDataTable());

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
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("SP_SETTING_MENU_SELECT", new Dictionary<string, object?>());

            if (datatable != null)
            {
                if (!datatable.Columns.Contains("ICON_URI"))
                    datatable.Columns.Add("ICON_URI", typeof(string));
                if (!datatable.Columns.Contains("ICON_CSS_CLASS"))
                    datatable.Columns.Add("ICON_CSS_CLASS", typeof(string));

                foreach (DataRow row in datatable.Rows)
                {
                    row["ICON_URI"] = row["MENU_TYPE"].ToStringTrim().Equals("UI") ? "images/main/setting_menu_ui.png" : "images/main/setting_menu_folder.png";
                    row["ICON_CSS_CLASS"] = "setting-menu-treelist-icon";
                }
            }

            return datatable;
        }

        public DataTable GetMenuTypeData()
        {
            DataTable datatable = new DataTable();
            datatable.Columns.Add("CODE", typeof(string));
            datatable.Columns.Add("CODE_NAME", typeof(string));
            datatable.Columns.Add("IMG_PATH", typeof(string));

            DataRow row = datatable.NewRow();
            row["CODE"] = "FOLDER";
            row["CODE_NAME"] = "폴더";
            row["IMG_PATH"] = "images/main/setting_menu_folder.png";
            datatable.Rows.Add(row);

            row = datatable.NewRow();
            row["CODE"] = "UI";
            row["CODE_NAME"] = "화면";
            row["IMG_PATH"] = "images/main/setting_menu_ui.png";
            datatable.Rows.Add(row);

            return datatable;
        }

        private async Task DBSave(DataTable datatable)
        {
            if (QueryService == null)
                return;

            await QueryService.ExecuteNonQuery("SP_SETTING_MENU_SAVE", datatable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        private async Task DBDelete(DataTable datatable)
        {
            if (QueryService == null)
                return;

            await QueryService.ExecuteNonQuery("SP_SETTING_MENU_DELETE", datatable, new Dictionary<string, object?>()
            {
                { "REG_ID", USER_ID },
            });
        }

        #endregion [ 데이터 정의 메소드 ]

    }
}
