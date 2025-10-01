using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;
using System.Linq.Dynamic.Core;
using JinRestApi.Dtos;
using JinRestApi.Helpers;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.ComponentModel;

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
            () => db.Requests
            //.Include(r => r.Comments)
            //.Include(r => r.Customer)
            //.Include(r => r.Admin)
            //.Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.Id == id)
        ));

        // 특정 요청에 대한 덧글 목록 조회
        group.MapGet("/{id}/comments", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var comments = await db.Comments
                .Where(c => c.RequestId == id)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();
            return comments;
        }, "Comments retrieved successfully."));




        /*
                // 검색
                group.MapGet("/srch", (AppDbContext db, HttpContext http) => ApiResponseBuilder.CreateAsync(async () =>
                {
                    var baseQuery = db.Requests.AsQueryable();

                    // 포함 관계 필요하면 Include 이후 ApplyAll 호출
                    baseQuery = baseQuery.Include(c => c.Attachments).Include(r => r.Customer);

                    // ApplyAll 은 IQueryable 반환 (동적 타입 가능)
                    var resultQuery = baseQuery.ApplyAll(http.Request.Query);

                    // ToListAsync 은 dynamic IQueryable 에서도 작동
                    var list = await (resultQuery is IQueryable<object> q ? q.ToDynamicListAsync() : ((IQueryable)resultQuery).ToDynamicListAsync());
                    return list;
                }, "Request srch successfully.", 201));

        */


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

            var baseQuery = db.Requests.AsQueryable();

            var queryWithIncludes = baseQuery
            .Include(c => c.Comments)
            .Include(r => r.Customer)
            .Include(r => r.Admin)
            ;

            // ApplyAll 은 IQueryable 반환 (동적 타입 가능)
            var resultQuery = queryWithIncludes.ApplyAll(finalQuery);


            Console.WriteLine($"resultQuery : {resultQuery}");

// 필요한 모든 첨부파일을 한 번에 가져오기

/*
var attachments = await db.Attachments
    .Where(a => a.EntityType == "ImprovementRequest" && requestIds.Contains(a.EntityId))
    .ToListAsync();

*/
/*
// 매핑
var requests = tempRequests.Select(r => new {
    Request = r,
    Attachments = attachments.Where(a => a.EntityId == r.Id).ToList()
}).ToList();

*/
            

    
            // ToListAsync 은 dynamic IQueryable 에서도 작동
            var requests = await (resultQuery is IQueryable<object> q ? q.ToDynamicListAsync() : ((IQueryable)resultQuery).ToDynamicListAsync());

            // 모든 ImprovementRequest에 대한 첨부파일 수를 한 번에 조회
            var attachmentCounts = await db.Attachments
                .Where(a => a.EntityType == "ImprovementRequest")
                .GroupBy(a => a.EntityId)
                .Select(g => new { RequestId = g.Key, Count = g.Count() })
                .ToListAsync();

            var attachmentCountMap = attachmentCounts.ToDictionary(x => x.RequestId, x => x.Count);

            // 요청 목록에 AttachmentCount 추가
            /*
            var requestsWithAttachmentCount = requests.Select(r => {
                var expando = new ExpandoObject() as IDictionary<string, object>;
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(r))
                {
                    expando.Add(property.Name, property.GetValue(r));
                }
                var requestId = (int)expando["Id"];
                expando.TryAdd("AttachmentCount", attachmentCountMap.GetValueOrDefault(requestId, 0));


                return expando;
            })
            .ToList();
            ;
            */
            var requestsWithAttachmentCount = requests
    //.Cast<object>()
    .Select(r => {
        var expando = new ExpandoObject() as IDictionary<string, object>;

        //foreach (var prop in r.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(r))
        {
             expando.Add(property.Name, property.GetValue(r));
        }

        var requestId = (int)expando["Id"];
        expando["AttachmentCount"] = attachmentCountMap.GetValueOrDefault(requestId, 0);

        return expando;
    })
    .ToList();


    Console.WriteLine($"requests : {requests}");
    Console.WriteLine($"requestsWithAttachmentCount : {requestsWithAttachmentCount}");

            return requestsWithAttachmentCount; //.ToDynamicListAsync()





        }, "Request srch successfully.", 201));







        group.MapPost("/", async (HttpRequest httpRequest, AppDbContext db, IRabbitMqConnectionProvider provider, ILoggerFactory loggerFactory) =>
        {
            var form = await httpRequest.ReadFormAsync();
            var requestDto = new RequestCreateDto(
                Title: form["Title"],
                Description: form["Description"],
                CustomerId: int.Parse(form["CustomerId"]),
                CreatedBy: form["CreatedBy"],
                MenuContext: form["MenuContext"]
            );
            var files = form.Files;

            var result = await ApiResponseBuilder.CreateAsync(async () => 
            {
                // 1. ImprovementRequest 생성 및 저장
                var request = new ImprovementRequest
                {
                    Title = requestDto.Title,
                    Description = requestDto.Description,
                    CustomerId = requestDto.CustomerId,
                    Status = ImprovementStatus.Pending,
                    CreatedBy = requestDto.CreatedBy,
                    MenuContext = requestDto.MenuContext
                };

                db.Requests.Add(request);
                await db.SaveChangesAsync(); // request.Id가 생성됨

                // 2. 파일 업로드 및 Attachment 생성
                if (files.Count > 0)
                {
                    var attachments = new List<Attachment>();
                    var storagePath = "/home/quri/jinAttachment";
                    Directory.CreateDirectory(storagePath);

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var extension = Path.GetExtension(file.FileName);
                            var storedFileName = $"{Guid.NewGuid()}{extension}";
                            var filePath = Path.Combine(storagePath, storedFileName);

                            await using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var attachment = new Attachment
                            {
                                OriginalFileName = file.FileName,
                                StoredFileName = storedFileName,
                                FilePath = storagePath,
                                FileType = file.ContentType,
                                FileSize = file.Length,
                                EntityType = "ImprovementRequest",
                                EntityId = request.Id, // 생성된 요청의 ID 사용
                                UploadedAt = DateTime.UtcNow
                            };
                            attachments.Add(attachment);
                        }
                    }

                    if (attachments.Any())
                    {
                        db.Attachments.AddRange(attachments);
                        await db.SaveChangesAsync();
                    }
                }

                // 3. RabbitMQ 메시지 전송 (기존 로직)
                var logger = loggerFactory.CreateLogger("RequestEndpoints");
                logger.LogInformation("Attempting to publish message to RabbitMQ.");

                if (!provider.IsConnected)
                {
                    logger.LogWarning("RabbitMQ is not connected. Cannot send message.");
                }
                else
                {
                    try
                    {
                        using var channel = provider.Connection!.CreateModel();
                        channel.QueueDeclare(queue: "run_script", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        logger.LogInformation("RabbitMQ channel created and queue 'run_script' declared.");

                        string scriptPath = "/home/quri/projects/wrkScripts/wrkRecept.sh";
                        string[] args = new[] { requestDto.Title, requestDto.Description };

                        var payload = new { script = scriptPath, args = args };
                        string json = JsonSerializer.Serialize(payload);
                        var body = Encoding.UTF8.GetBytes(json);

                        channel.BasicPublish(exchange: "", routingKey: "run_script", basicProperties: null, body: body);
                        logger.LogInformation("Successfully published message to RabbitMQ.");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to publish message to RabbitMQ.");
                    }
                }

                return request;
            }, "Request created successfully.", 201);
            return result;
        })
        .DisableAntiforgery();

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
