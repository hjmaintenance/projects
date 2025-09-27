using MIT.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.DataAccess
{
    /// <summary>
    /// 데이터 베이스 관리 함수
    /// </summary>
    public class DatabaseMgr : IDisposable
    {
        private IDatabase? _database = null;

        public DatabaseMgr(DATABASE_TYPE dbType, string? connectionString, int timeout = 600) 
        {
            _database = DatabaseFactory.CreateDatabase(dbType, connectionString, timeout);

            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");
        }

        public async Task<QueryResponse> Execute(QueryRequest request)
        {
            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");

            return await _database.ExecuteQueryResponse(request);
        }

        public async Task<QueryResponse> ExecuteAsync(QueryRequest request)
        {
            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");

            return await _database.ExecuteQueryResponseAsync(request);
        }

        public async Task<QueryResponses> Execute(QueryRequests requests)
        {
            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");

            return await _database.ExecuteQueryResponse(requests);
        }

        public async Task<QueryResponses> ExecuteAsync(QueryRequests requests)
        {
            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");

            return await _database.ExecuteQueryResponseAsync(requests);
        }

        public void Dispose()
        {
            if (_database != null)
                _database.Dispose();
        }
    }
}
