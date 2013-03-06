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
    public abstract class InterFace
    {
        protected Rectangle rectangle;
        protected MouseState oldMState;
        protected object tag;

        protected Texture2D texture;

        protected bool enabled;

        #region Properties

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(rectangle.Location.X - (int)Origin.X, rectangle.Location.Y - (int)Origin.Y, rectangle.Width, rectangle.Height); }
        }

        public Vector2 Origin
        {
            get { return new Vector2(rectangle.Width / 2, rectangle.Height / 2); }
        }

        public Point Location
        {
            get { return rectangle.Location; }
            set { rectangle.Location = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        #endregion

        public InterFace(Rectangle rectangle, Texture2D texture)
        {
            this.rectangle = rectangle;
            this.texture = texture;

            this.enabled = true;
        }

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}