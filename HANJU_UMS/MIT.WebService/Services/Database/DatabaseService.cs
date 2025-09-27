using MIT.DataAccess;

namespace MIT.WebService.Services.Database
{
    /// <summary>
    /// 데이터 베이스 추상 클래스 서비스
    /// </summary>
    public abstract class DatabaseService : IDisposable
    {
        /// <summary>
        /// 데이터 베이스 정보
        /// </summary>
        public IDatabase? _database = null;

        /// <summary>
        /// 데이터 베이스 ConnectionString 문자열
        /// </summary>
        public string? ConnectionString { get; set; }
        /// <summary>
        /// 데이터 베이스 요청 시간
        /// </summary>
        public int Timeout { get; set; } = 600;

        /// <summary>
        /// 데이터 베이스 타입
        /// </summary>
        public DATABASE_TYPE DatabaseType { get; init; }

        public DatabaseService() {}

        /// <summary>
        /// 데이터 베이스 생성
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IDatabase? CreateDatabase()
        {
            _database = DatabaseFactory.CreateDatabase(DatabaseType, this.ConnectionString, this.Timeout);
            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");
            return _database;
        }

        /// <summary>
        /// 데이터 베이스 생성
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IDatabase? CreateDatabase(string? connectionString, int timeout = 600)
        {
            this.ConnectionString = connectionString;
            this.Timeout = timeout;

            _database = DatabaseFactory.CreateDatabase(DatabaseType, this.ConnectionString, this.Timeout);
            if (_database == null)
                throw new Exception("데이터베이스 생성에 실패하였습니다.");
            return _database;
        }

        /// <summary>
        /// 데이터 베이스 소멸 호출
        /// </summary>
        public abstract void Dispose();
    }

}
