using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.DTOs;

public class EmployeeUpdateDto : EmployeeCreateDto
{
    [Required]
    public int Id { get; set; }
}
