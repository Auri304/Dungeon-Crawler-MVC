namespace DungeonCrawlerV9
{
    public class MonsterRoom : Room
    {
        public Monster Monster { get; set; }

        public override RoomType Type => RoomType.Monster;

        public MonsterRoom(string description, Monster monster, GameEvents events) : base(description, events)
        {
            Monster = monster;
            Monster.OnDeath += LootSystem.Instance.LootDeadEnemy;
        }

        public override bool EnterRoom(Player player)
        {
            RaiseRoomEntered();

            if (IsCleared)
            {
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
                    Monster.Attack(player);
                    if (player.IsDead())
                    {
                        didPlayerWin = false;
                    }
                }
            }

            if (!didPlayerWin.Value)
            {
                return false;
            }

            events.Raise(new MonsterDiedEvent(Monster));

            events.Raise(new GameMessage($"Exp received: +{Monster.Exp}"));
            player.GainExp(Monster.Exp);
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