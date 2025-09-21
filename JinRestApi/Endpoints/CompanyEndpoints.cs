using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;

namespace JinRestApi.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/companys");

        // 전체 조회
        group.MapGet("/", async (AppDbContext db) =>
            await db.Companies.Include(c => c.Attachments).ToListAsync());

        // 상세 조회
        group.MapGet("/{id}", async (AppDbContext db, int id) =>
            await db.Companies.Include(c => c.Attachments).FirstOrDefaultAsync(c => c.Id == id));

        // 등록
        group.MapPost("/", async (AppDbContext db, CustomerCompany company) =>
        {
            db.Companies.Add(company);
            await db.SaveChangesAsync();
            return Results.Created($"/api/companys/{company.Id}", company);
        });

        // 수정
        group.MapPut("/{id}", async (AppDbContext db, int id, CustomerCompany input) =>
        {
            var company = await db.Companies.FindAsync(id);
            if (company is null) return Results.NotFound();

            company.Name = input.Name;
            company.ModifiedBy = input.ModifiedBy;
            company.MenuContext = input.MenuContext;
            company.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return Results.Ok(company);
        });

        // 삭제
        group.MapDelete("/{id}", async (AppDbContext db, int id) =>
        {
            var company = await db.Companies.FindAsync(id);
            if (company is null) return Results.NotFound();

            db.Companies.Remove(company);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}
