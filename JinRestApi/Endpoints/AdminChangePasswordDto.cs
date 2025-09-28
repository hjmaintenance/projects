using System.ComponentModel.DataAnnotations;

namespace JinRestApi.Dtos;

public record AdminChangePasswordDto([Required] string OldPassword, [Required] string NewPassword);
