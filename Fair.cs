using Fair.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Fair
{
	public class Fair : Mod
	{
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            float t = 0f;
            Player player = Main.LocalPlayer;

            if (player.ZoneDirtLayerHeight)
            {
                t = 0.3f;
            }
            if (player.ZoneRockLayerHeight)
            {
                t = 0.375f;
            }
            if (player.ZoneUnderworldHeight)
            {
                t = 0.64f;
            }

            spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * t);

            //DEBUG
            /*foreach (NPC npc in from n in Main.npc where n.boss select n)
            {
                FairGlobalNPC fg = npc.GetGlobalNPC<FairGlobalNPC>();
                ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, $"CURRENT: {npc.ai[0]}, {npc.ai[1]}, {npc.ai[2]}, {npc.ai[3]}\nOLD: {fg.oldAI[0]}, {fg.oldAI[1]}, {fg.oldAI[2]}, {fg.oldAI[3]}\nDIFF: {npc.ai[0] != fg.oldAI[0]}, {npc.ai[1] != fg.oldAI[1]}, {npc.ai[2] != fg.oldAI[2]}, {npc.ai[3] != fg.oldAI[3]}", Vector2.Zero, Color.Red, 0f, Vector2.Zero, new Vector2(1f)); ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, $"CURRENT: {npc.ai[0]}, {npc.ai[1]}, {npc.ai[2]}, {npc.ai[3]}\nOLD: {fg.oldAI[0]}, {fg.oldAI[1]}, {fg.oldAI[2]}, {fg.oldAI[3]}", Vector2.Zero, Color.Red, 0f, Vector2.Zero, new Vector2(1f));
            }*/
        }
    }
}