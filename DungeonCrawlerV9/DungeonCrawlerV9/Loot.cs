namespace DungeonCrawlerV9
{
    public abstract class Loot
    {
        public abstract string Name { get; }
        public abstract void Apply(Player player);
    }
}