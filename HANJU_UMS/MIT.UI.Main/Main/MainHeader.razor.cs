using DevExpress.Blazor;
//using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509.Qualified;
using Microsoft.AspNetCore.Components;
//using MIT.Devexp.ThemeSwitcher;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Popup;
using MIT.UI.SETTINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MIT.UI.Main.Main
{
    public class MainHeaderBase : CommonUIComponentBase
    {
        [Parameter]
        public bool ToggleOn { get; set; }
        [Parameter]
        public EventCallback<bool> ToggleOnChanged { get; set; }
        [Parameter]
        public bool IsMobileLayout { get; set; }




    protected async Task OnToggleClick() => await Toggle();

        protected FlyoutPosition Position { get; set; } = FlyoutPosition.Bottom;
        protected bool IsOpen { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {

     

            if (!await IsAuthenticatedCheck())
                return;
        }

        protected async Task Toggle(bool? value = null)
        {
            var newValue = value ?? !ToggleOn;
            if (ToggleOn != newValue)
            {
                ToggleOn = newValue;
                await ToggleOnChanged.InvokeAsync(ToggleOn);
            }
        }

        protected private async Task OnLogout()
        {
            if (AccountService != null)
                await AccountService.LogOutAsync();
        }

        protected void OnSetting()
        {
            if (CommonPopupService != null)
                CommonPopupService.Show(typeof(SettingPopup), "셋팅", Width: 1280, Height: 740, CloseCallback: OnCloseCallback);
        }

        protected async Task OnCloseCallback(PopupResult result)
        {
            if (result.PopupResultType != PopupResultType.OK)
                return;

            await Task.Delay(1);

        }
    }
}
