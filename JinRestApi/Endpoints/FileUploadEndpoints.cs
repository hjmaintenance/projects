
using JinRestApi.Data;
using JinRestApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JinRestApi.Endpoints;

public static class FileUploadEndpoints
{
    public static void MapFileUploadEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/files");

        group.MapPost("/upload", async (
            [FromForm] IFormFileCollection files,
            [FromForm] string entityType,
            [FromForm] int entityId,
            AppDbContext db) =>
        {
            var attachments = new List<Attachment>();
            var storagePath = "/home/quri/jinAttachment";
            Directory.CreateDirectory(storagePath);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var storedFileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(storagePath, storedFileName);

                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var attachment = new Attachment
                    {
                        OriginalFileName = file.FileName,
                        StoredFileName = storedFileName,
                        FilePath = storagePath,
                        FileType = file.ContentType,
                        FileSize = file.Length,
                        EntityType = entityType,
                        EntityId = entityId,
                        UploadedAt = DateTime.UtcNow
                    };
                    attachments.Add(attachment);
                }
            }

            if (attachments.Any())
            {
                db.Attachments.AddRange(attachments);
                await db.SaveChangesAsync();
            }

            return Results.Ok(attachments);
        })
        .DisableAntiforgery();
    }
}
