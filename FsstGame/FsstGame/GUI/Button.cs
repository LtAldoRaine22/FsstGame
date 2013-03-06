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
    public class Button : InterFace
    {
        public static int STATE_NOTHING = 0;
        public static int STATE_PRESSED = 1;
        public static int STATE_HOVER = 2;

        private int state; // 0 = nothing; 1 = pressed; 2 = hover;
        private int oldState;
        private string text;

        private Vector2 textOffset = new Vector2(8, 3);

        public bool Pressed
        {
            get { return state == STATE_PRESSED; }
        }

        public bool Hover
        {
            get { return state == STATE_HOVER; }
        }

        public bool Clicked
        {
            get;
            set;
        }

        public Button(Rectangle rectangle, Texture2D texture, string text)
            :base(rectangle, texture)
        {
            this.state = 0;
            this.text = text;
        }

        public Button(Rectangle rectangle, Texture2D texture, string text, Vector2 textOffset)
            : base(rectangle, texture)
        {
            this.state = 0;
            this.text = text;
            this.textOffset = textOffset;
        }

        public override void Update()
        {
            MouseState mState = Mouse.GetState();
            state = STATE_NOTHING;

            if (!enabled)
                return;
            if (Bounds.Contains(new Point(mState.X, mState.Y)))
                state = STATE_HOVER;
            
            if (state == STATE_HOVER && mState.LeftButton == ButtonState.Pressed)
                state = STATE_PRESSED;

            if ((state == STATE_NOTHING || state == STATE_HOVER) && oldState == STATE_PRESSED)
                Clicked = true;
            else
                Clicked = false;
            
            oldMState = mState;
            oldState = state;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (enabled)
                spriteBatch.Draw(texture, Functions.V2fromP(rectangle.Location), SRecFromState(), Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);
            else
                spriteBatch.Draw(texture, Functions.V2fromP(rectangle.Location), SRecFromState(), Color.DimGray, 0f, Origin, 1f, SpriteEffects.None, 0);
            spriteBatch.DrawString(Fonts.ButtonFont, text, Functions.V2fromP(rectangle.Location) + textOffset, Color.Black, 0f, Origin, 1f, SpriteEffects.None, 0);
        }

        private Rectangle SRecFromState()
        {
            return new Rectangle(state * rectangle.Width, 0, rectangle.Width, rectangle.Height);
        }
    }
}