using Microsoft.AspNetCore.Components;
using MIT.ServiceModel;
using System.Diagnostics;
using MIT.DataUtil.Common;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Data;
using MIT.Razor.Pages.Data;
using MIT.Razor.Pages.Common;
using DevExpress.Blazor.Internal;
using DevExpress.Blazor;
using DevExpress.ClipboardSource.SpreadsheetML;
using Newtonsoft.Json.Linq;
using MIT.Razor.Pages.Component.MessageBox;

namespace MIT.Razor.Pages.Service {
  /// <summary>
  /// 계정 관련 인터페이스 서비스
  /// </summary>
  public interface IAccountService {
    /// <summary>
    /// 아이디 체크 관련 호출 함수
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    Task<bool> CheckIDAsync(string id);
    /// <summary>
    /// 계정 정보 DB 저장
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pwd"></param>
    /// <param name="pwd_c"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task RegisterAsync(string id, string pwd, string pwd_c, string userName);
    /// <summary>
    /// 계정 로그인
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pwd"></param>
    /// <returns></returns>
    Task<bool> LoginAsync(string id, string pwd, string isHan);
    /// <summary>
    /// 계정 로그아웃
    /// </summary>
    /// <returns></returns>
    Task<bool> LogOutAsync();
  }

  /// <summary>
  /// 계정 관련 클래스 구현 서비스
  /// </summary>
  public class AccountService : IAccountService {
    private readonly IQueryService _queryService;
    private readonly ISessionStorageService _sessionStorageService;
    private readonly NavigationManager _navigationManager;
    private readonly IConfigurationService _configurationService;

    public AccountService(IQueryService queryService,
        ISessionStorageService sessionStorageService,
        NavigationManager navigationManager,
        IConfigurationService configurationService) {
      this._queryService = queryService;
      this._sessionStorageService = sessionStorageService;
      this._navigationManager = navigationManager;
      this._configurationService = configurationService;
    }


    /// <summary>
    /// 아이디 체크 관련 호출 함수
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> CheckIDAsync(string id) {
#if DEBUG
      Console.WriteLine("[Razor.Pages]CheckIDAsync");
#endif
      var dataTable = await _queryService.ExecuteDatatableAsync("Account/CheckID", "SP_USER_ID_OVERLAP_CHECK_SELECT", new Dictionary<string, object?>() {
                { "USER_ID", id },
            });

      if (dataTable == null || dataTable.Rows.Count == 0)
        throw new Exception("Sussess Failed.");

      return dataTable.Rows[0]["CNT"].ToStringTrim().Equals("0");
    }

    /// <summary>
    /// 계정 정보 DB 저장
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pwd"></param>
    /// <param name="pwd_c"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task RegisterAsync(string id, string pwd, string pwd_c, string userName) {
      await _queryService.ExecuteNonQuery("Account/Register", "SP_USER_REGISTER_SAVE", new Dictionary<string, object?>() {
                { "USER_ID", id },
                { "PASSWORD", EncryptHelper.EncryptSHA512(pwd) },
                { "PASSWORD_C", EncryptHelper.EncryptSHA512(pwd_c) },
                { "USER_NAME", userName },
            });
    }

    /// <summary>
    /// 계정 로그인
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pwd"></param>
    /// <returns></returns>
    public async Task<bool> LoginAsync(string id, string pwd, string isHan) {
#if DEBUG
      Console.WriteLine("[Razor.Pages]LoginAsync.." + id.ToString() + isHan.ToString() +",  "+ EncryptHelper.EncryptSHA512(pwd));

      var datatable = await _queryService.ExecuteDatatableAsync("Account/Login", "SP_USER_LOGIN_SELECT", new Dictionary<string, object?>() {
                { "USER_ID", id },
                { "PASSWORD", EncryptHelper.EncryptSHA512(pwd) },
                { "PASSWORD_C", pwd },
                { "IS_HAN", isHan },
            });
      
#endif

#if DEBUG
      Console.WriteLine("[Razor.Pages]LoginAsync..called.." + datatable.ToString());


#endif

      if (datatable == null || datatable.Rows.Count == 0)
        return false;

      DataRow dr = datatable.Rows[0];

      User user = new User();
      user.USER_ID = datatable.Rows[0]["USER_ID"].ToStringTrim();
      user.ROLE_GRP_ID = datatable.Rows[0]["ROLE_GRP_ID"].ToStringTrim();
      user.USER_NAME = datatable.Rows[0]["USER_NAME"].ToStringTrim();
      user.USER_NAME_ENG = datatable.Rows[0]["USER_NAME_ENG"].ToStringTrim();
      user.EMP_NO = datatable.Rows[0]["EMP_NO"].ToStringTrim();
      user.CUST_CODE = datatable.Rows[0]["CUST_CODE"].ToStringTrim();
      user.DEPT_CODE = datatable.Rows[0]["DEPT_CODE"].ToStringTrim();
      user.OFFICE_NUM = datatable.Rows[0]["OFFICE_NUM"].ToStringTrim();
      user.PHONE_NUM = datatable.Rows[0]["PHONE_NUM"].ToStringTrim();
      user.ADDRESS_1 = datatable.Rows[0]["ADDRESS_1"].ToStringTrim();
      user.ADDRESS_2 = datatable.Rows[0]["ADDRESS_2"].ToStringTrim();
      user.COUNTRY = datatable.Rows[0]["COUNTRY"].ToStringTrim();
      user.CREATETIME = DateTime.Parse(datatable.Rows[0]["CREATETIME"].ToStringTrim());
      user.Token = datatable.Rows[0]["TOKEN"].ToStringTrim();
      user.ACCESS_TOKEN_ID = datatable.Rows[0]["ACCESS_TOKEN_ID"].ToStringTrim();
      user.REFRESH_TOKEN = datatable.Rows[0]["REFRESH_TOKEN"].ToStringTrim();
      user.USER_TYPE = datatable.Rows[0]["USER_TYPE"].ToStringTrim();


#if DEBUG
      Console.WriteLine("Logged in.. DataTable" + user.USER_ID + user.ROLE_GRP_ID + user.USER_NAME);
#endif
      user.Theme = dr["THEME"].ToStringTrim();
      user.Size = dr["BSIZE"].ToStringTrim();
      user.Photo = dr["PHOTO"].ToStringTrim();

      // 계정 정보 SessionStorage에 저장
      await _sessionStorageService.SetItemAsync("user", user);

      var defaultRoute = _configurationService?.Get("DefaultRoute");

      _navigationManager.NavigateTo($"{defaultRoute.ToStringTrim() + "/"}", true);

      return true;
    }

    /// <summary>
    /// 계정 로그아웃
    /// </summary>
    /// <returns></returns>
    public async Task<bool> LogOutAsync() {
      var defaultRoute = _configurationService?.Get("DefaultRoute");

      await _sessionStorageService.RemoveItemAsync("user");
      _navigationManager.NavigateTo($"{defaultRoute.ToStringTrim()}/Login", true);

      return true;
    }
  }
}
