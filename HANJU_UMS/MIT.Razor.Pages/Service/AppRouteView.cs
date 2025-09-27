using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Net;
using System.Security.Principal;

namespace MIT.Razor.Pages.Service
{
    /// <summary>
    /// Launcher에 AppRoute로 통해 페이지 열때마다 렌더 체크 및 계정 권한 관련 체크 가능한 클래스
    /// 현재 사용안함
    /// </summary>
    public class AppRouteView : RouteView
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IConfigurationService? ConfigurationService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;

            if (authorize)
            {
                if (NavigationManager == null)
                    throw new Exception("NavigationManager null");

                var defaultRoute = ConfigurationService?.Get("DefaultRoute");

                var url = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                NavigationManager.NavigateTo($"{defaultRoute}/Login", true);
            }
            else
            {
                base.Render(builder);
            }
        }
    }
}
