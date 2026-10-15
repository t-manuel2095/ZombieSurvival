using Microsoft.EntityFrameworkCore;
using ZombieSurvival.Models;

namespace ZombieSurvival.Data
{
    public static class GameSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {   //Setting up the cities of the game
            if (!await context.Cities.AnyAsync())
            {
                var cities = new List<City>()
                {
                    new City { Name = "Los Angeles", Description = "Starting city on the west coast.", RouteOrder = 1, IsWinCity = false},
                    new City { Name = "Phoenix", Description = "Hot desert stop on the way to home.", RouteOrder = 2, IsWinCity = false},
                    new City { Name = "Dallas", Description = "The land of zombie cowboys.", RouteOrder = 3, IsWinCity = false},
                    new City { Name = "Chicago", Description = "Zombies of the Midway.", RouteOrder = 4, IsWinCity = false},
                    new City { Name = "New York", Description = "End of the road. Zombie mecca.", RouteOrder = 5, IsWinCity = true},
                };

                context.Cities.AddRange(cities);
                await context.SaveChangesAsync();

                //Setting up the different areas in each city. Vanilla for now.
                var areaTypes = new[] { "Gas station", "Grocery Store", "Church", "House", "Gun Store", "Jewelry Store" };

                //Putting each area type each of our cities. Still vanilla for now to get the game running. 
                foreach (var city in cities)
                {
                    foreach (var type in areaTypes)
                    {
                        var label = type switch
                        {
                            "GasStation" => "Gas Station",
                            "GroceryStore" => "Grocery Store",
                            "Church" => "Church",
                            "House" => "House",
                            "Gun Store" => "Gun Store",
                            "Jewelry Store" => "Jewelry Store",
                            _ => type
                        };

                        context.Locations.Add(new Location
                        {
                            Name = $"{city.Name} - {label}",
                            Description = $"{label} in {city.Name}.",
                            LocationType = type,
                            DangerLevel = city.RouteOrder,
                            CityId = city.Id
                        });
                    }
                }

                await context.SaveChangesAsync();
            }

            //Setting up the different items on initial set up. 
            if (!await context.Items.AnyAsync())
            {
                var items = new List<Item>()
                {
                    new Item{Name = "Canned Food", Description = "The maggots are just extra protein.", ItemType = "Food", MaxStackSize = 20, Price = 4},
                    new Item{Name = "Bottled Water", Description = "Fantasizing it's a margarita", ItemType = "Water", MaxStackSize = 20, Price = 5},
                    new Item{Name = "Ammo", Description = "Maybe one day I'll eat one.", ItemType = "Ammo", MaxStackSize = 10, Price = 2},
                    new Item{Name = "Fuel Can", Description = "Have you seen the gas prices lately?", ItemType = "Fuel", MaxStackSize = 10, Price = 1},
                    new Item{Name = "Car parts", Description = "Fixes the ouchies on the car.", ItemType = "Car Part", MaxStackSize = 10, Price = 8},
                    new Item{Name = "Gold Nugget", Description = "I'd rather have a chicken nugget.", ItemType = "Jewelry", MaxStackSize = 100, Price = 10},


                };

                context.Items.AddRange(items);
                await context.SaveChangesAsync();
            }

            if(!await context.Guns.AnyAsync())
            {
                var guns = new List<Gun>
                {
                    new Gun { Name = "Glock", Description = "Trusty sidearm.", ItemType = "Gun", MaxStackSize = 1,
                        Damage = 20, Accuracy = 100, ReloadSpead = 20, CurrentMagCapacity = 12, MaxMagCapacity = 12, Price = 0},
                    new Gun { Name = "Magnum", Description = "Big boom.", ItemType = "Gun", MaxStackSize = 1,
                        Damage = 40, Accuracy = 100, ReloadSpead = 30, CurrentMagCapacity = 6, MaxMagCapacity = 6, Price = 20},
                    new Gun { Name = "Sniper", Description = "Long range.", ItemType = "Gun", MaxStackSize = 1,
                        Damage = 80, Accuracy = 100, ReloadSpead = 60, CurrentMagCapacity = 5, MaxMagCapacity = 5, Price = 40},
                };

                context.Guns.AddRange(guns);
                await context.SaveChangesAsync();
            }
        }
    }
}
