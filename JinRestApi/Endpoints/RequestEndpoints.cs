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
        group.MapGet("/", async (AppDbContext db, ImprovementStatus? status) =>
        {
            var query = db.Requests.Include(r => r.Comments).Include(r => r.Attachments).AsQueryable();
            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);
            return await query.ToListAsync();
        });

        // 요청 상세
        group.MapGet("/{id}", async (AppDbContext db, int id) =>
            await db.Requests.Include(r => r.Comments).Include(r => r.Attachments).FirstOrDefaultAsync(r => r.Id == id));

        // 요청 등록
        group.MapPost("/", async (AppDbContext db, ImprovementRequest request) =>
        {
            db.Requests.Add(request);
            await db.SaveChangesAsync();
            return Results.Created($"/api/requests/{request.Id}", request);
        });

        // 요청 수정
        group.MapPut("/{id}", async (AppDbContext db, int id, ImprovementRequest input) =>
        {
            var req = await db.Requests.FindAsync(id);
            if (req is null) return Results.NotFound();

            req.Title = input.Title;
            req.Description = input.Description;
            req.Status = input.Status;
            req.ModifiedBy = input.ModifiedBy;
            req.MenuContext = input.MenuContext;
            req.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return Results.Ok(req);
        });

        // 요청 삭제
        group.MapDelete("/{id}", async (AppDbContext db, int id) =>
        {
            var req = await db.Requests.FindAsync(id);
            if (req is null) return Results.NotFound();

            db.Requests.Remove(req);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}
