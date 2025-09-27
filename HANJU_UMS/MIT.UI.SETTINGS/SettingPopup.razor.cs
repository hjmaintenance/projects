using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using MIT.Razor.Pages.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MIT.UI.SETTINGS
{
    public class SettingPopupBase : CommonPopupComponentBase
    {
        protected bool IsPopupVisible { get; set; } = false;

        protected int ActiveTabIndex { get; set; } = 1;

        public void Show()
        {
            IsPopupVisible = true;

            StateHasChanged();
        }

        protected void EulaPopupClosed()
        {
            IsPopupVisible = false;

            StateHasChanged();
        }

        protected void TestClose()
        {
            PopupClose();
        }
    }
}
