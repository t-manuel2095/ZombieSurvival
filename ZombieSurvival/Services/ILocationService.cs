using ZombieSurvival.Models;

namespace ZombieSurvival.Services
{
    public interface ILocationService
    {
        Task<IReadOnlyList<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(int id);
        Task<Location> CreateAsync(Location location);
        Task<bool> UpdateAsync(int id, Location location);
        Task<bool> DeleteAsync(int id);
    }
}
