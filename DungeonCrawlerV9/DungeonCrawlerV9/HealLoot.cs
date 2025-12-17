namespace DungeonCrawlerV9
{
    public class HealLoot : Loot
    {
        public override string Name => "Full Heal";

        public override void Apply(Player player)
        {
            player.HealToFull();
        }
    }
}