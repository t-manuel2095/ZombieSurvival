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
                var areaTypes = new[] { "Gas station", "Grocery Store", "Church", "House", "Gun Store" };

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
                    new Item{Name = "Canned Food", Description = "The maggots are just extra protein.", ItemType = "Food", MaxStackSize = 20},
                    new Item{Name = "Bottled Water", Description = "Fantasizing it's a margarita", ItemType = "Water", MaxStackSize = 20},
                    new Item{Name = "Ammo", Description = "Maybe one day I'll eat one.", ItemType = "Ammo", MaxStackSize = 10},
                    new Item{Name = "Fuel Can", Description = "Have you seen the gas prices lately?", ItemType = "Fuel", MaxStackSize = 10},
                    new Item{Name = "Car parts", Description = "Fixes the ouchies on the car.", ItemType = "Car Part", MaxStackSize = 10},

                };

                context.Items.AddRange(items);
                await context.SaveChangesAsync();
            }
        }
    }
}
