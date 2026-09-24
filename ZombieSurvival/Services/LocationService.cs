using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Data;
using ZombieSurvival.Models;

namespace ZombieSurvival.Services
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Location>> GetAllAsync()
        {
            return await _context.Locations
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Location?> GetByIdAsync(int id)
        {
            return await _context.Locations
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Location> CreateAsync(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<bool> UpdateAsync(int id, Location location)
        {
            if (id != location.Id)
            {
                return false;
            }

            var exists = await _context.Locations.AnyAsync(l => l.Id == id);
            if (!exists)
            {
                return false;
            }

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Locations.AnyAsync(l => l.Id == id))
                {
                    return false;
                }

                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location is null)
            {
                return false;
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
