namespace HRManagement.API.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public DateTime BirthDate { get; set; }
    public decimal Salary { get; set; }
    public string DepartmentName { get; set; } = null!;
    public string PositionTitle { get; set; } = null!;
}
