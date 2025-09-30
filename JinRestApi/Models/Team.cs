

using System;
using JinRestApi.Services;
using System.Collections.Generic;

namespace JinRestApi.Models
{


    /// <summary>팀</summary>
    public class Team : BaseEntity
    {
        /// <summary>팀 명</summary>
        public string Name { get; set; } = string.Empty;

        public ICollection<Admin> Admins { get; set; } = new List<Admin>();
        public ICollection<TeamCompany> TeamCompanies { get; set; } = new List<TeamCompany>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}