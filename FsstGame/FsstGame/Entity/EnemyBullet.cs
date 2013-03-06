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
    public class EnemyBullet : Entity
    {
        public EnemyBullet(Texture2D texture, Vector2 position, int maxHealth, Vector2 velocity)
            : base(texture, position, maxHealth, velocity, false)
        {
        }

        public override void Update(World mc, GameTime gameTime)
        {
            if (health <= 0)
            {
                isdead = true;
                return;
            }
            position += velocity;

            CheckBounds();
        }

        public override void Draw(SpriteBatch spriteBatch, bool showDebug)
        {
            DrawTextureR(spriteBatch);
        }        
    }
}