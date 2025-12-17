using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerV9
{
    public sealed class GameEvents
    {
        public event Action<RoomEnteredEvent> RoomEntered;
        public event Action<RoomClearedEvent> RoomCleared;
        public event Action<DamageEvent> Damage;
        public event Action<MonsterDiedEvent> MonsterDied;
        public event Action<PlayerDiedEvent> PlayerDied;
        public event Action<LootGrantedEvent> LootGranted;
        public event Action<GameLogEvent> Log; // optional, for logs
        public event Action<GameMessage> Message; // optional, for flavor instead of adding boilerplate

        public void Raise(RoomEnteredEvent e) => RoomEntered?.Invoke(e);
        public void Raise(RoomClearedEvent e) => RoomCleared?.Invoke(e);
        public void Raise(DamageEvent e) => Damage?.Invoke(e);
        public void Raise(MonsterDiedEvent e) => MonsterDied?.Invoke(e);
        public void Raise(PlayerDiedEvent e) => PlayerDied?.Invoke(e);
        public void Raise(LootGrantedEvent e) => LootGranted?.Invoke(e);
        public void Raise(GameLogEvent e) => Log?.Invoke(e);
        public void Raise(GameMessage e) => Message?.Invoke(e);
    }

    public record RoomEnteredEvent(string Description, RoomType Type, int Row, int Col);   /// Added RoomType Type Col for future context
    public record RoomClearedEvent(string Description, RoomType Type, int Row, int Col); /// Added int Row, int Col for future context
    /// <summary>
    ///  Added info that is not in use yet in to records above. Will appricite your opinion (Aviv):
    ///  Thought- events could pass information instead of creating more events and less refactoring later.
    ///  For example game logic will require to create achivment/condition based on entering or clearing X rooms X types.
    /// </summary>
    public record DamageEvent(string Attacker, string Target, int Amount, int TargetHpAfter, int TargetMaxHp);
    public record MonsterDiedEvent(Monster Monster);
    public record PlayerDiedEvent(int Deaths);
    public record LootGrantedEvent(Loot Loot);
    public record GameLogEvent(string Message);
    public record GameMessage(string Text);
}
