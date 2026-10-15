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

                ["Jewelry Store"] =
                [
                    ("Jewelry", 25),
                    (null, 75)
                ],
                ["Church"] =
                [
                    ("Food", 40),
                    ("Water", 40),
                    (null, 20)
                ],
                ["House"] =
                [
                    ("Food", 38),
                    ("Water", 32),
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