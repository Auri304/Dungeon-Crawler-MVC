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
            RaiseRoomEntered();


            if (IsCleared)
            {
                return true;
            }

            bool giveExp = rng.Next(0, 2) == 0;

            if (giveExp)
            {
                events.Raise(new GameMessage($"You found magical scrolls! You gain {ExpReward} EXP!"));
                player.GainExp(ExpReward);
                player.CheckLevelUpCondition();
            }
            else
            {
                events.Raise(new GameMessage($"You found a shield! Your defense increases by {ShieldReward}."));
                player.AddShield(ShieldReward);
            }

            MarkCleared();
            return true;
        }
    }
}