using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MIT.ServiceModel
{
    /// <summary>
    /// 기본 WebService Parameter Message 리스트 클래스
    /// </summary>
    public class QueryParameters : IList<QueryParameter>
    {
        public QueryParameters() { }

        public QueryParameters(Dictionary<string, object?> dictionary, string prefix = "IN_") 
        {
            if (dictionary != null)
            {
                foreach (var pair in dictionary)
                {
                    this.Add(new QueryParameter() { 
                        ParameterDirection = System.Data.ParameterDirection.Input,
                        ParameterName = $"{pair.Key}",
                        Prefix = prefix ,
                        ParameterValue = pair.Value,
                    });
                }
            }
        }

        private Collection<QueryParameter> _queryParameterList = new Collection<QueryParameter>();

        public QueryParameter this[int index] { get => _queryParameterList[index]; set => _queryParameterList[index] = value; }

        public int Count => _queryParameterList.Count;

        public bool IsReadOnly => ((IList<QueryParameter>)_queryParameterList).IsReadOnly;

        public void Add(string name, object? value, string prefix = "IN_", System.Data.ParameterDirection dir = System.Data.ParameterDirection.Input)
        {
            this.Add(new QueryParameter($"{name}", value, prefix, dir));
        }

        public void Add(QueryParameter item)
        {
            _queryParameterList.Add(item);
        }

        public void Clear()
        {
            _queryParameterList.Clear();
        }

        public bool Contains(QueryParameter item)
        {
            return _queryParameterList.Contains(item);
        }

        public void CopyTo(QueryParameter[] array, int arrayIndex)
        {
            _queryParameterList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<QueryParameter> GetEnumerator()
        {
            return _queryParameterList.GetEnumerator();
        }

        public int IndexOf(QueryParameter item)
        {
            return this._queryParameterList.IndexOf(item);
        }

        public void Insert(int index, QueryParameter item)
        {
            this._queryParameterList.Insert(index, item);
        }

        public bool Remove(QueryParameter item)
        {
            return this._queryParameterList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _queryParameterList.GetEnumerator();
        }
    }
}
