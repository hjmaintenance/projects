using System.ComponentModel.DataAnnotations;

namespace JinRestApi.Dtos;

public record CustomerCreateDto([Required] string LoginId, [Required] string UserName, [Required] string Email, [Required] string Password, int CompanyId, string? Sex, string? Photo, string? CreatedBy, string? MenuContext);