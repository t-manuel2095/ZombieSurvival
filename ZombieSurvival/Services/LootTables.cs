namespace ZombieSurvival.Services
{
    // Weighted loot rolls by area type. Because weights are fixed, DI doesn't seem neccessary. 
    public static class LootTables
    {
        private static readonly Dictionary<string, List<(string? ItemType, int Weight)>> Tables =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Grocery Store"] =
                [
                    ("Food", 40),
                    ("Water", 35),
                    (null, 25) // Nothing
                ],
                ["Gas station"] =
                [
                    ("Fuel", 45),
                    ("Car Part", 30),
                    ("Food", 5),
                    ("Water", 5),
                    (null, 15)
                ],
                // Gun + ammo areas: until a Gun model exists, both rolls grant ItemType "Ammo"
                ["Gun Store"] =
                [
                    ("Ammo", 70), // high chance of guns/ammo (Ammo stands in for guns for now)
                    ("Food", 8),
                    ("Water", 7),
                    (null, 15)
                ],
                ["Church"] =
                [
                    ("Food", 30),
                    ("Water", 25),
                    ("Ammo", 25), // can find guns or ammo; Ammo placeholder until Gun model
                    (null, 20)
                ],
                ["House"] =
                [
                    ("Food", 28),
                    ("Water", 22),
                    ("Ammo", 20), // can find guns or ammo; Ammo placeholder until Gun model
                    ("Fuel", 5),
                    ("Car Part", 10),
                    (null, 15)
                ]
            };
        // Returns ItemType to grant, or null if the roll is "Nothing"
        public static string? Roll(string locationType)
        {
            if (!Tables.TryGetValue(locationType, out var entries) || entries.Count == 0)
                return null;

            var total = entries.Sum(e => e.Weight);
            var roll = Random.Shared.Next(1, total + 1);
            var running = 0;

            foreach (var (itemType, weight) in entries)
            {
                running += weight;

                if (roll <= running)
                    return itemType;
            }

            return null;
        }
    }
}