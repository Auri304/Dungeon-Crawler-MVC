namespace DungeonCrawlerV9
{
    public class MaxHpLoot : Loot
    {
        public int Amount { get; }
        public override string Name => $"Max HP +{Amount}";

        public MaxHpLoot(int amount) => Amount = amount;

        public override void Apply(Player player)
        {
            player.AddMaxHp(Amount);
        }
    }
}