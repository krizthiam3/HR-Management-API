using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.DTOs;

public class PositionCreateDto
{
    [Required]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}
