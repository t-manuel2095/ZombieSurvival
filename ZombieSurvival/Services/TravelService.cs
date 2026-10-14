using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Data;
using ZombieSurvival.DTOs;

namespace ZombieSurvival.Services
{
    public class TravelService : ITravelService
    {
        private readonly ApplicationDbContext _context;

        public TravelService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<TravelResponse?> TravelAsync(TravelRequest request)
        {
            //Find player if they exist
            var player = await _context.Players.FindAsync(request.PlayerId);

            if (player is null)
                return null;

            //Making sure the rquest location exists
            var location = await _context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == request.LocationId);

            if (location is null)
                return null;

            //Trial Stamina loss per location 
            const int travelCost = 1;

            if (player.Stamina < travelCost)
                return null;

            //updated player data
            player.locationId = request.LocationId;
            player.Stamina -= travelCost;
            player.LastActiveAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new TravelResponse
            {
                PlayerID = player.Id,
                LocationID = location.Id,
                LocationName = location.Name,
                Stamina = player.Stamina, 
                Message = $"{player.Name} traveled to - {location.Name}."
            };
        }
    }
}
