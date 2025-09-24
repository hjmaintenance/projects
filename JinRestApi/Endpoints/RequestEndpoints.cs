using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;
using System.Linq.Dynamic.Core;
using JinRestApi.Dtos;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace JinRestApi.Endpoints;

/// <summary> 요청(ImprovementRequest) 엔드포인트 </summary>
public static class RequestEndpoints
{
    public static void MapRequestEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/requests");

        // 요청 목록 (필터링 포함)
        group.MapGet("/", (AppDbContext db, ImprovementStatus? status) => ApiResponseBuilder.CreateAsync(() =>
        {
            var query = db.Requests.Include(r => r.Comments).Include(r => r.Attachments).AsQueryable();
            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);
            return query.ToListAsync();
        }));

        // 요청 상세
        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Requests.Include(r => r.Comments).Include(r => r.Attachments).FirstOrDefaultAsync(r => r.Id == id)
        ));



        // 검색
        group.MapGet("/srch", (AppDbContext db, HttpContext http) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var baseQuery = db.Requests.AsQueryable();

            // 포함 관계 필요하면 Include 이후 ApplyAll 호출
            baseQuery = baseQuery.Include(c => c.Attachments);

            // ApplyAll 은 IQueryable 반환 (동적 타입 가능)
            var resultQuery = baseQuery.ApplyAll(http.Request.Query);

            // ToListAsync 은 dynamic IQueryable 에서도 작동
            var list = await (resultQuery is IQueryable<object> q ? q.ToDynamicListAsync() : ((IQueryable)resultQuery).ToDynamicListAsync());
            return list;
        }, "Request srch successfully.", 201));




        // 검색
        group.MapPost("/srch", (AppDbContext db, HttpContext http) => ApiResponseBuilder.CreateAsync(async () =>
        {

            // 1) JSON Body 읽기
            using var reader = new StreamReader(http.Request.Body);
            var body = await reader.ReadToEndAsync();

            IQueryCollection bodyQuery = new QueryCollection();
            if (!string.IsNullOrWhiteSpace(body))
            {
                bodyQuery = JsonToQueryHelper.ConvertJsonToQuery(body);
            }

            // 2) GET QueryString 과 병합
            var finalQuery = QueryCollectionMerger.Merge(http.Request.Query, bodyQuery);

            // 3) DynamicFilterHelper 재사용

            var baseQuery = db.Requests.AsQueryable();

            // 포함 관계 필요하면 Include 이후 ApplyAll 호출
            baseQuery = baseQuery.Include(c => c.Attachments);

            // ApplyAll 은 IQueryable 반환 (동적 타입 가능)
            //var resultQuery = baseQuery.ApplyAll(http.Request.Query);
            var resultQuery = baseQuery.ApplyAll(finalQuery);

            // ToListAsync 은 dynamic IQueryable 에서도 작동
            var list = await (resultQuery is IQueryable<object> q ? q.ToDynamicListAsync() : ((IQueryable)resultQuery).ToDynamicListAsync());
            return list;
        }, "Request srch successfully.", 201));







        // 요청 등록
        group.MapPost("/", (AppDbContext db, RequestCreateDto requestDto, IConnection rabbitMqConnection, ILoggerFactory loggerFactory) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var request = new ImprovementRequest
            {
                Title = requestDto.Title,
                Description = requestDto.Description,
                CustomerId = requestDto.CustomerId,
                Status = ImprovementStatus.Pending, // 새 요청의 기본 상태
                CreatedBy = requestDto.CreatedBy,
                MenuContext = requestDto.MenuContext
            };

            db.Requests.Add(request);
            await db.SaveChangesAsync();

            var logger = loggerFactory.CreateLogger("RequestEndpoints");
            logger.LogInformation("Attempting to publish message to RabbitMQ.");
            try
            {
                using var channel = rabbitMqConnection.CreateModel();

                channel.QueueDeclare(queue: "run_script",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                logger.LogInformation("RabbitMQ channel created and queue 'run_script' declared.");

                // 실행할 쉘 스크립트와 인자
                string scriptPath = "/home/quri/projects/wrkScripts/wrkRecept.sh";
                string[] args = new[] { requestDto.Title, requestDto.Description };

                // JSON 메시지 만들기
                var payload = new
                {
                    script = scriptPath,
                    args = args
                };

                string json = JsonSerializer.Serialize(payload);

                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "",
                                    routingKey: "run_script",
                                    basicProperties: null,
                                    body: body);
                logger.LogInformation("Successfully published message to RabbitMQ.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to publish message to RabbitMQ.");
                // You might want to re-throw or handle the exception depending on your requirements.
            }

            return request;
        }, "Request created successfully.", 201));

        // 요청 수정
        group.MapPut("/{id}", (AppDbContext db, int id, ImprovementRequest input) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var req = await db.Requests.FindAsync(id);
            if (req is null) return null;

            req.Title = input.Title;
            req.Description = input.Description;
            req.Status = input.Status;

            await db.SaveChangesAsync();
            return req;
        }, "Request updated successfully."));

        // 요청 삭제
        group.MapDelete("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var req = await db.Requests.FindAsync(id);
            if (req is null) return null;

            db.Requests.Remove(req);
            await db.SaveChangesAsync();
            return new { DeletedId = id };
        }, "Request deleted successfully."));
    }
}
