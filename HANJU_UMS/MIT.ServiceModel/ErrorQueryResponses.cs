using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ServiceModel
{
    /// <summary>
    /// 에러 메시지 Responses
    /// </summary>
    public class ErrorQueryResponses : QueryResponses
    {
        public ErrorQueryResponses(string message) : base(false, message) {}

        public ErrorQueryResponses(string message, Exception ex) : base(false, message, ex) { }
    }
}
