using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Data;
using ZombieSurvival.DTOs;
using ZombieSurvival.Models;

namespace ZombieSurvival.Services
{
    public class CampServices : ICampServices
    {
        private readonly ApplicationDbContext _context;
        public CampServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> EndMorningAsync(int playerId)
        {
            //Getting player and making sure they exist
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player is null)
                return false;

            if (player.DayPhase != "Morning")
                throw new InvalidOperationException("Morning is already over.");

            player.DayPhase = "Evening";
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UseItemRespone?> UseItemAsync(int playerId, int itemId, int qty)
        {
            if (qty <= 0)
                throw new InvalidOperationException("Quantity must be at least 1.");

            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player is null)
                return null;

            if (player.DayPhase != "Morning")
                throw new InvalidOperationException("I can't do anything with this right now.");

            //Get current stack
            var stack = await _context.InventoryItems
                .FirstOrDefaultAsync(i => i.PlayerId == playerId && i.ItemId == itemId);

            if (stack is null || stack.Quantity < qty)
                return null;

            var item = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item is null)
                return null;

            //Number subject to change, Hunger and Stamina capped at 100
            switch (item.ItemType)
            {
                case "Food":
                    player.Hunger = Math.Min(100, player.Hunger + 20 * qty);
                    break;

                case "Water":
                    player.Thirst = Math.Min(100, player.Thirst + 20 * qty);
                    break;

                case "Car Part":
                    player.CarCondition = Math.Min(100, player.CarCondition + 15 * qty);
                    break;

                default:
                    throw new InvalidOperationException("Can't use that in the morning.");
            }

            stack.Quantity -= qty;
                
            //Removing item from stack
            if (stack.Quantity == 0)
                _context.InventoryItems.Remove(stack);

            await _context.SaveChangesAsync();

            return new UseItemRespone
            {
                ItemName = item.Name,
                Hunger = player.Hunger,
                Thirst = player.Thirst,
                Stamina = player.Stamina,
                CarCondition = player.CarCondition,
                Message = $"{player.Name} used {qty}x {item.Name}."
            };
        }
    }
}
