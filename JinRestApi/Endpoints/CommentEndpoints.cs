using JinRestApi.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using JinRestApi.Data;

namespace JinRestApi.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/comments");

        group.MapGet("/", async (AppDbContext db) =>
            await db.Comments.Include(c => c.Attachments).ToListAsync());

        group.MapGet("/{id}", async (AppDbContext db, int id) =>
            await db.Comments.Include(c => c.Attachments).FirstOrDefaultAsync(c => c.Id == id));

        group.MapPost("/", async (AppDbContext db, ImprovementComment comment) =>
        {
            db.Comments.Add(comment);
            await db.SaveChangesAsync();
            return Results.Created($"/api/comments/{comment.Id}", comment);
        });

        group.MapPut("/{id}", async (AppDbContext db, int id, ImprovementComment input) =>
        {
            var comment = await db.Comments.FindAsync(id);
            if (comment is null) return Results.NotFound();

            comment.CommentText = input.CommentText;
            comment.RequestId = input.RequestId;
            comment.ModifiedBy = input.ModifiedBy;
            comment.MenuContext = input.MenuContext;
            comment.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
            return Results.Ok(comment);
        });

        group.MapDelete("/{id}", async (AppDbContext db, int id) =>
        {
            var comment = await db.Comments.FindAsync(id);
            if (comment is null) return Results.NotFound();

            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}
