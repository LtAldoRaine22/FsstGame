using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace FsstGame
{
    public class Enemy : Entity
    {
        private Timer shootTimer;
        private int level;
        private float bulletSpeed = 5f;

        public Enemy(Texture2D texture, Vector2 position, int maxHealth, Vector2 velocity, int level)
            : base(texture, position, maxHealth, velocity, true)
        {
            this.level = level;
            shootTimer = new Timer(0, 150, false);
        }

        public override void Draw(SpriteBatch spriteBatch, bool showDebug)
        {
            DrawTextureR(spriteBatch);
            Functions.drawHealthBar(spriteBatch, new Rectangle(Bounds.X, Bounds.Bottom, Bounds.Width, 3), health, maxHealth, Functions.GetColorFromHealth(health, maxHealth), Color.DarkGray, true);
            if (showDebug)
            {
                Functions.drawRectangle(spriteBatch, Bounds, Color.Green);
                spriteBatch.DrawString(Fonts.Font, velocity.ToString(), new Vector2(Bounds.X, Bounds.Y - 15), Color.White);
                Functions.drawHealthBar(spriteBatch, new Rectangle(Bounds.X, Bounds.Bottom + 3, Bounds.Width, 3), shootTimer.Value, shootTimer.Freq, Color.Cyan, Color.Transparent, true);
            }
        }

        public override void Update(World mc, GameTime gameTime)
        {
            if (health <= 0)
            {
                isdead = true;
                causesExplosion = true;
                return;
            }

            #region Shooting

            shootTimer.Update();
            if (shootTimer.State)
            {
                shootTimer.State = false;
                Vector2 dir = Functions.VectorDir(position, mc.Player.Position);
                float rot = Functions.getRotation(dir);

                switch (level)
                {
                    case 0:
                        mc.entities.Add(new EnemyBullet(Textures.EnemyBullet, position, 1, dir * bulletSpeed));
                        break;
                    case 1:
                        mc.entities.Add(new EnemyBullet(Textures.EnemyBullet, position, 1, Functions.fromRotation(rot - 0.1f) * bulletSpeed));
                        mc.entities.Add(new EnemyBullet(Textures.EnemyBullet, position, 1, dir * bulletSpeed));
                        mc.entities.Add(new EnemyBullet(Textures.EnemyBullet, position, 1, Functions.fromRotation(rot + 0.1f) * bulletSpeed));
                        break;
                }
            }

            #endregion

            position += velocity;

            CheckBounds();
        }

        protected override void CheckBounds()
        {
            if (Bounds.Left < Gui.StarFieldBounds.Left - Bounds.Width - 10)
                isDead = true;
            else if (Bounds.Right > Gui.StarFieldBounds.Right + Bounds.Width + 10)
                isDead = true;
            else if (Bounds.Bottom > Gui.StarFieldBounds.Bottom + Bounds.Height + 10)
                isDead = true;

            if (isDead)
                causesExplosion = false;
        }
    }
}