using System.ComponentModel.DataAnnotations;

namespace HRManagement.API.DTOs;

public class PositionUpdateDto : PositionCreateDto
{
    [Required]
    public int Id { get; set; }
}
