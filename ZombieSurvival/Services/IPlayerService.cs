using ZombieSurvival.DTOs;
using ZombieSurvival.Models;

namespace ZombieSurvival.Services
{
    public interface IPlayerService
    {
        Task<IReadOnlyList<Player>> GetAllAsync();
        Task<Player?> GetByIdAsync(int id);
        Task<Player?> CreateAsync(CreatePlayerRequest request);
        Task<bool> UpdateAsync(int id, Player player);
        Task<bool> DeleteAsync(int id);
        Task<IReadOnlyList<InventoryItemResponse>?> GetInventoryAsync(int playerId);
    }
}
