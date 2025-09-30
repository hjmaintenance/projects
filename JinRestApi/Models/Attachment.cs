using System;
using JinRestApi.Services;
using System.Collections.Generic;

namespace JinRestApi.Models
{
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