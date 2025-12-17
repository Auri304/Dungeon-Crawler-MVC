using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerV9
{
    public class GameController
    {
        private readonly IGameView view;
        private readonly GameEvents events;

        private Player player;
        private Dungeon dungeon;

        private const int MaxDeaths = 10;

        public GameController(IGameView view)
        {
            this.view = view;
            events = new GameEvents();
            
            LootSystem.Instance.Initialize(events);

            // Controller subscribes once and forwards to View
            events.RoomEntered += view.ShowRoomEntered;
            events.RoomCleared += view.ShowRoomCleared;
            events.Damage += view.ShowDamage;
            events.MonsterDied += view.ShowMonsterDied;
            events.PlayerDied += view.ShowPlayerDied;
            events.LootGranted += view.ShowLoot;
            events.Log += view.ShowLog;
            events.Message += view.ShowMessage;
        }

        public void Start()
        {
            view.ShowIntro();

            int level = view.AskDifficulty();

            player = new Player(startingHp: 15, startingPower: 3, maxExp: 0, events: events);
            dungeon = new Dungeon(level, events);

            // Keep existing “key only if door exists” rule:
            LootSystem.Instance.CurrentLevel = level;
            LootSystem.Instance.CurrentDungeonHasDoor = dungeon.ContainsDoorRoom;

            RunLoop();
        }

        private void RunLoop()
        {
            while (true)
            {
                view.ShowStatus(player, dungeon);

                var room = dungeon.Rooms[dungeon.PlayerRow, dungeon.PlayerCol];
                bool alive = room.EnterRoom(player);

                if (!alive)
                {
                    if (player.Deaths >= MaxDeaths) { view.ShowLose(); return; }
                    player.DieAndRespawn();
                    dungeon.ResetMonstersAndRooms();
                    continue;
                }

                if (dungeon.IsAtGoal()) { view.ShowWin(); return; }

                bool moved = false;
                while (!moved)
                {
                    //var dir = view.AskMove();
                    //moved = dungeon.Move(dir, player);
                    /// 2 lines above replaced with lines below, seemed to be more strict mvc (not sure if it's the best solution) Aviv?
                    view.ShowMovePrompt();
                    var dir = Console.ReadLine()?.Trim().ToLower();   // controller reads input (mvc teacher guidline)
                    moved = dungeon.Move(dir, player);

                    if (!moved)
                        view.ShowInvalidMove();
                    /// end here
                }
            }
        }
    }
}
