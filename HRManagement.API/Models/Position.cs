using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.Models;

public class Position
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }
}
