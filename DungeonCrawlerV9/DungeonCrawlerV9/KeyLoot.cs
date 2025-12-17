namespace DungeonCrawlerV9
{
    public class KeyLoot : Loot
    {
        public override string Name => $"Key";

        public override void Apply(Player player)
        {
            player.GainKey();
        }
    }
}