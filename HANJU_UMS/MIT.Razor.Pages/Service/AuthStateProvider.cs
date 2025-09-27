using MIT.DataUtil.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MIT.Razor.Pages.Data;

namespace MIT.Razor.Pages.Service
{
    /// <summary>
    /// 유저 정보 인증 클래스
    /// </summary>
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ISessionStorageService _sessionStorageService;
        private readonly NavigationManager _navigationManager;
        private readonly IConfigurationService _configurationService;

        public AuthStateProvider(ISessionStorageService sessionStorageService,
            NavigationManager navigationManager,
            IConfigurationService configurationService) 
        {
            this._sessionStorageService = sessionStorageService;
            this._navigationManager = navigationManager;
            this._configurationService = configurationService;
        }

        /// <summary>
        /// 유저 정보 권한 체크 함수
        /// </summary>
        /// <returns></returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await _sessionStorageService.GetItemAsync<User>("user");

            if (user != null && !string.IsNullOrEmpty(user.USER_ID))
            {
                var claimUserID = new Claim("USER_ID", user.USER_ID);
                var claimRoles = new Claim("ROLE_GRP_ID", user.ROLE_GRP_ID.ToStringTrim());

                var claimsIdentity = new ClaimsIdentity(new[] { claimUserID, claimRoles }, "JwtAuth");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);



        AuthenticationState acs = new AuthenticationState(claimsPrincipal);
        return acs;
            }
            else
            {
                var defaultRoute = _configurationService?.Get("DefaultRoute");

        //_navigationManager.NavigateTo("/Login");

                _navigationManager.NavigateTo($"{defaultRoute.ToStringTrim()}/Login");
        //await Task.CompletedTask;
        AuthenticationState acs = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        return acs;
            }
        }













  }
}
