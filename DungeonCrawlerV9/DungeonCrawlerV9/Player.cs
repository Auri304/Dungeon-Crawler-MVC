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
            //Console.WriteLine("[DEBUG] Player subscribed to OnLootGenerated");
            //LootSystem.Instance.OnLootGenerated += LootReceived;
            this.events = events;
            events.LootGranted += OnLootGranted;
        }

        public void Dispose()       //Do I need it?
        {
            //LootSystem.Instance.OnLootGenerated -= LootReceived;
            events.LootGranted -= OnLootGranted;
        }

        private void OnLootGranted(LootGrantedEvent e)
        {
            //events.Raise(new GameMessage("\nShiney~ Loot found!"));         //minor
            e.Loot.Apply(this);
        }

        //private void LootReceived(Loot loot)
        //{
        //    //Console.WriteLine($"[DEBUG] Player received loot: {loot.GetType().Name}");
        //    loot.Apply(this);
        //}

        public void Attack(Monster monster)
        {
            //Console.WriteLine($"\nTake this! damage dealt:{Power}.");
            //events.Raise(new DamageEvent("Player", "Monster", Power, CurrentHp, MaxHp));
            monster.TakeDamage(Power);
        }

        public void TakeDamage(int damage)
        {
            if (Shield > 0)
            {
                Shield--;
                //Console.WriteLine($"Shield absorbed damage! Remaining shield: {Shield}");
                return;
            }

            //CurrentHp -= damage;  //changed 2 lines to Math.Max
            //if (CurrentHp < 0) CurrentHp = 0;  //changed 2 lines to Math.Max
            CurrentHp = Math.Max(CurrentHp - damage, 0);
            //Console.WriteLine($"You take {damage} damage. Your HP: {CurrentHp}/{MaxHp}");
            events.Raise(new DamageEvent("Monster", "Player", damage, CurrentHp, MaxHp));
        }


        public void HealToFull()
        {
            CurrentHp = MaxHp;
            //events.Raise(new GameMessage($"You heal to full HP: {CurrentHp}/{MaxHp}"));  /// minor optional message
            //Console.WriteLine($"You heal to full HP: {CurrentHp}/{MaxHp}");
        }

        public void AddMaxHp(int amount)
        {
            CurrentHp += Math.Min(amount, MaxHp);
            //events.Raise(new GameMessage($"You heal {amount} HP: {CurrentHp}/{MaxHp}"));  /// minor optional message
            //Console.WriteLine($"You heal {amount} HP: {CurrentHp}/{MaxHp}");
        }

        public void GainExp(int amount)
        {
            CurrentExp += amount;
            //if (CurrentExp < MaxExp) CurrentExp = MaxExp;
            //Console.WriteLine($"Exp Bar: [{CurrentExp}/{MaxExp}]");
            //Console.WriteLine($"Exp Bar: [{Math.Min(MaxExp, CurrentExp)}/{MaxExp}]");
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
            /// Might change to record event later, if level affects game logic or it seems major
            //Console.WriteLine($"*** Level Up! You are now level {Level}.");
            //Console.WriteLine($"New stats — Level: {Level}, HP: +{gainHp} ({CurrentHp}/{MaxHp}), Power: +{gainPower} ({Power})");
            events.Raise(new GameMessage($"*** Level Up! You are now level {Level}.\n" +
                $"New stats — Level: {Level}, HP: +{gainHp} ({CurrentHp}/{MaxHp}), Power: +{gainPower} ({Power})"));
        }

        public void AddShield(int amount)
        {
            Shield += amount;
            //Console.WriteLine($"Your shield increased to {Shield}.");
            //events.Raise(new GameMessage($"Your shield increased to {Shield}."));   /// minor optional message
        }

        public void IncreasePower(int amount)
        {
            Power += amount;
            //Console.WriteLine($"Your power increased to {Power}.");
            //events.Raise(new GameMessage($"Your power increased to {Power}."));   /// minor optional message
        }

        internal void GainKey()
        {
            Keys++;
            //Console.WriteLine($"You got a key! Keys: {Keys}");
            //events.Raise(new GameMessage($"You got a key! Keys: {Keys}"));   /// minor optional message
        }
        public void UseKey()
        {
            if (Keys > 0)
                Keys--;
            //Console.WriteLine($"Keys: {Keys}");
            events.Raise(new GameMessage($"Door unlocks... Keys: {Keys}"));   /// minor optional message
        }

        public void DieAndRespawn()
        {
            Deaths++;
            //Console.WriteLine($"\n*** You died. Respawning... ({Deaths} / 10 deaths) ***\n");
            events.Raise(new PlayerDiedEvent(Deaths));
            HealToFull();
        }

        public bool IsDead() => CurrentHp <= 0;
    }
}