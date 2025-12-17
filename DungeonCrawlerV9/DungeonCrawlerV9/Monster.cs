using static System.Net.Mime.MediaTypeNames;

namespace DungeonCrawlerV9
{
    public class Monster
    {
        protected readonly GameEvents events;

        public event Action<Monster> OnDeath;
        public int MaxHp { get; protected set; }
        public int CurrentHp { get; protected set; }
        public int Power { get; protected set; }
        public int Exp { get; protected set; }

        public Monster(int maxHp, int power, int exp, GameEvents events)
        {
            this.events = events;
            MaxHp = maxHp;
            CurrentHp = maxHp;
            Power = power;
            Exp = exp;
        }

        //public Monster(int maxHp, int power, int exp)
        //{
        //    MaxHp = maxHp;
        //    Power = power;
        //    Exp = exp;
        //}

        public virtual void TakeDamage(int damage)
        {
            CurrentHp = Math.Max(CurrentHp - damage, 0);
            //Console.WriteLine($"Monster recieved {damage} damage! Monster's HP: {CurrentHp}/{MaxHp}");
            events.Raise(new DamageEvent("Player", "Monster", damage, CurrentHp, MaxHp));

            if (IsDead())
            {
                events.Raise(new MonsterDiedEvent(this));
                OnDeath?.Invoke(this);
                return;
            }
        }

        public virtual void Attack(Player player)
        {
            player.TakeDamage(Power);
        }

        public virtual void Reset()
        {
            CurrentHp = MaxHp;
        }

        public virtual bool IsDead() => CurrentHp <= 0;
    }
}