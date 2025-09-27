using Microsoft.AspNetCore.Mvc;
using MIT.ServiceModel;
using MIT.WebService.Services;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace MIT.WebService.Common
{
    /// <summary>
    /// 권한 인증 실패 결과 Result클래스
    /// </summary>
    public class UnauthorizedJsonResult : JsonResult
    {
        public UnauthorizedJsonResult(UnauthorizedResponse value) : base(value)
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }

        public UnauthorizedJsonResult(UnauthorizedResponse value, object? serializerSettings) : base(value, serializerSettings)
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
