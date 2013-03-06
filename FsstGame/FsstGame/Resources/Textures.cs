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
    public class Textures
    {
        public static Texture2D Player;
        public static Texture2D Bullet;
        public static Texture2D Bullet1;
        public static Texture2D Bullet2;
        public static Texture2D Bullet3;
        public static Texture2D Background;       
        public static Texture2D Pixel;
        public static Texture2D Gui;
        public static Texture2D Asteroid1;
        public static Texture2D Asteroid2;
        public static Texture2D Asteroid3;
        public static Texture2D Explosion;
        public static Texture2D EnemyBullet;
        public static Texture2D Enemy;
        public static Texture2D Crystal;
        public static Texture2D Metal;
        public static Texture2D UpgradeMenu;
        public static Texture2D Explosion2;

        public static Texture2D Button1;
        public static Texture2D SpeedButton;
        public static Texture2D UpgradeMenuPart;

        public static void Initialize(ContentManager Content)
        {
            Player = Content.Load<Texture2D>("textures/entities/ship");
            Bullet = Content.Load<Texture2D>("textures/entities/bullet");
            Bullet1 = Content.Load<Texture2D>("textures/entities/bullet1");
            Bullet2 = Content.Load<Texture2D>("textures/entities/bullet2");
            Bullet3 = Content.Load<Texture2D>("textures/entities/bullet3");
            Background = Content.Load<Texture2D>("textures/gui/starfield");
            Pixel = Content.Load<Texture2D>("textures/pixel");
            Gui = Content.Load<Texture2D>("textures/gui/gui");
            Asteroid1 = Content.Load<Texture2D>("textures/entities/asteroid1");
            Asteroid2 = Content.Load<Texture2D>("textures/entities/asteroid2");
            Asteroid3 = Content.Load<Texture2D>("textures/entities/asteroid3");
            Explosion = Content.Load<Texture2D>("textures/entities/explosion");
            EnemyBullet = Content.Load<Texture2D>("textures/entities/enemybullet");
            Enemy = Content.Load<Texture2D>("textures/entities/enemy");
            Crystal = Content.Load<Texture2D>("textures/entities/crystal2");
            Metal = Content.Load<Texture2D>("textures/entities/Barren");
            UpgradeMenu = Content.Load<Texture2D>("textures/gui/upgrademenu");
            Explosion2 = Content.Load<Texture2D>("textures/entities/explosion2");

            Button1 = Content.Load<Texture2D>("textures/buttons/button1");
            SpeedButton = Content.Load<Texture2D>("textures/buttons/speedButton");
            UpgradeMenuPart = Content.Load<Texture2D>("textures/gui/upgrademenupart");
        }
    }
}