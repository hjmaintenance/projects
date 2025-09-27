using MIT.ServiceModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace MIT.DataAccess
{
    /// <summary>
    /// 데이터 베이스 타입
    /// 현재는 MSSQL만 구현되어있음
    /// </summary>
    public enum DATABASE_TYPE
    {
        NONE,
        MSSQL,
        ORACLE,
        MYSQL,
        MARIADB
    }

    /// <summary>
    /// 기본 데이터 베이스 인터페이스
    /// </summary>
    public interface IDatabase : IDisposable
    {
    /// <summary>
    /// QueryRequest로 요청한 DB 쿼리 실행 함수
    /// </summary>
    /// <param name="request"></param>
    /// <param name="isTransation"></param>
    /// <returns></returns>
    Task<QueryResponse> ExecuteQueryResponse(QueryRequest request, bool isTransation = true);

        /// <summary>
        /// QueryRequest로 요청한 DB 쿼리 실행 비동기 함수
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        Task<QueryResponse> ExecuteQueryResponseAsync(QueryRequest request, bool isTransation = true);

    /// <summary>
    /// QueryRequest 리스트로 요청한 DB 쿼리 실행 함수
    /// </summary>
    /// <param name="requests"></param>
    /// <param name="isTransation"></param>
    /// <returns></returns>
    Task<QueryResponses> ExecuteQueryResponse(QueryRequests requests, bool isTransation = true);

        /// <summary>
        /// QueryRequest 리스트로 요청한 DB 쿼리 실행 비동기 함수
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        Task<QueryResponses> ExecuteQueryResponseAsync(QueryRequests requests, bool isTransation = true);
    }

    /// <summary>
    /// 기본 데이터 베이스 추상 클래스
    /// </summary>
    internal abstract class Database : IDatabase, IDisposable
    {
        /// <summary>
        /// 데이터 베이스 타입
        /// </summary>
        protected DATABASE_TYPE _emDbType = DATABASE_TYPE.NONE;
        /// <summary>
        /// 데이터 베이스 Connection 클래스
        /// </summary>
        protected DbConnection? _dbConnection = null;
        /// <summary>
        /// 데이터 베이스 Command 클래스
        /// </summary>
        protected DbCommand? _dbCommand = null;
        /// <summary>
        /// 데이터 베이스 ConnectString 문자열
        /// </summary>
        protected string _connectionString = string.Empty;
        /// <summary>
        /// 데이터 베이스 요청 대기 시간
        /// 기본 10분
        /// </summary>
        public int Timeout { get; set; } = 60 * 10;

        public Database(string connectionString, DATABASE_TYPE emDbType, int timeout = 600) 
        { 
            this._connectionString = connectionString;
            this._emDbType = emDbType;
            this.Timeout = timeout; 
        }
        /// <summary>
        /// 데이터 베이스 Connect 호출 함수
        /// </summary>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        public abstract DbCommand Connect(bool isTransation = true);
        /// <summary>
        /// 데이터 베이스 Connect 비동기 호출 함수
        /// </summary>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        public abstract Task<DbCommand> ConnectAsync(bool isTransation = true);

        /// <summary>
        /// 데이터 베이스 Disconnect 호출 함수
        /// </summary>
        public void DisConnect()
        {
            if (_dbCommand != null)
            {
                if (_dbCommand.Transaction != null)
                {
                    _dbCommand.Transaction.Dispose();
                }

                _dbCommand.Dispose();
            }

            if (_dbConnection != null)
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
        }

        /// <summary>
        /// 데이터 베이스 Disconnect 비동기 호출 함수
        /// </summary>
        /// <returns></returns>
        public async Task<bool> DisConnectAsync()
        {
            if (_dbCommand != null)
            {
                if (_dbCommand.Transaction != null)
                {
                    await _dbCommand.Transaction.DisposeAsync();
                }

                await _dbCommand.DisposeAsync();
            }

            if (_dbConnection != null)
            {
                await _dbConnection.CloseAsync();
                await _dbConnection.DisposeAsync();
            }

            return true;
        }

        /// <summary>
        /// 데이터 베이스 프로시저 파라미터 정보 셋팅 가상 함수
        /// </summary>
        /// <param name="request"></param>
        public abstract void SetProcedureParametersInfo(QueryRequest request);
        /// <summary>
        /// 데이터 베이스 프로시저 파라미터 정보 셋팅 가상 비동기 함수
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public abstract Task<bool> SetProcedureParametersInfoAsync(QueryRequest request);

        /// <summary>
        /// 데이터 베이스 Commit Transaction 호출 함수
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void CommitTransaction()
        {
            if (_dbCommand == null)
                throw new Exception("CommitTransaction dbCommand null Failed.");
            
            CommitTransaction(_dbCommand.Transaction);
        }

        /// <summary>
        /// 데이터 베이스 Commit Transaction 비동기 호출 함수
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DbCommand?> CommitTransactionAsync()
        {
            if (_dbCommand == null)
                throw new Exception("CommitTransactionAsync dbCommand null Failed.");

            await CommitTransactionAsync(_dbCommand.Transaction);

            return _dbCommand;
        }

        /// <summary>
        /// 데이터 베이스 Commit Transaction 호출 함수
        /// </summary>
        /// <param name="dbTransaction"></param>
        public void CommitTransaction(DbTransaction? dbTransaction)
        {
            if (dbTransaction == null)
                return;

            dbTransaction.Commit();
        }

        /// <summary>
        /// 데이터 베이스 Commit Transaction 비동기 호출 함수
        /// </summary>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public async Task<DbTransaction?> CommitTransactionAsync(DbTransaction? dbTransaction)
        {
            if (dbTransaction == null)
                return null;

            await dbTransaction.CommitAsync();

            return dbTransaction;
        }

        /// <summary>
        /// 데이터 베이스 Rellback Transaction 호출 함수
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void RollbackTransaction()
        {
            if (_dbCommand == null)
                throw new Exception("RollbackTransaction _dbCommand null Failed.");

            RollbackTransaction(_dbCommand.Transaction);
        }

        /// <summary>
        /// 데이터 베이스 Rellback Transaction 비동기 호출 함수
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<DbCommand?> RollbackTransactionAsync()
        {
            if (_dbCommand == null)
                throw new Exception("RollbackTransactionAsync _dbCommand null Failed.");

            await RollbackTransactionAsync(_dbCommand.Transaction);


            return _dbCommand;
        }

        /// <summary>
        /// 데이터 베이스 Rellback Transaction 호출 함수
        /// </summary>
        /// <param name="dbTransaction"></param>
        public void RollbackTransaction(DbTransaction? dbTransaction)
        {
            if (dbTransaction == null)
                return;

            dbTransaction.Rollback();
        }

        /// <summary>
        /// 데이터 베이스 Rellback Transaction 비동기 호출 함수
        /// </summary>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        public async Task<DbTransaction?> RollbackTransactionAsync(DbTransaction? dbTransaction)
        {
            if (dbTransaction == null)
                return null;

            await dbTransaction.RollbackAsync();

            return dbTransaction;
        }

        /// <summary>
        /// 데이터 베이스 소멸 호출 함수
        /// </summary>
        public void Dispose()
        {
            this.DisConnect();
        }

        /// <summary>
        /// 데이터 베이스 ExecuteReader 비동기 호출 함수
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<QueryResponse> ExecuteReaderQueryResponseAsync(QueryRequest request)
        {
            QueryResponse response = new QueryResponse();
       var string_log_key = await SetLog(request);
            try
            {
                if (_dbCommand == null)
                    throw new Exception("ExecuteReader dbCommand Connected Failed.");

                await SetProcedureParametersInfoAsync(request);

                _dbCommand.CommandText = request.QueryName;
                _dbCommand.CommandType = System.Data.CommandType.StoredProcedure;

                using (var reader = await _dbCommand.ExecuteReaderAsync())
                {
                    int tableIndex = 0;

                    do
                    {
                        var dt = CreateDataTable(reader, $"dt{tableIndex++}");

                        response.DataSet.Tables.Add(dt);
                    }
                    while (await reader.NextResultAsync());
                }

                if (request.ReturnQueryParameter)
                    SetResponseQueryParameters(_dbCommand, response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

      await SetLog(request,response, string_log_key);
      return response;
        }


    private async Task<string> SetLog(QueryRequest request) {

      string log_key = DateTime.Now.Ticks.ToString();

      try {
        //await SetProcedureParametersInfoAsync(request);
        // soquri
        //_dbCommand.CommandText = request.QueryName;

        _dbCommand.Parameters.Clear();

          _dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
        _dbCommand.CommandText = "p_logproc";


        // 프로시저 이름 셋팅

        SqlParameter prockey = new();
        prockey.ParameterName = "@IN_LOG_KEY";
        prockey.Direction = ParameterDirection.Input;
        prockey.SqlDbType = SqlDbType.VarChar;
        prockey.Value = log_key;
        _dbCommand.Parameters.Add(prockey);

        SqlParameter procParam = new();
        procParam.ParameterName = "@IN_PROC_NM";
        procParam.Direction = ParameterDirection.Input;
        procParam.SqlDbType = SqlDbType.VarChar;
        procParam.Value = request.QueryName;
        _dbCommand.Parameters.Add(procParam);


        SqlParameter pParam = new();
        pParam.ParameterName = "@IN_PRAMS";
        pParam.Direction = ParameterDirection.Input;
        pParam.SqlDbType = SqlDbType.Text;
        string paramstr = "";
        foreach (QueryParameter qp in request.QueryParameters) {
          if ((qp.Prefix+ qp.ParameterName).IndexOf("IN_SS_") != 0 ) {
            paramstr += qp.ParameterName + " : " + qp.ParameterValue + "," + Environment.NewLine;
          }
        }
        pParam.Value = paramstr;
        _dbCommand.Parameters.Add(pParam);


        string[] tmps = "SS_USER_ID,SS_TOKEN,SS_ACCESS_TOKEN_ID,SS_REFRESH_TOKEN,SS_CLIENT_IP,SS_WEB_IP,SS_SOURCE".Split(',');

        foreach (string tmp in tmps) {
          //if (qp.ParameterName == tmp) {
          string tmpVal = "";


          foreach (QueryParameter qp in request.QueryParameters) {
              if (qp.ParameterName == tmp) {
              tmpVal = qp.ParameterValue + "";
              break;
              }
          }
          _dbCommand.Parameters.Add(new SqlParameter() { ParameterName = "@IN_" + tmp, Value = tmpVal, Direction = ParameterDirection.Input, SqlDbType = SqlDbType.VarChar });
          //}
        }



        await _dbCommand.ExecuteNonQueryAsync();
      }
      catch (Exception ee) {
        string kkk = "xxxxxxx";

      }
      await CommitTransactionAsync();
      return log_key;
    }



    private async Task SetLog(QueryRequest request, QueryResponse res, string log_key) {

      try {
        _dbCommand.Parameters.Clear();

        _dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
        _dbCommand.CommandText = "p_logproc_end";


        // 프로시저 이름 셋팅


        _dbCommand.Parameters.Add(new SqlParameter() { ParameterName = "@IN_LOG_KEY", Value = log_key, Direction = ParameterDirection.Input, SqlDbType = SqlDbType.VarChar });

        _dbCommand.Parameters.Add(new SqlParameter() { ParameterName = "@IN_IsSuccess", Value = res.IsSuccess + "", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.VarChar });

        _dbCommand.Parameters.Add(new SqlParameter() { ParameterName = "@IN_Msg", Value = res.Message + "", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.VarChar });

        string cntstr = "";
        if (res.DataSet.Tables.Count > 0) {
          int rcnt = 1;
          foreach (DataTable dt in res.DataSet.Tables) {
            
            cntstr += rcnt+" : "+ dt.Rows.Count;
            if (rcnt > 1) {
              cntstr += " , " + Environment.NewLine;
            }
              rcnt++;
          }
        }
        _dbCommand.Parameters.Add(new SqlParameter() { ParameterName = "@IN_ReturnCnt", Value = cntstr, Direction = ParameterDirection.Input, SqlDbType = SqlDbType.VarChar });




        string[] tmps = "SS_USER_ID,SS_TOKEN,SS_ACCESS_TOKEN_ID,SS_REFRESH_TOKEN".Split(',');

        foreach (string tmp in tmps) {
          //if (qp.ParameterName == tmp) {
          string tmpVal = "";


          foreach (QueryParameter qp in request.QueryParameters) {
            if (qp.ParameterName == tmp) {
              tmpVal = qp.ParameterValue + "";
              break;
            }
          }
          _dbCommand.Parameters.Add(new SqlParameter() { ParameterName = "@IN_" + tmp, Value = tmpVal, Direction = ParameterDirection.Input, SqlDbType = SqlDbType.VarChar });
          //}
        }




        /*


        string[] tmps = "IN_SS_USER_ID,IN_SS_TOKEN,IN_SS_ACCESS_TOKEN_ID,IN_SS_REFRESH_TOKEN".Split(',');

        foreach (QueryParameter qp in request.QueryParameters) {
          foreach (string tmp in tmps) {

            if (qp.ParameterName == tmp) {

              _dbCommand.Parameters.Add(new SqlParameter() { ParameterName = "@" + tmp, Value = qp.ParameterValue + "", Direction = ParameterDirection.Input, SqlDbType = SqlDbType.VarChar });

            }

          }
        }
        */



        await _dbCommand.ExecuteNonQueryAsync();
      }
      catch (Exception ee) {
        string kkk = "";
      }
      await CommitTransactionAsync();
    }

    

        /// <summary>
        /// 데이터 베이스 ExecuteReader 호출 함수
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<QueryResponse> ExecuteReaderQueryResponse(QueryRequest request)
        {
            QueryResponse response = new QueryResponse();
      var string_log_key = await SetLog(request);
      try
            {
                if (_dbCommand == null)
                    throw new Exception("ExecuteReader dbCommand Connected Failed.");

                SetProcedureParametersInfo(request);

                _dbCommand.CommandText = request.QueryName;
                _dbCommand.CommandType = System.Data.CommandType.StoredProcedure;

        //using (var reader = await _dbCommand.ExecuteReader())
        using (var reader = await _dbCommand.ExecuteReaderAsync()) {
                    int tableIndex = 0;

                    do
                    {
                        var dt = CreateDataTable(reader, $"dt{tableIndex++}");

                        response.DataSet.Tables.Add(dt);
                    }
                    while (reader.NextResult());
                }

                if (request.ReturnQueryParameter)
                    SetResponseQueryParameters(_dbCommand, response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Exception = ex;
            }

      await SetLog(request, response, string_log_key);

      return response;
        }

        /// <summary>
        /// 데이터 베이스에서 받은 파라메터 값 셋팅
        /// </summary>
        /// <param name="dbCommand"></param>
        /// <param name="response"></param>
        private void SetResponseQueryParameters(DbCommand dbCommand, QueryResponse response)
        {
            response.QueryParameters = new QueryParameters();

            foreach (SqlParameter param in dbCommand.Parameters)
            {
                var queryParameter = new QueryParameter()
                {
                    ParameterName = param.ParameterName.Replace("@", ""),
                    ParameterDirection = param.Direction,
                    ParameterValue = param.Value,
                };
                response.QueryParameters.Add(queryParameter);
            }
        }

        /// <summary>
        /// 데이터 베이스에서 받은 데이터 값 DataTable 로 변환 및 생성
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private DataTable CreateDataTable(DbDataReader reader, string tableName)
        {
            var dt = new DataTable(tableName);

            for (int i = 0; i < reader.FieldCount; ++i)
            {
                var colName = reader.GetName(i);
                var colType = reader.GetFieldType(i);

                dt.Columns.Add(colName, colType);
            }

            while (reader.Read())
            {
                var row = dt.NewRow();
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    row[i] = reader.GetValue(i);
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// 데이터베이스 ExecuteQuery 요청
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        public async Task<QueryResponses> ExecuteQueryResponse(QueryRequests requests, bool isTransation = true)
        {
            var responses = new QueryResponses();

            try
            {
                if (Connect(isTransation) == null)
                    throw new Exception("Execute requests Connect Failed.");

                responses.IsSuccessMain = true;

                foreach (var request in requests)
                {
                    var response = await ExecuteReaderQueryResponse(request);

                    if (!response.IsSuccess)
                    {
                        responses.IsSuccessMain = false;
                        responses.MessageMain = response.Message;
                        responses.ExceptionMain = response.Exception;
                        break;
                    }

                    responses.Add(response);
                }

                if (responses.IsSuccessMain)
                    CommitTransaction();
                else
                    RollbackTransaction();
            }
            catch (Exception ex)
            {
                responses.IsSuccessMain = false;
                responses.MessageMain = ex.Message;
                responses.ExceptionMain = ex;
                RollbackTransaction();
            }
            finally
            {
                
                this.DisConnect();
            }

            return responses;
        }

        /// <summary>
        /// 데이터베이스 ExecuteQuery 요청
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        public async Task<QueryResponses> ExecuteQueryResponseAsync(QueryRequests requests, bool isTransation = true)
        {
            var responses = new QueryResponses();

            try
            {
                if (await ConnectAsync(isTransation) == null)
                    throw new Exception("ExecuteAsync requests Connect Failed.");

                responses.IsSuccessMain = true;

                foreach (var request in requests)
                {
                    var response = await ExecuteReaderQueryResponseAsync(request);

                    if (!response.IsSuccess)
                    {
                        responses.IsSuccessMain = false;
                        responses.MessageMain = response.Message;
                        responses.ExceptionMain = response.Exception;
                        break;
                    }

                    responses.Add(response);
                }

                if (responses.IsSuccessMain)
                    await CommitTransactionAsync();
                else
                    await RollbackTransactionAsync();
            }
            catch (Exception ex)
            {
                responses.IsSuccessMain = false;
                responses.MessageMain = ex.Message;
                responses.ExceptionMain = ex;
                await RollbackTransactionAsync();
            }
            finally
            {
                await this.DisConnectAsync();
            }

            return responses;
        }

        /// <summary>
        /// 데이터베이스 ExecuteQuery 요청
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        public async Task<QueryResponse> ExecuteQueryResponse(QueryRequest request, bool isTransation = true)
        {
            try
            {
                if (Connect(isTransation) == null)
                    throw new Exception("Execute request Connect Failed.");

                var response = await ExecuteReaderQueryResponse(request);

                if (response.IsSuccess)
                    CommitTransaction();
                else
                    RollbackTransaction();
                return response;
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                return new QueryResponse(false, ex.Message, ex);
            }
            finally
            {
                this.DisConnect();
            }
        }

        /// <summary>
        /// 데이터베이스 ExecuteQuery 요청
        /// </summary>
        /// <param name="request"></param>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        public async Task<QueryResponse> ExecuteQueryResponseAsync(QueryRequest request, bool isTransation = true)
        {
            try
            {
                if (await ConnectAsync(isTransation) == null)
                    throw new Exception("ExecuteAsync request Connect Failed.");

                var response = await ExecuteReaderQueryResponseAsync(request);

                if (response.IsSuccess)
                    await CommitTransactionAsync();
                else
                    await RollbackTransactionAsync();

                return response;
            }
            catch (Exception ex) 
            {
                await RollbackTransactionAsync();
                return new QueryResponse(false, ex.Message, ex);
            }
            finally
            {
                await this.DisConnectAsync();
            }
        }
    }
}
