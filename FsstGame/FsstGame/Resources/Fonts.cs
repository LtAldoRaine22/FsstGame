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
    public class Fonts
    {
        public static SpriteFont Font;
        public static SpriteFont BigFont;
        public static SpriteFont ButtonFont;

        public static void Initialize(ContentManager content)
        {
            Font = content.Load<SpriteFont>("fonts/Font");
            BigFont = content.Load<SpriteFont>("fonts/BigFont");
            ButtonFont = content.Load<SpriteFont>("fonts/ButtonFont");
        }
    }
}
