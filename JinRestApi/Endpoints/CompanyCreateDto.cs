using System.ComponentModel.DataAnnotations;

namespace JinRestApi.Dtos;

public record CompanyCreateDto([Required] string Name, string? ModifiedBy, string? MenuContext);