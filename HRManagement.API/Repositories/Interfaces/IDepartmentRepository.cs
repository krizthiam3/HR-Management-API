using HRManagement.API.Models;

namespace HRManagement.API.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<IEnumerable<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
    Task AddAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);

    Task<bool> ExistsByNamelAsync(string name);

}
