using ZombieSurvival.Models;
using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Data;

namespace ZombieSurvival.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly ApplicationDbContext _context;

        public PlayerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Player> CreateAsync(Player player)
        {
            _context.Players.Add(player);

            player.CreatedAt = DateTime.UtcNow;
            player.LastActiveAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
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
    }
}
