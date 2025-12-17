namespace DungeonCrawlerV9
{
    public class LootSystem
    {
        //public event Action<Loot> OnLootGenerated;    // replace with gameevents

        private LootSystem() { }
        private static LootSystem _instance;
        public static LootSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LootSystem();
                }
                return _instance;
            }
        }

        public int CurrentLevel { get; set; } = 1;
        public bool CurrentDungeonHasDoor { get; set; } = false;

        private Random rng = new Random();
        private GameEvents events;

        /// <summary>
        /// Initialize: Before, LootSystem didn’t need GameEvents. Now it does — and since it’s a singleton,
        /// it won’t magically receive events unless you explicitly set it. >:-( arg >:-( !
        /// Called in GameController.
        /// </summary>
        public void Initialize(GameEvents events)
        {
            this.events = events;
        }

        public void LootDeadEnemy(Monster enemy)
        {
            //Console.WriteLine("[DEBUG] LootSystem.EnemyDied CALLED!");
            if (rng.NextDouble() > 0.9) return; // 50% drop chance => 90% for debugging

            Loot loot = GenerateLoot();
            //Console.WriteLine("[DEBUG] Loot generated: " + (loot?.GetType().Name ?? "NULL"));

            if (loot == null) return;       // Safety - remove? 
            if (events == null)             // Safety - remove? 
                throw new InvalidOperationException("LootSystem not initialized. Call LootSystem.Instance.Initialize(events) in GameController.");

            events.Raise(new LootGrantedEvent(loot));
            //OnLootGenerated?.Invoke(loot);
        }

        private Loot GenerateLoot()
        {
            int roll = rng.Next(5);

            switch (roll)
            {
                case 0: return new HealLoot();
                case 1: return new ShieldLoot(1);
                case 2: return new PowerLoot(2);
                case 3: return new MaxHpLoot(5);
                case 4:
                    if (CurrentLevel >= 2 && CurrentDungeonHasDoor)
                    {
                        return new KeyLoot();
                    }
                    return GenerateLoot(); //Or return null;?

                default: return null;
            }
        }
    }
}