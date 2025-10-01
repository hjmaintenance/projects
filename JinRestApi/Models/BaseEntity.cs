using System;
using JinRestApi.Services;
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

        /// <summary>작업 서비스</summary>
        public string? ActionService { get; set; }
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
        // public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }


    /// <summary>팀-회사 관계 (N:N)</summary>
    public class TeamCompany
    {
        public int TeamId { get; set; }
        public Team? Team { get; set; }

        public int CompanyId { get; set; }
        public CustomerCompany? Company { get; set; }
    }


    /// <summary>개선 요청 상태</summary>
    public enum ImprovementStatus
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "접수대기")]
        Pending,

        [System.ComponentModel.DataAnnotations.Display(Name = "처리중")]
        InProgress,

        [System.ComponentModel.DataAnnotations.Display(Name = "처리완료")]
        Completed,

        [System.ComponentModel.DataAnnotations.Display(Name = "반려")]
        Rejected,

        [System.ComponentModel.DataAnnotations.Display(Name = "삭제")]
        Delete
    }

}
