namespace DungeonCrawlerV9
{
    public class MonsterRoom : Room
    {
        public Monster Monster { get; set; }

        public override RoomType Type => RoomType.Monster;

        public MonsterRoom(string description, Monster monster, GameEvents events) : base(description, events)
        {
            Monster = monster;
            //Console.WriteLine("[DEBUG] Subscribing Monster.OnDeath to LootSystem.EnemyDied");
            Monster.OnDeath += LootSystem.Instance.LootDeadEnemy;
        }

        public override bool EnterRoom(Player player)
        {
            //Console.WriteLine($"\n-- Room: {Description} --");
            RaiseRoomEntered();

            if (IsCleared)
            {
                //Console.WriteLine("It's quiet here... The monster here is 100% dead.");
                events.Raise(new RoomClearedEvent(Description, Type, Row, Col));
                return true;
            }

            bool? didPlayerWin = null;

            while (!didPlayerWin.HasValue)
            {
                player.Attack(Monster);

                if (Monster.IsDead())
                {
                    didPlayerWin = true;
                }
                else
                {
                    //Console.WriteLine("Monster attacks back!");
                    Monster.Attack(player);
                    if (player.IsDead())
                    {
                        didPlayerWin = false;
                    }
                }
            }

            if (!didPlayerWin.Value)
            {
                //Console.WriteLine("You have been defeated by the monster...");
                //events.Raise(new GameMessage("You have been defeated by the monster..."));
                return false;
            }

            //Console.WriteLine("You defeated the monster!");
            events.Raise(new MonsterDiedEvent(Monster));

            //Console.WriteLine($"Exp received: +{Monster.Exp}");
            events.Raise(new GameMessage($"Exp received: +{Monster.Exp}"));
            player.GainExp(Monster.Exp);
            //IsCleared = true;
            //dungeon.OnRoomCleared(this);
            MarkCleared();
            player.CheckLevelUpCondition();

            return true;
        }

        public override void Reset()
        {
            base.Reset();
            Monster.Reset();
        }
    }
}