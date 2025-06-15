using HRManagement.API.Models;

namespace HRManagement.API.Repositories.Interfaces;

public interface IPositionRepository
{
    Task<IEnumerable<Position>> GetAllAsync();
    Task<Position?> GetByIdAsync(int id);
    Task AddAsync(Position position);
    Task UpdateAsync(Position position);
    Task DeleteAsync(int id);
    Task<bool> ExistsByTittlelAsync(string tittle);

    Task<bool> ExistsAsync(int id);
}
