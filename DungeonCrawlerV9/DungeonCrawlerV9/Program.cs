namespace DungeonCrawlerV9
{
    internal class Program
    {
        static void Main()
        {
            IGameView view = new ConsoleView();
            var gameController = new GameController(view);
            gameController.Start();
        }
    }
}
