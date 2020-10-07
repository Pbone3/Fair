using Fair.Buffs;
using Fair.Resources;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fair.Items
{
    public class FairGlobalItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            HungerPlayer mp = player.GetModPlayer<HungerPlayer>();

            if (item.healLife > 0)
                return !player.HasBuff(BuffID.ManaSickness);

            if (item.buffTime != 0)
                return !player.HasBuff(BuffID.PotionSickness);

            return true;
        }

        public override bool UseItem(Item item, Player player)
        {
            HungerPlayer hp = player.GetModPlayer<HungerPlayer>();

            if (item.healLife > 0)
            {
                if (player.HasBuff(ModContent.BuffType<PotionNausea>()))
                {
                    int buffChoice = 0;
                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            buffChoice = Main.hardMode ? BuffID.CursedInferno : BuffID.OnFire;
                            break;
                        case 1:
                            buffChoice = Main.hardMode ? BuffID.Frostburn : BuffID.OnFire;
                            break;
                        case 2:
                            buffChoice = Main.hardMode ? BuffID.Venom : BuffID.Poisoned;
                            break;
                        case 3:
                            buffChoice = Main.hardMode ? BuffID.Frozen : BuffID.Chilled;
                            break;
                        case 4:
                            buffChoice = Main.hardMode ? BuffID.OgreSpit : BuffID.Chilled;
                            break;
                    }

                    player.AddBuff(buffChoice, Main.hardMode ? 720 : 360);
                }
                else
                {
                    player.AddBuff(ModContent.BuffType<PotionNausea>(), int.MaxValue);
                }
                player.AddBuff(BuffID.ManaSickness, 360);
                return true;
            }

            if (item.buffType == BuffID.WellFed)
            {
                hp.SatedAmount += (item.buffTime / 3600f) / 5f;
                hp.ClampSated();
                hp.HungerCurrent += (int)(item.buffTime / 3600f);
                hp.ClampHunger();
                hp.StarvingCounter = 0;
                hp.ClampStarvingCounter();
            }

            return base.UseItem(item, player);
        }
    }
}
