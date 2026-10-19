using ZombieSurvival.DTOs;

namespace ZombieSurvival.Services
{
    public interface ICampServices
    {
        Task<UseItemRespone?> UseItemAsync(int playerId, int itemId, int qty);
        Task<bool> EndMorningAsync(int playerId);
    }
}
