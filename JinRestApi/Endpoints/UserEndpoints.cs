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

public static class UserEndpoints
{

    public static void MapUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users");


        var passwordService = new PasswordService();

        // 회원가입
        group.MapPost("/singup", async (AppDbContext db, User user) =>
        {
            user.PasswordHash = passwordService.HashPassword(user, user.PasswordHash);
            // ...저장 로직...

            db.Users.Add(user);
            await db.SaveChangesAsync();
            //return Results.Created($"/users/{user.Id}", user);

            var userResponse = new
            {
                user.Id,
                user.Name,
                user.LoginId,
                user.Sex,
                user.Photo,
                user.EMail
            };
            return Results.Created($"/api/users/{user.Id}", userResponse);


        });


        // 1. 전체 조회 (GET /users)
        group.MapGet("/", async (AppDbContext db) =>
        {    //await db.Users.ToListAsync();
          await db.Users.Select(u => new { u.Id, u.Name, u.LoginId, u.Sex, u.Photo, u.EMail }).ToListAsync();
        });

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
            await db.SaveChangesAsync();var userResponse = new
            {
                user.Id,
                user.Name,
                user.LoginId,
                user.Sex,
                user.Photo,
                user.EMail
            };
            return Results.Created($"/api/users/{user.Id}", userResponse);
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


        // 로그인 (JWT 토큰 발급)
        group.MapPost("/login", async (AppDbContext db, IConfiguration config, LoginRequest req) =>
        {
            var user = db.Users.FirstOrDefault(u => u.LoginId == req.LoginId);
            if (user is null || !passwordService.VerifyPassword(user, req.Password))
                return Results.Unauthorized();

            // JWT 토큰 생성
            var jwtKey = config["Jwt:Key"] ?? "quristyle_blabbbbbla_secret_key_1234567890!@#$";
            var jwtIssuer = config["Jwt:Issuer"] ?? "JinRestApi";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.LoginId ?? user.Name ?? user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            //return Results.Ok(new { token = tokenString });
             return Results.Ok(new
            {
                token = tokenString,
                user = new {
                    user.Id,
                    user.Name,
                    user.LoginId,
                    user.Sex,
                    user.Photo,
                    user.EMail
                }
            });
        });


    }
}
