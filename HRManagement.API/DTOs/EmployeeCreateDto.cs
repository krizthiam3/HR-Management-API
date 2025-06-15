using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.DTOs;

public class EmployeeCreateDto
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    public string Phone { get; set; } = null!;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public int PositionId { get; set; }


}
