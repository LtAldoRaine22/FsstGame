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
    public class Player : Animation
    {
        #region Static
        private static float friction = 0.75f;
        private static float moveSpeed = 0.45f; 
        public static float scale = 1f;

        public static Keys[] InputKeys = new Keys[]{
            Keys.W, // Up
            Keys.S, // Down
            Keys.A, // Left
            Keys.D, // Right
            Keys.Space // Shoot
        };

        public static Rectangle PlayerBounds = new Rectangle(14, 417, 943, 290);

        private static int[] mainGunFreqs = new int[] { 
            23,
            19,
            16,
            13,
            10,
            7
        };

        private static int[] secGunFreqs = new int[] { 
            100,
            85,
            70,
            55,
            40
        };

        #endregion

        #region Upgrades

        public int mainGunLevel = 0;
        private int mGunFreq = 0;
        public int mainGunFreq
        {
            get { return mGunFreq; }
            set
            {
                mGunFreq = value;
                if (mGunFreq >= mainGunFreqs.Length)
                    mGunFreq = mainGunFreqs.Length - 1;
                gunTimer.Freq = mainGunFreqs[mGunFreq]; 
            }
        }

        public int speedLevel = 0;

        public int MaxShieldHealth = 0;
        public int ShieldRegenLevel = 0;

        #endregion

        #region Variables
        
        //private int secShootFreq
        //{
        //    get 
        //    {
        //        if (secGunLevel >= secGunFreqs.Length)
        //            secGunLevel = secGunFreqs.Length - 1;
        //        return secGunFreqs[secGunLevel];
        //    }
        //}
        //private int secShootValue = 0;
        //private bool secCanShoot = true;

        private float maxShieldHealth
        {
            get { return MaxShieldHealth + 1f; }
        }
        private float shieldHealth;
        private float shieldRegen
        {
            get { return (ShieldRegenLevel + 1) * 0.009f; }
        }

        private Timer gunTimer;
        private Timer shieldTimer;

        private SpriteEffects sEffect = SpriteEffects.None;
        private float rot;

        #endregion
        
        public Player(Texture2D texture, Vector2 position, int maxHealth)
            : base(texture, position, maxHealth, Vector2.Zero, 3, 41, 43, 6, 1, false)
        {
            gunTimer = new Timer(0, mainGunFreqs[0], true);
            shieldTimer = new Timer(0, 65, false);
        }

        public override void Draw(SpriteBatch spriteBatch, bool showDebug)
        {
            spriteBatch.Draw(texture, position, this.SourceRectangle(), Color.White, 0, AOrigin, scale, sEffect, 0);
            Functions.drawHealthBar(spriteBatch, new Rectangle(Bounds.X, Bounds.Bottom, Bounds.Width, 3), health, maxHealth, Functions.GetColorFromHealth(health, maxHealth), Color.DarkGray, true);
            Functions.drawHealthBar(spriteBatch, new Rectangle(Bounds.X, Bounds.Bottom + 3, Bounds.Width, 3), shieldHealth, maxShieldHealth, Color.BlueViolet, Color.DarkGray, true);
            
            if (showDebug)
            {
                Functions.fillRectangle(spriteBatch, new Rectangle((int)position.X - 1, (int)position.Y - 1, 3, 3), Color.White);
                Functions.drawRectangle(spriteBatch, PlayerBounds, Color.Lime, 3);
                Functions.drawRectangle(spriteBatch, Bounds, Color.Blue, 1);
                Functions.drawHealthBar(spriteBatch, new Rectangle(Bounds.X, Bounds.Bottom + 6, Bounds.Width, 3), shieldTimer.Value, shieldTimer.Freq, Color.Cyan, Color.Transparent, false);

                spriteBatch.DrawString(Fonts.Font, velocity.ToString(), new Vector2(Bounds.X, Bounds.Y - 15), Color.White);
            }
        }

        public override void Update(World mc, GameTime gameTime)
        {
            if (health <= 0)
            {
                isdead = true;
                return;
            }

            i++;
            if (i >= delay)
            {
                i = 0;
                frameX++;
                if (frameX >= maxFramesX)
                {
                    frameX = 0;
                }
            }

            MouseState mState = Mouse.GetState();
            KeyboardState Kstate = Keyboard.GetState();
            int time = gameTime.ElapsedGameTime.Milliseconds;
            Move(gameTime, Kstate, time);
            rot = Functions.getRotation(Functions.VectorDir(position, new Vector2(mState.X, mState.Y)));

            if (MaxShieldHealth > 0)
            {
                shieldTimer.Update();
                if (shieldTimer.State)
                {
                    shieldHealth += shieldRegen;
                    if (shieldHealth >= maxShieldHealth)
                        shieldHealth = maxShieldHealth;
                }
            }

            position += velocity;

            #region Animation-Update-Logic

            if (velocity.X < 0)
                this.frameY = 1;
            else if (velocity.X > 0)
            {
                this.frameY = 1;
                sEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                this.frameY = 0;
                sEffect = SpriteEffects.None;
            }

            if (velocity.Y > 0)
            {
                frameX = 0;
            }

            #endregion

            CheckBounds();

            #region Shooting1

            gunTimer.Update();
            if (Kstate.IsKeyDown(Keys.Space) && gunTimer.State)
            {
                gunTimer.State = false;
                switch (mainGunLevel)
                { 
                    case 0:
                        mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(0, -6f)));
                        break;
                    case 1:
                        mc.entities.Add(new Bullet(Textures.Bullet2, position + new Vector2(5, 0), 1, new Vector2(0.3f, -6f)));
                        mc.entities.Add(new Bullet(Textures.Bullet2, position + new Vector2(-5, 0), 1, new Vector2(-0.3f, -6f)));
                        break;
                    case 2:
                        mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(0, -6f)));
                        mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(-1f, -6f)));
                        mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(1f, -6f)));
                        break;
                    case 3:
                         mc.entities.Add(new Bullet(Textures.Bullet2, position + new Vector2(5, 0), 1, new Vector2(0, -6f)));
                         mc.entities.Add(new Bullet(Textures.Bullet2, position + new Vector2(-5, 0), 1, new Vector2(0, -6f)));
                         mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(-1f, -6f)));
                         mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(1f, -6f)));
                        break;
                    case 4:
                    default:
                        mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(0, -6f)));
                        mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(-0.5f, -6f)));
                        mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(0.5f, -6f)));
                        mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(-1f, -6f)));
                        mc.entities.Add(new Bullet(Textures.Bullet2, position, 1, new Vector2(1f, -6f)));
                        break;
                }
                Sounds.shoot.Play(0.5f,0.0f, 0.0f);
            }

            #endregion

            #region Shooting2
            //if (secGunLevel > 0)
            //{
            //    UpdateTimer(ref secShootValue, secShootFreq, ref secCanShoot);
            //    if (mState.LeftButton == ButtonState.Pressed && secCanShoot && Gui.StarFieldBounds.Contains(new Point(mState.X, mState.Y)))
            //    {
            //        secCanShoot = false;
            //        Vector2 dir = new Vector2(mState.X - position.X, mState.Y - position.Y);
            //        dir.Normalize();
            //        mc.entities.Add(new Bullet(Textures.Bullet1, position, 5, dir * 4.5f));
            //        Sounds.shoot1.Play(0.15f, 0.0f, 0.0f);
            //    }
            //}
            #endregion
        }

        protected override void CheckBounds()
        {
            if (Bounds.Left < PlayerBounds.Left)
                position.X = PlayerBounds.Left + (Bounds.Width / 2 * scale);
            if (Bounds.Top < PlayerBounds.Top)
                position.Y = PlayerBounds.Top + (Bounds.Height / 2 * scale);
            if (Bounds.Bottom > PlayerBounds.Bottom)
                position.Y = PlayerBounds.Bottom - (Bounds.Height / 2 * scale);
            if (Bounds.Right > PlayerBounds.Right)
                position.X = PlayerBounds.Right - (Bounds.Width / 2 * scale);
        }

        private void Move(GameTime gameTime, KeyboardState Kstate, int time)
        {
            float m = 1f + speedLevel * 0.5f;

            if (Kstate.IsKeyDown(InputKeys[2]))
                velocity.X = -moveSpeed * time * m;
            else if (Kstate.IsKeyDown(InputKeys[3]))
                velocity.X = moveSpeed * time * m;

            if (Kstate.IsKeyDown(InputKeys[0]))
                velocity.Y = -moveSpeed * time * 0.6f * m;
            else if (Kstate.IsKeyDown(InputKeys[1]))
                velocity.Y = moveSpeed * time * 0.6f * m;

            velocity *= friction;

            if (Math.Abs(velocity.X) < 10e-1)
                velocity.X = 0;
            if (Math.Abs(velocity.Y) < 10e-1)
                velocity.Y = 0;
        }

        public override void Hurt(float amount)
        {
            shieldTimer.State = false;
            shieldTimer.Value = 0;
            shieldHealth -= amount;
            if (shieldHealth < 0)
            {
                health += shieldHealth;
                shieldHealth = 0;
            }
        }
    }
}