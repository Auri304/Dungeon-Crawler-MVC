using System;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace DungeonCrawlerV9
{
    public class Player
    {
        private readonly GameEvents events;
        public int Level { get; private set; }
        public int CurrentHp { get; private set; }
        public int Power { get; private set; }
        public int Shield { get; private set; }
        public int Deaths { get; private set; }
        public int MaxExp { get; private set; }

        public bool HasKey => Keys > 0;

        private int Keys = 0;
        private int CurrentExp;
        private int MaxHp;

        public Player(int startingHp, int startingPower, int maxExp, GameEvents events)
        {
            Level = 1;
            MaxHp = startingHp;
            CurrentHp = MaxHp;
            Power = startingPower;
            Shield = 0;
            Deaths = 0;
            MaxExp = maxExp;
            CurrentExp = 0;
            this.events = events;
            events.LootGranted += OnLootGranted;
        }

        public void Dispose()       //Do I need it?
        {
            events.LootGranted -= OnLootGranted;
        }

        private void OnLootGranted(LootGrantedEvent e)
        {
            e.Loot.Apply(this);
        }

        public void Attack(Monster monster)
        {
            monster.TakeDamage(Power);
        }

        public void TakeDamage(int damage)
        {
            if (Shield > 0)
            {
                Shield--;
                //Console.WriteLine($"Shield absorbed damage! Remaining shield: {Shield}"); /// maybe raise GameMessage,minor optional message
                return;
            }

            //CurrentHp -= damage;  //changed 2 lines to Math.Max
            //if (CurrentHp < 0) CurrentHp = 0;  //changed 2 lines to Math.Max
            CurrentHp = Math.Max(CurrentHp - damage, 0);
            events.Raise(new DamageEvent("Monster", "Player", damage, CurrentHp, MaxHp));
        }


        public void HealToFull()
        {
            CurrentHp = MaxHp;
        }

        public void AddMaxHp(int amount)
        {
            CurrentHp += Math.Min(amount, MaxHp);
        }

        public void GainExp(int amount)
        {
            CurrentExp += amount;
            events.Raise(new GameMessage($"Exp Bar: [{ Math.Min(MaxExp, CurrentExp) } /{ MaxExp}]"));   /// minor optional message
        }

        public void CheckLevelUpCondition()
        {
            if (CurrentExp >= MaxExp)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            int gainHp = 5;
            int gainPower = 2;
            Level++;
            MaxHp += gainHp;
            Power += gainPower;
            //CurrentExp = 0;   //can level only once
            int addMaxExp = (Level * 3);
            MaxExp += addMaxExp;
            HealToFull();
            events.Raise(new GameMessage($"*** Level Up! You are now level {Level}.\n" +
                $"New stats — Level: {Level}, HP: +{gainHp} ({CurrentHp}/{MaxHp}), Power: +{gainPower} ({Power})"));
        }

        public void AddShield(int amount)
        {
            Shield += amount;
        }

        public void IncreasePower(int amount)
        {
            Power += amount;
        }

        internal void GainKey()
        {
            Keys++;
        }
        public void UseKey()
        {
            if (Keys > 0)
                Keys--;
            events.Raise(new GameMessage($"Door unlocks... Keys: {Keys}"));   /// minor optional message
        }

        public void DieAndRespawn()
        {
            Deaths++;
            events.Raise(new PlayerDiedEvent(Deaths));
            HealToFull();
        }

        public bool IsDead() => CurrentHp <= 0;
    }
}