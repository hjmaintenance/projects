using DevExpress.DataProcessing;
using MIT.DataUtil.Common;
using System.Net;

namespace MIT.WebService.Services
{
    /// <summary>
    /// WebService Util 서비스
    /// </summary>
    public interface IUtilService
    {
        string GetIPAddress();
    }

    /// <summary>
    /// WebService Util 서비스
    /// </summary>
    public class UtilService : IUtilService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UtilService(IHttpContextAccessor httpContextAccessor) 
        { 
            _contextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 클라이언트 아이피 가져오기.. 이거 구형로직이다..
        /// </summary>
        /// <returns></returns>
        public string GetIPAddress()
        {
      
            if (_contextAccessor.HttpContext == null)
                return string.Empty;

            if (_contextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                return _contextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToStringTrim();
            else
                return _contextAccessor.HttpContext.Connection.RemoteIpAddress == null ? string.Empty : _contextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToStringTrim();
        }


    public static string GetIPAddressX(IHttpContextAccessor ihca) {

      if (ihca.HttpContext == null)
        return string.Empty;

      if (ihca.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        return ihca.HttpContext.Request.Headers["X-Forwarded-For"].ToStringTrim();
      else
        return ihca.HttpContext.Connection.RemoteIpAddress == null ? string.Empty : ihca.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToStringTrim();
    }

    public static string GetIPAddress(IHttpContextAccessor ihca) {
      if (ihca.HttpContext == null)
        return string.Empty;





      IPAddress remoteIpAddress = ihca.HttpContext.Connection.RemoteIpAddress;
      string result = "";
      if (remoteIpAddress != null) {
        if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) {
          remoteIpAddress = System.Net.Dns.GetHostEntry(remoteIpAddress).AddressList
  .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
        }
        result = remoteIpAddress.ToString();
      }


      return result;



    }
  }
}
