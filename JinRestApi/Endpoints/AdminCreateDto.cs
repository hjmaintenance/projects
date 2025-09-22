using System.ComponentModel.DataAnnotations;

namespace JinRestApi.Dtos;

public record AdminCreateDto([Required] string LoginId, [Required] string UserName, [Required] string Email, [Required] string Password, int TeamId, string? CreatedBy, string? MenuContext);