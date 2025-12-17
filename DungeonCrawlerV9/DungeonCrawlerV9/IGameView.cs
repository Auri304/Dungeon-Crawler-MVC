namespace DungeonCrawlerV9
{
    public interface IGameView
    {
        void ShowIntro();
        int AskDifficulty();
        void ShowStatus(Player player, Dungeon dungeon);
        //string AskMove();     // replaced with two functions below, seemed to be more strict mvc (not sure if it's the best solution)
        void ShowMovePrompt();
        void ShowInvalidMove();

        void ShowWin();
        void ShowLose();

        // Event-driven rendering
        void ShowRoomEntered(RoomEnteredEvent e);
        void ShowRoomCleared(RoomClearedEvent e);
        void ShowDamage(DamageEvent e);
        void ShowMonsterDied(MonsterDiedEvent e);
        void ShowPlayerDied(PlayerDiedEvent e);
        void ShowLoot(LootGrantedEvent e);
        void ShowLog(GameLogEvent e);
        void ShowMessage(GameMessage e);
    }
}