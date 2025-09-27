using MIT.DataUtil.Common;
using DevExpress.XtraPrinting.Native.MarkupText;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Service;
using MIT.Razor.Pages.Component.MessageBox;
using System.Data;
using MIT.Razor.Pages.Data;
using DevExpress.Blazor;

namespace MIT.Razor.Pages.Component {
  /// <summary>
  /// Razor UI 관련 기본 정보들 클래스
  /// </summary>
  public class CommonUIComponentBase : CommonComponentBase {

    [Inject] public IToastNotificationService ToastService { get; set; }

    /// <summary>
    /// 유저 인증 및 권한 정보
    /// </summary>
    [CascadingParameter]
    public Task<AuthenticationState>? authenticationState { get; set; }
    /// <summary>
    /// 메인 메뉴 버튼 상태 및 버튼 눌렀을때 이벤트 셋팅
    /// </summary>
    [Parameter]
    public MainCommonButtonData? MainCommonButtonData {
      get { return _mainCommonButtonData; }
      set {
        if (value == null)
          return;

        value.SearchButtonClick = EventCallback.Factory.Create(this, Btn_Common_Search_Click);
        value.SaveButtonClick = EventCallback.Factory.Create(this, Btn_Common_Save_Click);
        value.DeleteButtonClick = EventCallback.Factory.Create(this, Btn_Common_Delete_Click);
        value.PrintButtonClick = EventCallback.Factory.Create(this, Btn_Common_Print_Click);
        value.CloseButtonClick = EventCallback.Factory.Create(this, Btn_Common_Close_Click);
        value.FavoritesButtonClick = EventCallback.Factory.Create(this, Btn_Common_Favorites_Click);

        _mainCommonButtonData = value;






      }
    }



    protected bool IsActive => MENU_ID == appData.ActiveTabMenuID;

    /// <summary>
    /// 메인 메뉴 아이디
    /// </summary>
    [Parameter] public string MENU_ID { get; set; } = string.Empty;
    /// <summary>
    /// 메인 메뉴에서 생성된 클래스인지 구분
    /// </summary>
    [Parameter] public bool IsMainMenuUIMode { get; set; } = false;
    /// <summary>
    /// 현재 화면이 모바일 모드인지 구분
    /// </summary>
    public static bool IsMobileMode { get; set; } = false;
    /// <summary>
    /// authenticationState 에서 받은 USER_ID 셋팅
    /// </summary>
    protected string USER_ID { get; set; } = string.Empty;

    /// <summary> 수용가 인지, 한주인 인지의 구분 </summary>
    protected string USER_TYPE { get; set; } = "H";


    protected string USER_THEME { get; set; }
    /// <summary>
    /// authenticationState 에서 받은 ROLE_GRP_ID 셋팅
    /// </summary>
    protected string ROLE_GRP_ID { get; set; } = string.Empty;
    /// <summary>
    /// authenticationState 에서 받은 USER_ID 와 연동된 sessionstorage에 있는 User 클래스에 userName 정보
    /// </summary>
    protected string USER_NAME { get; set; } = string.Empty;
    /// <summary>
    /// authenticationState 에서 받은 USER_ID 와 연동된 sessionstorage에 있는 User 클래스
    /// </summary>
    protected User? UserData { get; set; }

    /// <summary>
    /// 메인 메뉴에서 생성된 클래스에 부여된 버튼 권한 정보
    /// </summary>
    private MainCommonButtonData? _mainCommonButtonData;


    protected bool isAuthCehck = false;

    /// <summary>
    /// authenticationState 권한 정보 체크
    /// OnInitialized 또는 OnAfterRender 에서 사용
    /// </summary>
    /// <returns></returns>
    protected async Task<bool> IsAuthenticatedCheck() {
      if (authenticationState == null)
        return false;

      if(isAuthCehck) return true; 

      var authState = authenticationState.Result;
      var user = authState.User;

      if (user == null || user.Identity == null || !user.Identity.IsAuthenticated) {
        var defaultRoute = ConfigurationService?.Get("DefaultRoute");
        NavigationManager?.NavigateTo($"{defaultRoute.ToStringTrim()}/Login", true);
        return false;
      }
      StateHasChanged();

      var userId = user.FindFirst(s => s.Type == "USER_ID")?.Value;
      var userTp = user.FindFirst(s => s.Type == "USER_TYPE")?.Value;
      var roleGrpId = user.FindFirst(s => s.Type == "ROLE_GRP_ID")?.Value;
      USER_ID = userId.ToStringTrim();
      USER_TYPE = userTp.ToStringTrim();
      ROLE_GRP_ID = roleGrpId.ToStringTrim();

      if (SessionStorageService != null) {
        UserData = await SessionStorageService.GetItemAsync<User>("user");
        USER_NAME = UserData == null ? string.Empty : UserData.USER_NAME.ToStringTrim();
        USER_TYPE = UserData == null ? string.Empty : UserData.USER_TYPE.ToStringTrim();
        ROLE_GRP_ID = UserData == null ? string.Empty : UserData.ROLE_GRP_ID.ToStringTrim();
        USER_THEME = UserData == null ? string.Empty : UserData.Theme.ToStringTrim();

      }
      isAuthCehck = true;
      return true;
    }



    /// <summary>
    /// 메인 UI 공통 Search 버튼 이벤트 함수 호출
    /// </summary>
    /// <returns></returns>
    protected virtual Task Btn_Common_Search_Click() {
      return Task.CompletedTask;
    }

    /// <summary>
    /// 메인 UI 공통 Save 버튼 이벤트 함수 호출
    /// </summary>
    /// <returns></returns>
    protected virtual Task Btn_Common_Save_Click() {
      return Task.CompletedTask;
    }

    /// <summary>
    /// 메인 UI 공통 Delete 버튼 이벤트 함수 호출
    /// </summary>
    /// <returns></returns>
    protected virtual Task Btn_Common_Delete_Click() {
      return Task.CompletedTask;
    }

    /// <summary>
    /// 메인 UI 공통 Print 버튼 이벤트 함수 호출
    /// </summary>
    /// <returns></returns>
    protected virtual Task Btn_Common_Print_Click() {
      return Task.CompletedTask;
    }

    /// <summary>
    /// 메인 UI 공통 Close 버튼 이벤트 함수 호출
    /// </summary>
    /// <returns></returns>
    protected virtual Task Btn_Common_Close_Click() {
      return Task.CompletedTask;
    }

    /// <summary>
    /// 메인 UI 공통 즐겨찾기 버튼 이벤트 함수 호출
    /// </summary>
    /// <returns></returns>
    protected virtual async Task Btn_Common_Favorites_Click() {
      try {
        //ShowLoadingPanel();

        var datatable = await this.DBSaveFavorites();

        if (datatable == null || datatable.Rows.Count == 0) return;



        string msg = "즐겨찾기에 추가되었습니다.";

        // Y이면 즐겨찾기 추가 N이면 즐겨찾기 삭제.
        if (datatable.Rows[0]["RESULT"].ToStringTrim().Equals("Y")) {

          //MessageBoxService?.Show("즐겨찾기에 추가되었습니다.");
        }
        else {
          //MessageBoxService?.Show("즐겨찾기에서 삭제되었습니다.");
          msg = "즐겨찾기에 삭제되었습니다.";
        }


        ToastService.ShowToast(new ToastOptions {
          ProviderName = "Positioning",
          Title = "알림",
          Text = msg
        });


      }
      catch (Exception ex) {
        //CloseLoadingPanel();
        //MessageBoxService?.Show(ex.Message, type: CommonMsgBoxIcon.Error);
      }
      finally {
        //CloseLoadingPanel();
      }
    }

    /// <summary>
    /// 즐겨찾기 저장 및 삭제 DB 저장 함수
    /// </summary>
    /// <returns></returns>
    private async Task<DataTable?> DBSaveFavorites() {
      if (QueryService == null)        return null;

      var datatable = await QueryService.ExecuteDatatableAsync("SP_MAIN_MENU_FAVORITES_SAVE", new Dictionary<string, object?>()      {
                { "USER_ID", USER_ID },
                { "MENU_ID", MENU_ID },
            });

      return datatable;
    }












  }
}