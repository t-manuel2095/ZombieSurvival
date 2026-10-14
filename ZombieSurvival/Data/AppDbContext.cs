using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Models;


namespace ZombieSurvival.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Player> Players => Set<Player>();
        public DbSet<Location> Locations => Set<Location>(); 
        public DbSet<City> Cities => Set<City>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
        public DbSet<Gun> Guns => Set<Gun>();
    }
}