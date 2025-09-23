using System.ComponentModel.DataAnnotations;

namespace JinRestApi.Dtos;

public record RequestCreateDto(
    [Required] string Title,
    string? Description,
    [Required] int CustomerId,
    string? CreatedBy,
    string? MenuContext
);