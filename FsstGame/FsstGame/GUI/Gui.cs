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
    public class Gui
    {
        private Vector2 backGroundOffset;
        
        public static Rectangle StarFieldBounds
        {
            get { return new Rectangle(14, 14, 942, 692); }
        }

        public static Rectangle ScoreBoardBounds
        {
            get { return new Rectangle(984, 11, 284, 229); }
        }
        
        public Gui()
        {
            backGroundOffset = new Vector2(-850, 14);  
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.Background, backGroundOffset, Color.White);
        }

        public void DrawGui(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.Gui, new Vector2(0, 0), Color.White);
        }
    }
}