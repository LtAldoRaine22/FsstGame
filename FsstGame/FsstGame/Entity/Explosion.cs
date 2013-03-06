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
    public class Explosion : Entity
    {
        public static int TILE_WIDTH = 53;
        public static int TILE_HEIGHT = 49;

        private int frame = 0;
        private int i = 0;

        public Explosion(Vector2 position)
            :base(Textures.Explosion, position, 1, false)
        {
            Sounds.explosion.Play(0.5f, 0.0f, 0.0f);
        }

        public override void Update(World mc, GameTime gameTime)
        {
            i++;
            if (i >= 3)
            {
                i = 0;
                frame++;
            }
            if (frame >= 8)
                isDead = true;
        }

        public override void Draw(SpriteBatch spriteBatch, bool showDebug)
        {
            spriteBatch.Draw(texture, position, getSourceRectangle(), Color.White, 0f, new Vector2(TILE_WIDTH / 2, TILE_HEIGHT / 2), 1f, SpriteEffects.None, 0);
        }

        private Rectangle getSourceRectangle()
        {
            return new Rectangle() { 
                X = frame*TILE_WIDTH,
                Y = 0,
                Width = TILE_WIDTH,
                Height = TILE_HEIGHT
            };
        }
    }
}