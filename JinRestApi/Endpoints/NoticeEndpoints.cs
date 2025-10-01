using JinRestApi.Data;
using JinRestApi.Dtos;
using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Helpers;
using System.Linq.Dynamic.Core;

namespace JinRestApi.Endpoints;

/// <summary> 공지사항 엔드포인트 </summary>
public static class NoticeEndpoints
{
    public static void MapNoticeEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/notices");

        // 전체 조회
        group.MapGet("/", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Notices.ToListAsync()
        ));

        // 단일 조회
        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Notices.FirstOrDefaultAsync(n => n.Id == id)
        ));

        // 검색 (POST /srch)
        group.MapPost("/srch", (AppDbContext db, HttpContext http) => ApiResponseBuilder.CreateAsync(async () =>
        {
            using var reader = new StreamReader(http.Request.Body);
            var body = await reader.ReadToEndAsync();

            var bodyQuery = string.IsNullOrWhiteSpace(body)
                ? new QueryCollection()
                : JsonToQueryHelper.ConvertJsonToQuery(body);

            var finalQuery = QueryCollectionMerger.Merge(http.Request.Query, bodyQuery);

            var baseQuery = db.Notices.AsQueryable();
            var resultQuery = baseQuery.ApplyAll(finalQuery);

            return await (resultQuery is IQueryable<object> q ? q.ToDynamicListAsync() : ((IQueryable)resultQuery).ToDynamicListAsync());
        }));

        // 등록
        group.MapPost("/", (AppDbContext db, NoticeCreateDto dto) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var notice = new Notice
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.Now
            };

            db.Notices.Add(notice);
            await db.SaveChangesAsync();

            return notice;
        }));

        // 수정
        group.MapPut("/{id}", (AppDbContext db, int id, NoticeCreateDto dto) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var notice = await db.Notices.FindAsync(id);
            if (notice is null) return null;

            notice.Title = dto.Title;
            notice.Content = dto.Content;

            await db.SaveChangesAsync();
            return notice;
        }));

        // 삭제
        group.MapDelete("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var notice = await db.Notices.FindAsync(id);
            if (notice is null) return null;

            db.Notices.Remove(notice);
            await db.SaveChangesAsync();
            return new { DeletedId = id };
        }));
    }
}
