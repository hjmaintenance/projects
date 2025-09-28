using System.ComponentModel.DataAnnotations;

namespace JinRestApi.Dtos;

public record TeamCreateDto([Required] string Name, string? ModifiedBy, string? MenuContext);
