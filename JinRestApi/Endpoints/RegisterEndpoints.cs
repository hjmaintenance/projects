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

public static class RegisterEndpoints
{

    public static void MapRegistEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users");


        var passwordService = new PasswordService();

        // 회원가입
        group.MapPost("/singup", async (AppDbContext db, Customer user) =>
        {
            user.PasswordHash = passwordService.HashPassword(user, user.PasswordHash);
            // ...저장 로직...

            db.Customers.Add(user);
            await db.SaveChangesAsync();
            //return Results.Created($"/users/{user.Id}", user);

            var userResponse = new
            {
                user.Id,
                user.UserName,
                user.LoginId,
                user.Sex,
                user.Photo,
                user.Email
            };
            return Results.Created($"/api/users/{user.Id}", userResponse);


        });

        // 로그인 (JWT 토큰 발급)
        group.MapPost("/login", async (AppDbContext db, IConfiguration config, LoginRequest req) =>
        {
            var user = db.Customers.FirstOrDefault(u => u.LoginId == req.LoginId);
            if (user is null || !passwordService.VerifyPassword(user, req.Password))
                return Results.Unauthorized();

            // JWT 토큰 생성
            var jwtKey = config["Jwt:Key"] ?? "quristyle_blabbbbbla_secret_key_1234567890!@#$";
            var jwtIssuer = config["Jwt:Issuer"] ?? "JinRestApi";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.LoginId ?? user.UserName ?? user.Id.ToString()),
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
                    user.UserName,
                    user.LoginId,
                    user.Sex,
                    user.Photo,
                    user.Email
                }
            });
        });


    }
}
