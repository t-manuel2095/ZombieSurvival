using ZombieSurvival.DTOs;
using ZombieSurvival.Models;

namespace ZombieSurvival.Services
{
    public interface IGunShopService
    {
        Task<List<GunShopItemResponse>?> GetCatalogAsync(int playerId);
        Task<BuyGunResponse?> BuyGunAsync(int playerId, int gunId);
        Task<SellItemResponse?> SellItemAsync(int playerId, int itemId, int quantity);
    }
}
