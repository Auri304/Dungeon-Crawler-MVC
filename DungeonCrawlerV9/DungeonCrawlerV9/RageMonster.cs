using static System.Net.Mime.MediaTypeNames;

namespace DungeonCrawlerV9
{
    internal class RageMonster : Monster
    {
        private int RageBoost;
        private int BasePower;

        public RageMonster(int maxHp, int power, int exp, int rageBoost, GameEvents events)
            : base(maxHp, power, exp, events)
        {
            RageBoost = rageBoost;
            BasePower = power;
        }

        public override void Attack(Player player)
        {
            base.Attack(player);
            Power += RageBoost;
            events.Raise(new GameMessage($"The Rage Monster grows angrier! Power is now {Power}."));
        }
        public override void Reset()
        {
            base.Reset();
            Power = BasePower;
        }
    }
}