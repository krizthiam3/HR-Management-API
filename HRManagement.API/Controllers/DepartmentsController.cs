using Microsoft.AspNetCore.Mvc;
using HRManagement.API.Repositories.Interfaces;
using HRManagement.API.Models;
using HRManagement.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace HRManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _repo;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(IDepartmentRepository repo, ILogger<DepartmentsController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var departments = await _repo.GetAllAsync();

            var result = departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Location = d.Location
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener departamentos.");
            return StatusCode(500, new { message = "Ocurrió un error al obtener los departamentos." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var d = await _repo.GetByIdAsync(id);
            if (d == null)
                return NotFound(new { message = "Departamento no encontrado." });

            return Ok(new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Location = d.Location
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener departamento con ID {Id}", id);
            return StatusCode(500, new { message = "Error interno al consultar el departamento." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(DepartmentCreateDto dto)
    {
        try
        {
            var exists = await _repo.ExistsByNamelAsync(dto.Name);
            if (exists)
                return BadRequest(new { message = $"El Departamento '{dto.Name}' ya está registrado." });

            var d = new Department
            {
                Name = dto.Name.Trim(),
                Location = dto.Location.Trim()
            };

            await _repo.AddAsync(d);
            return CreatedAtAction(nameof(GetById), new { id = d.Id }, dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear departamento.");
            return StatusCode(500, new { message = "Error interno al crear el departamento." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DepartmentUpdateDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID de URL no coincide con el cuerpo." });

            var exists = await _repo.ExistsAsync(id);
            if (!exists)
                return NotFound(new { message = "Departamento no encontrado." });

            var d = new Department
            {
                Id = dto.Id,
                Name = dto.Name.Trim(),
                Location = dto.Location.Trim()
            };

            await _repo.UpdateAsync(d);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar departamento con ID {Id}", id);
            return StatusCode(500, new { message = "Error interno al actualizar el departamento." });
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
                return NotFound(new { message = "Departamento no encontrado." });

            await _repo.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar departamento con ID {Id}", id);
            return StatusCode(500, new { message = "Error interno al eliminar el departamento." });
        }
    }
}
