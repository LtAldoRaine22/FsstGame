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
    public class Asteroid : Entity
    {
        public int size;

        public Asteroid(int size, Vector2 position, Vector2 velocity)
            : base(TextureFromSize(size), position, HealthFromSize(size), velocity, true)
        {
            this.size = size;
        }

        public override void Update(World mc, GameTime gameTime)
        {
            if (health <= 0)
            {
                isdead = true;
                causesExplosion = true;
                return;
            }

            position += velocity;
            
            CheckBounds();
        }

        public override void Draw(SpriteBatch spriteBatch, bool showDebug)
        {
            DrawTexture(spriteBatch);
            Functions.drawHealthBar(spriteBatch, new Rectangle(Bounds.X, Bounds.Bottom, Bounds.Width, 3), health, maxHealth, Functions.GetColorFromHealth(health, maxHealth), Color.Transparent, false);
            if (showDebug)
            {
                Functions.drawRectangle(spriteBatch, Bounds, Color.Red);
                spriteBatch.DrawString(Fonts.Font, velocity.ToString(), new Vector2(Bounds.X, Bounds.Y - 10), Color.White);
            }
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

        private static Texture2D TextureFromSize(int size)
        {
            Texture2D tex = Textures.Asteroid3;
            switch (size)
            { 
                case 1:
                    tex = Textures.Asteroid2;
                    break;
                case 2:
                    tex = Textures.Asteroid1;
                    break;
            }
            return tex;
        }

        private static int HealthFromSize(int size)
        {
            switch (size)
            { 
                case 1:
                    return 5;
                case 2:
                    return 15;
            }
            return 1;
        }
    }
}