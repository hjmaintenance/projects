using MIT.DataUtil.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using MIT.WebService.Data;

namespace MIT.WebService.Services {
  /// <summary>
  /// JWT 체크 타입
  /// </summary>
  public enum JWT_CHECK_TYPE {
    /// <summary>
    /// 토큰 검증 성공
    /// </summary>
    SUCCESS,
    /// <summary>
    /// Request의 Token 값이 없습니다.
    /// </summary>
    NOT_JWT,
    /// <summary>
    /// JWT SecretKey키 값이 없습니다.
    /// </summary>
    JWT_KEY_EMPTY,
    /// <summary>
    /// 토큰 검증 변수가 null입니다.
    /// </summary>
    VALID_NULL,
    /// <summary>
    /// 토큰이 만료되었습니다.
    /// </summary>
    VALID_EXPIRES,
    /// <summary>
    /// 위조된 토큰입니다.
    /// </summary>
    FORGERY,
  }

  /// <summary>
  /// JWToen 생성 인증 서비스
  /// </summary>
  public interface IJwtService {
    /// <summary>
    /// JWToken Access / Refresh 토큰 발급
    /// </summary>
    /// <param name="USER_ID"></param>
    /// <returns></returns>D
    (string accessToken, string accessTokenID) GenerateJwtToken(string USER_ID);
    /// <summary>
    /// JWToken 검증
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<(JWT_CHECK_TYPE jwtCheckType, string USER_ID, string accessTokenID)> ValidateJwtTokenAsync(string token);

    /// <summary>
    /// RefreshToken 다시 발급
    /// </summary>
    /// <param name="USER_ID"></param>
    /// <param name="ip"></param>
    /// <returns></returns>
    RefreshToken GenerateRefreshToken(string USER_ID, string ip);
  }

  /// <summary>
  /// JWToen 생성 인증 서비스
  /// </summary>
  public class JwtService : IJwtService {
    /// <summary>
    /// appsettings.json 데이터 서비스
    /// </summary>
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration) {
      _configuration = configuration;
    }

    /// <summary>
    /// JWToken Access / Refresh 토큰 생성
    /// </summary>
    /// <param name="USER_ID"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public (string accessToken, string accessTokenID) GenerateJwtToken(string USER_ID) {
      // JWToken 시크릿 키 Config에서 가져오기
      string? jwtKey = _configuration["JWT:SecretKey"];
      // JWToken 인증시간 Config에서 가져오기
      int expiresTime = _configuration["JWT:ExpiresTime"].ToInt();
      // 시크릿키 없을때 예외처리
      if (jwtKey == null)
        throw new Exception("JWT Key를 찾을 수 없습니다.");
      // 시크릿키 Byte로 변환
      var key = Encoding.ASCII.GetBytes(jwtKey);
      // AccessTokenID 생성 (저장된 Token정보 데이터 베이스 검증용)
      var tokenID = Guid.NewGuid().ToString();

      // 인증생성
      var claimsIdentity = new ClaimsIdentity(new[] { new Claim("id", USER_ID), new Claim("aid", tokenID) });
      // sha256 으로 인증키 셋팅
      var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
      var descriptor = new SecurityTokenDescriptor {
        Subject = claimsIdentity, // 크레임 정보 셋팅
        Expires = DateTime.Now.AddSeconds(expiresTime), // 인증 만료 시간
                                                        //Expires = DateTime.Now.AddSeconds(10),
        SigningCredentials = signingCredentials // 인증키 셋팅
      };

      // jwtoken 토큰 생성
      var handler = new JwtSecurityTokenHandler();
      var token = handler.CreateToken(descriptor);

      return (handler.WriteToken(token), tokenID);
    }

    /// <summary>
    /// JWToken 검증
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<(JWT_CHECK_TYPE jwtCheckType, string USER_ID, string accessTokenID)> ValidateJwtTokenAsync(string token) {
      // 토큰 정보가 NULL일때
      if (string.IsNullOrEmpty(token))
        return (JWT_CHECK_TYPE.NOT_JWT, string.Empty, string.Empty);

      try {
        // JWToken 시크릿 키 Config에서 가져오기
        string? jwtKey = _configuration["JWT:SecretKey"];

        // 시크릿 키가 NULL일때
        if (string.IsNullOrEmpty(jwtKey))
          return (JWT_CHECK_TYPE.JWT_KEY_EMPTY, string.Empty, string.Empty);

        // 시크릿키 Byte변환
        var key = Encoding.ASCII.GetBytes(jwtKey);

        var handler = new JwtSecurityTokenHandler();
        // 토큰 키 일치 검증
        var validateForgeryToken = await handler.ValidateTokenAsync(token, new TokenValidationParameters {
          ValidateIssuerSigningKey = true, // 키 일치 검증
          IssuerSigningKey = new SymmetricSecurityKey(key), // 시크릿 키 
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = false,
          ClockSkew = TimeSpan.Zero,
        });

        /// 토큰 키 불일치
        if (!validateForgeryToken.IsValid) {
          return (JWT_CHECK_TYPE.FORGERY, string.Empty, string.Empty);
        }

        // 토큰 만료 시간 검증
        var validateExpiredTimeToken = await handler.ValidateTokenAsync(token, new TokenValidationParameters {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true, // 만료시간 검증
          ClockSkew = TimeSpan.Zero,
        });

        // 토큰에서 기본 정보 가져와 셋팅
        var USER_ID = validateForgeryToken.Claims["id"].ToStringTrim();
        var ACCESS_TOKEN_ID = validateForgeryToken.Claims["aid"].ToStringTrim();

        // 토큰 만료시간 지났는지 체크
        if (!validateExpiredTimeToken.IsValid) {
          return (JWT_CHECK_TYPE.VALID_EXPIRES, USER_ID, ACCESS_TOKEN_ID);
        }

        // 토큰 인증 성공
        return (JWT_CHECK_TYPE.SUCCESS, USER_ID, ACCESS_TOKEN_ID);
      }
      catch (Exception ex) {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    /// <summary>
    /// RefreshToken 다시 발급
    /// </summary>
    /// <param name="USER_ID"></param>
    /// <param name="ip"></param>
    /// <returns></returns>
    public RefreshToken GenerateRefreshToken(string USER_ID, string ip) {
      var token = new RefreshToken();
      token.USER_ID = USER_ID;
      token.REFRESH_TOKEN = Guid.NewGuid().ToStringTrim();
      token.EXPIRES_DATE = DateTime.Now.AddDays(1);
      token.CREATE_IP = ip;
      token.CREATE_DATE = DateTime.Now;

      return token;
    }
  }
}
