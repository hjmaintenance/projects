using System.ComponentModel.DataAnnotations;

namespace JinRestApi.Dtos;

public record NoticeCreateDto(
    int? Id,
    [Required] string Title,
    [Required] string Content,
    string? CreatedBy = null,
    DateTime? CreatedAt = null
);
