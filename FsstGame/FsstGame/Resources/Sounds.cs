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
    public class Sounds
    {
        public static SoundEffect shoot;
        public static SoundEffect shoot1;
        public static SoundEffect explosion;
        public static SoundEffect pickup;
        public static SoundEffect hit;

        public static void Initialize(ContentManager content)
        {
            shoot = content.Load<SoundEffect>("sounds/shoot2");
            shoot1 = content.Load<SoundEffect>("sounds/shoot");
            explosion = content.Load<SoundEffect>("sounds/explosion");
            pickup = content.Load<SoundEffect>("sounds/pickup");
            hit = content.Load<SoundEffect>("sounds/hit");
        }
    }
}