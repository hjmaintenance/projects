using Microsoft.EntityFrameworkCore;
using JinRestApi.Models;

using System.Reflection;
using System.Xml.Linq;



namespace JinRestApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
















    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // set default schema
        modelBuilder.HasDefaultSchema("jsini"); 

        // lower 
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
                entity.SetTableName(tableName.ToLower());

            foreach (var property in entity.GetProperties())
            {
                if (!string.IsNullOrEmpty(property.Name))
                    property.SetColumnName(property.Name.ToLower());
            }
        }

        // comment from 코드 주석
        var xmlPath = Path.Combine(AppContext.BaseDirectory, "JinRestApi.xml");
        if (File.Exists(xmlPath))
        {
            var xml = XDocument.Load(xmlPath);

            foreach (var prop in typeof(User).GetProperties())
            {
                var memberName = $"P:{prop.DeclaringType.FullName}.{prop.Name}";
                var summary = xml.Descendants("member")
                    .FirstOrDefault(m => m.Attribute("name")?.Value == memberName)?
                    .Element("summary")?.Value.Trim();

                if (!string.IsNullOrEmpty(summary))
                {
                    modelBuilder.Entity<User>()
                        .Property(prop.Name)
                        .HasComment(summary);
                }
            }
        }
    }



}

