﻿using MIT.DataAccess;
using System.Runtime.CompilerServices;

namespace MIT.WebService.Services.Database
{
    /// <summary>
    /// 데이터 베이스 서비스 추가 확장 메소드
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// 서비스에 추가 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddDatabase<T>(this IServiceCollection serviceCollection) where T : DatabaseService, new()
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddScoped<T>();

            return serviceCollection;
        }

    }
}
