using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.DTOs;

public class DepartmentCreateDto
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Location { get; set; } = null!;
}
