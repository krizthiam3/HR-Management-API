using Microsoft.AspNetCore.Mvc;
using HRManagement.API.Repositories.Interfaces;
using HRManagement.API.Models;
using HRManagement.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace HRManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly IPositionRepository _repo;
    private readonly ILogger<PositionsController> _logger;

    public PositionsController(IPositionRepository repo, ILogger<PositionsController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var positions = await _repo.GetAllAsync();

            var result = positions.Select(p => new PositionDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener posiciones.");
            return StatusCode(500, new { message = "Ocurrió un error al obtener las posiciones." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null)
                return NotFound(new { message = "Puesto no encontrado." });

            return Ok(new PositionDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener puesto con ID {Id}", id);
            return StatusCode(500, new { message = "Error interno al consultar el puesto." });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(PositionCreateDto dto)
    {
        try
        {
            var exists = await _repo.ExistsByTittlelAsync(dto.Title);
            if (exists)
                return BadRequest(new { message = $"La Posicion '{dto.Title}' ya está registrado." });

            var p = new Position
            {
                Title = dto.Title.Trim(),
                Description = dto.Description.Trim()
            };

            await _repo.AddAsync(p);
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear puesto.");
            return StatusCode(500, new { message = "Error interno al crear el puesto." });
        }
    }

    [Authorize(Roles = "Admin")]

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PositionUpdateDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID de URL no coincide con el cuerpo." });

            var exists = await _repo.ExistsAsync(id);
            if (!exists)
                return NotFound(new { message = "Puesto no encontrado." });

            var p = new Position
            {
                Id = dto.Id,
                Title = dto.Title.Trim(),
                Description = dto.Description.Trim()
            };

            await _repo.UpdateAsync(p);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar puesto con ID {Id}", id);
            return StatusCode(500, new { message = "Error interno al actualizar el puesto." });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var exists = await _repo.ExistsAsync(id);
            if (!exists)
                return NotFound(new { message = "Puesto no encontrado." });

            await _repo.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar puesto con ID {Id}", id);
            return StatusCode(500, new { message = "Error interno al eliminar el puesto." });
        }
    }
}
