using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Data;
using ZombieSurvival.Models;

namespace ZombieSurvival.Services
{
    public class LootService : ILootService
    {
        private readonly ApplicationDbContext _context;

        public LootService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Calls our LootTable roll to see what item we get back
        public async Task<Item?> RollItemForAreaAsync(int playerId, string locationType)
        {
            var itemType = LootTables.Roll(locationType);

            if (itemType is null)
                return null;

            return await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.ItemType == itemType);

        }
        public async Task AddToInventoryAsync(int playerId, int itemId, int quantity)
        {
            //Get current stack
            var stack = await _context.InventoryItems
                .FirstOrDefaultAsync(i => i.PlayerId == playerId && i.ItemId == itemId);

            //If nothing is there, add it to the stack
            if (stack is null)
            {
                _context.InventoryItems.Add(new InventoryItem
                {
                    PlayerId = playerId,
                    ItemId = itemId,
                    Quantity = quantity
                });
            }
            //Else just increase quantity
            else
                stack.Quantity += quantity;

            await _context.SaveChangesAsync();
        }
    }
}
