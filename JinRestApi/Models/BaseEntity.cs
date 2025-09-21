using System;
using System.Collections.Generic;

namespace JinRestApi.Models
{
    /// <summary>공통 베이스 엔티티 (생성/수정 이력 관리)</summary>
    public abstract class BaseEntity
    {
        /// <summary>PK</summary>
        public int Id { get; set; }

        /// <summary>생성자 (로그인 ID)</summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>생성일시</summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>수정자 (로그인 ID)</summary>
        public string? ModifiedBy { get; set; }

        /// <summary>수정일시</summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>작업 메뉴/화면 정보</summary>
        public string? MenuContext { get; set; }
    }

    /// <summary>고객사</summary>
    public class CustomerCompany : BaseEntity
    {
        /// <summary>고객사 명</summary>
        public string Name { get; set; } = string.Empty;

        public ICollection<Customer> Customers { get; set; } = new List<Customer>();
        public ICollection<TeamCompany> TeamCompanies { get; set; } = new List<TeamCompany>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }


    /// <summary>관리자</summary>
    public class Admin : BaseEntity
    {
        /// <summary>로그인 ID</summary>
        public string LoginId { get; set; } = string.Empty;

        /// <summary>관리자 이름</summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>이메일</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>비밀번호 해시</summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>팀 ID</summary>
        public int TeamId { get; set; }

        public Team? Team { get; set; }

        public ICollection<ImprovementRequest> AssignedRequests { get; set; } = new List<ImprovementRequest>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
   
   
    }

    /// <summary>팀</summary>
    public class Team : BaseEntity
    {
        /// <summary>팀 명</summary>
        public string Name { get; set; } = string.Empty;

        public ICollection<Admin> Admins { get; set; } = new List<Admin>();
        public ICollection<TeamCompany> TeamCompanies { get; set; } = new List<TeamCompany>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    /// <summary>팀-회사 관계 (N:N)</summary>
    public class TeamCompany
    {
        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public int CompanyId { get; set; }
        public CustomerCompany? Company { get; set; }
    }

    /// <summary>개선 요청</summary>
    public class ImprovementRequest : BaseEntity
    {
        /// <summary>제목</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>내용</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>요청일시</summary>
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        /// <summary>고객 ID</summary>
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        /// <summary>담당 관리자 ID</summary>
        public int? AdminId { get; set; }

        public Admin? Admin { get; set; }

        /// <summary>상태</summary>
        public ImprovementStatus Status { get; set; } = ImprovementStatus.Pending;

        public ICollection<ImprovementComment> Comments { get; set; } = new List<ImprovementComment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    /// <summary>개선 요청 상태</summary>
    public enum ImprovementStatus
    {
        Pending,
        InProgress,
        Completed,
        Rejected
    }

    /// <summary>개선 요청 덧글</summary>
    public class ImprovementComment : BaseEntity
    {
        /// <summary>개선 요청 ID</summary>
        public int RequestId { get; set; }

        public ImprovementRequest? Request { get; set; }

        /// <summary>덧글 내용</summary>
        public string CommentText { get; set; } = string.Empty;

        /// <summary>작성자 구분</summary>
        public string AuthorType { get; set; } = "Customer"; // "Customer" or "Admin"

        /// <summary>작성자 ID</summary>
        public int AuthorId { get; set; }

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    /// <summary>공통 첨부파일</summary>
    public class Attachment : BaseEntity
    {
        /// <summary>연결된 엔티티 타입</summary>
        public string EntityType { get; set; } = string.Empty;  // "ImprovementRequest", "Team", "Company", "Comment"

        /// <summary>연결된 엔티티 PK</summary>
        public int EntityId { get; set; }

        /// <summary>파일명</summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>파일 경로</summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>업로드 일시</summary>
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
