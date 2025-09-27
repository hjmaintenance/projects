using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIT.DataAccess
{
    /// <summary>
    /// 데이터 베이스 생성 클래스
    /// </summary>
    public class DatabaseFactory
    {
        /// <summary>
        /// 데이터 베이스 생성 함수
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connectionString"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static IDatabase? CreateDatabase(DATABASE_TYPE dbType, string? connectionString, int timeout = 600)
        {
            if (connectionString == null)
                return null;

            switch (dbType)
            {
                case DATABASE_TYPE.MSSQL: return new MSSQLDatabase(connectionString, timeout);
            }

            return null;
        }
    }
}
