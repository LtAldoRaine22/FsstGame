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
    public abstract class Entity
    {
        protected Vector2 position;
        protected Vector2 velocity;

        protected Texture2D texture;
        protected Color[] colorData;
        protected bool isdead = false;
        protected bool causesExplosion = true;

        protected float health;
        protected float maxHealth;

        #region Properties
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public bool isDead
        {
            get { return isdead; }
            set { isdead = value; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X - texture.Width / 2, (int)position.Y - texture.Height / 2, texture.Width, texture.Height); }
        }

        public Vector2 Origin
        {
            get { return new Vector2(texture.Width / 2, texture.Height / 2); }
        }

        public Color[] ColorData
        {
            get { return colorData; }
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        public float MaxHealth
        {
            get { return maxHealth; }
        }

        public bool CausesExplosion
        {
            get { return causesExplosion; }
            set { causesExplosion = value; }
        }
        #endregion

        public Entity(Texture2D texture, Vector2 position, float maxHealth, bool causesExplosion)
        {
            this.causesExplosion = causesExplosion;
            this.maxHealth = maxHealth;
            this.health = maxHealth;
            this.texture = texture;
            this.position = position;
            this.velocity = new Vector2(0, 0);

            this.colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);
        }

        public Entity(Texture2D texture, Vector2 position, float maxHealth, Vector2 velocity, bool causesExplosion)
        {
            this.causesExplosion = causesExplosion;
            this.maxHealth = maxHealth;
            this.health = maxHealth;
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;

            this.colorData = new Color[texture.Width * texture.Height];
            texture.GetData(colorData);
        }

        public abstract void Draw(SpriteBatch spriteBatch, bool showDebug);

        protected void DrawTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);
        }

        protected void DrawTextureR(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, Functions.getRotation(velocity), Origin, 1f, SpriteEffects.None, 0);
        }

        public abstract void Update(World mc, GameTime gameTime);

        public virtual void Hurt(float amount)
        {
            health -= amount;
        }

        protected virtual void CheckBounds()
        {
            if (Bounds.Left < Gui.StarFieldBounds.Left - Bounds.Width - 10)
                isDead = true;
            else if (Bounds.Top < Gui.StarFieldBounds.Top - Bounds.Height - 10)
                isDead = true;
            else if (Bounds.Right > Gui.StarFieldBounds.Right + Bounds.Width + 10)
                isDead = true;
            else if (Bounds.Bottom > Gui.StarFieldBounds.Bottom + Bounds.Height + 10)
                isDead = true;
        }
    }
}