using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fair.Projectiles
{
    public class FairGlobalProjectile : GlobalProjectile
    {
        int boulderStyle = 0;
        public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.Boulder)
            {
                projectile.timeLeft = 2880;
            }
        }

        public override void AI(Projectile projectile)
        {
            if (projectile.type == ProjectileID.Boulder)
            {
                if (projectile.timeLeft == 2880)
                {
                    boulderStyle = Main.rand.Next(2);
                }

                if (boulderStyle == 0)
                {
                    projectile.extraUpdates = 4;
                }
                else
                {
                    projectile.tileCollide = false;

                    Player player = new Player();
                    float closestDistance = float.MaxValue;

                    foreach (Player query in Main.player)
                    {
                        float dist = Vector2.Distance(query.Center, projectile.Center);

                        if (dist < closestDistance)
                        {
                            player = query;
                            closestDistance = dist;
                        }
                    }

                    projectile.velocity = projectile.DirectionTo(player.Center) * 18f;
                }
            }
        }

        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            if (projectile.type == ProjectileID.Boulder && boulderStyle == 1)
            {
                projectile.Kill();
            }
        }

        /*public override void PostDraw(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.type == ProjectileID.PhantasmalEye && projectile.ai[0])
            {

            }
        }*/
    }
}
