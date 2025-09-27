using MIT.DataAccess;
using MIT.ServiceModel;
using System.Data;
using System.Threading;

namespace MIT.WebService.Services.Database
{

    /// <summary>
    /// MSSQL 데이터 베이스 서비스
    /// </summary>
    public class MSSQLDatabaseService : DatabaseService
    {
        public MSSQLDatabaseService()
        {
            var timeoutEnv = Environment.GetEnvironmentVariable("ConnectionTimeout_MSSQLConnection");
            Timeout = string.IsNullOrEmpty(timeoutEnv) ? 600 : int.Parse(timeoutEnv);

            ConnectionString = Environment.GetEnvironmentVariable("MSSQLConnection");
            
            Console.WriteLine($"ConnectionString from Environment: {ConnectionString}");

            DatabaseType = DATABASE_TYPE.MSSQL;
        }

        /// <summary>
        /// 데이터 베이스 쿼리 호출
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QueryResponse> Execute(QueryRequest request)
        {
            if (_database == null)
                CreateDatabase();

            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");

            return await _database.ExecuteQueryResponse(request);
        }

        /// <summary>
        /// 데이터 베이스 쿼리 비동기 호출
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QueryResponse> ExecuteAsync(QueryRequest qreq)
        {
            if (_database == null)
                CreateDatabase();

            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");

            return await _database.ExecuteQueryResponseAsync(qreq);
        }

        /// <summary>
        /// 데이터 베이스 쿼리 리스트 호출
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async  Task<QueryResponses> Execute(QueryRequests requests)
        {
            if (_database == null)
                CreateDatabase();

            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");


            return await _database.ExecuteQueryResponse(requests);
        }

        /// <summary>
        /// 데이터 베이스 쿼리 리스트 비동기 호출
        /// </summary>
        /// <param name="requests"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<QueryResponses> ExecuteAsync(QueryRequests requests)
        {
            if (_database == null)
                CreateDatabase();

            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");

            return await _database.ExecuteQueryResponseAsync(requests);
        }

        /// <summary>
        /// 데이터 베이스 해제
        /// </summary>
        public override void Dispose()
        {
            if (_database != null)
                _database.Dispose();
        }
    }
}
