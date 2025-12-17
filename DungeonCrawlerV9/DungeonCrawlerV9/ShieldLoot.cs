namespace DungeonCrawlerV9
{
    public class ShieldLoot : Loot
    {
        public int Amount { get; }
        public override string Name => $"Shield +{Amount}";

        public ShieldLoot(int amount) => Amount = amount;

        public override void Apply(Player player)
        {
            player.AddShield(Amount);
        }
    }
}