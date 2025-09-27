using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Blazor;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;
using System.Diagnostics;

namespace MIT.UI.Main.MainFrame
{
    public class MainFrameBase : CommonUIComponentBase
    {
        [Inject]
        protected IMainFrameService? MainFrameService { get; set; }

        [Parameter]
        public int MaxTabCount { get; set; } = 20;

        protected int ActiveTabIndex { get; set; } = 0;

        protected List<MainFrameDataButtonData> Pages { get; set; } = new List<MainFrameDataButtonData>();

        protected override async Task OnInitializedAsync()
        {
            if (!await IsAuthenticatedCheck())
                return;

            var mainFrameService = MainFrameService as IMainFrameSenderService;
            if (mainFrameService != null)
                mainFrameService.OnOpenPage += OnOpenPage;

            if (NavigationManager != null)
                NavigationManager.LocationChanged += OnLocationChanged;

        }

        public void Dispose()
        {
            var mainFrameService = MainFrameService as IMainFrameSenderService;
            if (mainFrameService != null)
                mainFrameService.OnOpenPage -= OnOpenPage;
            if (NavigationManager != null)
                NavigationManager.LocationChanged -= OnLocationChanged;
        }

        protected void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            Pages.Clear();
            StateHasChanged();
        }

        private void InitButton()
        {
        }

        protected async void OnOpenPage(MainFrameData mainFrameData)
        {
            int index = Pages.FindIndex(w => w.MENU_ID.ToStringTrim().Equals(mainFrameData.MENU_ID.ToStringTrim()));

            if (index < 0)
            {
                bool IsButtonSearch = false;
                bool IsButtonSave = false;
                bool IsButtonDelete = false;
                bool IsButtonPrint = false;

                try
                {
                    ShowLoadingPanel();

                    InitButton();

                    var datatable = await DBSearchMainRoleButton(this.USER_ID, mainFrameData.MENU_ID.ToStringTrim());

                    if (datatable == null || datatable.Rows.Count == 0)
                        return;

                    if (!datatable.AsEnumerable().Any(w => w["BUTTONID"].ToStringTrim().Equals("OPEN")))
                        throw new Exception("OPEN 권한이 없습니다.");

                    foreach (DataRow row in datatable.Rows)
                    {
                        if (row["BUTTONID"].ToStringTrim().Equals("SELECT"))
                            IsButtonSearch = true;
                        else if (row["BUTTONID"].ToStringTrim().Equals("SAVE"))
                            IsButtonSave = true;
                        else if (row["BUTTONID"].ToStringTrim().Equals("DELETE"))
                            IsButtonDelete = true;
                        else if (row["BUTTONID"].ToStringTrim().Equals("PRINT"))
                            IsButtonPrint = true;
                    }
                }
                catch (Exception ex)
                {
                    CloseLoadingPanel();
                    MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
                    return;
                }
                finally
                {
                    CloseLoadingPanel();
                }

                if (Pages.Count >= MaxTabCount)
                {
                    MessageBoxService?.Show($"화면 개수는 최대 {MaxTabCount}개만 가능합니다!");
                    return;
                }

                var page = new MIT.UI.Main.MainFrame.MainFrameDataButtonData(mainFrameData, new Razor.Pages.Common.MainCommonButtonData()
                {
                    IsButtonSearch = IsButtonSearch,
                    IsButtonSave = IsButtonSave,
                    IsButtonDelete = IsButtonDelete,
                    IsButtonPrint = IsButtonPrint,
                });

                Pages.Add(page);
                index = Pages.Count - 1;
            }

            ActiveTabIndex = index + 1;

            OnActiveTabIndexChanged(ActiveTabIndex);
            StateHasChanged();
        }

        protected void RemovePage(MainFrameDataButtonData page)
        {
            Pages.Remove(page);
            //StateHasChanged();
        }


        protected RenderFragment? BuildPage(MainFrameDataButtonData page)
        {
            if (page.ClassType == null)
                return null;

            RenderFragment renderPage = b =>
            {
                b.OpenComponent(0, page.ClassType);
                b.AddAttribute(1, "MainCommonButtonData", page.MainCommonButtonData);
                b.AddAttribute(2, "MENU_ID", page.MENU_ID);
                b.AddAttribute(3, "IsMainMenuUIMode", true);
                b.CloseComponent();
            };
            return renderPage;
        }

        private async Task<DataTable?> DBSearchMainRoleButton(string UserID, string MenuID)
        {
            if (QueryService == null)
                return null;

            var datatable = await QueryService.ExecuteDatatableAsync("SP_MAIN_ROLE_BTN_SELECT", new Dictionary<string, object?>()
                {
                    { "USER_ID", UserID },
                    { "MENU_ID", MenuID },
                });

            return datatable;
        }

        protected void OnButtonClick(MainFrameDataButtonData page, EventCallback eventCallback, bool isRemoved = false)
        {
            eventCallback.InvokeAsync();

            if (isRemoved)
                RemovePage(page);
        }

        protected void OnActiveTabIndexChanged(int index)
        {
            ActiveTabIndex = index;

            if (index <= 0)
            {
                (MainFrameService as IMainFrameSenderService)?.PageFocusedChanged(null);
            }
            else
            {
                (MainFrameService as IMainFrameSenderService)?.PageFocusedChanged(Pages[index - 1]);
            }

            StateHasChanged();
        }
    }
}
