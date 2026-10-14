using ZombieSurvival.DTOs;

namespace ZombieSurvival.Services
{
    public interface IScavengerService
    {
        // Scavenge a chosen area in the player's current city
        Task<ScavengeResponse?> ScavengeAsync(ScavengeRequest request);
    }
}
