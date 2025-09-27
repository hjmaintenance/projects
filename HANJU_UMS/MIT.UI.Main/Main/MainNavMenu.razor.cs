using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.TreeView;
using MIT.Razor.Pages.Component.TreeView.Data;
using MIT.UI.Main.MainFrame;
using System.Data;

namespace MIT.UI.Main.Main
{
    public class MainNavMenuBase : CommonUIComponentBase
    {
        [Inject]
        protected IMainFrameService? MainFrameService { get; set; }

        [Parameter]
        public string? StateCssClass { get; set; }

        public static event EventHandler<bool>? ToggleSidebarEvent;

        protected CommonTreeView? TreeView { get; set; }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (!await IsAuthenticatedCheck())
                    return;

                await Search();
            }
        }

        #region [ 사용자 이벤트 메소드 ]

        protected void OnNodeClick(TreeViewNodeClickExEventArgs e)
        {
            if (e.itemRow == null)
                return;

            if (e.itemRow["MENU_TYPE"].ToStringTrim().Equals("UI"))
            {
                var PGM_ID = e.itemRow["PGM_ID"].ToStringTrim();
                var PGM_CLASS = e.itemRow["PGM_CLASS"].ToStringTrim();
                var PGM_PATH = e.itemRow["PGM_PATH"].ToStringTrim();
                var MENU_ID = e.itemRow["MENU_ID"].ToStringTrim();
                var MENU_NAME = e.itemRow["MENU_NAME"].ToStringTrim();

        Console.WriteLine("Menu:{0},{1},{2},{3},{4}", PGM_ID, PGM_CLASS, PGM_PATH, MENU_ID, MENU_NAME);

        MainFrameService?.OpenPage(PGM_ID, PGM_CLASS, PGM_PATH, MENU_ID, MENU_NAME);

        Console.WriteLine("Menu Open.. {0}", CommonUIComponentBase.IsMobileMode.ToString());

        if (CommonUIComponentBase.IsMobileMode)
                {
                    StateCssClass = "collapse";
                    ToggleSidebarEvent?.Invoke(this, false);
                }
            }
        }

        #endregion [ 사용자 이벤트 메소드 ]

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

                StateHasChanged();
            }
            catch (Exception)
            {

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

            var datatable = await QueryService.ExecuteDatatableAsync("SP_MAIN_MENU_SELECT", new Dictionary<string, object?>() {
                { "USER_ID", USER_ID },
            });

            if (datatable != null)
            {
                if (!datatable.Columns.Contains("ICON_URI"))
                    datatable.Columns.Add("ICON_URI", typeof(string));
                if (!datatable.Columns.Contains("ICON_CSS_CLASS"))
                    datatable.Columns.Add("ICON_CSS_CLASS", typeof(string));

                foreach (DataRow row in datatable.Rows)
                {
                    if (row["MENU_TYPE"].ToStringTrim().Equals("FOLDER"))
                    {
                        switch(row["MENU_ID"].ToStringTrim())
                        {
                            case "FAVORITE":    // 즐겨찾기
                                row["ICON_URI"] = "images/main/UI_SideMenu_1.png";
                                break;
                                    
                            case "SYS":         // 시스템관리
                                row["ICON_URI"] = "images/main/UI_SideMenu_3.png";
                                break;

                            case "COM":         // 기준정보관리
                                row["ICON_URI"] = "images/main/UI_SideMenu_5.png";
                                break;

                            default:           // 그 외
                                row["ICON_URI"] = "images/main/UI_SideMenu_4.png";
                                break;
                        }
                    }

                    row["ICON_CSS_CLASS"] = "main-nav-treelist-icon";
                }
            }
            
            return datatable;
        }

        #endregion [ 데이터 정의 메소드 ]
    }
}
