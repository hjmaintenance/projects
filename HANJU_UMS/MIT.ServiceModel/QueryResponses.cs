using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.ServiceModel
{
    /// <summary>
    /// 데이터 베이스 쿼리 전달 리스트 클래스
    /// </summary>
    public class QueryResponses
    {
        public bool IsSuccessMain { get; set; }
        public string? MessageMain { get; set; }
        public Exception? ExceptionMain { get; set; }

        public List<QueryResponse> QueryResponseList { get; set; } = new List<QueryResponse>();

        public QueryResponses()
        { 
        }

        public QueryResponses(bool isSuccess, string? message)
        {
            IsSuccessMain = isSuccess;
            MessageMain = message;
        }

        public QueryResponses(bool isSuccess, string? message, Exception ex)
        {
            IsSuccessMain = isSuccess;
            MessageMain = message;
            ExceptionMain = ex;
        }

        public void Add(QueryResponse item)
        {
            QueryResponseList.Add(item);
        }

        public void Clear()
        {
            QueryResponseList.Clear();
        }

        public bool Contains(QueryResponse item)
        {
            return QueryResponseList.Contains(item);
        }

        public void CopyTo(QueryResponse[] array, int arrayIndex)
        {
            QueryResponseList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<QueryResponse> GetEnumerator()
        {
            return QueryResponseList.GetEnumerator();
        }

        public int IndexOf(QueryResponse item)
        {
            return QueryResponseList.IndexOf(item);
        }

        public void Insert(int index, QueryResponse item)
        {
            QueryResponseList.Insert(index, item);
        }

        public bool Remove(QueryResponse item)
        {
            return QueryResponseList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            QueryResponseList.RemoveAt(index);
        }
    }
}
