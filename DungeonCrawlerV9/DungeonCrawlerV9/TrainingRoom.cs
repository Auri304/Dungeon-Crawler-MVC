namespace DungeonCrawlerV9
{
    internal class TrainingRoom : Room
    {
        public int PowerBoost { get; private set; }

        public override RoomType Type => RoomType.Training;

        public TrainingRoom(string description, int poweBoost, GameEvents events) : base(description, events)
        {
            PowerBoost = poweBoost;
        }

        public override bool EnterRoom(Player player)
        {
            RaiseRoomEntered();

            if (IsCleared)
            {
                events.Raise(new RoomClearedEvent(Description, Type, Row, Col));
                return true;
            }

            events.Raise(new GameMessage($"You train hard and increase your power by {PowerBoost}!"));
            player.IncreasePower(PowerBoost);

            MarkCleared();
            return true;
        }
    }
}