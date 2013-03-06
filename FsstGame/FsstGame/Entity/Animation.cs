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
    public class Animation : Entity
    {
        protected int tile_width;
        protected int tile_height;

        protected int maxFramesX;
        protected int maxFramesY;
        protected int frameX;
        protected int frameY;

        protected int delay;
        protected int i;
        protected bool dieAfter1Anim = true;

        public Vector2 AOrigin
        {
            get { return new Vector2(tile_width / 2, tile_height / 2); }
        }
        
        public new Rectangle Bounds
        {
            get { return new Rectangle((int)position.X - tile_width / 2, (int)position.Y - tile_height / 2, tile_width, tile_height); }
        }

        public Animation(Texture2D texture, Vector2 position, float maxHealth, Vector2 velocity, int delay, int tile_width, int tile_height, int maxFramesX, int maxFramesY)
            : base(texture, position, maxHealth, velocity, false)
        {
            this.tile_width = tile_width;
            this.tile_height = tile_height;
            this.maxFramesX = maxFramesX;
            this.maxFramesY = maxFramesY;
            this.frameX = 0;
            this.frameY = 0;
            this.i = 0;
            this.delay = delay;
        }

        public Animation(Texture2D texture, Vector2 position, float maxHealth, Vector2 velocity, int delay, int tile_width, int tile_height, int maxFramesX, int maxFramesY, bool dieAfter1Anim)
            : base(texture, position, maxHealth, velocity, false)
        {
            this.tile_width = tile_width;
            this.tile_height = tile_height;
            this.maxFramesX = maxFramesX;
            this.maxFramesY = maxFramesY;
            this.frameX = 0;
            this.frameY = 0;
            this.i = 0;
            this.delay = delay;
            this.dieAfter1Anim = dieAfter1Anim;
        }

        public override void Update(World mc, GameTime gameTime)
        {
            i++;
            if (i >= delay)
            {
                i = 0;
                frameX++;
                if (frameX >= maxFramesX)
                {
                    frameX = 0;
                    frameY++;
                    if (frameY >= maxFramesY)
                    {
                        frameY = 0;
                        if (dieAfter1Anim)
                            isdead = true;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, bool showDebug)
        {
            spriteBatch.Draw(texture, position, SourceRectangle(), Color.White, 0f, AOrigin, 1f, SpriteEffects.None, 0); 
        }

        protected virtual Rectangle SourceRectangle()
        {
            return new Rectangle(frameX * tile_width, frameY * tile_height, tile_width, tile_height);
        }
    }
}