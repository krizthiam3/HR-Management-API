namespace HRManagement.API.Models;

public class ErrorResponse
{
    public int Status { get; set; }
    public string Message { get; set; } = "Ocurrió un error inesperado.";
    public string? Details { get; set; }
}
