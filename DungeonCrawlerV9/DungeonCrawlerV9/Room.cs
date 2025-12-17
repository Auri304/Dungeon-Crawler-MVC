using System;

namespace DungeonCrawlerV9
{
    public abstract class Room
    {
        protected readonly GameEvents events;
        public string Description { get; }
        public bool IsCleared { get; protected set; } = false;
        public abstract RoomType Type { get; }
        public int Row { get; private set; }
        public int Col { get; private set; }

        protected Room(string description, GameEvents events)
        {
            Description = description;
            this.events = events;
        }

        public void SetPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public abstract bool EnterRoom(Player player);
        protected void RaiseRoomEntered() 
            => events.Raise(new RoomEnteredEvent(Description,Type, Row, Col));

        public virtual void Reset()
        {
            IsCleared = false;
        }

        protected void MarkCleared()
        {
            if (!IsCleared)
            {
                IsCleared = true;
                events.Raise(new RoomClearedEvent(Description, Type, Row, Col));
            }
        }
    }
}