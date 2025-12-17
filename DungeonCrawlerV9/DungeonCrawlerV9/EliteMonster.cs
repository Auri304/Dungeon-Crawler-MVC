namespace DungeonCrawlerV9
{
    public class EliteMonster : Monster
    {
        private int ExtraLives;   // number of resurrections left
        private int MaxLives;

        public EliteMonster(int maxHp, int power, int exp, int extraLives, GameEvents events)
            : base(maxHp, power, exp, events)
        {
            ExtraLives = extraLives;
            MaxLives = extraLives;
        }

        public override bool IsDead()
        {
            if (base.IsDead() && ExtraLives > 0)
            {
                ExtraLives--;
                base.Reset();
                events.Raise(new GameMessage($"The Elite Monster revives! Lives remaining: {ExtraLives}"));
                return false;
            }
            return base.IsDead();
        }

        public override void Reset()
        {
            base.Reset();
            ExtraLives = MaxLives;
        }
    }
}