using JinRestApi.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;
using JinRestApi.Dtos;
using System.Linq.Dynamic.Core;

namespace JinRestApi.Endpoints;

public static class CompanyEndpoints
{
    // DTO for creating a company to avoid expecting an ID from the client.
    //public record CompanyCreateDto([Required] string Name, string? ModifiedBy, string? MenuContext);

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



        // 검색
        group.MapGet("/srch", (AppDbContext db, HttpContext http) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var baseQuery = db.Companies.AsQueryable();

            // 포함 관계 필요하면 Include 이후 ApplyAll 호출
            baseQuery = baseQuery.Include(c => c.Attachments);

            // ApplyAll 은 IQueryable 반환 (동적 타입 가능)
            var resultQuery = baseQuery.ApplyAll(http.Request.Query);

            // ToListAsync 은 dynamic IQueryable 에서도 작동
            var list = await (resultQuery is IQueryable<object> q ? q.ToDynamicListAsync() : ((IQueryable)resultQuery).ToDynamicListAsync());
            return list;
        }, "Company srch successfully.", 201));


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
            
            var baseQuery = db.Companies.AsQueryable();

            // 포함 관계 필요하면 Include 이후 ApplyAll 호출
            baseQuery = baseQuery.Include(c => c.Attachments);

            // ApplyAll 은 IQueryable 반환 (동적 타입 가능)
            //var resultQuery = baseQuery.ApplyAll(http.Request.Query);
            var resultQuery = baseQuery.ApplyAll(finalQuery);

            // ToListAsync 은 dynamic IQueryable 에서도 작동
            var list = await (resultQuery is IQueryable<object> q ? q.ToDynamicListAsync() : ((IQueryable)resultQuery).ToDynamicListAsync());
            return list;
        }, "Company search successfully.", 201));




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
