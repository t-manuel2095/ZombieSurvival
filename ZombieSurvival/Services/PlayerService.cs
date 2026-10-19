using ZombieSurvival.Models;
using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Data;
using ZombieSurvival.DTOs;

namespace ZombieSurvival.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ApplicationDbContext _context;

        public PlayerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Player?> CreateAsync(CreatePlayerRequest request)
        {
            var startingLocation = await _context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Name == "Los Angeles - House");

            if (startingLocation is null)
                return null;

            var player = new Player
            {
                Name = request.Name,
                CurrentHealth = 100,
                MaxHealth = 100,
                Hunger = 100,
                Stamina = 100,
                Thirst = 100,
                locationId = startingLocation.Id,
                CityId = startingLocation.CityId,
                DayPhase = "Morning",
                CreatedAt = DateTime.UtcNow,
                LastActiveAt = DateTime.UtcNow
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            // Starter sidearm
            var glock = await _context.Guns
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name == "Glock");

            if (glock is not null)
            {
                _context.InventoryItems.Add(new InventoryItem
                {
                    PlayerId = player.Id,
                    ItemId = glock.Id,
                    Quantity = 1
                });

                await _context.SaveChangesAsync();
            }

            return player;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if(player is null)
            {
                return false;
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IReadOnlyList<Player>> GetAllAsync()
        {
            return await _context.Players
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Player?> GetByIdAsync(int id)
        {
            return await _context.Players
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> UpdateAsync(int id, Player player)
        {
            if(id != player.Id)
            {
                return false;
            }

            var exists = await _context.Players.AnyAsync(p => p.Id == id);
            if (!exists)
            {
                return false;
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }

            catch (DbUpdateConcurrencyException) {
                if (!await _context.Players.AnyAsync(p => p.Id == id))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<IReadOnlyList<InventoryItemResponse>?> GetInventoryAsync(int playerId)
        {
            //Getting player
            var exists = await _context.Players.AnyAsync(p => p.Id == playerId);

            //Checking if player exist
            if (!exists)
                return null;

            //Selecting the items in the current players inventory and returning
            return await _context.InventoryItems
                .AsNoTracking()
                .Where(i => i.PlayerId == playerId)
                .Select(i => new InventoryItemResponse
                {
                    ItemId = i.ItemId,
                    Name = i.Item!.Name,
                    ItemType = i.Item.ItemType,
                    Quantity = i.Quantity
                }).ToListAsync();
        }
    }
}