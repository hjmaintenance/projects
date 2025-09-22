using JinRestApi.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;

namespace JinRestApi.Endpoints;

public static class CompanyEndpoints
{
    // DTO for creating a company to avoid expecting an ID from the client.
    public record CompanyCreateDto([Required] string Name, string? ModifiedBy, string? MenuContext);

    public static void MapCompanyEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/companys");

        // 전체 조회
        group.MapGet("/", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Companies.Include(c => c.Attachments).ToListAsync()
        ));

        // 상세 조회
        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Companies.Include(c => c.Attachments).FirstOrDefaultAsync(c => c.Id == id)
        ));

        // 등록
        group.MapPost("/", (AppDbContext db, CompanyCreateDto companyDto) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var company = new CustomerCompany
            {
                Name = companyDto.Name,
                ModifiedBy = companyDto.ModifiedBy,
                MenuContext = companyDto.MenuContext,
                // CreatedAt, ModifiedAt은 AppDbContext의 SaveChangesAsync에서 자동으로 설정됩니다.
            };
            db.Companies.Add(company);
            await db.SaveChangesAsync();
            return company;
        }, "Company created successfully.", 201));

        // 수정
        group.MapPut("/{id}", (AppDbContext db, int id, CustomerCompany input) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var company = await db.Companies.FindAsync(id);
            if (company is null) return null;

            company.Name = input.Name;
            company.ModifiedBy = input.ModifiedBy;
            company.MenuContext = input.MenuContext;
            // ModifiedAt은 AppDbContext의 SaveChangesAsync에서 자동으로 설정됩니다.

            await db.SaveChangesAsync();
            return company;
        }, "Company updated successfully."));

        // 삭제
        group.MapDelete("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var company = await db.Companies.FindAsync(id);
            if (company is null) return null;

            db.Companies.Remove(company);
            await db.SaveChangesAsync();
            // 삭제 성공 시 데이터는 없으므로 간단한 객체를 반환합니다.
            return new { DeletedId = id };
        }, "Company deleted successfully."));
    }
}
