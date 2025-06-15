using Microsoft.AspNetCore.Mvc;
using HRManagement.API.Models;
using HRManagement.API.DTOs;
using HRManagement.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HRManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repo;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(IEmployeeRepository repo, ILogger<EmployeesController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var employees = await _repo.GetAllAsync();

            var result = employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                FullName = $"{e.FirstName} {e.LastName}",
                Email = e.Email,
                Phone = e.Phone,
                HireDate = e.HireDate,
                Salary = e.Salary,
                DepartmentName = e.Department?.Name ?? "-",
                PositionTitle = e.Position?.Title ?? "-"
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de empleados");
            return StatusCode(500, new { message = "Ocurrió un error inesperado al consultar empleados." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateDto dto)
    {
        try
        {
            var exists = await _repo.ExistsByEmailAsync(dto.Email);
            if (exists)
                return BadRequest(new { message = $"El correo '{dto.Email}' ya está registrado." });

            var employee = new Employee
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                Phone = dto.Phone.Trim(),
                HireDate = dto.HireDate,
                BirthDate = dto.BirthDate,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId,
                PositionId = dto.PositionId
            };

            await _repo.AddAsync(employee);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear un empleado");
            return StatusCode(500, new { message = "Error interno al crear el empleado." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var e = await _repo.GetByIdAsync(id);
            if (e == null) return NotFound(new { message = "Empleado no encontrado." });

            var dto = new EmployeeDto
            {
                Id = e.Id,
                FullName = $"{e.FirstName.Trim()} {e.LastName.Trim()}",
                Email = e.Email.Trim(),
                Phone = e.Phone.Trim(),
                HireDate = e.HireDate,
                BirthDate = e.BirthDate,
                Salary = e.Salary,
                DepartmentName = e.Department?.Name ?? "-",
                PositionTitle = e.Position?.Title ?? "-"
               
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar el empleado con ID {Id}", id);
            return StatusCode(500, new { message = "Error interno al consultar el empleado." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, EmployeeUpdateDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(new { message = "El ID de la URL no coincide con el del cuerpo." });

            var exists = await _repo.ExistsAsync(id);
            if (!exists)
                return NotFound(new { message = "Empleado no encontrado." });

            var e = new Employee
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                BirthDate = dto.BirthDate,
                HireDate = dto.HireDate,
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId,
                PositionId = dto.PositionId
            };

            await _repo.UpdateAsync(e);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar el empleado con ID {Id}", id);
            return StatusCode(500, new { message = "Error interno al actualizar el empleado." });
        }
    }

    [Authorize(Roles = "Admin")]

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var exists = await _repo.ExistsAsync(id);
            if (!exists)
                return NotFound(new { message = "Empleado no encontrado." });

            await _repo.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el empleado con ID {Id}", id);
            return StatusCode(500, new { message = "Error interno al eliminar el empleado." });
        }
    }
}
