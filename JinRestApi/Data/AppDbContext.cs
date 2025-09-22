using Microsoft.EntityFrameworkCore;
using JinRestApi.Models;

using System.Reflection;
using System.Xml.Linq; 



namespace JinRestApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



    /// <summary>고객사 테이블</summary>
    public DbSet<CustomerCompany> Companies { get; set; }

    /// <summary>고객 테이블</summary>
    public DbSet<Customer> Customers { get; set; }

    /// <summary>관리자 테이블</summary>
    public DbSet<Admin> Admins { get; set; }

    /// <summary>팀 테이블</summary>
    public DbSet<Team> Teams { get; set; }

    /// <summary>팀-회사 매핑 테이블</summary>
    public DbSet<TeamCompany> TeamCompanies { get; set; }

    /// <summary>개선요청 테이블</summary>
    public DbSet<ImprovementRequest> Requests { get; set; }

    /// <summary>덧글 테이블</summary>
    public DbSet<ImprovementComment> Comments { get; set; }

    /// <summary>첨부파일 테이블</summary>
    public DbSet<Attachment> Attachments { get; set; }




    public override int SaveChanges()
    {
        SetAuditProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditProperties()
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            var now = DateTime.UtcNow;
            var user = "system"; // 기본값

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedAt = now;
                if (string.IsNullOrEmpty(entityEntry.Entity.CreatedBy))
                {
                    entityEntry.Entity.CreatedBy = user;
                }
            }

            entityEntry.Entity.ModifiedAt = now;

            if (string.IsNullOrEmpty(entityEntry.Entity.ModifiedBy)) entityEntry.Entity.ModifiedBy = user;
            if (string.IsNullOrEmpty(entityEntry.Entity.MenuContext)) entityEntry.Entity.MenuContext = user;
        }
    }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // set default schema
        modelBuilder.HasDefaultSchema("jsini");





        // 팀-회사 N:N 관계 키 설정
        modelBuilder.Entity<TeamCompany>()
            .HasKey(tc => new { tc.TeamId, tc.CompanyId });

        modelBuilder.Entity<TeamCompany>()
            .HasOne(tc => tc.Team)
            .WithMany(t => t.TeamCompanies)
            .HasForeignKey(tc => tc.TeamId);

        modelBuilder.Entity<TeamCompany>()
            .HasOne(tc => tc.Company)
            .WithMany()
            .HasForeignKey(tc => tc.CompanyId);

        // 개선요청과 덧글 관계
        modelBuilder.Entity<ImprovementComment>()
            .HasOne(c => c.Request)
            .WithMany(r => r.Comments)
            .HasForeignKey(c => c.RequestId);

        // 개선요청과 관리자 관계
        modelBuilder.Entity<ImprovementRequest>()
            .HasOne(r => r.Admin)
            .WithMany(a => a.AssignedRequests)
            .HasForeignKey(r => r.AdminId)
            .OnDelete(DeleteBehavior.SetNull);

        // 개선요청과 고객 관계
        modelBuilder.Entity<ImprovementRequest>()
            .HasOne(r => r.Customer)
            .WithMany(c => c.ImprovementRequests)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // 첨부파일: 다형성 관계 (EntityType, EntityId)
        modelBuilder.Entity<Attachment>()
            .Property(a => a.EntityType)
            .IsRequired()
            .HasMaxLength(50);













        /*

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
        */


        //foreach (var entity in modelBuilder.Model.GetEntityTypes())        {
        // 테이블/컬럼명 소문자 변환 및 XML 주석을 DB 코멘트로 적용
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

        //var xmlPath = Path.Combine(AppContext.BaseDirectory, "JinRestApi.xml");
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        XDocument xmlDoc = null;
        if (File.Exists(xmlPath))
        {
            xmlDoc = XDocument.Load(xmlPath);
        }

        // BaseEntity를 상속하는 모든 엔티티의 Id 속성에 대해 자동 증가(Identity) 설정
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(e => e.ClrType.IsSubclassOf(typeof(BaseEntity))))
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(BaseEntity.Id))
                .UseIdentityByDefaultColumn();
        }

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // 테이블명 소문자화
            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                entity.SetTableName(tableName.ToLower());
            }

            if (xmlDoc != null)
            {
                // 클래스(테이블) 주석 설정
                var typeMemberName = $"T:{entity.ClrType.FullName}";
                var typeSummary = xmlDoc.Descendants("member")
                    .FirstOrDefault(m => m.Attribute("name")?.Value == typeMemberName)?
                    .Element("summary")?.Value.Trim();

                if (!string.IsNullOrEmpty(typeSummary))
                {
                    entity.SetComment(typeSummary);
                }
            }

            foreach (var property in entity.GetProperties())
            {

                // 컬럼명 소문자화
                property.SetColumnName(property.Name.ToLower());

                if (xmlDoc != null)
                {
                    // 속성(컬럼) 주석 설정
                    var propMemberName = $"P:{entity.ClrType.FullName}.{property.Name}";
                    var propSummary = xmlDoc.Descendants("member")
                        .FirstOrDefault(m => m.Attribute("name")?.Value == propMemberName)?
                        .Element("summary")?.Value.Trim();

                    if (!string.IsNullOrEmpty(propSummary))
                    {
                        property.SetComment(propSummary);
                    }
                }
            }

        }

    }

}
