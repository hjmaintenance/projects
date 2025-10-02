using System;
using JinRestApi.Services;
using System.Collections.Generic;

namespace JinRestApi.Models
{

    /// <summary>관리자</summary>
    public class Admin : BaseEntity, IPasswordEnabled
    {
        /// <summary>로그인 ID</summary>
        public string LoginId { get; set; } = string.Empty;

        /// <summary>관리자 이름</summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>이메일</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>비밀번호 해시</summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>최초 로그인시 암호 변경 필요 여부</summary>
        public bool MustChangePassword { get; set; } = true;

        /// <summary>팀 ID</summary>
        public int TeamId { get; set; }

        public Team? Team { get; set; }

        public ICollection<ImprovementRequest> AssignedRequests { get; set; } = new List<ImprovementRequest>();
        //public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();


    }

}