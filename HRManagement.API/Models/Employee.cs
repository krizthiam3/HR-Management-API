using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagement.API.Models;

public class Employee
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Phone]
    public string Phone { get; set; } = null!;

    [Required]
    public DateTime HireDate { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Salary must be positive.")]
    public decimal Salary { get; set; }

    // Foreign Keys
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;
    public byte[]? Photo { get; set; } = null!;
}
