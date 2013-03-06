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
    public class World
    {
        public const int Width = 1280;
        public const int Height = 720;

        private GraphicsDeviceManager graphics;
        private bool showDebug = false;
        private KeyboardState oldState;
        public List<Entity> entities;
        private bool paused = false;
        private UpgradeMenu ugm;
        private Player player;
        private Random random;
        private Gui gui;
        private double time;

        private Timer asteroidTimer;
        private float asteroidSpawnChance = 0.75f;
        private Timer enemyTimer;
        private float enemySpawnChance = 0.5f;
        
        #region Properties

        public Player Player
        {
            get { return player; }
        }

        public bool Paused
        {
            get { return paused; }
            set { paused = value; }
        }

        #endregion

        public World(ContentManager Content, GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;

            InitWindow(Width, Height, false);
            Inititialize(Content);
        }

        private void Inititialize(ContentManager Content)
        {
            random = new Random();
            Textures.Initialize(Content);
            Sounds.Initialize(Content);
            Fonts.Initialize(Content);
            entities = new List<Entity>();
            asteroidTimer = new Timer(0, 75, false);
            enemyTimer = new Timer(0, 65, false);

            gui = new Gui();
            player = new Player(Textures.Player, new Vector2(Gui.StarFieldBounds.X + Gui.StarFieldBounds.Width / 2, Gui.StarFieldBounds.Y + Gui.StarFieldBounds.Height / 2 + 100), 50);
            ugm = new UpgradeMenu(this, new Rectangle(Gui.StarFieldBounds.X + Gui.StarFieldBounds.Width / 2, Gui.StarFieldBounds.Y + Gui.StarFieldBounds.Height / 2, Textures.UpgradeMenu.Width, Textures.UpgradeMenu.Height), Textures.UpgradeMenu);
        }

        private void InitWindow(int width, int height, bool fullscreen)
        {
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.IsFullScreen = fullscreen;
            graphics.ApplyChanges();
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            #region KeyInput

            if (state.IsKeyDown(Keys.F1) && oldState.IsKeyUp(Keys.F1))
                showDebug = !showDebug;

            if (state.IsKeyDown(Keys.E) && oldState.IsKeyUp(Keys.E))
                paused = !paused;

            if (state.IsKeyDown(Keys.H))
                player.Health = player.MaxHealth;

            if (state.IsKeyDown(Keys.G) && oldState.IsKeyUp(Keys.G))
                entities.Add(new Animation(Textures.Explosion2, new Vector2(200, 200), 1f, Vector2.Zero, 3, 94, 100, 5, 1));

            ugm.Update();

            #endregion

            #region Update-Stuff

            if (!paused)
            {
                player.Update(this, gameTime);

                #region Player Death

                if (player.isDead)
                {
                    paused = true;
                    player.Health = player.MaxHealth;
                    player.isDead = false;
                    entities.Clear();
                    ugm.CrystalAmount = 0;
                    ugm.EnergyAmount = 0;
                    ugm.MetalAmount = 0;

                    player.speedLevel = 0;
                    player.mainGunFreq = 0;
                    player.mainGunLevel = 0;
                }

                #endregion

                #region entities Update

                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].Update(this, gameTime);

                    #region isDead & Explosion
                    if (entities[i].isDead)
                    {
                        if (entities[i].CausesExplosion)
                            entities.Add(new Explosion(entities[i].Position));

                        if (entities[i] is Asteroid)
                        {
                            float f = (float)random.NextDouble();
                            if (f < 0.1)
                                entities.Add(new Item(Item.CRYSTAL, entities[i].Position, new Vector2(0, 0.95f)));
                        }
                        if (entities[i] is Enemy)
                        {
                            float f = (float)random.NextDouble();
                            if (f < 0.25)
                                entities.Add(new Item(Item.METAL, entities[i].Position, new Vector2(0, 0.95f)));
                        }

                        entities.RemoveAt(i);
                        i--;
                        if (i < 0)
                            i = 0;
                        if (entities.Count == 0)
                            return;
                    }
                    #endregion

                    #region (Asteroids & EnemyBullet & Enemy) collide with Player
                    if (entities[i] is Asteroid || entities[i] is EnemyBullet || entities[i] is Enemy)
                    {
                        if (Functions.Intersect(player.Bounds, player.ColorData, entities[i].Bounds, entities[i].ColorData))
                        {
                            player.Hurt(entities[i].Health);
                            entities[i].Hurt(entities[i].Health);
                            Sounds.hit.Play(0.5f, 0.0f, 0.0f);
                        }
                    }
                    #endregion

                    #region EnemyBullet with Asteroid
                    if (entities[i] is EnemyBullet)
                    {
                        for (int j = 0; j < entities.Count; j++)
                        {
                            if (j != i && entities[j] is Asteroid)
                            { 
                                if(Functions.Intersect(entities[i].Bounds, entities[i].ColorData, entities[j].Bounds, entities[j].ColorData))
                                {
                                    entities[i].isDead = true;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion

                    #region Items

                    if (entities[i] is Item)
                    {
                        if (Functions.Intersect(player.Bounds, player.ColorData, entities[i].Bounds, entities[i].ColorData))
                        {
                            switch (((Item)entities[i]).itemType)
                            { 
                                case Item.CRYSTAL:
                                    ugm.CrystalAmount++;
                                    break;
                                case Item.METAL:
                                    ugm.MetalAmount++;
                                    break;
                            }
                            entities[i].isDead = true;
                            Sounds.pickup.Play(0.5f, 0.0f, 0.0f);
                        }
                    }

                    #endregion

                    #region Player Bullet
                    if (entities[i] is Bullet || entities[i] is GuidedBullet)
                    {
                        for (int j = 0; j < entities.Count; j++)
                        {
                            if (i == j || entities[j] is Bullet || entities[j] is Explosion || entities[j] is Player || entities[j] is Item || entities[j] is GuidedBullet || entities[j] is EnemyBullet)
                                continue;

                            if (Functions.Intersect(entities[i].Bounds, entities[i].ColorData, entities[j].Bounds, entities[j].ColorData))
                            {
                                entities[j].Hurt(entities[i].Health);
                                entities[i].Hurt(entities[i].Health);
                                break;
                            }
                        }
                    }
                    #endregion
                }

                #endregion

                #region Asteroid Spawning

                asteroidTimer.Update();
                if (asteroidTimer.State)
                {
                    asteroidTimer.State = false;
                    if (random.NextDouble() < asteroidSpawnChance)
                    {
                        int size = random.Next(3);

                        int x = random.Next(Gui.StarFieldBounds.Left + 100, Gui.StarFieldBounds.Right - 100);
                        entities.Add(new Asteroid(size, new Vector2(x, -75), new Vector2(MathHelper.Clamp((float)random.NextDouble() * 2 - 1, -0.25f, 0.25f), (float)random.NextDouble() + 0.15f)));
                    }
                }

                #endregion

                #region EnemySpawing

                enemyTimer.Update();
                if (enemyTimer.State)
                {
                    enemyTimer.State = false;
                    if (random.NextDouble() < enemySpawnChance)
                    {
                        int x = random.Next(Gui.StarFieldBounds.Left + Textures.Enemy.Width, Gui.StarFieldBounds.Right - Textures.Enemy.Width);
                        entities.Add(new Enemy(Textures.Enemy, new Vector2(x, -50), 5, new Vector2(0, (float)random.NextDouble() + 0.15f), 0));
                    }
                }

                #endregion
                
                time += gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            }
            #endregion

            oldState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            gui.DrawBackground(spriteBatch);

            for (int i = 0; i < entities.Count; i++)
                entities[i].Draw(spriteBatch, showDebug);

            player.Draw(spriteBatch, showDebug);
            gui.DrawGui(spriteBatch);

            #region Player Healthbar
            Functions.drawHealthBar(spriteBatch, new Rectangle(Gui.ScoreBoardBounds.X + (int)Fonts.Font.MeasureString("Health: ").X + 2, Gui.ScoreBoardBounds.Y + 60, 65, 14), Player.Health, Player.MaxHealth, Functions.GetColorFromHealth(Player.Health, Player.MaxHealth), Color.DarkGray, true);
            spriteBatch.DrawString(Fonts.Font, "Health: " + Player.Health + " / " + Player.MaxHealth, new Vector2(Gui.ScoreBoardBounds.X + 10, Gui.ScoreBoardBounds.Y + 58), Color.White);
            #endregion

            #region showDebug
            if (showDebug)
            {
                spriteBatch.DrawString(Fonts.Font, "Debug Screen", new Vector2(Gui.ScoreBoardBounds.X + 10, Gui.ScoreBoardBounds.Y + 10), Color.White);
                spriteBatch.DrawString(Fonts.Font, "entities.Count: " + entities.Count.ToString(), new Vector2(Gui.ScoreBoardBounds.X + 10, Gui.ScoreBoardBounds.Y + 25), Color.White);
                //spriteBatch.DrawString(Fonts.Font, asteroidSpawnValue + " / " + asteroidSpawnFreq + " - " + asteroidSpawnChance, new Vector2(Gui.ScoreBoardBounds.X + 10, Gui.ScoreBoardBounds.Y + 40), Color.White);
                spriteBatch.DrawString(Fonts.Font, "Time: " + time, new Vector2(Gui.ScoreBoardBounds.X + 150, Gui.ScoreBoardBounds.Y + 25), Color.White);
            }
            #endregion

            ugm.Draw(spriteBatch);
        }
    }
}