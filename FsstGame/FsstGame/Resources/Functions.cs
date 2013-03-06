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
    public class Functions
    {
        public static float getRotation(Vector2 vector)
        {
            float f = (float)Math.Atan2(vector.Y, vector.X);
            if (f > 0)
                f -= MathHelper.TwoPi;
            return f;
        }

        public static Vector2 fromRotation(float rotation)
        {
            return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }

        public static void fillRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(Textures.Pixel, rectangle, color);
        }

        public static void drawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int width = 1)
        {
            spriteBatch.Draw(Textures.Pixel, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, width), color); // Oben
            spriteBatch.Draw(Textures.Pixel, new Rectangle(rectangle.X + rectangle.Width - width, rectangle.Y, width, rectangle.Height), color); // Rechts
            spriteBatch.Draw(Textures.Pixel, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - width, rectangle.Width, width), color); // Unten
            spriteBatch.Draw(Textures.Pixel, new Rectangle(rectangle.X, rectangle.Y, width, rectangle.Height), color); // Links
        }

        public static void drawHealthBar(SpriteBatch spriteBatch, Rectangle rectangle, float health, float maxHealth, Color color, Color backColor, bool drawIfFull)
        {
            if (!drawIfFull &&( health >= maxHealth || health <= 0))
                return;
            fillRectangle(spriteBatch, rectangle, backColor);
            fillRectangle(spriteBatch, new Rectangle(rectangle.X, rectangle.Y, (int)(rectangle.Width * (health / maxHealth)), rectangle.Height), color);
        }

        public static Color GetColorFromHealth(float health, float maxHealth)
        {
            if (health == 1 && maxHealth == 1)
                return Color.Green;

            if (health > (maxHealth - (maxHealth / 3.0f)))
                return Color.Green;
            if (health > (maxHealth - (maxHealth / 1.5f)))
                return Color.Orange;

            return Color.Red;
        }

        public static bool Intersect(Rectangle rec1, Color[] data1, Rectangle rec2, Color[] data2)
        {
            if (!rec1.Intersects(rec2))
                return false;

            int left = Math.Max(rec1.Left, rec2.Left);
            int right = Math.Min(rec1.Right, rec2.Right);

            int top = Math.Max(rec1.Top, rec2.Top);
            int bottom = Math.Min(rec1.Bottom, rec2.Bottom);

            for (int x = left; x < right; x++)
            {
                for (int y = top; y < bottom; y++)
                {
                    int i1 = (y - rec1.Top) * rec1.Width + (x - rec1.Left);
                    int i2 = (y - rec2.Top) * rec2.Width + (x - rec2.Left);
                    if (i1 >= data1.Length || i2 >= data2.Length)
                        continue;
                    Color c1 = data1[i1];
                    Color c2 = data2[i2];

                    if (c1.A != 0 && c2.A != 0)
                        return true;
                }
            }

            return false;
        }

        public static Vector2 VectorDir(Vector2 v1, Vector2 v2)
        {
            Vector2 dir = new Vector2(v2.X - v1.X, v2.Y - v1.Y);
            dir.Normalize();
            return dir;
        }

        public static Vector2 V2fromP(Point p)
        {
            return new Vector2(p.X, p.Y);
        }
    }
}