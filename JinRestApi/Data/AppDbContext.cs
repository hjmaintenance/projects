using Microsoft.EntityFrameworkCore;
using JinRestApi.Models;

using System.Reflection;
using System.Xml.Linq;



namespace JinRestApi.Data;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    // 마이그레이션 도구가 매개변수 없는 생성자를 사용할 수 있도록 유지합니다.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DI를 통해 IHttpContextAccessor를 주입받는 생성자를 추가합니다.
    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

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

    /// <summary>공지사항 테이블</summary>
    public DbSet<Notice> Notices { get; set; }



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
            var claimsPrincipal = _httpContextAccessor?.HttpContext?.User;
            string user;

            if (claimsPrincipal?.Identity?.IsAuthenticated ?? false)
            {
                // 'uid' 클레임을 먼저 찾고, 없으면 'sub'(LoginId)를, 그것도 없으면 "system"을 사용합니다.
                user = claimsPrincipal.FindFirst("uid")?.Value ?? claimsPrincipal.Identity.Name ?? "system";
            }
            else
            {
                user = "system"; // 인증되지 않은 요청의 경우
            }

            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Entity.CreatedAt = now;
                if (string.IsNullOrEmpty(entityEntry.Entity.CreatedBy))
                {
                    entityEntry.Entity.CreatedBy = user; // 생성자 자동 설정
                }
            }

            entityEntry.Entity.ModifiedAt = now;
            entityEntry.Entity.ModifiedBy = user; // 수정자 자동 설정

            entityEntry.Entity.ActionService = _httpContextAccessor?.HttpContext?.Request.Headers["X-Service-Name"]; // ActionService 자동 설정
            entityEntry.Entity.MenuContext = _httpContextAccessor?.HttpContext?.Request.Headers["X-Menu-Name"]; // MenuContext 자동 설정

            if (string.IsNullOrEmpty(entityEntry.Entity.MenuContext)) entityEntry.Entity.MenuContext = user; // MenuContext 기본값
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

        // 덧글의 계층 구조(대댓글)를 위한 자체 참조 관계 설정
        modelBuilder.Entity<ImprovementComment>()
            .HasOne(c => c.ParentComment) // 각 덧글은 하나의 부모를 가질 수 있음
            .WithMany(c => c.Children)    // 각 덧글은 여러 자식 덧글을 가질 수 있음
            .HasForeignKey(c => c.ParentCommentId) // 외래 키는 ParentCommentId
            .OnDelete(DeleteBehavior.Restrict); // 순환 참조 또는 다중 캐스케이드 경로 문제를 방지하기 위해 Restrict 사용

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
