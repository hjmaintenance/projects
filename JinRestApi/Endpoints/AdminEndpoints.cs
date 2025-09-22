using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;
using JinRestApi.Services;
using System.ComponentModel.DataAnnotations;

namespace JinRestApi.Endpoints;

public static class AdminEndpoints
{
    public record AdminCreateDto([Required] string LoginId, [Required] string UserName, [Required] string Email, [Required] string Password, int TeamId, string? CreatedBy, string? MenuContext);

    public static void MapAdminEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/admins");

        group.MapGet("/", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Admins.Include(a => a.Attachments).ToListAsync()
        ));

        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Admins.Include(a => a.Attachments).FirstOrDefaultAsync(a => a.Id == id)
        ));

        group.MapPost("/", (AppDbContext db, AdminCreateDto adminDto) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var passwordService = new PasswordService();
            var admin = new Admin
            {
                LoginId = adminDto.LoginId,
                UserName = adminDto.UserName,
                Email = adminDto.Email,
                TeamId = adminDto.TeamId,
                CreatedBy = adminDto.CreatedBy ?? "system",
                MenuContext = adminDto.MenuContext
            };
            admin.PasswordHash = passwordService.HashPassword<Admin>(admin, adminDto.Password);

            db.Admins.Add(admin);
            await db.SaveChangesAsync();
            return admin;
        }, "Admin created successfully.", 201));

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
