namespace DungeonCrawlerV9
{
    public class PowerLoot : Loot
    {
        public int Amount { get; }
        public override string Name => $"Power +{Amount}";
        public PowerLoot(int amount) => Amount = amount;

        public override void Apply(Player player)
        {
            player.IncreasePower(Amount);
        }
    }
}