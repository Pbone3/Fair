using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace Fair
{
    public class FairPlayer : ModPlayer
    {
        int hitCounter;
        bool screenShakeControl;

        public override void Initialize()
        {
            hitCounter = 0;
            screenShakeControl = false;
        }

        public override void PostUpdate()
        {
            if (player.lavaWet)
            {
                player.AddBuff(Main.hardMode ? BuffID.CursedInferno : BuffID.OnFire, 2);
            }
        }

        public override void PreUpdate()
        {
            if (!Main.expertMode)
            {
                Main.expertMode = true;
                Main.NewText("Expert mode has been enabeld!", Main.hcColor);
                if (Main.netMode != NetmodeID.SinglePlayer) NetMessage.SendData(MessageID.WorldData);
            }

            player.breathMax = 3;

            if (player.ZoneSkyHeight)
            {
                player.AddBuff(BuffID.Suffocation, 2);
            }

            if (!player.accDivingHelm)
            {
                foreach (Dust dust in from Dust d in Main.dust where d.active select d)
                {
                    if (dust.type != DustID.Blood && player.getRect().Contains(dust.position.ToPoint()))
                    {
                        player.Hurt(PlayerDeathReason.ByCustomReason($"{player.name} was dusted to death."), 1, 0);
                        player.immuneTime = 0;
                    }
                }
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (player.wet && player.electrified)
            {
                player.lifeRegen -= 56;
            }

            if ((player.onFire || player.onFire2) && player.lavaWet)
            {
                player.lifeRegen -= 24;
            }

            if (player.CCed)
            {
                player.lifeRegen -= 8;
            }
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            foreach (Item item in items)
            {
                if (item.type == ItemID.CopperShortsword)
                {
                    item.TurnToAir();
                }

                if (item.type == ItemID.CopperPickaxe)
                {
                    item.TurnToAir();
                }

                if (item.type == ItemID.CopperAxe)
                {
                    item.prefix = PrefixID.Sluggish;
                }
            }
        }

        public override bool ModifyNurseHeal(NPC nurse, ref int health, ref bool removeDebuffs, ref string chatText)
        {
            if (Main.npc.Where(n => n.active && n.boss).ToList().Count != 0)
            {
                string name = Main.LocalPlayer.name;
                nurse.StrikeNPCNoInteraction(int.MaxValue, 0f, 0);
                if (Main.netMode == NetmodeID.MultiplayerClient) NetMessage.BroadcastChatMessage(NetworkText.FromLiteral($"{Main.LocalPlayer.name} tried to heal at the hurse during a boss fight, therefor making them tonights biggest loser"), Color.Orange);
                else Main.NewText($"{name} tried to heal at the nurse during a boss fight, therefore making them tonights biggest loser", Color.Orange);

                return false;
            }

            health /= 2;
            Main.LocalPlayer.AddBuff(BuffID.Poisoned, 180);

            return base.ModifyNurseHeal(nurse, ref health, ref removeDebuffs, ref chatText);
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (anyBoss())
            {
                hitCounter++;

                if (hitCounter >= 13) player.KillMe(PlayerDeathReason.ByCustomReason($"{player.name} got hit."), double.MaxValue, 0);
            }
            else
            {
                hitCounter = 0;
            }

            if (Main.rand.NextBool(100)) screenShakeControl = true;
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            if (anyBoss())
            {
                hitCounter++;

                if (hitCounter >= 13) player.KillMe(PlayerDeathReason.ByCustomReason($"{player.name} got hit."), double.MaxValue, 0);
            }
            else
            {
                hitCounter = 0;
            }

            if (Main.rand.NextBool(100)) screenShakeControl = true;
        }

        public override void ModifyScreenPosition()
        {
            if (screenShakeControl)
            {
                Main.screenPosition += Main.rand.NextVector2Square(-15, 15);
                if (Main.rand.NextBool(120)) screenShakeControl = false;
            }
        }

        public override void UpdateDead()
        {
            hitCounter = 0;
        }

        private bool anyBoss()
        {
            foreach (NPC npc in from NPC n in Main.npc where n.boss select n)
            {
                return true;
            }

            return false;
        }
    }
}
