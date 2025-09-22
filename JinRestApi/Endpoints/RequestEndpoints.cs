using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;

namespace JinRestApi.Endpoints;

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

        // 요청 등록
        group.MapPost("/", (AppDbContext db, ImprovementRequest request) => ApiResponseBuilder.CreateAsync(async () =>
        {
            db.Requests.Add(request);
            await db.SaveChangesAsync();
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
