using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerV9
{
    public class ConsoleView : IGameView
    {
        public void ShowIntro()
        {
            Console.WriteLine("=== Dungeon Crawler Game ===\n");
            Console.WriteLine("Starting your very first encounter.....\n");
        }

        public int AskDifficulty()
        {
            while (true)
            {
                Console.Write("\nWhich dungeon difficuly you want? available levels (1 / 2): ");
                var input = Console.ReadLine()?.Trim().ToLower();

                if (input == "1" || input == "one") return 1;
                if (input == "2" || input == "two") return 2;

                Console.WriteLine("Invalid level.");
            }
        }

        public void ShowStatus(Player player, Dungeon dungeon)
        {
            Console.WriteLine($"You are now in room: ({dungeon.PlayerRow}, {dungeon.PlayerCol})\n");
            Console.WriteLine("Dungeon Map:");
            ShowDungeonMap(dungeon.GetMapSnapshot());
            ShowUnclearedRooms(dungeon.GetUnclearedCounts());
        }

        //public string AskMove()       // replaced with two functions below, seemed to be more strict mvc (not sure if it's the best solution) (Aviv?)
        //{
        //    Console.Write("\nWhere would you like to move? (up / down / left / right): ");
        //    return Console.ReadLine()?.Trim().ToLower();
        //}
        public void ShowMovePrompt()
        {
            Console.Write("\nWhere would you like to move? (up / down / left / right): ");      // .Write = continue on the same line
        }

        public void ShowInvalidMove()
        {
            Console.WriteLine("Invalid direction or blocked path.");
        }


        public void ShowDungeonMap(string[,] map)
        {
            for (int r = 0; r < map.GetLength(0); r++)
            {
                for (int c = 0; c < map.GetLength(1); c++)
                {
                    Console.Write(map[r, c]);
                    if (c < map.GetLength(1) - 1) Console.Write(", ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void ShowUnclearedRooms(IReadOnlyDictionary<RoomType, int> counts)
        {
            Console.WriteLine("Uncleared rooms:");
            foreach (var kvp in counts)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }

        public void ShowWin() => Console.WriteLine("\n=== Congratulations! You reached the goal (bottom-right). You win! ===");
        public void ShowLose() => Console.WriteLine("\n=== You have reached 10 deaths. You lose. ===");

        public void ShowRoomEntered(RoomEnteredEvent e) => Console.WriteLine($"\n-- Room: {e.Description} -- at ({e.Row},{e.Col}");
        public void ShowRoomCleared(RoomClearedEvent e) => Console.WriteLine($"[CLEARED] {e.Description} ({e.Type})");
        public void ShowDamage(DamageEvent e) => Console.WriteLine($"{e.Attacker} hits {e.Target} for {e.Amount}. {e.Target} HP: {e.TargetHpAfter}/{e.TargetMaxHp}");
        public void ShowMonsterDied(MonsterDiedEvent e) => Console.WriteLine("The monster has died!");
        public void ShowPlayerDied(PlayerDiedEvent e) => Console.WriteLine($"\n*** You died. Respawning... ({e.Deaths} / 10 deaths) ***\n");
        public void ShowLoot(LootGrantedEvent e) => Console.WriteLine($"[Loot] You received: {e.Loot.GetType().Name}");
        public void ShowLog(GameLogEvent e) => Console.WriteLine(e.Message);
        public void ShowMessage(GameMessage e) => Console.WriteLine(e.Text);
    }
}
