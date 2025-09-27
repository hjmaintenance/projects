using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ServiceModel
{
    /// <summary>
    /// 에러 메시지 Response
    /// </summary>
    public class ErrorQueryResponse : QueryResponse
    {
        public ErrorQueryResponse(string message) : base(false, message) {}

        public ErrorQueryResponse(string message, Exception ex) : base(false, message, ex) { }
    }
}
