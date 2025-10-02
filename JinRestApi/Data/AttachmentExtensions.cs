using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using JinRestApi.Models;

using System.Reflection;
using System.Xml.Linq;



namespace JinRestApi.Data;

public static class AttachmentExtensions
{
    
    public static IQueryable<TEntityWithAttachments<TEntity>> IncludeAttachments<TEntity>(
        this IQueryable<TEntity> query,
        DbContext db
    )
        where TEntity : class
    {
        var entityTypeName = typeof(TEntity).Name;

        return query.Select(entity => new TEntityWithAttachments<TEntity>
        {
            Entity = entity,
            Attachments = db.Set<Attachment>()
                .Where(a => a.EntityType == entityTypeName &&
                            a.EntityId == EF.Property<int>(entity, "Id"))
                .ToList()
        });
    }
}

/// <summary>
/// Attachments를 포함한 DTO Wrapper
/// </summary>
public class TEntityWithAttachments<TEntity>
{
    public TEntity Entity { get; set; } = default!;
    public List<Attachment> Attachments { get; set; } = new();
}

