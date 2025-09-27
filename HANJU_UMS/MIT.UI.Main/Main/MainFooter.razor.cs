using DevExpress.Blazor;
//using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509.Qualified;
using Microsoft.AspNetCore.Components;
using MIT.DataUtil.Common;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Popup;
using MIT.UI.Main.MainFrame;
using MIT.UI.SETTINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MIT.UI.Main.Main
{
    public class MainFooterBase : CommonUIComponentBase
    {
        [Parameter]
        public bool IsMobileLayout { get; set; }

        [Inject]
        protected IMainFrameService? MainFrameService { get; set; }

        protected string? ProgramID { get; set; }
        protected string? MenuName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!await IsAuthenticatedCheck())
                return;

            if (MainFrameService != null)
                MainFrameService.PageChanged += OnPageChanged;
        }

        protected void OnPageChanged(MainFrameData? page) 
        {
            ProgramID = page?.PGM_ID.ToStringTrim();
            MenuName = page?.MENU_NAME.ToStringTrim();
            StateHasChanged();
        }


    }
}
