using MIT.WebService.Services;
using System.Diagnostics.Metrics;

namespace MIT.WebService.Middlewares
{
    /// <summary>
    /// JWToken 인증 체크 미들웨어
    /// </summary>
    public class JWTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtService jwtService)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            // 클라이언트에서 보내온 Request Header에서 Authorization Token 정보 추출
            var auth = context.Request.Headers["Authorization"].FirstOrDefault();
            if (auth == null)
            {
                await _next(context);
                return;
            }
            // Token 정보 추출
            // 배열 0번째는 Bearer, 1번째는 토큰 정보
            var arrAuth = auth.Split(" ");
            var token = arrAuth.Last();
            // 토큰 검증
            var result = await jwtService.ValidateJwtTokenAsync(token);
            // 검증한 토큰 타입
            var jwtType = result.jwtCheckType;
            // AuthorizeAttribute에서 토큰 타입 체크를 위해서 Item에 JWT_TYPE 셋팅
            context.Items["JWT_TYPE"] = jwtType;

            // 토큰인증에 성공하였을 경우 USER_ID  값 셋팅
            if (jwtType == JWT_CHECK_TYPE.SUCCESS && !string.IsNullOrEmpty(result.Item2))
            {
                context.Items["USER_ID"] = result.USER_ID;
            }

            await _next(context);

            
        }
    }
}
