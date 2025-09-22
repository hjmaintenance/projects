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

        group.MapGet("/", (AppDbContext db) => ApiResponseBuilder.CreateAsync(
            () => db.Comments.Include(c => c.Attachments).ToListAsync()
        ));

        group.MapGet("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(
            () => db.Comments.Include(c => c.Attachments).FirstOrDefaultAsync(c => c.Id == id)
        ));

        group.MapPost("/", (AppDbContext db, ImprovementComment comment) => ApiResponseBuilder.CreateAsync(async () =>
        {
            db.Comments.Add(comment);
            await db.SaveChangesAsync();
            return comment;
        }, "Comment created successfully.", 201));

        group.MapPut("/{id}", (AppDbContext db, int id, ImprovementComment input) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var comment = await db.Comments.FindAsync(id);
            if (comment is null) return null;

            comment.CommentText = input.CommentText;
            comment.RequestId = input.RequestId;
            comment.AuthorType = input.AuthorType;
            comment.AuthorId = input.AuthorId;

            await db.SaveChangesAsync();
            return comment;
        }, "Comment updated successfully."));

        group.MapDelete("/{id}", (AppDbContext db, int id) => ApiResponseBuilder.CreateAsync(async () =>
        {
            var comment = await db.Comments.FindAsync(id);
            if (comment is null) return null;

            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return new { DeletedId = id };
        }, "Comment deleted successfully."));
    }
}
