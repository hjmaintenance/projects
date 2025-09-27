using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using DevExpress.Pdf;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MIT.ServiceModel;
using MIT.Razor.Pages.Common;
using MIT.Razor.Pages.Data;
using MIT.DataUtil.Common;
using DevExpress.Pdf.Native;
using DevExpress.Blazor;
using System.ServiceModel.Channels;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
//using DevExpress.Pdf.Native.BouncyCastle.Asn1.Ocsp;

namespace MIT.Razor.Pages.Service {
  /// <summary>
  /// 웹서비스에 Http 통신 관련 인터페이스 서비스
  /// </summary>
  public interface IHttpService {

    //Task PostAsync(string uri, object value);
    Task<T?> PostAsync<T>(string uri, object value, string uriName = "BaseAddress");
    string GetUrl();
    string GetUrl(string uriName);
  }

  /// <summary>
  /// 웹서비스에 Http 통신 관련 클래스 서비스
  /// </summary>
  public class HttpService : IHttpService {
    private HttpClient _httpClient;
    private ISessionStorageService _sessionStorageService;
    private readonly NavigationManager _navigationManager;
    private readonly IJSRuntime _jsRuntime;
    private readonly IConfigurationService _configurationService;

    private static Dictionary<string, string> _uri = new Dictionary<string, string>();

    public HttpService(HttpClient httpClient
        , ISessionStorageService sessionStorageService
        , NavigationManager navigationManager
        , IJSRuntime JS,
        IConfigurationService configurationService) {
      _httpClient = httpClient;
      _sessionStorageService = sessionStorageService;
      this._navigationManager = navigationManager;
      _jsRuntime = JS;
      this._configurationService = configurationService;
    }

    /// <summary>
    /// 초기 URi 주소 셋팅
    /// </summary>
    /// <param name="name"></param>
    /// <param name="uri"></param>
    public void AddUri(string name, string uri) {
      if (_uri.ContainsKey(name))
        _uri.Add(name, uri);
      else
        _uri[name] = uri;
    }

    public string GetUrl() {
      return GetUrl("BaseAddress");
    }
    public string GetUrl(string uriName = "BaseAddress") {
      return _uri[uriName];
    }

    /// <summary>
    /// Post 데이터 보내기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uri"></param>
    /// <param name="value"></param>
    /// <param name="uriName"></param>
    /// <returns></returns>
    public async Task<T?> PostAsync<T>(string uri, object value, string uriName = "BaseAddress") {
      return await SendRequestAsync<T>(CreateRequest(HttpMethod.Post, new Uri($"{_uri[uriName]}{uri}"), value), uriName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="request"></param>
    /// <param name="uriName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task<T?> SendRequestAsync<T>(HttpRequestMessage request, string uriName) {
      // JWT Token Authorization 헤더에 추가
      await SetHeaderJwt(request);

      // WebService 호출
      var response = await _httpClient.SendAsync(request);

      // Response 에러 체크 핸들러
      response = await ErrorHandle(request, response, uriName);
      // 메시지가 성공적으로 들어왔을때 Body Content Stream 받기
      var stream = await response.Content.ReadAsStreamAsync();
      using var reader = new StreamReader(stream);
      // Body Content Stream 데이터를 String 데이터 변환
      var json = await reader.ReadToEndAsync();

      if (json == null)
        throw new Exception("변환에 실패하였습니다.");
      // json 데이터를 성공적으로 들어왔을때
      // 데이터 변환 후 리턴
      JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
      serializerSettings.Formatting = Formatting.Indented;
      serializerSettings.NullValueHandling = NullValueHandling.Include;
      serializerSettings.Converters.Add(new HttpJsonConverter());
      return JsonConvert.DeserializeObject<T>(json, serializerSettings);
    }

    /// <summary>
    /// HttpRequestMessage 클래스 생성
    /// Request할 주소, ContentType 셋팅
    /// </summary>
    /// <param name="method"></param>
    /// <param name="uri"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, object? value = null) {
      var request = new HttpRequestMessage(method, uri);

      if (value != null) {
        var serialize = JsonConvert.SerializeObject(value);
        request.Content = new StringContent(serialize, Encoding.UTF8, "application/json");
      }

      return request;
    }

    /// <summary>
    /// HttpRequestMessage에 User정보에 있는 인증 토큰을 Authorization헤더에 셋팅
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task SetHeaderJwt(HttpRequestMessage request) {
      var user = await _sessionStorageService.GetItemAsync<User>("user");
      if (user != null)
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", user.Token);
    }

    /// <summary>
    /// Resport 받은 메시지에 인증 만료 및 에러 메시지 체크 함수
    /// </summary>
    /// <param name="request"></param>
    /// <param name="res"></param>
    /// <param name="uriName"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task<HttpResponseMessage> ErrorHandle(HttpRequestMessage request, HttpResponseMessage res, string uriName) {
      // 메시지가 성공정으로 들어왔는지 체크
      if (!res.IsSuccessStatusCode) {
        // 에러 메시지 Stream 으로 읽기
        var stream = await res.Content.ReadAsStreamAsync();

        using var reader = new StreamReader(stream);
        // 에러메시지 Stream 데이터에서 String으로 변환
        var json = await reader.ReadToEndAsync();

        // string데이터에서 읽은 기본 에러 메시지 셋팅
        var error = $"StatusCode : {res.StatusCode}, ErrorMessage : {json}";

        // 권한 없는 코드를 제외하고 나머지 에러는 예외처리 리턴
        if (res.StatusCode != System.Net.HttpStatusCode.Unauthorized)
          throw new Exception(error);

        // UnauthorizedResponse 권한 없음 Response Json 데이터를 클래스로 변환
        JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
        serializerSettings.Formatting = Formatting.Indented;
        serializerSettings.NullValueHandling = NullValueHandling.Include;
        var unauthResponse = JsonConvert.DeserializeObject<UnauthorizedResponse>(json, serializerSettings);

        // User정보가 있는지 체크
        var user = await _sessionStorageService.GetItemAsync<User>("user");

        if (user == null || unauthResponse == null || unauthResponse.UnauthorizedType == UnauthorizedType.Unauthorized) {
          // 유저 정보가 없고 메시지 권한이 없을 경우 
          // 유저 정보를 삭제
          await _sessionStorageService.RemoveItemAsync("user");
          // 에러 경고 메시지 출력
          //await _jsRuntime.InvokeVoidAsync("alert", error);

          var defaultRoute = _configurationService?.Get("DefaultRoute");

          // 로그인 페이지로 이동
          _navigationManager.NavigateTo($"{defaultRoute.ToStringTrim()}/Login", true);
        }
        else if (unauthResponse.UnauthorizedType == UnauthorizedType.JWToken_Expires) {
          // JWToken 정보가 만료된 경우

          var defaultRoute = _configurationService?.Get("DefaultRoute");
          _navigationManager.NavigateTo($"{defaultRoute.ToStringTrim()}/Login", true);

          // RefresToken 요청
          //res = await SendRefreshTokenRequest(request, user, uriName, error);

          return res;
        }
      }

      return res;
    }

    private async Task<HttpResponseMessage> SendRefreshTokenRequest(HttpRequestMessage request, User user, string uriName, string errorMessage) {
      // RefreshToken 요청 쿼리 생성
      QueryRequest queryRefreshToken = new QueryRequest("", new Dictionary<string, object?>() {
                            { "REFRESH_TOKEN", user.REFRESH_TOKEN.ToStringTrim() },
                            { "USER_ID", user.USER_ID.ToStringTrim() },
                            { "ACCESS_TOKEN_ID", user.ACCESS_TOKEN_ID.ToStringTrim() },
                        });

      // RefreshToken Request 생성
      var requestRefreshToken = CreateRequest(HttpMethod.Post, new Uri($"{_uri[uriName]}Account/RefreshToken"), queryRefreshToken);
      // RefhreshToken Request WebService 호출
      var responseRefreshToken = await _httpClient.SendAsync(requestRefreshToken);
      // RefhreshToken Response Stream 으로 읽기
      var stream = await responseRefreshToken.Content.ReadAsStreamAsync();

      using var reader = new StreamReader(stream);
      // RefhreshToken Response Stream 데이터 String 으로 변환
      var json = await reader.ReadToEndAsync();

      // json 데이터가 NULL일때
      if (json == null) {
        // 유저정보삭제
        await _sessionStorageService.RemoveItemAsync("user");
        // 에러메시지 호출
        //await _jsRuntime.InvokeVoidAsync("alert", errorMessage);

        var defaultRoute = _configurationService?.Get("DefaultRoute");

        // 로그인 페이지로 이동
        _navigationManager.NavigateTo($"{defaultRoute.ToStringTrim()}/Login", true);
        throw new Exception("변환에 실패하였습니다.");
      }

      // RefreshToken 정보 Json 데이터를 QueryResponse로 변환
      JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
      serializerSettings.Formatting = Formatting.Indented;
      serializerSettings.NullValueHandling = NullValueHandling.Include;
      //serializerSettings.Converters.Add(new HttpJsonConverter());
      var resJson = JsonConvert.DeserializeObject<QueryResponse>(json, serializerSettings);
      // Refresh Token정보 Dictionary로 변환
      var param = resJson?.QueryParameters?.ToDictionary(k => k.ParameterName == null ? "" : k.ParameterName, v => v.ParameterValue);

      // Refresh 정보가 없다면
      if (param == null) {
        // 유저정보삭제
        await _sessionStorageService.RemoveItemAsync("user");
        // 에러메시지 호출
        //await _jsRuntime.InvokeVoidAsync("alert", errorMessage);

        var defaultRoute = _configurationService?.Get("DefaultRoute");

        // 로그인 페이지로 이동
        _navigationManager.NavigateTo($"{defaultRoute.ToStringTrim()}/Login", true);
        throw new Exception("변환에 실패하였습니다.");
      }

      // 유저 정보에 새로 받은 Access 토큰 아이디 셋팅
      user.ACCESS_TOKEN_ID = param["ACCESS_TOKEN_ID"].ToStringTrim();
      // 유저 정보에 새로 받은 Refresh 토큰 셋팅
      user.REFRESH_TOKEN = param["REFRESH_TOKEN"].ToStringTrim();
      // 유저 정보에 새로 받은 Access 토큰 셋팅
      user.Token = param["ACCESS_TOKEN"].ToStringTrim();
      // SessionStorage에 user 정보 저장
      await _sessionStorageService.SetItemAsync("user", user);

      // 다시 기존에 보낼려고 했었던 RequestMessage 생성
      var newRequest = new HttpRequestMessage(request.Method, request.RequestUri);
      newRequest.Content = request.Content;
      // 다시 Reqeust 보내기
      var res = await SendTryRequestAsync(newRequest);

      return res;
    }

    /// <summary>
    /// Refresh Token 새로 발급받은 후 이전에 보낼려고 했었던 요청 다시 보내기
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<HttpResponseMessage> SendTryRequestAsync(HttpRequestMessage request) {
      // header에 Authorization Jwtoken 정보 셋팅
      await SetHeaderJwt(request);
      // WebService 호충
      var response = await _httpClient.SendAsync(request);
      // 에러체크 
      await TryErrorHandle(response);

      return response;
    }

    /// <summary>
    /// Refresh 토큰 새로 발급 받은 후 다시 Request 오류발생시 간단히 오류만 체크
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private async Task TryErrorHandle(HttpResponseMessage res) {
      if (!res.IsSuccessStatusCode) {
        // 에러 메시지 Stream 읽기
        var stream = await res.Content.ReadAsStreamAsync();

        using var reader = new StreamReader(stream);
        // 에러 메시지 Stream 데이터를 String 으로 변환
        var json = await reader.ReadToEndAsync();
        // 에러메시지 셋팅
        var error = $"StatusCode : {res.StatusCode}, ErrorMessage : {json}";
        // Refresh 토큰을 새로 발급 받은 후에도 인증에 실패하였을때
        if (res.StatusCode == System.Net.HttpStatusCode.Unauthorized) {
          // 유저정보삭제
          await _sessionStorageService.RemoveItemAsync("user");
          // 에러메시지 출력
          //await _jsRuntime.InvokeVoidAsync("alert", error);

          var defaultRoute = _configurationService?.Get("DefaultRoute");

          // 로그인 페이지로 이동
          _navigationManager.NavigateTo($"{defaultRoute.ToStringTrim()}/Login", true);
        }

        throw new Exception(error);
      }
    }
  }
}