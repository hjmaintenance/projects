using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ServiceModel
{
    /// <summary>
    /// 데이터 베이스 쿼리 요청 클래스
    /// </summary>
    public class QueryRequest
    {
        public string? QueryName { get; set; }

        public QueryParameters? QueryParameters { get; set; }

        public bool ReturnQueryParameter { get; set; } = false;

        public QueryRequest() { }

        public QueryRequest(string queryName, QueryParameters queryParameters)
        {
            QueryName = queryName;
            QueryParameters = queryParameters;
        }

        public QueryRequest(string queryName, Dictionary<string, object?> dictionary)
        {
            QueryName = queryName;
            QueryParameters = new QueryParameters(dictionary);
        }


    public QueryRequest(string queryName, Dictionary<string, object?> dictionary, string prefix)
    {
      QueryName = queryName;
      QueryParameters = new QueryParameters(dictionary, prefix);
    }



  }
}
