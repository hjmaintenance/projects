using MIT.DataUtil.Common;
using Newtonsoft.Json;
using MIT.ServiceModel;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using MIT.WebService.Common;
using MIT.WebService.Data;
using MIT.WebService.Services.Database;

namespace MIT.WebService.Services {
  /// <summary>
  /// 유저 정보 서비스
  /// </summary>
  public interface IAccountService {
    /// <summary>
    /// 유저 아이디 유무 체크
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<QueryResponse> CheckID(QueryRequest request);
    /// <summary>
    /// 유저 로그인
    /// </summary>
    /// <param name="request"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    Task<QueryResponse> Login(QueryRequest request, HttpContext httpContext);
    /// <summary>
    /// 유저 로그아웃
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<QueryResponse> Logout(QueryRequest request);
    /// <summary>
    /// 유저 정보 등록
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<QueryResponse> Register(QueryRequest request);
    /// <summary>
    /// 토큰 만료시 Refresh 토큰 발급
    /// </summary>
    /// <param name="request"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    Task<QueryResponse> RefreshToken(QueryRequest request, HttpContext httpContext);
  }

  /// <summary>
  /// 유저 정보 서비스
  /// </summary>
  public class AccountService : IAccountService {
    private readonly MSSQLDatabaseService _databaseService;
    private readonly IJwtService _jwtService;
    private readonly IUtilService _utilService;

    public AccountService(MSSQLDatabaseService databaseService
        , IJwtService jwtService
        , IUtilService utilService) {
      _databaseService = databaseService;
      _databaseService.CreateDatabase();
      _jwtService = jwtService;
      _utilService = utilService;
    }

    /// <summary>
    /// 유저 아이디 유무 체크
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<QueryResponse> CheckID(QueryRequest request) {
      try {
        Console.WriteLine("checkID..");
        // 유저 체크 파라미터 셋팅
        var param = request.QueryParameters?.ToDictionary(k => k.ParameterName == null ? "" : k.ParameterName, v => v.ParameterValue);

        if (param == null)
          throw new Exception("CheckID QueryRequest QueryParameters Null.");
        // 아이디가 없을때 예외처리
        if (!param.ContainsKey("USER_ID") || param["USER_ID"].IsStringEmpty())
          throw new Exception("아이디를 입력하세요.");
        // 아이디 유무 체크
        var res = await _databaseService.ExecuteAsync(request);

        return res;
      }
      catch (Exception ex) {
        return new ErrorQueryResponse(ex.Message, ex);
      }
    }

    /// <summary>
    /// 유저 정보 등록
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<QueryResponse> Register(QueryRequest request) {
      try {
        // 유저 등록 정보 파라미터 셋팅
        var param = request.QueryParameters?.ToDictionary(k => k.ParameterName == null ? "" : k.ParameterName, v => v.ParameterValue);

        if (param == null)
          throw new Exception("Register QueryRequest QueryParameters Null.");

        // 아이디가 없을때 예외처리
        if (!param.ContainsKey("USER_ID") || param["USER_ID"].IsStringEmpty())
          throw new Exception("아이디를 입력하세요.");
        // 패스워드가 없을때 예외처리
        if (!param.ContainsKey("PASSWORD") || !param.ContainsKey("PASSWORD_C") ||
            param["PASSWORD"].IsStringEmpty() || param["PASSWORD_C"].IsStringEmpty())
          throw new Exception("패스워드를 입력하세요.");

        var PASSWORD = param["PASSWORD"].ToStringTrim();
        var PASSWORD_C = param["PASSWORD_C"].ToStringTrim();
        // 패스워드가 다른지 체크
        if (!PASSWORD.Equals(PASSWORD_C))
          throw new Exception("패스워드와 패스워드확인이 다릅니다.");
        // 유저 등록정보 데이터 베이스 저장
        var res = await _databaseService.ExecuteAsync(request);

        return res;
      }
      catch (Exception ex) {
        return new ErrorQueryResponse(ex.Message, ex);
      }
    }

    /// <summary>
    /// 유저 로그인 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public async Task<QueryResponse> Login(QueryRequest request, HttpContext httpContext) {
      try {
#if DEBUG
        Console.WriteLine("Account/Login.. " );
#endif

        // 유저 로그인 정보 파라미터 셋팅
        var param = request.QueryParameters?.ToDictionary(k => k.ParameterName == null ? "" : k.ParameterName, v => v.ParameterValue);

        if (param == null)
          throw new Exception("Register QueryRequest QueryParameters Null.");
        // 아이디가 없을때
        if (!param.ContainsKey("USER_ID") || param["USER_ID"].IsStringEmpty())
          throw new Exception("아이디를 입력하세요.");
        // 패스워드가 없을때
        if (!param.ContainsKey("PASSWORD") || param["PASSWORD"].IsStringEmpty())
          throw new Exception("패스워드를 입력하세요.");

#if DEBUG
        string jsonString = JsonConvert.SerializeObject(param);

        Console.WriteLine("Account/Login.. " + jsonString);
#endif
        // 유저정보 로그인 체크
        QueryResponse res = new QueryResponse();
        try {
          res = await _databaseService.ExecuteAsync(request);
        }
        catch(Exception ex) {
          jsonString = JsonConvert.SerializeObject(ex);
          Console.WriteLine("error.. " + jsonString);
          return new ErrorQueryResponse(ex.Message, ex);
        }
        

#if DEBUG
        jsonString = JsonConvert.SerializeObject(res);

        Console.WriteLine("Account/Login.. " + jsonString);
#endif

        if (res == null || !res.IsSuccess)
          throw new Exception("쿼리 실패!");

        var datatable = res?.DataSet.Tables[0];

        if (datatable == null || datatable.Rows.Count == 0)
          throw new Exception("로그인 실패! 아이디 및 비밀번호를 확인해 주세요!");

        // 유저 정보 받기 성공했을때 유저정보 셋팅
        var USER_ID = datatable.Rows[0]["USER_ID"].ToStringTrim();
        // Access JWToken 생성
        var token = _jwtService.GenerateJwtToken(USER_ID);
        // Refresh Token 생성
        var refreshToken = _jwtService.GenerateRefreshToken(USER_ID, _utilService.GetIPAddress());
        refreshToken.ACCESS_TOKEN_ID = token.accessTokenID;

        // Access Token 셋팅
        datatable.Rows[0]["TOKEN"] = token.accessToken;
        // Refresh Token 셋팅
        datatable.Rows[0]["REFRESH_TOKEN"] = refreshToken.REFRESH_TOKEN;
        // Access Token ID 셋팅
        datatable.Rows[0]["ACCESS_TOKEN_ID"] = refreshToken.ACCESS_TOKEN_ID;

        // Refresh Token/AccessTokenID 저장
        QueryRequest reqRefreshToken = new QueryRequest("SP_USER_TOKEN_SAVE", new Dictionary<string, object?>() {
                    { "USER_ID", USER_ID },
                    { "ACCESS_TOKEN_ID", refreshToken.ACCESS_TOKEN_ID.ToStringTrim() },
                    { "REFRESH_TOKEN", refreshToken.REFRESH_TOKEN.ToStringTrim() },
                    { "EXPIRES_DATE", refreshToken.EXPIRES_DATE },
                    { "CREATE_IP", refreshToken.CREATE_IP.ToStringTrim() },
                    { "CREATE_DATE", refreshToken.CREATE_DATE },
                    { "REVOKE_IP", DBNull.Value },
                    { "REVOKE_DATE", DBNull.Value },
                    { "IN_SS_USER_ID", USER_ID },
                    { "IN_SS_TOKEN", "" },
                    { "IN_SS_ACCESS_TOKEN_ID", refreshToken.ACCESS_TOKEN_ID.ToStringTrim() },
                    { "IN_SS_REFRESH_TOKEN", refreshToken.REFRESH_TOKEN.ToStringTrim() },
                    { "IN_CREATE_IP", refreshToken.CREATE_IP.ToStringTrim() },
                });

        var resRefreshToken = await _databaseService.ExecuteAsync(reqRefreshToken);

        if (!resRefreshToken.IsSuccess)
          throw new Exception("토큰 생성에 실패하였습니다!");

        //httpContext.Response.Cookies.Append("REF_TOKEN", refreshToken.REFRESH_TOKEN.ToStringTrim());

        return res == null ? new QueryResponse(false, "생성실패!") : res;
      }
      catch (Exception ex) {
        return new ErrorQueryResponse(ex.Message, ex);
      }
    }

    /// <summary>
    /// 유저 로그아웃
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<QueryResponse> Logout(QueryRequest request) {
      throw new NotImplementedException();
    }

    /// <summary>
    /// 토큰 만료시 Refresh 토큰 발급
    /// </summary>
    /// <param name="request"></param>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public async Task<QueryResponse> RefreshToken(QueryRequest request, HttpContext httpContext) {
      if (request == null)
        return new ErrorQueryResponse("QueryRequest null");

      try {
        // Refresh 토큰 파라메터 정보 셋팅
        var param = request.QueryParameters?.ToDictionary(k => k.ParameterName == null ? "" : k.ParameterName, v => v.ParameterValue);

        if (param == null)
          throw new Exception("Register QueryRequest QueryParameters Null.");

        // 클라이언트에서 요청 온 토큰 정보 체크
        request.QueryName = "SP_USER_TOKEN_SELECT";

        var res = await _databaseService.ExecuteAsync(request);

        if (res == null || !res.IsSuccess)
          throw new Exception("쿼리 실패!");

        var datatable = res?.DataSet.Tables?[0];

        if (datatable == null || datatable.Rows.Count == 0)
          throw new Exception("토큰 값을 찾을 수 없습니다.");

        var USER_ID = datatable.Rows[0]["USER_ID"].ToStringTrim();
        var EXPIRES_DATE = DateTime.Parse(datatable.Rows[0]["EXPIRES_DATE"].ToStringTrim());

        // Refresh 토큰 만료 되었는지 체크
        // 만료 되었을때는 로그아웃 후 다시 재로그인 한다.
        if (DateTime.Now >= EXPIRES_DATE)
          throw new Exception("Refresh 토큰이 만료되었습니다.");

        // AccessToken 생성
        var token = _jwtService.GenerateJwtToken(USER_ID);
        // RefreshToken 생성
        var refreshToken = _jwtService.GenerateRefreshToken(USER_ID, _utilService.GetIPAddress());
        refreshToken.ACCESS_TOKEN_ID = token.accessTokenID;

        // Refresh Token/AccessTokenID 저장
        QueryRequest reqRefreshToken = new QueryRequest("SP_USER_TOKEN_SAVE", new Dictionary<string, object?>() {
                    { "USER_ID", USER_ID },
                    { "ACCESS_TOKEN_ID", refreshToken.ACCESS_TOKEN_ID.ToStringTrim() },
                    { "REFRESH_TOKEN", refreshToken.REFRESH_TOKEN.ToStringTrim() },
                    { "EXPIRES_DATE", refreshToken.EXPIRES_DATE },
                    { "CREATE_IP", refreshToken.CREATE_IP.ToStringTrim() },
                    { "CREATE_DATE", refreshToken.CREATE_DATE },
                    { "REVOKE_IP", DBNull.Value },
                    { "REVOKE_DATE", DBNull.Value },
                });

        var resRefreshToken = await _databaseService.ExecuteAsync(reqRefreshToken);

        if (!resRefreshToken.IsSuccess)
          throw new Exception("토큰 생성에 실패하였습니다!");

        //httpContext.Response.Cookies.Append("REF_TOKEN", refreshToken.REFRESH_TOKEN.ToStringTrim());

        // Refresh Token / AccessToken / AccessTokenID 정보 클라이언트에게 보내기
        var queryParameters = new QueryParameters();
        queryParameters.Add("USER_ID", USER_ID);
        queryParameters.Add("ACCESS_TOKEN", token.accessToken);
        queryParameters.Add("REFRESH_TOKEN", refreshToken.REFRESH_TOKEN.ToStringTrim());
        queryParameters.Add("ACCESS_TOKEN_ID", token.accessTokenID);

        QueryResponse resResult = new QueryResponse();
        resResult.QueryParameters = queryParameters;

        return resResult;
      }
      catch (Exception ex) {
        return new ErrorQueryResponse(ex.Message, ex);
      }
    }
  }
}
