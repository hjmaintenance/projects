using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;
using JinRestApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using JinRestApi.Dtos;

namespace JinRestApi.Endpoints;

public static class AdminEndpoints
{
    //public record AdminCreateDto([Required] string LoginId, [Required] string UserName, [Required] string Email, [Required] string Password, int TeamId, string? CreatedBy, string? MenuContext);

    public static void MapAdminEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/admins");

        group.MapGet("/", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Admins.ToListAsync()
        ));

        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Admins.FirstOrDefaultAsync(a => a.Id == id)
        ));

        group.MapPost("/", (AppDbContext db, AdminCreateDto adminDto) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var passwordService = new PasswordService();
            var tempPassword = Guid.NewGuid().ToString().Substring(0, 8);
            var admin = new Admin
            {
                LoginId = adminDto.LoginId,
                UserName = adminDto.UserName,
                Email = adminDto.Email,
                TeamId = adminDto.TeamId,
                CreatedBy = adminDto.CreatedBy ?? "system",
                MenuContext = adminDto.MenuContext,
                MustChangePassword = true
            };
            admin.PasswordHash = passwordService.HashPassword<Admin>(admin, tempPassword);

            db.Admins.Add(admin);
            await db.SaveChangesAsync();
            return new { admin, tempPassword };
        }, "Admin created successfully.", 201));

        group.MapPost("/change-password", async (HttpContext http, AppDbContext db, AdminChangePasswordDto changePasswordDto) =>
        {
            var passwordService = new PasswordService();

/*
                    new Claim(JwtRegisteredClaimNames.Sub, login_id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("uid", user_uid),
                    new Claim("login_type", login_type)
                    */

            var loginId = http.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var uid = http.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            var loginType = http.User.Claims.FirstOrDefault(c => c.Type == "login_type")?.Value;



            Console.WriteLine($"change-password uid : {uid}");
            Console.WriteLine($"change-password loginId : {loginId}");
            Console.WriteLine($"change-password loginType : {loginType}");
            


            if (loginId == null)
            {
                return Results.Unauthorized();
            }

            if (loginType == "admin")
            {
                var admin = await db.Admins.FirstOrDefaultAsync(a => a.Id+"" == uid);
                if (admin == null)
                {
                    return Results.NotFound("Admin not found.");
                }

                if (!passwordService.VerifyPassword(admin, changePasswordDto.OldPassword))
                {
                    return Results.BadRequest("Invalid old password.");
                }

                admin.PasswordHash = passwordService.HashPassword<Admin>(admin, changePasswordDto.NewPassword);
                admin.MustChangePassword = false;
                await db.SaveChangesAsync();

            }
            else
            {


                var customer = await db.Customers.FirstOrDefaultAsync(a => a.Id+"" == uid);
                if (customer == null)
                {
                    return Results.NotFound("Customer not found.");
                }

                if (!passwordService.VerifyPassword(customer, changePasswordDto.OldPassword))
                {
                    return Results.BadRequest("Invalid old password.");
                }

                customer.PasswordHash = passwordService.HashPassword<Customer>(customer, changePasswordDto.NewPassword);
                //customer.MustChangePassword = false;
                await db.SaveChangesAsync();
                
            }
                return Results.Ok("Password changed successfully.");
        }).RequireAuthorization();

        group.MapPut("/{id}", (AppDbContext db, int id, Admin input) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var admin = await db.Admins.FindAsync(id);
            if (admin is null) return null;

            admin.UserName = input.UserName;
            admin.Email = input.Email;
            admin.TeamId = input.TeamId;

            await db.SaveChangesAsync();
            return admin;
        }, "Admin updated successfully."));

        group.MapDelete("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var admin = await db.Admins.FindAsync(id);
            if (admin is null) return null;

            db.Admins.Remove(admin);
            await db.SaveChangesAsync();
            return new { DeletedId = id };
        }, "Admin deleted successfully."));
    }
}
