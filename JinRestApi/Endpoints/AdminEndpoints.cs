using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;

namespace JinRestApi.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/admins");

        group.MapGet("/", async (AppDbContext db) =>
            await db.Admins.Include(a => a.Attachments).ToListAsync());

        group.MapGet("/{id}", async (AppDbContext db, int id) =>
            await db.Admins.Include(a => a.Attachments).FirstOrDefaultAsync(a => a.Id == id));

        group.MapPost("/", async (AppDbContext db, Admin admin) =>
        {
            db.Admins.Add(admin);
            await db.SaveChangesAsync();
            return Results.Created($"/api/admins/{admin.Id}", admin);
        });

        group.MapPut("/{id}", async (AppDbContext db, int id, Admin input) =>
        {
            var admin = await db.Admins.FindAsync(id);
            if (admin is null) return Results.NotFound();

            admin.UserName = input.UserName;
            admin.ModifiedBy = input.ModifiedBy;
            admin.MenuContext = input.MenuContext;
            admin.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return Results.Ok(admin);
        });

        group.MapDelete("/{id}", async (AppDbContext db, int id) =>
        {
            var admin = await db.Admins.FindAsync(id);
            if (admin is null) return Results.NotFound();

            db.Admins.Remove(admin);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}
