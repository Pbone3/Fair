using Fair.Resources;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fair.Buffs
{
    public class HungerBuff : ModBuff
    {
        const string _lineEnd = "\n";

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hunger");
            Description.SetDefault("Verbose");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            canBeCleared = false;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            HungerPlayer mp = Main.LocalPlayer.GetModPlayer<HungerPlayer>();

            if (mp.GetFullness() < Fullness.OK) rare = ItemRarityID.LightRed;
            if (mp.IsStarving) rare = ItemRarityID.Green;

            tip = string.Concat(new string[]
            {
                "You are " + Regex.Replace(mp.GetFullness().ToString(), "([A-Z])", " $1").Trim(), _lineEnd,
                "Hunger: " + mp.HungerCurrent.ToString(), "/", HungerPlayer.HungerMax.ToString(), _lineEnd,
                "Sated: " + (mp.IsSated ? "Yes" : "No"), _lineEnd,
                "Saturation: " + mp.SatedAmount.ToString(), "/", HungerPlayer.SatedMax.ToString(), _lineEnd
            });
        }
    }
}
