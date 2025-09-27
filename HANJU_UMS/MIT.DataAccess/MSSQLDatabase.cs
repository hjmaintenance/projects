using MIT.ServiceModel;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace MIT.DataAccess
{
    /// <summary>
    /// Mssql 데이터 베이스 클래스 구현
    /// </summary>
    internal class MSSQLDatabase : Database
    {
        /// <summary>
        /// MSSQL 데이터 베이스 셋팅
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="timeout"></param>
        public MSSQLDatabase(string connectionString, int timeout = 600) 
            : base(connectionString, DATABASE_TYPE.MSSQL, timeout)
        {

        }

        /// <summary>
        /// MSSQL 데이터 베이스 연결
        /// </summary>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        public override DbCommand Connect(bool isTransation = true)
        {
            _dbConnection = new SqlConnection(_connectionString);
            _dbConnection.Open();

            _dbCommand = new SqlCommand();
            _dbCommand.Connection = _dbConnection;
            _dbCommand.CommandTimeout = Timeout;

            if (isTransation)
            {
                _dbCommand.Transaction = _dbConnection.BeginTransaction();
            }

            return _dbCommand;
        }

        /// <summary>
        /// MSSQL 데이터 베이스 비동기 연결
        /// </summary>
        /// <param name="isTransation"></param>
        /// <returns></returns>
        public override async Task<DbCommand> ConnectAsync(bool isTransation = true)
        {
            _dbConnection = new SqlConnection(_connectionString);
            await _dbConnection.OpenAsync();

            _dbCommand = new SqlCommand();
            _dbCommand.Connection = _dbConnection;
            _dbCommand.CommandTimeout = Timeout;

            if (isTransation)
            {
                _dbCommand.Transaction = await _dbConnection.BeginTransactionAsync();
            }

            return _dbCommand;
        }

        /// <summary>
        /// MSSQL 데이터 베이스 프로시저 파라메터 비동기 셋팅
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override async Task<bool> SetProcedureParametersInfoAsync(QueryRequest request)
        {
            if (_dbCommand == null)
                throw new Exception("DbCommand가 할당되지 않았습니다.");

            // 프로시저에 정의된 입력 변수 파라미터 정보를 가져오기 위한 셋팅
            string[] tmp_array = request.QueryName == null ? new string[0] : request.QueryName.Split('.');
            string databasename = tmp_array.Length == 3 ? $"{tmp_array[0]}." : string.Empty;

            _dbCommand.Parameters.Clear();
            _dbCommand.CommandText = $@"select 'NAME' = name,
                                               'TYPE'         = type_name(user_type_id),  
                                               'Length'       = max_length,  
                                               'Prec'         = case when type_name(system_type_id) = 'uniqueidentifier' then precision  
                                                                     else OdbcPrec(system_type_id, max_length, precision) 
					                                            end,  
                                               'Scale'        = OdbcScale(system_type_id, scale),  
                                               'Param_order'  = parameter_id,  
                                               'Collation'    = convert(sysname,  case when system_type_id in (35, 99, 167, 175, 231, 239)  then ServerProperty('collation') end)  ,
	                                           'IS_OUTPUT'       = case when is_output = 1 then 'Y' else 'N' END
                                          from {databasename}sys.parameters
                                         where object_id = object_id(@IN_PROCEDURE_NAME);";
            _dbCommand.CommandType = CommandType.Text;

            // 프로시저 이름 셋팅
            SqlParameter procParam = new SqlParameter();
            procParam.ParameterName = "@IN_PROCEDURE_NAME";
            procParam.Direction = ParameterDirection.Input;
            procParam.SqlDbType = SqlDbType.VarChar;
            procParam.Value = request.QueryName;
            _dbCommand.Parameters.Add(procParam);

            // 프로시저 정의된 변수 이름 및 정보 호출
            using (var reader = await _dbCommand.ExecuteReaderAsync())
            {
                
                var datatable = new DataTable();
                for (int index = 0; index < reader.FieldCount; index++)
                {
                    var columnName = reader.GetName(index);
                    var columnType = reader.GetFieldType(index);
                    datatable.Columns.Add(columnName, columnType);
                }
                
                while (reader.Read())
                {
                    var row = datatable.NewRow();
                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        row[index] = reader.GetValue(index);
                    }
                    datatable.Rows.Add(row);
                }

                _dbCommand.Parameters.Clear();
                // 프로시저 정의된 변수 및 정보 command Parameter에 셋팅
                foreach (DataRow row in datatable.Rows)
                {
                    var param = new SqlParameter();
                    param.ParameterName = row.Field<string>("NAME");
                    param.Direction = row.Field<string>("IS_OUTPUT") == "Y" ? ParameterDirection.Output : ParameterDirection.Input;
                    param.SqlDbType = ParseDataType(row.Field<string>("TYPE"));

                    if (param.Direction == ParameterDirection.Output)
                    {
                        switch (param.SqlDbType)
                        {
                            case SqlDbType.VarChar: param.Size = 4000; break;
                        }
                    }
                    else
                    {
                        var value = request?.QueryParameters?.FirstOrDefault(x => $"{x.Prefix}{x.ParameterName}".ToLower() == param.ParameterName?.Replace("@", "").ToLower())?.ParameterValue;
                        param.Value = value == null ? DBNull.Value : value;
                        //param.Value = value;
                    }

                    _dbCommand.Parameters.Add(param);
                }
            }

            return true;
        }

        /// <summary>
        /// MSSQL 데이터 베이스 프로시저 파라메터 셋팅
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception"></exception>
        public override void SetProcedureParametersInfo(QueryRequest request)
        {
            if (_dbCommand == null)
                throw new Exception("DbCommand가 할당되지 않았습니다.");

            
            string[] tmp_array = request.QueryName == null ? new string[0] : request.QueryName.Split('.');
            string databasename = tmp_array.Length == 3 ? $"{tmp_array[0]}." : string.Empty;

            _dbCommand.Parameters.Clear();
            _dbCommand.CommandText = $@"select 'NAME' = name,
                                               'TYPE'         = type_name(user_type_id),  
                                               'Length'       = max_length,  
                                               'Prec'         = case when type_name(system_type_id) = 'uniqueidentifier' then precision  
                                                                     else OdbcPrec(system_type_id, max_length, precision) 
					                                            end,  
                                               'Scale'        = OdbcScale(system_type_id, scale),  
                                               'Param_order'  = parameter_id,  
                                               'Collation'    = convert(sysname,  case when system_type_id in (35, 99, 167, 175, 231, 239)  then ServerProperty('collation') end)  ,
	                                           'IS_OUTPUT'       = case when is_output = 1 then 'Y' else 'N' END
                                          from {databasename}sys.parameters
                                         where object_id = object_id(@IN_PROCEDURE_NAME);";
            _dbCommand.CommandType = CommandType.Text;

            SqlParameter procParam = new SqlParameter();
            procParam.ParameterName = "@IN_PROCEDURE_NAME";
            procParam.Direction = ParameterDirection.Input;
            procParam.SqlDbType = SqlDbType.VarChar;
            procParam.Value = request.QueryName;
            _dbCommand.Parameters.Add(procParam);


            using (var reader = _dbCommand.ExecuteReader())
            {
                var datatable = new DataTable();
                for (int index = 0; index < reader.FieldCount; index++)
                {
                    var columnName = reader.GetName(index);
                    var columnType = reader.GetFieldType(index);
                    datatable.Columns.Add(columnName, columnType);
                }

                while (reader.Read())
                {
                    var row = datatable.NewRow();
                    for (int index = 0; index < reader.FieldCount; index++)
                    {
                        row[index] = reader.GetValue(index);
                    }
                    datatable.Rows.Add(row);
                }

                _dbCommand.Parameters.Clear();

                foreach (DataRow row in datatable.Rows)
                {
                    var param = new SqlParameter();
                    param.ParameterName = row.Field<string>("NAME");
                    param.Direction = row.Field<string>("IS_OUTPUT") == "Y" ? ParameterDirection.Output : ParameterDirection.Input;
                    param.SqlDbType = ParseDataType(row.Field<string>("TYPE"));

                    if (param.Direction == ParameterDirection.Output)
                    {
                        switch (param.SqlDbType)
                        {
                            case SqlDbType.VarChar: param.Size = 4000; break;
                        }
                    }
                    else
                    {
                        param.Value = request?.QueryParameters?.FirstOrDefault(x => $"{x.Prefix}{x.ParameterName}" == param.ParameterName?.Replace("@", ""))?.ParameterValue;
                    }

                    _dbCommand.Parameters.Add(param);
                }
            }
        }

        /// <summary>
        /// MSSQL 타입 셋팅
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static SqlDbType ParseDataType(string? dataType)
        {
            // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
            switch (dataType?.ToUpper())
            {
                case "BIGINT": return SqlDbType.BigInt;
                case "BINARY": return SqlDbType.Binary;
                case "CHAR": return SqlDbType.Char;
                case "VARCHAR": return SqlDbType.VarChar;
                case "TEXT": return SqlDbType.Text;
                case "INT": return SqlDbType.Int;
                case "DECIMAL": return SqlDbType.Decimal;
                case "DATETIME": return SqlDbType.DateTime;

                default: return SqlDbType.VarChar;
            }
        }
    }
}
