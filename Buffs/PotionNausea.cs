using Terraria;
using Terraria.ModLoader;

namespace Fair.Buffs
{
    public class PotionNausea : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Potion Nausea");
            Description.SetDefault("Too much of a good thing");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
}
