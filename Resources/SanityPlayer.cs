/*using Fair.Buffs;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Effects;

namespace Fair.Resources
{
    public class SanityPlayer : ResourcePlayer
    {
        public int SanityNearbyTileRegen { get => (player.HasBuff(BuffID.Campfire) ? 1 : 0) + (player.HasBuff(BuffID.StarInBottle) ? 1 : 0) + (player.HasBuff(BuffID.HeartLamp) ? 1 : 0); }

        public const int SanityMax = 100;
        public int SanityCounter;

        public Sanity GetSanity()
        {
            if (SanityCounter >= 75) return Sanity.Sane;
            if (SanityCounter >= 50) return Sanity.OK;
            if (SanityCounter >= 25) return Sanity.Alright;
            if (SanityCounter > 1) return Sanity.Crazy;
            return Sanity.Insane;
        }

        public void UpdateSanity()
        {
            int decrement = 0;
            decrement += SanityNearbyTileRegen;

            if (player.ZoneUnderworldHeight) decrement *= -2;
            if (player.ZoneCorrupt || player.ZoneCrimson) decrement -= 1;
            if (player.ZoneDungeon) decrement -= NPC.downedPlantBoss ? 2 : 1;
            if (Main.eclipse) decrement -= 1;
            if (player.ZoneHoly) decrement += 1;
            if (player.ZoneSkyHeight) decrement += 1;

            SanityCounter += decrement;
            SanityCounter = (int)MathHelper.Clamp(SanityCounter, 0, SanityMax);

            if (GetSanity() < Sanity.OK) player.AddBuff(BuffID.Darkness, 60);
        }

        public void UpdateInsanse()
        {
        }

        public override void Tick(ulong howManyTicks)
        {
            if (howManyTicks % 45 == 0) UpdateSanity();
            if (GetSanity() == Sanity.Insane) UpdateInsanse();
        }

        public override void Initialize()
        {
            SanityCounter = SanityMax;
        }

        public override void PreUpdate()
        {
            Main.NewText(SanityCounter + "/" + SanityMax);

            base.PreUpdate();
        }
    }

    public enum Sanity : byte
    {
        Insane,
        Crazy,
        Alright,
        OK,
        Sane
    }
}
*/