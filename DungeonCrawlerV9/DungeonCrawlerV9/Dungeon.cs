using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerV9
{
    public class Dungeon
    {
        private readonly GameEvents events;
        public Room[,] Rooms { get; private set; }
        public int PlayerRow { get; private set; }
        public int PlayerCol { get; private set; }
        public int Level { get; private set; }
        public bool ContainsDoorRoom { get; private set; }

        private readonly int rows;
        private readonly int cols;
        private Dictionary<RoomType, int> unclearedCount; // O(1)

        public Dungeon(int level, GameEvents events)
        {
            this.events = events;
            PlayerRow = 0;
            PlayerCol = 0;
            Level = level;
            Rooms = CreateDungoenByLevel();
            rows = Rooms.GetLength(0);
            cols = Rooms.GetLength(1);
            SetRoomPosition();  // test
            //SubscribeToRooms();
            events.RoomCleared += HandleRoomCleared;
            InitializeRoomCounts(); // O(1)
        }

        public bool Move(string direction, Player player)
        {
            direction = direction?.Trim().ToLower();
            int newRow = PlayerRow;
            int newCol = PlayerCol;

            switch (direction)
            {
                case "up":
                case "u":
                    newRow = newRow - 1;
                    break;

                case "down":
                case "d":
                    newRow = PlayerRow + 1;
                    break;

                case "left":
                case "l":
                    newCol = PlayerCol - 1;
                    break;

                case "right":
                case "r":
                    newCol = PlayerCol + 1;
                    break;

                default:
                    Console.WriteLine("Invalid direction. Use up/down/left/right.");
                    return false;
            }

            if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
            {
                Console.WriteLine("You can't move there — that's outside the dungeon.");
                return false;
            }

            Room target = Rooms[newRow, newCol];

            if (IsDoorLocked(target, player))
            {
                Console.WriteLine("The door is locked. You need a key to enter.");
                return false;
            }

            PlayerRow = newRow;
            PlayerCol = newCol;
            return true;
        }

        private void SetRoomPosition()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Rooms[r, c].SetPosition(r, c);
                }
            }
        }

        private bool IsDoorLocked(Room room, Player player)
        {
            // not a door room -> not locked, move on
            if (room.Type != RoomType.Door)
                return false;

            DoorRoom door = (DoorRoom)room;

            // door is unlocked -> ok, move on
            if (!door.IsLocked)
                return false;

            // door is locked AND player has no key -> block
            if (!player.HasKey)
                return true;

            // player has key -> unlock door
            Console.WriteLine("You use a key to unlock the door.");
            door.Unlock();
            player.UseKey();

            return false;
        }

        public void ResetMonstersAndRooms()
        {
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    Rooms[r, c].Reset();

            PlayerRow = 0;
            PlayerCol = 0;

            InitializeRoomCounts(); // recalculate counts (O(1))
        }

        public bool IsAtGoal()
        {
            return PlayerRow == rows - 1 && PlayerCol == cols - 1;
        }

        //public void PrintUserPosition()
        //{
        //    for (int r = 0; r < rows; r++)
        //    {
        //        for (int c = 0; c < cols; c++)
        //        {
        //            Room room = Rooms[r, c];

        //            if (PlayerRow == r && PlayerCol == c)
        //            {
        //                Console.Write($"[{room.Description}]");
        //            }
        //            else
        //            {
        //                Console.Write($"{room.Description}");
        //            }

        //            if (c < cols - 1)
        //            {
        //                Console.Write(", ");
        //            }
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.WriteLine();
        //}

        public string[,] GetMapSnapshot()       //replaced PrintUserPosition()
        {
            var map = new string[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var room = Rooms[r, c];
                    string label = room.Description; // or room.Type.ToString()

                    if (r == PlayerRow && c == PlayerCol)
                        label = $"[{label}]";

                    map[r, c] = label;
                }
            }

            return map;
        }

        private void InitializeRoomCounts() // O(1)
        {
            ContainsDoorRoom = false;       // Didn't use it yet, might replace and delete 

            unclearedCount = new Dictionary<RoomType, int>();
            foreach (RoomType type in Enum.GetValues(typeof(RoomType)))
            {
                unclearedCount[type] = 0;
            }

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Room room = Rooms[r, c];
                    unclearedCount[room.Type]++;

                    if (room.Type == RoomType.Door)
                        ContainsDoorRoom = true;
                }
            }
        }

        //public void PrintUnclearedRooms_O1() // O(1)
        //{
        //    Console.WriteLine("Uncleared rooms (O(1) version):");
        //    foreach (var kvp in unclearedCount)
        //    {
        //        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        //    }
        //}

        public IReadOnlyDictionary<RoomType, int> GetUnclearedCounts()      //replaced PrintUnclearedRooms_O1()
        {
            return unclearedCount;
        }


        //private void SubscribeToRooms()
        //{
        //    for (int r = 0; r < rows; r++)
        //    {
        //        for (int c = 0; c < cols; c++)
        //        {
        //            Room room = Rooms[r, c];
        //            //room.OnCleared += HandleRoomCleared;
        //            //events.RoomCleared += HandleRoomCleared;
        //        }
        //    }
        //}

        //private void HandleRoomCleared(Room room) // O(1)
        //{
        //    if (!room.IsCleared) return; // safety check

        //    unclearedCount[room.Type]--;
        //}

        private void HandleRoomCleared(RoomClearedEvent e)
        {
            unclearedCount[e.Type]--;
        }

        public Room[,] CreateDungoenByLevel()
        {
            if (Level == 1)
            {
                return CreateDungeonLevel1(events);
            }
            if (Level == 2)
            {
                return CreateDungeonLevel2(events);
            }
            return null;
        }

        public static Room[,] CreateDungeonLevel1(GameEvents events)
        {
            Room[,] rooms = new Room[3, 3];

            rooms[0, 0] = new MonsterRoom("Entrance hall", new Monster(maxHp: 5, power: 1, exp: 5, events), events); //10 3
            rooms[0, 1] = new MonsterRoom("Mossy corridor", new Monster(maxHp: 7, power: 2, exp: 6, events), events); //12 4
            rooms[0, 2] = new TreasureRoom("Throne of the fallen king", expReward: 12, shieldReward: 2, events);

            rooms[1, 0] = new MonsterRoom("Echoing chamber", new Monster(maxHp: 11, power: 4, exp: 11, events), events); //14 4
            rooms[1, 1] = new TrainingRoom("Abandoned dojo", poweBoost: 3, events);
            rooms[1, 2] = new MonsterRoom("Spider loft", new ShieldMonster(maxHp: 16, power: 5, exp: 20, shield: 2, events), events); //20 6

            rooms[2, 0] = new MonsterRoom("Underground stream", new Monster(maxHp: 18, power: 7, exp: 23, events), events); //22 7
            rooms[2, 1] = new MonsterRoom("Collapsed library", new RageMonster(maxHp: 22, power: 8, exp: 25, rageBoost: 2, events), events); //24 8
            rooms[2, 2] = new MonsterRoom("Boss's lair", new EliteMonster(maxHp: 25, power: 9, exp: 35, extraLives: 1, events), events); // 30 9, goal room

            return rooms;
        }

        public static Room[,] CreateDungeonLevel2(GameEvents events)
        {
            Room[,] rooms = new Room[3, 4];

            rooms[0, 0] = new MonsterRoom("Entrance hall", new Monster(maxHp: 5, power: 1, exp: 5, events), events); //10 3
            rooms[0, 1] = new MonsterRoom("Mossy corridor", new Monster(maxHp: 12, power: 2, exp: 8, events), events); //12 4
            rooms[0, 2] = new MonsterRoom("The Fallen Gravyard", new RageMonster(maxHp: 16, power: 5, exp: 15, rageBoost: 2, events), events);
            rooms[0, 3] = new TreasureRoom("The Throne", expReward: 18, shieldReward: 2, events);

            rooms[1, 0] = new MonsterRoom("Echoing chamber", new Monster(maxHp: 14, power: 4, exp: 15, events), events);
            rooms[1, 1] = new MonsterRoom("Abandoned armory", new ShieldMonster(maxHp: 18, power: 6, exp: 20, shield: 2, events), events);
            //rooms[1, 2] = new MonsterRoom("Spider loft", new RageMonster(maxHp: 20, power: 6, exp: 25, rageBoost: 2)); 
            rooms[1, 2] = new DoorRoom("Spider loft", events);
            rooms[1, 3] = new TrainingRoom("Abandoned dojo", poweBoost: 5, events);

            rooms[2, 0] = new MonsterRoom("Underground stream", new ShieldMonster(maxHp: 22, power: 7, exp: 28, shield: 2, events), events);
            rooms[2, 1] = new TreasureRoom("Glittering Courtyard", expReward: 22, shieldReward: 3, events);
            rooms[2, 2] = new MonsterRoom("Collapsed library", new RageMonster(maxHp: 24, power: 8, exp: 32, rageBoost: 2, events), events);
            rooms[2, 3] = new MonsterRoom("Boss's lair", new EliteMonster(maxHp: 30, power: 9, exp: 40, extraLives: 2, events), events);

            return rooms;
        }


        /// 
        /// Homework- not in use:
        //public void PrintUnclearedRooms_ON() // O(n)
        //{
        //    int trainingCount = 0;
        //    int treasureCount = 0;
        //    int monsterCount = 0;

        //    for (int r = 0; r < rows; r++)
        //    {
        //        for (int c = 0; c < cols; c++)
        //        {
        //            Room room = Rooms[r, c];

        //            if (!room.IsCleared)
        //            {
        //                switch (room.Type)
        //                {
        //                    case RoomType.Training:
        //                        trainingCount++;
        //                        break;
        //                    case RoomType.Treasure:
        //                        treasureCount++;
        //                        break;
        //                    case RoomType.Monster:
        //                        monsterCount++;
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    Console.WriteLine("Uncleared rooms:");
        //    Console.WriteLine($"Training: {trainingCount}");
        //    Console.WriteLine($"Treasure: {treasureCount}");
        //    Console.WriteLine($"Monster: {monsterCount}");
        //}
        ///
        ///
    }
}