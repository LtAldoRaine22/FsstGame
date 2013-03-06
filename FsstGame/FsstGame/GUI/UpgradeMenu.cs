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
    public class UpgradeMenu : InterFace
    {
        public int CrystalAmount = 5;
        public int MetalAmount = 5;
        public int EnergyAmount = 0;

        private World world;

        private Button pause;
        private Button upgrade;
        private Button UpgradeSpeed;

        public UpgradeMenu(World world, Rectangle rectangle, Texture2D texture)
            : base(rectangle, texture)
        {
            this.world = world;
            Initialize();
        }

        private void Initialize()
        {
            pause = new Button(new Rectangle(this.Bounds.X + 200, this.Bounds.Y + 300, 50, 20), Textures.Button1, "Close");
            upgrade = new Button(new Rectangle(this.Bounds.X + 200, this.Bounds.Y + 200, 50, 20), Textures.Button1, "Upgrade", new Vector2(3,3));
            UpgradeSpeed = new Button(new Rectangle(this.Bounds.X + 200, this.Bounds.Y + 100, 32, 32), Textures.SpeedButton, "");
        }

        public override void Update()
        {
            if (world.Paused)
            {
                pause.Update();
                upgrade.Update();
                UpgradeSpeed.Update();

                if (pause.Clicked)
                    world.Paused = false;
                if (upgrade.Clicked)
                {
                    world.Player.mainGunFreq++;
                    world.Player.mainGunLevel++;
                    world.Player.MaxShieldHealth++;
                    world.Player.ShieldRegenLevel++;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawResource(spriteBatch, Textures.Crystal, CrystalAmount, 20, 24, 96);
            DrawResource(spriteBatch, Textures.Metal, MetalAmount, 20, 24, 125);
            
            if (world.Paused)
            {
                spriteBatch.Draw(texture, Functions.V2fromP(rectangle.Location), null, Color.White, 0f, Origin, 1f, SpriteEffects.None, 0);
                pause.Draw(spriteBatch);
                upgrade.Draw(spriteBatch);
                UpgradeSpeed.Draw(spriteBatch);
            }
        }

        private void DrawResource(SpriteBatch spriteBatch, Texture2D t, int amount, int max, int xOffset, int yOffset)
        {
            if (amount > max)
            {
                spriteBatch.Draw(t, new Vector2(Gui.ScoreBoardBounds.X + xOffset, Gui.ScoreBoardBounds.Y + yOffset), null, Color.White, 0f, new Vector2(t.Width / 2, t.Height / 2), 1f, SpriteEffects.None, 0);
                spriteBatch.DrawString(Fonts.Font, amount.ToString(), new Vector2(Gui.ScoreBoardBounds.X + xOffset + t.Width, Gui.ScoreBoardBounds.Y + yOffset - t.Height / 3), Color.White);
            }
            else
                for(int i = 0; i < amount; i++)
                    spriteBatch.Draw(t, new Vector2(Gui.ScoreBoardBounds.X + xOffset + i * t.Width / 3*2, Gui.ScoreBoardBounds.Y + yOffset), null, Color.White, 0f, new Vector2(t.Width / 2, t.Height / 2), 1f, SpriteEffects.None, 0);
        }
    }
}