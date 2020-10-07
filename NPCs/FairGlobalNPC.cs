using Fair.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fair.NPCs
{
    public class FairGlobalNPC : GlobalNPC
    {
        int timer = 0;
        public float[] oldAI = new float[4];
        public override bool InstancePerEntity => true;

        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position) => false;

        public override void SetDefaults(NPC npc)
        {
            if (npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall)
            {
                npc.damage = 222;
                npc.lifeMax = 2222;
                npc.GivenName = "Emissary of Twenty Two";
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            switch (npc.type)
            {
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    int index = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.VileSpit);
                    Main.npc[index].damage = 12;
                    break;
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            switch (npc.type)
            {
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    int index = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.VileSpit);
                    Main.npc[index].damage = 12;
                    break;
            }
        }

        public override bool CheckDead(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.EaterofWorldsHead:
                case NPCID.EaterofWorldsBody:
                case NPCID.EaterofWorldsTail:
                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.BigEater);
                    break;
            }
            return base.CheckDead(npc);
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.Bee || npc.type == NPCID.BeeSmall)
            {
                if (Main.GameUpdateCount % 30 == 0)
                    Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center), ProjectileID.PhantasmalEye, 22, 22);
            }

            if (npc.type == NPCID.KingSlime)
            {
                if (npc.ai[0] + 3 <= 0)
                {
                    npc.ai[0] += npc.life <= npc.lifeMax / 2f ? 2 : 1;
                }

                if (npc.ai[2] != 200) npc.ai[2]++;

                timer++;

                if ((npc.life <= npc.lifeMax / 2f && timer % 40 == 0) || (timer % 80 == 0))
                {
                    for (int i = -12; i < 11; i++)
                    {
                        Dust.NewDust(Main.player[npc.target].Center + new Vector2(i * 32, -1400), 1, 1, DustID.t_Slime);
                        Projectile.NewProjectile(Main.player[npc.target].Center + new Vector2(i * 32, -1400), new Vector2(0, 32).RotatedByRandom(MathHelper.ToRadians(5)), ProjectileID.SpikedSlimeSpike, 3, 0);
                    }
                }
            }

            if (npc.type == NPCID.EyeofCthulhu)
            {
                if (npc.ai[2] == 1)
                {
                    int type = npc.ai[0] == 3 ? ProjectileID.PhantasmalEye : ProjectileID.EyeLaser;
                    int times = type == ProjectileID.EyeLaser ? 8 : 6;
                    for (int i = 0; i < times; i++)
                    {
                        int index = Projectile.NewProjectile(npc.Center, new Vector2(8, 8).RotatedBy(MathHelper.ToRadians((360 / times) * i)), type, type == ProjectileID.EyeLaser ? 5 : 4, 0f);
                        Main.projectile[index].tileCollide = false;
                        Main.projectile[index].timeLeft = 240;
                    }
                }

                if (npc.ai[0] == 1f || npc.ai[0] == 2f)
                {
                    npc.dontTakeDamage = true;
                    npc.immortal = true;
                }
                else
                {
                    npc.dontTakeDamage = false;
                    npc.immortal = false;
                }
            }

            if (npc.type == NPCID.EaterofWorldsHead)
            {
                if (Main.rand.NextBool(60)) Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 6f, ProjectileID.CursedFlameHostile, 7, 1f);
                if (Main.rand.NextBool(7)) Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 8f, ProjectileID.CorruptSpray, 5, 1f, Main.myPlayer);
            }

            if (npc.type == NPCID.BrainofCthulhu)
            {
                if (Main.rand.NextBool(7)) Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center) * 8f, ProjectileID.CrimsonSpray, 5, 1f, Main.myPlayer);

                if (npc.ai[0] < 0)
                {
                    timer++;
                    if (timer % 60 == 0)
                    {
                        for (int i = -3; i < 4; i++)
                        {
                            Projectile.NewProjectile(npc.Center, npc.DirectionTo(Main.player[npc.target].Center).RotatedBy(MathHelper.ToRadians(24 * i)) * 16, ProjectileID.GoldenShowerHostile, 6, 0f);
                        }
                    }
                }
            }

            if (npc.type == NPCID.QueenBee)
            {
                timer++;

                // staying still
                if (npc.ai[0] == 1f || npc.ai[0] == 3f)
                {
                    int projType = Main.player[npc.target].ZoneRockLayerHeight && Main.player[npc.target].ZoneJungle ? ProjectileID.BulletDeadeye : ProjectileID.SniperBullet;
                    if (timer % 5 == 0) Projectile.NewProjectile(npc.Center, (npc.DirectionTo(Main.player[npc.target].Center) * 80f * MathHelper.Clamp(Main.rand.NextFloat(), 0.5f, 0.75f)).RotatedByRandom(MathHelper.ToRadians(18)), projType, 8, 16f);
                }

                return true;
            }

            oldAI = npc.ai;
            return base.PreAI(npc);
        }

        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            if (npc.type == NPCID.KingSlime)
            {
                if (Main.rand.NextBool(3))
                {
                    int type = 0;

                    switch (Main.rand.Next(5))
                    {
                        case 0:
                            type = NPCID.SpikedJungleSlime;
                            break;
                        case 1:
                            type = NPCID.Crimslime;
                            break;
                        case 2:
                            type = NPCID.Slimer;
                            break;
                        case 3:
                            type = NPCID.LavaSlime;
                            break;
                        case 4:
                            type = NPCID.SandSlime;
                            break;
                    }

                    NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, type);
                }
            }
        }
    }
}
