using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.DTOs;

public class DepartmentUpdateDto : DepartmentCreateDto
{
    [Required]
    public int Id { get; set; }
}
