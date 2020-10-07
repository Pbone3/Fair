using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fair.Items
{
    public class CreeperBloodOrb : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(string.Empty);
        }

        public override void SetDefaults()
        {
            item.Size = new Vector2(10, 10);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Dust.NewDust(item.Center, 1, 1, DustID.Blood);
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            Dust.NewDust(item.Center, 1, 1, DustID.Blood);
            grabRange = 86;
        }

        public override bool OnPickup(Player player)
        {
            player.Hurt(PlayerDeathReason.ByCustomReason($"{player.name} died lol"), 4, 0);
            return false;
        }
    }
}
