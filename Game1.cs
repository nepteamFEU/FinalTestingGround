using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Reflection;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace FinalTestingGround
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private PlayerStats pstats;
        private Texture2D menuBG, BG, rabbitLifebar, squirrelLifebar;
        private const string PATH = "stats.json";
        int speed;
        Platform platform1;
        Platform platform2;
        Ball obstacle1, obstacle2, obstacle3;
        Lifebar p1lifebar, p2lifebar;
        Score p1score, p2score, rounds;
        int WCBW, WCBH;
        List<projectile> projectiles;
        Texture2D chargeSprite;
        Texture2D ammoSprite;
        SpriteFont Text;
        Song bgm;
        SoundEffect hitsfx, winsfx, shootsfx;
        Selection start, cont, exit;
        bool StartGame, visibletext;



        string winner = "";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize()
        {
            WCBW = Window.ClientBounds.Width;
            WCBH = Window.ClientBounds.Height;
            speed = 9; // Initialize speed before creating platform instances

            projectiles = new List<projectile>();

            //still need to fix the collission
            obstacle1 = new Ball(Content.Load<Texture2D>("obstacle2"), Color.White, new Rectangle(500, 100, 200, 200),
                5, 5, WCBW, WCBH, true, true);
            obstacle2 = new Ball(Content.Load<Texture2D>("obstacle1"), Color.White, new Rectangle(200, 100, 200, 200),
                5, 5, WCBW, WCBH, true, true);
            obstacle3 = new Ball(Content.Load<Texture2D>("obstacle2"), Color.White, new Rectangle(150, 50, 200, 200),
                5, 5, WCBW, WCBH, true, true);



            platform1 = new Platform(Content.Load<Texture2D>("bunny_R"),
                new Rectangle(0, 150, 150, 150), Color.White, 0,
                WCBH, 0, WCBW, speed, 2, 5, 1, 0, 30, 0, 10, projectiles,
                Content.Load<Texture2D>("charge1"), Content.Load<Texture2D>("bullet_healthbar"), new Vector2(10, 10), new Vector2(138, -45));

            platform2 = new Platform(Content.Load<Texture2D>("squirrel_L"),
                new Rectangle(WCBW - 100, WCBH - 350, 150, 150), Color.White,
                0, WCBH, 0, WCBW, speed, 2, 5, 1, 0, 30, 0, 10, projectiles,
                Content.Load<Texture2D>("charge4"), Content.Load<Texture2D>("bullet_healthbar"), new Vector2(WCBW - 40, WCBH - 40), new Vector2(WCBW - 344, WCBH - 525));

            p1lifebar = new P1Lifebar(Content.Load<Texture2D>("box"),
                new Rectangle(200, 10, 100, 30), Color.Green, 120, 40);
            p2lifebar = new P2Lifebar(Content.Load<Texture2D>("box"),
                new Rectangle(500, 10, 100, 30), Color.Green, 120, 40);

            p1score = new P1Score(Color.White, 0);
            p2score = new P2Score(Color.White, 0);
            rounds = new Rounds(Color.White, 1);

            start = new Start(false, new Rectangle(50, 250, 300, 30), Content.Load<Texture2D>("box"), Color.White);
            cont = new Continue(false, new Rectangle(50, 300, 300, 30), Content.Load<Texture2D>("box"), Color.White);
            exit = new Exit(false, new Rectangle(50, 350, 300, 30), Content.Load<Texture2D>("box"), Color.White);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Text = Content.Load<SpriteFont>("File");
            hitsfx = Content.Load<SoundEffect> ("hit_sfx");
            shootsfx = Content.Load<SoundEffect>("shoot_sfx");
            winsfx = Content.Load<SoundEffect>("win");
            bgm = Content.Load<Song>("bgm");
            menuBG = Content.Load<Texture2D>("menu_bg");
            BG = Content.Load<Texture2D>("background");
            rabbitLifebar = Content.Load<Texture2D>("bunny_healthbar1");
            squirrelLifebar = Content.Load<Texture2D>("squirrel_healthbar");

            MediaPlayer.Play(bgm);

        }

        protected override void Update(GameTime gameTime)
        {

           
            
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // ball.ballMovement();

            // Check collision with platforms
            obstacle1.ballcollision(platform1.PlatRec);
            obstacle2.ballcollision(platform2.PlatRec);

            platform1.PlatformMovement(Keys.W, Keys.S, Keys.A, Keys.D, 1);
            platform2.PlatformMovement(Keys.I, Keys.K, Keys.J, Keys.L, 2);

            platform1.ShootControl(Keys.F, 1);
            platform1.UpdateProjectiles();

            platform2.ShootControl(Keys.H, 2);
            platform2.UpdateProjectiles();

            // Get the current and previous mouse state
            MouseState mouseState = Mouse.GetState();
            MouseState prevmouseState = Mouse.GetState();
            prevmouseState = mouseState;

            // Check for mouse button clicks
            // still unreliable and tends to flicker
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (start.SelectionRectangle.Contains(mouseState.X, mouseState.Y))
                {
                    StartGame = true;
                    pstats = new PlayerStats()
                    {
                        p1score = p1score.ScoreCount,
                        p2score = p2score.ScoreCount,
                        p1life = p1lifebar.LifebarWidth,
                        p2life = p2lifebar.LifebarWidth,
                        round = rounds.ScoreCount,
                        winner = winner

                    };

                    Save(pstats);
                    visibletext = !visibletext;

                }
                else if (cont.SelectionRectangle.Contains(mouseState.X, mouseState.Y))
                {
                    StartGame = true; //create method that continues or reads a save file
                    pstats = Load();
                    //updates the scores, lifebar, 
                    Trace.WriteLine($"{p1score.ScoreCount = pstats.p1score} {p2score.ScoreCount = pstats.p2score} " +
                        $"{p1lifebar.LifebarWidth = pstats.p1life} {p2lifebar.LifebarWidth = pstats.p2life} {rounds.ScoreCount = pstats.round} {winner = pstats.winner}");

                    visibletext = !visibletext;
                }
                else if (exit.SelectionRectangle.Contains(mouseState.X, mouseState.Y))
                {
                    Exit();

                }

            }
            /*
              if (obstacle2.BallRec.Intersects(platform1.PlatRec)) //player 2 scores 
              {

                  if (!obstacle1.BallRec.Intersects(platform2.PlatRec))
                  {
                      p1lifebar.LifebarWidth -= 15;
                      p1lifebar.LifebarNumber -= 5;

                      pstats = new PlayerStats()
                      {
                          p1score = p1score.ScoreCount,
                          p2score = p2score.ScoreCount,
                          p1life = p1lifebar.LifebarWidth,
                          p2life = p2lifebar.LifebarWidth,
                          round = rounds.ScoreCount,
                          winner = winner

                      };



                      Save(pstats);

                      if (p1lifebar.LifebarWidth <= 0)
                      {
                          p1lifebar.lifebarReset();
                          p2lifebar.lifebarReset();
                          p2score.Updatescore();
                          rounds.ScoreCount += 1;

                          pstats = new PlayerStats()
                          {
                              p1score = p1score.ScoreCount,
                              p2score = p2score.ScoreCount,
                              p1life = p1lifebar.LifebarWidth,
                              p2life = p2lifebar.LifebarWidth,
                              round = rounds.ScoreCount,
                              winner = winner

                          };

                          Save(pstats);

                          if (p2score.ScoreCount == 2)
                          {

                              winner = "Player 2 Wins!";
                              rounds.CountReset();
                              rounds.ScoreCount += 1;
                              p2score.CountReset();
                              p1score.CountReset();

                              pstats = new PlayerStats()
                              {
                                  p1score = p1score.ScoreCount,
                                  p2score = p2score.ScoreCount,
                                  p1life = p1lifebar.LifebarWidth,
                                  p2life = p2lifebar.LifebarWidth,
                                  round = rounds.ScoreCount,
                                  winner = winner

                              };

                              Save(pstats);

                          }
                      }
                  }

              }

              else if (obstacle1.BallRec.Intersects(platform2.PlatRec)) //player 1 scores
              {
                  if (!obstacle2.BallRec.Intersects(platform1.PlatRec))
                  {
                      p2lifebar.LifebarWidth -= 15;
                      p2lifebar.LifebarNumber -= 5;

                      pstats = new PlayerStats()
                      {
                          p1score = p1score.ScoreCount,
                          p2score = p2score.ScoreCount,
                          p1life = p1lifebar.LifebarWidth,
                          p2life = p2lifebar.LifebarWidth,
                          round = rounds.ScoreCount,
                          winner = winner

                      };



                      Save(pstats);

                      if (p2lifebar.LifebarWidth <= 0)
                      {
                          p1lifebar.lifebarReset();
                          p2lifebar.lifebarReset();
                          p1score.Updatescore(); //adds round score +1
                          rounds.ScoreCount += 1;

                          pstats = new PlayerStats()
                          {
                              p1score = p1score.ScoreCount,
                              p2score = p2score.ScoreCount,
                              p1life = p1lifebar.LifebarWidth,
                              p2life = p2lifebar.LifebarWidth,
                              round = rounds.ScoreCount,
                              winner = winner

                          };


                          Save(pstats);


                          if (p1score.ScoreCount == 2) //Player wins 1 round of a score of 3
                          {

                                 = "Player 1 Wins!";
                              rounds.CountReset();
                              rounds.ScoreCount += 1;
                              p2score.CountReset();
                              p1score.CountReset();

                              pstats = new PlayerStats()
                              {
                                  p1score = p1score.ScoreCount,
                                  p2score = p2score.ScoreCount,
                                  p1life = p1lifebar.LifebarWidth,
                                  p2life = p2lifebar.LifebarWidth,
                                  round = rounds.ScoreCount,
                                  winner = winner

                              };


                              Save(pstats);
                          }

                      }
                  }
              }
              */

            // bullets projectiles
            foreach (var projectile in projectiles)
            {
                platform1.damageCheck(1, projectile, p1lifebar, p2lifebar, p1score, p2score, rounds, pstats); //pstats prone to failure
                platform2.damageCheck(2, projectile, p2lifebar, p1lifebar, p2score, p1score, rounds, pstats);

              //  MediaPlayer.Play(shootsfx);
            }

            if (p1score.ScoreCount == 2)
            {
                Save((platform1.WinnerCheck(1, p1lifebar, p2lifebar, p1score, p2score, rounds, "", pstats)));
                winner = "Player 1 Wins!";

                winsfx.Play();
            }

            if (p2score.ScoreCount == 2)
            {
                Save((platform2.WinnerCheck(2, p2lifebar, p1lifebar, p2score, p1score, rounds, "", pstats)));
                winner = "Player 2 Wins!";

                winsfx.Play();
            }


            base.Update(gameTime);
        }

        private void Save(PlayerStats stats)
        {
            string serializedText = JsonSerializer.Serialize(stats);
            Trace.WriteLine(serializedText);
            File.WriteAllText(PATH, serializedText);

        }

        private PlayerStats Load()
        {
            var fileContents = File.ReadAllText(PATH);
            return JsonSerializer.Deserialize<PlayerStats>(fileContents);
        }




        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();



            if (!visibletext)
            {

                _spriteBatch.Draw(menuBG, new Vector2(0, 0), Color.White);

                _spriteBatch.DrawString(Text, " START", new Vector2(30, 250), Color.White);
                _spriteBatch.DrawString(Text, " CONTINUE", new Vector2(30, 300), Color.White);
                _spriteBatch.DrawString(Text, " EXIT", new Vector2(30, 350), Color.White);
                // spriteBatch.Draw(start.SelectionTexture, start.SelectionRectangle, start.SelectionColor);
                // spriteBatch.Draw(cont.SelectionTexture, cont.SelectionRectangle, cont.SelectionColor);
                // spriteBatch.Draw(exit.SelectionTexture, exit.SelectionRectangle, exit.SelectionColor);

            }

            if (StartGame)
            {

                _spriteBatch.Draw(BG, new Vector2(0, 0), Color.White);

                _spriteBatch.Draw(platform1.PlatText, platform1.PlatRec, platform1.PlatColor);
                _spriteBatch.Draw(platform2.PlatText, platform2.PlatRec, platform2.PlatColor);
                _spriteBatch.Draw(obstacle1.BallTexture, obstacle1.BallRec, obstacle1.BallColor);
                _spriteBatch.Draw(obstacle2.BallTexture, obstacle2.BallRec, obstacle2.BallColor);
                _spriteBatch.Draw(obstacle3.BallTexture, obstacle3.BallRec, obstacle3.BallColor);


                platform1.DrawProjectiles(_spriteBatch, Content.Load<Texture2D>("bullet_L")); //doesn't show any bullets???
                platform2.DrawProjectiles(_spriteBatch, Content.Load<Texture2D>("bullet_R"));

                platform1.DrawUI(_spriteBatch, 1);
                platform2.DrawUI(_spriteBatch, 2);

                _spriteBatch.Draw(rabbitLifebar, new Vector2(110, -79), Color.White);
                _spriteBatch.Draw(squirrelLifebar, new Vector2(470, -79), Color.White);

                _spriteBatch.Draw(p1lifebar.LifebarTexture, p1lifebar.LifebarRectangle, p1lifebar.LifebarColor);
                _spriteBatch.Draw(p2lifebar.LifebarTexture, p2lifebar.LifebarRectangle, p2lifebar.LifebarColor);

                _spriteBatch.DrawString(Text, p1score.ScoreCount.ToString(), new Vector2(205, 50), Color.Black);
                _spriteBatch.DrawString(Text, p2score.ScoreCount.ToString(), new Vector2(606, 50), Color.Black);
                _spriteBatch.DrawString(Text, "   ROUND", new Vector2(350, 10), Color.Black);
                _spriteBatch.DrawString(Text, rounds.ScoreCount.ToString(), new Vector2(400, 40), Color.Black);

                if (!string.IsNullOrEmpty(winner)) //checks winner
                {
                    _spriteBatch.DrawString(Text, winner.ToString(), new Vector2(320, 80), Color.Black);

                }

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}