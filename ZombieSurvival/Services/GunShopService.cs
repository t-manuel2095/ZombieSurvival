using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Data;
using ZombieSurvival.DTOs;
using ZombieSurvival.Models;

namespace ZombieSurvival.Services
{
    public class GunShopService : IGunShopService
    {
        private readonly ApplicationDbContext _context;
        public GunShopService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Full catalog with AlreadyOwned set from the player's inventory
        public async Task<List<GunShopItemResponse>?> GetCatalogAsync(int playerId)
        {
            //Getting player and making sure they exist
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player is null)
                return null;

            //Getting the guns the players already own
            var gunIds = await _context.Guns.Select(g => g.Id).ToListAsync();

            var ownedGuns = await _context.InventoryItems
                .AsNoTracking()
                .Where(i => i.PlayerId == playerId && gunIds.Contains(i.ItemId))
                .Select(i => i.ItemId)
                .ToListAsync();

            //All of the guns
            var allGuns = await _context.Guns
                .AsNoTracking()
                .Where(g => g.Name != "Glock")  // not for sale
                .ToListAsync();

            return allGuns.Select(g => new GunShopItemResponse
            {
                GunId = g.Id,
                GunName = g.Name,
                Price = g.Price,
                Damage = g.Damage,
                Accuracy = g.Accuracy,
                ReloadSpead = g.ReloadSpead,
                CurrentMagCapacity = g.CurrentMagCapacity,
                MaxMagCapacity = g.MaxMagCapacity,
                AlreadyOwned = ownedGuns.Contains(g.Id)
            }).ToList();
        }
        public async Task<BuyGunResponse?> BuyGunAsync(int playerId, int gunId)
        {
            //Getting player
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player is null)
                return null;
            
            //Getting gun
            var gun = await _context.Guns
                .FirstOrDefaultAsync(g => g.Id == gunId);

            if (gun is null)
                throw new InvalidOperationException("That gun isn't for sale.");

            //Making sure if I already own gun
            var alreadyOwned = await _context.InventoryItems
                .AnyAsync(i => i.PlayerId == playerId && i.ItemId == gunId);

            if (alreadyOwned)
                throw new InvalidOperationException("You already own that gun.");

            // Pay from wallet
            if (player.Money < gun.Price)
                throw new InvalidOperationException("Not enough money.");

            player.Money -= gun.Price;

            _context.InventoryItems.Add(new InventoryItem
            {
                PlayerId = playerId,
                ItemId = gun.Id,
                Quantity = 1
            });

            await _context.SaveChangesAsync();

            return new BuyGunResponse
            {
                GunName = gun.Name,
                PricePaid = gun.Price,
                Money = player.Money
            };
        }

        public async Task<SellItemResponse?> SellItemAsync(int playerId, int itemId, int quantity)
        {
            //Have to have some money
            if (quantity <= 0)
                throw new InvalidOperationException("Quantity must be at least 1.");

            //Getting player
            var player = await _context.Players
                .FirstOrDefaultAsync(p => p.Id == playerId);

            if (player is null)
                return null;

            //Making sure we can't sell our guns back
            var isGun = await _context.Guns.AnyAsync(g => g.Id == itemId);

            if (isGun)
                throw new InvalidOperationException("Can't sell guns here.");

            //Checking item
            var item = await _context.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item is null)
                throw new InvalidOperationException("Unknown item.");

            //Checking out stack of something
            var stack = await _context.InventoryItems
                .FirstOrDefaultAsync(i => i.PlayerId == playerId && i.ItemId == itemId);

            if (stack is null || stack.Quantity < quantity)
                throw new InvalidOperationException("Not enough of that item.");

            stack.Quantity -= quantity;

            if (stack.Quantity == 0)
                _context.InventoryItems.Remove(stack);

            var earned = item.Price * quantity;
            player.Money += earned;

            await _context.SaveChangesAsync();

            return new SellItemResponse
            {
                ItemName = item.Name,
                QuantitySold = quantity,
                MoneyEarned = earned,
                Money = player.Money
            };
        }
    }
}
