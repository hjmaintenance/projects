using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MIT.DataUtil.Common;
using MIT.WebService.Common;
using MIT.WebService.Services;

namespace MIT.WebService.Attributes {
  /// <summary>
  /// 토큰 인증 후 권한 체크 
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class AuthorizeAttribute : Attribute, IAuthorizationFilter {


    public void OnAuthorization(AuthorizationFilterContext context) {
      // 클래스 및 함수에 Attribute중에 AllowAnonymousAttribute 클래스가 있을경우 권한 체크안함
      var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
      if (allowAnonymous)
        return;

      // JWTTYPE에 따른 예외처리
      var jwtType = context.HttpContext.Items["JWT_TYPE"];
#if DEBUG
      Console.WriteLine("WebService Attributes");
#endif
      if (jwtType == null) {
        context.Result = new UnauthorizedJsonResult(new ServiceModel.UnauthorizedResponse(message: "토큰 타입이 null입니다."));
        return;
      }
      // JWTTYPE에 따른 예외처리
      switch ((JWT_CHECK_TYPE)jwtType) {
        case JWT_CHECK_TYPE.FORGERY:
          context.Result = new UnauthorizedJsonResult(new ServiceModel.UnauthorizedResponse(message: "권한 인증 실패! 위조된 토큰입니다."));
          return;
        case JWT_CHECK_TYPE.NOT_JWT:
          context.Result = new UnauthorizedJsonResult(new ServiceModel.UnauthorizedResponse(message: "권한 인증 실패! Request의 Token 값이 없습니다."));
          return;
        case JWT_CHECK_TYPE.JWT_KEY_EMPTY:
          context.Result = new UnauthorizedJsonResult(new ServiceModel.UnauthorizedResponse(message: "권한 인증 실패! JWT SecretKey키 값이 없습니다."));
          return;
        case JWT_CHECK_TYPE.VALID_NULL:
          context.Result = new UnauthorizedJsonResult(new ServiceModel.UnauthorizedResponse(message: "권한 인증 실패! 토큰 검증 변수가 null입니다."));
          return;
        case JWT_CHECK_TYPE.VALID_EXPIRES:
          context.Result = new UnauthorizedJsonResult(new ServiceModel.UnauthorizedResponse(type: ServiceModel.UnauthorizedType.JWToken_Expires, message: "토큰 인증시간 만료."));
          return;
        case JWT_CHECK_TYPE.SUCCESS:
          break;
      }

#if DEBUG
      Console.WriteLine("Authorized..");
#endif
      // JWTToken 인증에 성공했을경우
      // 유저 정보가 있을때 권한 인증 성공으로 인정
      var USER_ID = context.HttpContext.Items["USER_ID"].ToStringTrim();
      if (string.IsNullOrEmpty(USER_ID))
        context.Result = new UnauthorizedJsonResult(new ServiceModel.UnauthorizedResponse(message: "권한이 없습니다."));
    }
  }
}