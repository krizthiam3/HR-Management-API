using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.Models;

public class Department
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Location { get; set; } = null!;
}
