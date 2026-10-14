using ZombieSurvival.DTOs;

namespace ZombieSurvival.Services
{
    public interface ITravelService
    {
        Task<TravelResponse?> TravelAsync(TravelRequest request);
    }
}
