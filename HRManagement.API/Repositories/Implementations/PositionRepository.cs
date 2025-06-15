using HRManagement.API.Data;
using HRManagement.API.Models;
using HRManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repositories.Implementations;

public class PositionRepository : IPositionRepository
{
    private readonly AppDbContext _context;

    public PositionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Position>> GetAllAsync()
        => await _context.Positions.ToListAsync();

    public async Task<Position?> GetByIdAsync(int id)
        => await _context.Positions.FindAsync(id);

    public async Task AddAsync(Position position)
    {
        _context.Positions.Add(position);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Position position)
    {
        _context.Entry(position).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var p = await _context.Positions.FindAsync(id);
        if (p != null)
        {
            _context.Positions.Remove(p);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByTittlelAsync(string tittle)
    {
        return await _context.Positions.AnyAsync(e => e.Title == tittle);
    }


    public async Task<bool> ExistsAsync(int id)
        => await _context.Positions.AnyAsync(p => p.Id == id);
}
