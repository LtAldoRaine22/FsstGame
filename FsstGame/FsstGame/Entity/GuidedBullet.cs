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
    public class GuidedBullet : Animation
    {
        private int timeTillGuide;
        private int time;

        public new Vector2 AOrigin
        {
            get { return new Vector2(11, 2); }
        }

        public GuidedBullet(Texture2D texture, Vector2 position, float maxHealth, Vector2 velocity, int timeTillGuide)
            : base(texture, position, maxHealth, velocity, 3, 15, 6, 7, 1, false)
        {
            this.timeTillGuide = timeTillGuide;
        }

        public override void Draw(SpriteBatch spriteBatch, bool showDebug)
        {
            spriteBatch.Draw(texture, position, SourceRectangle(), Color.White, Functions.getRotation(velocity), AOrigin, 1f, SpriteEffects.None, 0);
            if (showDebug)
            {
                Functions.drawRectangle(spriteBatch, Bounds, Color.Red);
            }
        }

        public override void Update(World mc, GameTime gameTime)
        {
            if (health <= 0)
            {
                isDead = true;
                return;
            }

            time++;
            if (time >= timeTillGuide)
            {
                float distance = 1000000;
                int index = 0;
                for (int i = 0; i < mc.entities.Count; i++)
                {
                    if (this == mc.entities[i] || mc.entities[i] is Bullet || mc.entities[i] is GuidedBullet || mc.entities[i] is EnemyBullet || mc.entities[i] is Item)
                        continue;
                    float d = Vector2.Distance(position, mc.entities[i].Position);
                    if (d < distance)
                    {
                        distance = d;
                        index = i;
                    }
                }
                if(distance < 250)
                    velocity = Functions.VectorDir(position, mc.entities[index].Position) * 4f;
            }
            position += velocity;
            base.Update(mc, gameTime);
        }
    }
}