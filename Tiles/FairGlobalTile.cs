using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fair.Tiles
{
    public class FairGlobalTile : GlobalTile
    {
        public override void FloorVisuals(int type, Player player)
        {
            switch (type)
            {
                case TileID.IceBlock:
                    player.AddBuff(BuffID.Frostburn, 1);
                    break;
                case TileID.Hellstone:
                    player.AddBuff(BuffID.OnFire, 1);
                    break;
                case TileID.Ebonstone:
                    if (Main.rand.NextBool(360)) player.AddBuff(BuffID.CursedInferno, 60);
                    break;
                case TileID.Crimstone:
                    if (Main.rand.NextBool(360)) player.AddBuff(BuffID.Ichor, 60);
                    break;
                case TileID.SnowBlock:
                    player.AddBuff(BuffID.Chilled, 1);
                    break;
                case TileID.Ash:
                    player.AddBuff(BuffID.Darkness, 1);
                    break;
            }
        }

        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            switch (type)
            {
                case TileID.Hellstone:
                    NPC.NewNPC(i * 16, j * 16, NPCID.BurningSphere);
                    break;
            }
        }
    }
}
