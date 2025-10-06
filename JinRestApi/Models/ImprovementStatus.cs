
using System;
using JinRestApi.Services;
using System.Collections.Generic;

namespace JinRestApi.Models;

    /// <summary>개선 요청 상태</summary>
    public enum ImprovementStatus
    {
        [System.ComponentModel.DataAnnotations.Display(Name = "접수대기")]
        Pending,

        [System.ComponentModel.DataAnnotations.Display(Name = "처리중")]
        InProgress,

        [System.ComponentModel.DataAnnotations.Display(Name = "반려")]
        Rejected,

        [System.ComponentModel.DataAnnotations.Display(Name = "처리완료")]
        Completed,

        [System.ComponentModel.DataAnnotations.Display(Name = "삭제")]
        Delete
    }


