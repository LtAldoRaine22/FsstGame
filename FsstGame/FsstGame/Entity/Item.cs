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
    public class Item : Entity
    {
        public const int CRYSTAL = 0;
        public const int METAL = 1;

        public int itemType;

        public Item(int itemType, Vector2 position, Vector2 velocity)
            : base(TextureFromItemType(itemType), position, 1, velocity, false)
        {
            this.itemType = itemType;
        }

        public override void Draw(SpriteBatch spriteBatch, bool showDebug)
        {
            DrawTexture(spriteBatch);

            if (showDebug)
            {
                Functions.drawRectangle(spriteBatch, Bounds, Color.Blue, 1);
                spriteBatch.DrawString(Fonts.Font, velocity.ToString(), new Vector2(Bounds.X, Bounds.Y - 15), Color.White);
            }
        }

        public override void Update(World mc, GameTime gameTime)
        {
            position += velocity;

            CheckBounds();
        }

        public static Texture2D TextureFromItemType(int itemType)
        {
            Texture2D tex = Textures.Crystal;

            switch(itemType)
            {
                case CRYSTAL:
                    tex = Textures.Crystal;
                    break;
                case METAL:
                    tex = Textures.Metal;
                    break;
            }

            return tex;
        }
    }
}