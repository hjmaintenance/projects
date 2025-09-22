using System.Collections;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace JinRestApi.Models
{
    /// <summary>
    /// API 응답을 위한 표준 래퍼 클래스
    /// </summary>
    /// <typeparam name="T">데이터 페이로드의 타입</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>요청 성공 여부</summary>
        public bool Success { get; set; }

        /// <summary>응답 메시지</summary>
        public string Message { get; set; }

        /// <summary>실제 데이터</summary>
        public T? Data { get; set; }

        /// <summary>부가 정보</summary>
        public Metadata? Meta { get; set; }

        public ApiResponse(bool success, string message, T? data, Metadata? meta)
        {
            Success = success;
            Message = message;
            Data = data;
            Meta = meta;
        }
    }

    /// <summary>
    /// 응답에 대한 부가 정보
    /// </summary>
    public class Metadata
    {
        /// <summary>요청 시작 시간 (UTC)</summary>
        public string RequestTime { get; set; } = string.Empty;

        /// <summary>요청 완료 시간 (UTC)</summary>
        public string CompletionTime { get; set; } = string.Empty;

        /// <summary>총 처리 소요 시간</summary>
        public string Duration { get; set; } = string.Empty;

        /// <summary>데이터 행의 수</summary>
        public int? RowCount { get; set; }

        /// <summary>데이터 열(속성)의 수</summary>
        public int? ColumnCount { get; set; }
    }

    /// <summary>
    /// 표준 API 응답 객체를 생성하는 헬퍼 클래스
    /// </summary>
    public static class ApiResponseBuilder
    {
        public static async Task<IResult> CreateAsync<T>(Func<Task<T>> action, string successMessage = "Request processed successfully.", int successStatusCode = 200)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestTime = DateTime.UtcNow;

            try
            {
                var data = await action();
                stopwatch.Stop();
                var completionTime = DateTime.UtcNow;

                if (data == null)
                {
                    return Results.NotFound(new ApiResponse<object>(false, "Resource not found.", null, CreateMetadata(requestTime, completionTime, stopwatch.ElapsedMilliseconds)));
                }

                int? rowCount = null;
                int? colCount = null;

                if (data is IEnumerable enumerable)
                {
                    var items = new List<object>();
                    foreach (var item in enumerable) items.Add(item);
                    
                    rowCount = items.Count;
                    if (rowCount > 0)
                    {
                        colCount = items[0].GetType().GetProperties().Length;
                    }
                }
                else
                {
                    rowCount = 1;
                    colCount = data.GetType().GetProperties().Length;
                }

                var meta = CreateMetadata(requestTime, completionTime, stopwatch.ElapsedMilliseconds, rowCount, colCount);
                var response = new ApiResponse<T>(true, successMessage, data, meta);

                return successStatusCode switch
                {
                    201 => Results.Created(string.Empty, response),
                    _ => Results.Ok(response),
                };
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                var completionTime = DateTime.UtcNow;
                // 실제 운영 환경에서는 ex.Message 대신 일반적인 오류 메시지를 사용하고, ex는 로깅하는 것이 좋습니다.
                var response = new ApiResponse<object>(false, $"An error occurred: {ex.Message}", null, CreateMetadata(requestTime, completionTime, stopwatch.ElapsedMilliseconds));
                return Results.BadRequest(response);
            }
        }

        private static Metadata CreateMetadata(DateTime reqTime, DateTime compTime, long duration, int? rowCount = null, int? colCount = null) => new Metadata
        {
            RequestTime = reqTime.ToString("o"), // ISO 8601 format
            CompletionTime = compTime.ToString("o"),
            Duration = $"{duration}ms",
            RowCount = rowCount,
            ColumnCount = colCount
        };
    }
}