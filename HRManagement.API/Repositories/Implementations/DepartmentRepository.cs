using HRManagement.API.Data;
using HRManagement.API.Models;
using HRManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.API.Repositories.Implementations;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
        => await _context.Departments.ToListAsync();

    public async Task<Department?> GetByIdAsync(int id)
        => await _context.Departments.FindAsync(id);

    public async Task AddAsync(Department department)
    {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        _context.Entry(department).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var d = await _context.Departments.FindAsync(id);
        if (d != null)
        {
            _context.Departments.Remove(d);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByNamelAsync(string name)
    {
        return await _context.Departments.AnyAsync(e => e.Name == name);
    }


    public async Task<bool> ExistsAsync(int id)
        => await _context.Departments.AnyAsync(d => d.Id == id);



    

}
