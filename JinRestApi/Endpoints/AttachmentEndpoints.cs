using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;

namespace JinRestApi.Endpoints;

public static class AttachmentEndpoints
{
    public static void MapAttachmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/attachments");

        group.MapGet("/", async (AppDbContext db) =>
            await db.Attachments.ToListAsync());

        group.MapGet("/{id}", async (AppDbContext db, int id) =>
            await db.Attachments.FirstOrDefaultAsync(a => a.Id == id));

        group.MapPost("/", async (AppDbContext db, Attachment attachment) =>
        {
            db.Attachments.Add(attachment);
            await db.SaveChangesAsync();
            return Results.Created($"/api/attachments/{attachment.Id}", attachment);
        });

        group.MapDelete("/{id}", async (AppDbContext db, int id) =>
        {
            var attachment = await db.Attachments.FindAsync(id);
            if (attachment is null) return Results.NotFound();

            db.Attachments.Remove(attachment);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}
