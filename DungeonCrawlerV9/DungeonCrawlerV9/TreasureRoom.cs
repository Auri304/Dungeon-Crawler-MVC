namespace DungeonCrawlerV9
{
    internal class TreasureRoom : Room
    {
        public int ExpReward { get; private set; }
        public int ShieldReward { get; private set; }

        public override RoomType Type => RoomType.Treasure;

        private Random rng = new Random();

        public TreasureRoom(string description, int expReward, int shieldReward, GameEvents events) : base(description, events)
        {
            ExpReward = expReward;
            ShieldReward = shieldReward;
        }

        public override bool EnterRoom(Player player)
        {
            //Console.WriteLine($"\n-- Room: {Description} --");
            RaiseRoomEntered();


            if (IsCleared)
            {
                //Console.WriteLine("The treasure chest is already empty...");
                return true;
            }

            bool giveExp = rng.Next(0, 2) == 0;

            if (giveExp)
            {
                //Console.WriteLine($"You found magical scrolls! You gain {ExpReward} EXP!");
                events.Raise(new GameMessage($"You found magical scrolls! You gain {ExpReward} EXP!"));
                player.GainExp(ExpReward);
                player.CheckLevelUpCondition();
            }
            else
            {
                //Console.WriteLine($"You found a shield! Your defense increases by {ShieldReward}.");
                events.Raise(new GameMessage($"You found a shield! Your defense increases by {ShieldReward}."));
                player.AddShield(ShieldReward);
            }

            MarkCleared();
            return true;
        }
    }
}