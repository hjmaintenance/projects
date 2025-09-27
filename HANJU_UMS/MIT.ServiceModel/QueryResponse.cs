using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ServiceModel
{
    /// <summary>
    /// 데이터 베이스 쿼리 전달 클래스
    /// </summary>
    public class QueryResponse
    {
        public QueryResponse()
        {

        }

        public QueryResponse(string message)
        {
            this.Message = message;
        }

        public QueryResponse(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public QueryResponse(bool isSuccess, string message, Exception ex)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Exception = ex;
        }

        public QueryParameters? QueryParameters { get; set; }
        public Exception? Exception { get; set; }

        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public DataSet DataSet { get; set; } = new DataSet();
    }

    public class QueryResponse<T>
    {
        public QueryResponse()
        {

        }

        public QueryResponse(string message)
        {
            this.Message = message;
        }

        public QueryResponse(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public QueryResponse(bool isSuccess, string message, Exception ex)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Exception = ex;
        }

        public QueryParameters? QueryParameters { get; set; }
        public Exception? Exception { get; set; }

        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public List<List<T>>? datatableList { get; set; } = new List<List<T>>();
    }
}
