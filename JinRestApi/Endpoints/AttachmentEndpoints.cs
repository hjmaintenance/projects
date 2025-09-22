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

        group.MapGet("/", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Attachments.ToListAsync()
        ));

        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Attachments.FirstOrDefaultAsync(a => a.Id == id)
        ));

        group.MapPost("/", (AppDbContext db, Attachment attachment) => ApiResponseBuilder.CreateAsync(async () =>
        {
            db.Attachments.Add(attachment);
            await db.SaveChangesAsync();
            return attachment;
        }, "Attachment created successfully.", 201));

        group.MapDelete("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var attachment = await db.Attachments.FindAsync(id);
            if (attachment is null) return null;

            db.Attachments.Remove(attachment);
            await db.SaveChangesAsync();
            return new { DeletedId = id };
        }, "Attachment deleted successfully."));
    }
}
