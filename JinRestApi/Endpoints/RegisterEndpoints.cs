using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JinRestApi.Models;
using Microsoft.AspNetCore.Identity;
using JinRestApi.Services;
using JinRestApi.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using JinRestApi.Dtos;



namespace JinRestApi.Endpoints;

public static class RegisterEndpoints
{
    //public record CustomerCreateDto([Required] string LoginId, [Required] string UserName, [Required] string Email, [Required] string Password, int CompanyId, string? Sex, string? Photo, string? CreatedBy, string? MenuContext);

    public static void MapRegistEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/users");


        var passwordService = new PasswordService();

        // 회원가입
        group.MapPost("/singup", (AppDbContext db, CustomerCreateDto customerDto) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var customer = new Customer
            {
                LoginId = customerDto.LoginId,
                UserName = customerDto.UserName,
                Email = customerDto.Email,
                CompanyId = customerDto.CompanyId,
                Sex = customerDto.Sex ?? "M",
                Photo = customerDto.Photo ?? "",
                CreatedBy = customerDto.CreatedBy ?? "system",
                MenuContext = customerDto.MenuContext
            };
            customer.PasswordHash = passwordService.HashPassword<Customer>(customer, customerDto.Password);

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            return new
            {
                customer.Id,
                customer.UserName,
                customer.LoginId
            };
        }, "User registered successfully.", 201));

        // 로그인 (JWT 토큰 발급)
        group.MapPost("/login", (AppDbContext db, IConfiguration config, LoginRequest req) =>
        {
            // ApiResponseBuilder는 성공/실패만 다루므로, 인증 실패는 별도 처리합니다.
            return ApiResponseBuilder.CreateAsync(async () =>
            {

                string user_uid = "";
                string user_name = "";
                string login_id = "";
                string login_type = "";
                string Photo = "";
                string email = "";
                string affiliation = ""; // 소속
                bool mustChangePassword = false;


                Console.WriteLine($"req.LoginType : {req.LoginType}");
                Console.WriteLine($"req.LoginId : {req.LoginId}");
                Console.WriteLine($"req.Password : {req.Password}");    


                if (req.LoginType == "admin")
                {
                    var admin = await db.Admins.Include(a => a.Team).FirstOrDefaultAsync(a => a.LoginId == req.LoginId);

                    
                    Console.WriteLine($"admin is null : {(admin is null)}");    
                    Console.WriteLine($"admin password : {req.Password}");    
                    Console.WriteLine($"admin password check  : {(passwordService.VerifyPassword<Admin>(admin, req.Password))}");    

                    if (admin is null || !passwordService.VerifyPassword<Admin>(admin, req.Password))
                    {


                        Console.WriteLine($"admin not : {(admin is null)}");


                        return null;
                    }
                    user_name = admin.UserName;
                    login_id = admin.LoginId;
                    login_type = "admin";
                    Photo = "";
                    email = admin.Email;
                    user_uid = admin.Id.ToString();
                    mustChangePassword = admin.MustChangePassword;
                    affiliation = admin.Team?.Name ?? "";
                }
                else
                {

                    var user = await db.Customers.Include(u => u.Company).FirstOrDefaultAsync(u => u.LoginId == req.LoginId);
                    if (user is null || !passwordService.VerifyPassword<Customer>(user, req.Password))
                    {
                        // 성공 경로가 아니므로 예외를 발생시켜 Builder의 catch 블록에서 처리하도록 합니다.
                        // 또는 여기서 직접 Unauthorized를 반환할 수도 있습니다. 여기서는 null을 반환하여 404처럼 처리합니다.
                        // 더 나은 방법은 인증 실패에 대한 별도 응답을 만드는 것입니다.
                        // 여기서는 null을 반환하여 '찾을 수 없음'으로 처리합니다.
                        return null;
                    }
                    user_name = user.UserName;
                    login_id = user.LoginId;
                    login_type = "customer";
                    Photo = user.Photo;
                    email = user.Email;
                    user_uid = user.Id.ToString();
                    affiliation = user.Company?.Name ?? "";

                }       


                // JWT 토큰 생성
                var jwtKey = config["Jwt:Key"] ?? "quristyle_blabbbbbla_secret_key_1234567890!@#$";
                var jwtIssuer = config["Jwt:Issuer"] ?? "JinRestApi";
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, login_id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("uid", user_uid),
                    new Claim("login_type", login_type)
                };

                var token = new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: null,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(12),
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return new
                {
                    token = tokenString,
                    user = new
                    {
                        user_uid,
                        user_name,
                        login_id,
                        login_type,
                        Photo,
                        email,
                        affiliation, // 소속
                        mustChangePassword
                    }
                };
            }, "Login successful.");
        });
    }
}
