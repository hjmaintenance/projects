using MIT.DataUtil.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MIT.Razor.Pages.Component.MessageBox;
using MIT.Razor.Pages.Service;
using System.Data;
using System.Net.Http;
using MIT.Razor.Pages.Component;
using System.Diagnostics;

namespace MIT.UI.Main.Login;

public class LoginBase : CommonComponentBase {
  protected string? USER_ID { get; set; }
  protected string? PASSWORD { get; set; }

  public string hanju_choice { get; set; } = "btn-primary";
  public string company_choice { get; set; } = "";

  protected async Task OnLogin(MouseEventArgs args) {

    try {
#if DEBUG
      Console.WriteLine("LogIn process..");
      //MessageBoxService?.Show(message: "로그인", type: CommonMsgBoxIcon.Info);
#endif
      ShowLoadingPanel();

      if (AccountService == null)
        throw new Exception("AccountService가 null입니다.");

#if DEBUG
      Console.WriteLine("AccountService is not null..{0},{1},{2}", USER_ID.ToStringTrim(), PASSWORD.ToStringTrim(), string.IsNullOrEmpty(hanju_choice));
#endif
      var isSuccess = await AccountService.LoginAsync(USER_ID.ToStringTrim(), PASSWORD.ToStringTrim(), string.IsNullOrEmpty(hanju_choice)?"C":"H" );
#if DEBUG      
      Console.WriteLine("isSucess:", isSuccess);
#endif
      if (!isSuccess)
        throw new Exception("아이디 또는 비밀번호가 다릅니다.");
    }
    catch (Exception ex) {
      CloseLoadingPanel();
#if DEBUG
      Console.WriteLine("Fail Login..:"+ex.StackTrace+ex.Message);
#endif
      MessageBoxService?.Show(message: ex.Message, type: CommonMsgBoxIcon.Error);
    }
    finally {
      CloseLoadingPanel();
    }
  }

  protected async void OnKeyUp(KeyboardEventArgs e) {
    if (e.Key == "Enter") {
      await OnLogin(null);
    }

  }

  protected void OnRegister(MouseEventArgs args) {
    var DefaultRoute = ConfigurationService?.Get("DefaultRoute");
    NavigationManager?.NavigateTo($"{DefaultRoute.ToStringTrim()}/Register");
  }
}