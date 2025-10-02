using System;
using JinRestApi.Services;
using System.Collections.Generic;

namespace JinRestApi.Models
{

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
        //public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}