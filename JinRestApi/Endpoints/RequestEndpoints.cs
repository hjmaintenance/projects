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
            var query = db.Requests.Include(r => r.Comments)
            .AsQueryable();
            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);
            return query.ToListAsync();
        }));


        // 요청 상세
        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Requests
            .Include(r => r.Comments)
            .Include(r => r.Customer)
            .Include(r => r.Admin)
            .FirstOrDefaultAsync(r => r.Id == id)
        ));

        // 특정 요청에 대한 덧글 목록 조회
        group.MapGet("/{id}/comments", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var comments = await db.Comments
                .Where(c => c.RequestId == id)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            var adminIds = comments.Where(c => c.AuthorType == "admin").Select(c => c.AuthorId).Distinct().ToList();
            var customerIds = comments.Where(c => c.AuthorType != "admin").Select(c => c.AuthorId).Distinct().ToList();

            var admins = await db.Admins
                .Where(a => adminIds.Contains(a.Id))
                .ToDictionaryAsync(a => a.Id);

            var customers = await db.Customers
                .Where(c => customerIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            var result = comments.Select(c =>
            {
                object? author = null;
                if (c.AuthorType == "admin" && admins.TryGetValue(c.AuthorId, out var admin))
                {
                    author = new { admin.Id, admin.UserName, admin.Photo };
                }
                else if (customers.TryGetValue(c.AuthorId, out var customer))
                {
                    author = new { customer.Id, customer.UserName, customer.Photo };
                }

                return new
                {
                    c.Id,
                    c.RequestId,
                    c.CommentText,
                    c.AuthorType,
                    c.AuthorId,
                    c.ParentCommentId,
                    c.CreatedAt,
                    c.CreatedBy,
                    Author = author
                };
            }).ToList();

            return result;
        }, "Comments retrieved successfully."));


        // 검색
        group.MapPost("/srch", (AppDbContext db, HttpContext http) => ApiResponseBuilder.CreateAsync(async () =>
        {
            using var reader = new StreamReader(http.Request.Body);
            var body = await reader.ReadToEndAsync();

            IQueryCollection bodyQuery = new QueryCollection();
            if (!string.IsNullOrWhiteSpace(body))
            {
                bodyQuery = JsonToQueryHelper.ConvertJsonToQuery(body);
            }

            var finalQuery = QueryCollectionMerger.Merge(http.Request.Query, bodyQuery);

            var queryWithIncludes = db.Requests
                .Include(c => c.Comments)
                .Include(r => r.Customer)
                .Include(r => r.Admin)
                .AsQueryable();

            var resultQuery = queryWithIncludes.ApplyAll(finalQuery);
            var requests = await (resultQuery is IQueryable<object> q ? q.ToDynamicListAsync() : ((IQueryable)resultQuery).ToDynamicListAsync());

            return await AddAttachmentDataAsync(requests, db, "ImprovementRequest");

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
                await db.SaveChangesAsync();

                if (files.Count > 0)
                {
                    var attachments = new List<Attachment>();
                    var storagePath = "/home/lee/jinAttachment";
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
                                EntityId = request.Id,
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

                var logger = loggerFactory.CreateLogger("RequestEndpoints");
                if (provider.IsConnected)
                {
                    try
                    {
                        using var channel = provider.Connection!.CreateModel();
                        channel.QueueDeclare(queue: "run_script", durable: false, exclusive: false, autoDelete: false, arguments: null);
                        string scriptPath = "/home/lee/projects/wrkScripts/wrkRecept.sh";
                        string[] args = { requestDto.Title, requestDto.Description };
                        var payload = new { script = scriptPath, args };
                        string json = JsonSerializer.Serialize(payload);
                        var body = Encoding.UTF8.GetBytes(json);
                        channel.BasicPublish(exchange: "", routingKey: "run_script", basicProperties: null, body: body);
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

        group.MapPut("/{id}", (AppDbContext db, int id, ImprovementRequest input) => ApiResponseBuilder.CreateAsync(async () =>
        {
            //return null;
            var req = await db.Requests.FindAsync(id);
            if (req is null) return null;

            req.Title = input.Title;
            req.Description = input.Description;
            req.Status = input.Status;

            await db.SaveChangesAsync();
            return req;
        }, "Request updated successfully."));

        group.MapDelete("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var req = await db.Requests.FindAsync(id);
            if (req is null) return null;

            db.Requests.Remove(req);
            await db.SaveChangesAsync();
            return new { DeletedId = id };
        }, "Request deleted successfully."));
    }

    private static async Task<List<dynamic>> AddAttachmentDataAsync(List<dynamic> items, AppDbContext db, string entityType)
    {
        if (items == null || !items.Any())
        {
            return new List<dynamic>();
        }

        var itemIds = items.Select(x => (int)x.Id).ToList();

        var allAttachments = await db.Attachments
            .Where(a => a.EntityType == entityType && itemIds.Contains(a.EntityId))
            .ToListAsync();

        var attachmentsByEntityId = allAttachments.GroupBy(a => a.EntityId)
                                                  .ToDictionary(g => g.Key, g => g.ToList());

        var result = new List<dynamic>();
        foreach (var item in items)
        {
            var expando = (item as object).ToExpandoWithEnumNames() as IDictionary<string, object>;
            var entityId = (int)expando["id"];

            var attachments = attachmentsByEntityId.GetValueOrDefault(entityId, new List<Attachment>());

            expando["attachmentCount"] = attachments.Count;
            expando["attachments"] = attachments;

            result.Add(expando);
        }

        return result;
    }
}
