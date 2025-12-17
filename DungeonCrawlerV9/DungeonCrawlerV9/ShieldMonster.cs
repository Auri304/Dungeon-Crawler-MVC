namespace DungeonCrawlerV9
{
    internal class ShieldMonster : Monster
    {
        private int MaxShield;
        private int CurrentShield;

        public ShieldMonster(int maxHp, int power, int exp, int shield, GameEvents events)
            : base(maxHp, power, exp, events)
        {
            MaxShield = shield;
            CurrentShield = shield;
        }

        public override void TakeDamage(int damage)
        {
            //if (IsDead())         //Do I need this? I have it in base
            //{
            //    //Console.WriteLine("The shielded monster is already dead!");
            //    return;
            //}

            if (CurrentShield > 0)
            {
                CurrentShield--;
                events.Raise(new GameMessage($"Shield absorbed damage! Remaining shield: {CurrentShield}"));
                return;
            }

            base.TakeDamage(damage);
        }

        public override void Reset()
        {
            base.Reset();
            CurrentShield = MaxShield;
        }
    }
}