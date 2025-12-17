using System;

namespace DungeonCrawlerV9
{
    public abstract class Room
    {
        protected readonly GameEvents events;
        //public event Action<Room> OnCleared;
        public string Description { get; }
        //public string Description { get; protected set; }
        public bool IsCleared { get; protected set; } = false;
        //public RoomType Type { get; }
        public abstract RoomType Type { get; }
        public int Row { get; private set; }
        public int Col { get; private set; }

        protected Room(string description, GameEvents events)
        {
            Description = description;
            this.events = events;
        }

        //protected Room(string description, RoomType type, GameEvents events)
        //{
        //    Description = description;
        //    Type = type;
        //    IsCleared = false;
        //    this.events = events;
        //}
        public void SetPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        //public abstract bool EnterRoom(Player player, Dungeon dungeon);
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
                //OnCleared?.Invoke(this);
            }
        }
    }
}