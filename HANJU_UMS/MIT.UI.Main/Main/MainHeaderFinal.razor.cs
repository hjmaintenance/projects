using DevExpress.Blazor;
//using DevExpress.XtraRichEdit.Import.Html;

//using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509.Qualified;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MIT.DataUtil.Common;
//using MIT.Devexp;
//using MIT.Devexp.ThemeSwitcher;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.Popup;
using MIT.Razor.Pages.Data;
using MIT.UI.SETTINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MIT.UI.Main.Main {
  public class MainHeaderFinalBase : CommonUIComponentBase {
    [Parameter]
    public bool ToggleOn { get; set; }
    [Parameter]
    public EventCallback<bool> ToggleOnChanged { get; set; }
    [Parameter]
    public bool IsMobileLayout { get; set; }



    //public ThemeSwitcher themeSwitch;


    protected async Task OnToggleClick() => await Toggle();

    protected FlyoutPosition Position { get; set; } = FlyoutPosition.Bottom;
    protected bool IsOpen { get; set; } = false;

    protected bool IsSizeOpen { get; set; } = false;
    protected bool IsThemeOpen { get; set; } = false;




    protected override async Task OnInitializedAsync() {
      if (!await IsAuthenticatedCheck())        return;



      var user = await SessionStorageService.GetItemAsync<User>("user");

      if( user.Size == "Small") {
        await SetFontSize(SizeMode.Small);
      }
      else if (user.Size == "Medium") {
        await SetFontSize(SizeMode.Medium);
      }
      else if (user.Size == "Large") {
        await SetFontSize(SizeMode.Large);
      }

      if(!string.IsNullOrEmpty(user.Theme)) {
        await themeClick1(user.Theme);
      }


    }

    [Inject] AppData _appData {  get; set; }

    protected void FontSizeClick1(MouseEventArgs e) { SetFontSize(SizeMode.Small); }
    protected void FontSizeClick2(MouseEventArgs e) { SetFontSize(SizeMode.Medium); }
    protected void FontSizeClick3(MouseEventArgs e) { SetFontSize(SizeMode.Large); }



    public List<MitTheme> mitThemes = new List<MitTheme>() {
    new MitTheme(){
        title = "Mit",
        theme = "blazing-berry",
        bsThemeMode = "light",
        hlUrl = @"https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.15.6/styles/androidstudio.min.css"
      },
    new MitTheme(){        title = "Dark",        theme = "blazing-dark"      },
    new MitTheme(){        title = "Purple",        theme = "purple"      },
    //new MitTheme(){        title = "Fluent-dark",        theme = "fluent-dark"      },
    new MitTheme(){        title = "office-white",        theme = "office-white"      },
    new MitTheme(){        title = "fluent-light",        theme = "fluent-light"      },
    new MitTheme(){        title = "default-dark",        theme = "default-dark",        bsThemeMode = "dark",
      dxUrl = @"_content/DevExpress.Blazor.Themes/bootstrap-external.bs5.min.css",
      bsUrl = @"switcher-resources/css/themes/default/bootstrap.min.css",
      hlUrl = @"https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.15.6/styles/androidstudio.min.css"      },
    new MitTheme(){        title = "default",        theme = "default",
      dxUrl = @"_content/DevExpress.Blazor.Themes/bootstrap-external.bs5.min.css",
      bsUrl = @"switcher-resources/css/themes/default/bootstrap.min.css"     }
    };

    private IJSObjectReference? _module;
    protected async Task themeClick1(string theme_name) {
      _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./switcher-resources/theme-controller.js");
      if (_module != null) {
        MitTheme mt = mitThemes[0];
        foreach( var t in mitThemes) {
          if( theme_name == t.theme) {
            mt = t;
            break;
          }
        }

        USER_THEME = mt.theme;
        ChoiceThemeTitle = mt.title;
        UserData.Theme = mt.theme;




        if (SessionStorageService != null) {
          await   SessionStorageService.SetItemAsync("user", UserData);
        }



        await _module.InvokeVoidAsync("ThemeController.setStylesheetLinks", mt.theme, mt.bsUrl, mt.bsThemeMode, mt.dxUrl, mt.hlUrl, null);

        await QueryService.ExecuteNonQuery("sp_user_prop_save", new Dictionary<string, object?>()      {
                { "theme", mt.theme },
            });

        IsThemeOpen = false;
        StateHasChanged();

      }
    }

    public string ChoiceThemeTitle { get; set; }

    protected async Task SetFontSize(SizeMode _sm) {

      _appData.Sizemode = _sm;
      IsSizeOpen = false;

      await QueryService.ExecuteNonQuery("sp_user_prop_save", new Dictionary<string, object?>()      {
                { "SizeMode", _sm.ToString() },
            });

      StateHasChanged();
    }

    protected async Task Toggle(bool? value = null) {
      var newValue = value ?? !ToggleOn;
      if (ToggleOn != newValue) {
        ToggleOn = newValue;
        await ToggleOnChanged.InvokeAsync(ToggleOn);
      }
    }

    protected private async Task OnLogout() {
      if (AccountService != null)
        await AccountService.LogOutAsync();
    }

    protected void OnSetting() {
      if (CommonPopupService != null)
        CommonPopupService.Show(typeof(SettingPopup), "셋팅", Width: 1280, Height: 740, CloseCallback: OnCloseCallback);
    }

    protected async Task OnCloseCallback(PopupResult result) {
      if (result.PopupResultType != PopupResultType.OK)
        return;

      await Task.Delay(1);

    }
  }


  public class MitTheme {
    public string theme { get; set; }
    public string bsUrl { get; set; }
    public string bsThemeMode { get; set; } = "light";

    private string _dxUrl { get; set; }
    public string dxUrl {
      get {
        if (string.IsNullOrEmpty(_dxUrl)) {
          return "_content/DevExpress.Blazor.Themes/" + theme + ".bs5.min.css";
        }
        else {
          return _dxUrl;
        }
      }
      set {
        _dxUrl = value;
      }
    }
    public string hlUrl { get; set; } = "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/9.15.6/styles/default.min.css";
    public string title { get; set; }
    public string color { get; set; }
  }
}