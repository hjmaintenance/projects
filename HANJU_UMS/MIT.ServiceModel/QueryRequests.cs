using System;
using System.Collections;
using System.Collections.ObjectModel;

namespace MIT.ServiceModel
{
    /// <summary>
    /// 데이터 베이스 쿼리 요청 리스트 클래스
    /// </summary>
    public class QueryRequests
    {
        public List<QueryRequest> QueryResponseList { get; set; } = new List<QueryRequest>();

        public void Add(QueryRequest item)
        {
            QueryResponseList.Add(item);
        }

        public void Add(QueryRequests? items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                QueryResponseList.Add(item);
            }
        }

        public void Clear()
        {
            QueryResponseList.Clear();
        }

        public bool Contains(QueryRequest item)
        {
            return QueryResponseList.Contains(item);
        }

        public void CopyTo(QueryRequest[] array, int arrayIndex)
        {
            QueryResponseList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<QueryRequest> GetEnumerator()
        {
            return QueryResponseList.GetEnumerator();
        }

        public int IndexOf(QueryRequest item)
        {
            return QueryResponseList.IndexOf(item);
        }

        public void Insert(int index, QueryRequest item)
        {
            QueryResponseList.Insert(index, item);
        }

        public bool Remove(QueryRequest item)
        {
            return QueryResponseList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            QueryResponseList.RemoveAt(index);
        }
    }
}
