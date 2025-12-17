using System;

namespace DungeonCrawlerV9
{
    public class DoorRoom : Room
    {
        public bool IsLocked { get; private set; } = true;

        public override RoomType Type => RoomType.Door;

        public DoorRoom(string description, GameEvents events) : base(description, events)
        {
        }

        public void Unlock()
        {
            IsLocked = false;
        }

        public override bool EnterRoom(Player player)
        {
            if (IsLocked)
            {
                events.Raise(new GameMessage("This door is locked. Find a key!"));
                return false;
            }

            RaiseRoomEntered();
            events.Raise(new GameMessage("You pass through the open door."));
            MarkCleared();
            return true;
        }
    }
}