using JinRestApi.Models;
using Microsoft.AspNetCore.Identity;
using JinRestApi.Services;
using JinRestApi.Data;
using Microsoft.EntityFrameworkCore;



namespace JinRestApi.Endpoints;

public static class UserEndpoints
{

    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/users");


        var passwordService = new PasswordService();

        // 회원가입
        group.MapPost("/singup", async (AppDbContext db, User user) =>
        {
            user.PasswordHash = passwordService.HashPassword(user, user.PasswordHash);
            // ...저장 로직...

            db.Users.Add(user);
            await db.SaveChangesAsync();
            return Results.Created($"/users/{user.Id}", user);
        
        });


        // 1. 전체 조회 (GET /users)
        group.MapGet("/", async (AppDbContext db) =>
            await db.Users.ToListAsync()
        );

        // 2. 단일 조회 (GET /users/{id})
        group.MapGet("/{id:int}", async (AppDbContext db, int id) =>
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            return user is not null ? Results.Ok(user) : Results.NotFound();
        });

        // 3. 저장 (POST /users)
        group.MapPost("/", async (AppDbContext db, User user) =>
        {
            user.Id = db.Users.Any() ? db.Users.Max(u => u.Id) + 1 : 1;
            db.Users.Add(user);
            return Results.Created($"/users/{user.Id}", user);
        });

        // 4. 삭제 (DELETE /users/{id})
        group.MapDelete("/{id:int}", async (AppDbContext db, int id) =>
        {
            var user = await db.Users.FindAsync(id);
            if (user is null)
                return Results.NotFound();
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        // 로그인

        group.MapPost("/login", async (AppDbContext db, LoginRequest req) =>
        {
            var user = db.Users.FirstOrDefault(u => u.LoginId == req.LoginId);
            if (user is null || !passwordService.VerifyPassword(user, req.Password))
                return Results.Unauthorized();
            return Results.Ok(user);
        });


    }
}
