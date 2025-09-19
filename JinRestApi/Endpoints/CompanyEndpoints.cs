using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JinRestApi.Models;
using Microsoft.AspNetCore.Identity;
using JinRestApi.Services;
using JinRestApi.Data;
using Microsoft.EntityFrameworkCore;



namespace JinRestApi.Endpoints;

public static class CompanyEndpoints
{

    public static void MapCompanyEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/companys");

        //조회 
        group.MapGet("/", async (AppDbContext db) =>
            await db.Companys.ToListAsync()
        );

        // 1건 조회
        group.MapGet("/{id:int}", async (AppDbContext db, int id) =>
        {
            var company = db.Companys.FirstOrDefault(c => c.Id == id);
            return company is not null ? Results.Ok(company) : Results.NotFound();
        });

        // 저장
        group.MapPost("/", async (AppDbContext db, Company company) =>
        {
            company.Id = db.Companys.Any() ? db.Companys.Max(c => c.Id) + 1 : 1;
            db.Companys.Add(company);
            await db.SaveChangesAsync();
            return Results.Created($"/companys/{company.Id}", company);
        });

        //삭제 
        group.MapDelete("/{id:int}", async (AppDbContext db, int id) =>
        {
            var company = await db.Companys.FindAsync(id);
            if (company is null)
                return Results.NotFound();
            db.Companys.Remove(company);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });




    }
}
