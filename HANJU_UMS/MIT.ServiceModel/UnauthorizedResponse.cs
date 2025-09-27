using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ServiceModel
{
    /// <summary>
    /// Jwtoken 권한 체크 타입
    /// </summary>
    public enum UnauthorizedType
    {
        Unauthorized,
        JWToken_Expires,

    }

    /// <summary>
    /// JWTokne에 대한 권한 없을때 클라이언트에게 보내는 Response 메시지
    /// </summary>
    public class UnauthorizedResponse
    {
        public UnauthorizedType UnauthorizedType { get; set; } = UnauthorizedType.Unauthorized;
        public string? Message { get; set; } = string.Empty;

        public UnauthorizedResponse() { }

        public UnauthorizedResponse(UnauthorizedType type = UnauthorizedType.Unauthorized, string? message = "")
        {
            this.UnauthorizedType = type;
            this.Message = message;
        }
    }
}
