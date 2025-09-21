using Microsoft.EntityFrameworkCore;
using JinRestApi.Models;

using System.Reflection;
using System.Xml.Linq;



namespace JinRestApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companys => Set<Company>();



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

