using MIT.DataUtil.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MIT.Razor.Pages.Service;
using MIT.Razor.Pages.Component;
using MIT.Razor.Pages.Component.MessageBox;

namespace MIT.UI.Main.Login
{
    public class RegisterBase : CommonComponentBase
    {
        protected string? USER_ID { get; set; }
        protected string? PASSWORD { get; set; }
        protected string? PASSWORD_C { get; set; }
        protected string? USER_NAME { get; set; }

        protected string? Message { get; set; }

        protected bool _isComfirm = false;

        protected void OnLogin(MouseEventArgs args)
        {
            var DefaultRoute = ConfigurationService?.Get("DefaultRoute");
            //NavigationManager?.NavigateTo($"{DefaultRoute.ToStringTrim()}/Login");
        }

        protected async Task OnRegister(MouseEventArgs args)
        {
            try
            {
                MessageBoxService?.Show(message: "account chk", type: CommonMsgBoxIcon.Info);
                if (AccountService == null)
                    throw new Exception("Account Service가 null입니다.");

                if (!_isComfirm)
                    throw new Exception("아이디 중복확인 후 회원가입 가능합니다.");

                await AccountService.RegisterAsync(USER_ID.ToStringTrim(),
                    PASSWORD.ToStringTrim(),
                    PASSWORD_C.ToStringTrim(),
                    USER_NAME.ToStringTrim());

                //JSRuntime?.InvokeVoidAsync("alert", "가입이 완료되었습니다.");
                NavigationManager?.NavigateTo("Login");
            }
            catch (Exception ex)
            {
                //JSRuntime?.InvokeVoidAsync("alert", ex.Message);
            }

            Message = $"ID={USER_ID},PWD={PASSWORD},PWDC={PASSWORD_C}";
        }

        protected async Task OnCheckID(MouseEventArgs args)
        {
            try
            {
                MessageBoxService?.Show(message: "account chk", type: CommonMsgBoxIcon.Info);
                if (AccountService == null)
                    throw new Exception("Account Service가 null입니다.");

                var isCheck = await AccountService.CheckIDAsync(USER_ID.ToStringTrim());

                if (isCheck)
                    _isComfirm = true;
                else
                    throw new Exception("중복된 아이디 입니다.");
            }
            catch (Exception ex)
            {
                //JSRuntime?.InvokeVoidAsync("alert", ex.Message);
            }

            Message = $"ID={USER_ID}";
        }
    }
}
