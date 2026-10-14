using ZombieSurvival.Models;

namespace ZombieSurvival.Services
{
    public interface ILootService
    {
        Task<Item?> RollItemForAreaAsync(string locationType);
        Task AddToInventoryAsync(int playerId, int itemId, int quantity = 1);
    }
}
