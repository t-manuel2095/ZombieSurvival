using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Data;
using ZombieSurvival.DTOs;

namespace ZombieSurvival.Services
{
    public class ScavengerService : IScavengerService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILootService _lootService;

        public ScavengerService(ApplicationDbContext context, ILootService lootService)
        {
            _context = context;
            _lootService = lootService;
        }

        // Player picks an area in their city to scavenge
        public async Task<ScavengeResponse?> ScavengeAsync(ScavengeRequest request)
        {
            //Getting player and making sure they exist and then chicking their location exsits
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.Id == request.PlayerId);

            if (player is null)
                return null;

            if (player.DayPhase != "Evening")
                throw new InvalidOperationException("You can only scavenge in the evening.");

            // Must know which city the player is in
            if (player.CityId is null)
                return null;

            // Area the player chose to loot
            var location = await _context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == request.LocationId);

            if (location is null)
                return null;

            // Can only scavenge areas inside the current city
            if (location.CityId != player.CityId)
                throw new InvalidOperationException("That's a different city.");

            //We already scavenged here for today
            if (player.AreasScavengedToday.Contains(location.Name))
                throw new InvalidOperationException("We already scavenged this area today.");

            if (location.LocationType.Equals("Gun Store"))
                throw new InvalidOperationException("Talk to the merchant.");

            var cost = GetStaminaCost(location.LocationType);

            if (player.Stamina < cost)
                throw new InvalidOperationException("We ran out of stamina for the day.");

            player.Stamina -= cost;
            player.locationId = location.Id;
            player.LastActiveAt = DateTime.UtcNow;
            player.AreasScavengedToday.Add(location.Name); 

            //Get the items from our area, add if we get something
            var item = await _lootService.RollItemForAreaAsync(player.Id, location.LocationType);
            
            if (item is not null)
                await _lootService.AddToInventoryAsync(player.Id, item.Id);

            await _context.SaveChangesAsync();

            return new ScavengeResponse
            {
                Stamina = player.Stamina,
                ItemName = item?.Name,
                LocationName = location.Name,
                Message = item is null
                    ? $"{player.Name} scavenged {location.Name} and found nothing."
                    : $"{player.Name} scavenged {location.Name} and found {item.Name}."
            };
        }

        //Helper function that contains the cost of stamina given the area you want to go to. 
        //High value areas cost more to go to.
        //Number subject to change
        private static int GetStaminaCost(string locationType) => locationType switch
        {
            "House" => 5,
            "Church" => 6,
            "Grocery Store" => 8,
            "Gas station" => 10,
            "Jewelry Store" => 14,
            _ => 8
        };
    }
}
