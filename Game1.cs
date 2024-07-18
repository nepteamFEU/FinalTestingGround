using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace FinalTestingGround
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        int speed;
        Platform platform1;
        Platform platform2;
        Ball ball;
        Lifebar p1lifebar, p2lifebar;
        Score p1score, p2score, rounds;
        int WCBW, WCBH;
        List<projectile> projectiles;
        Texture2D chargeSprite;
        Texture2D ammoSprite;
        SpriteFont Text;
        Selection start, cont, exit;
        bool TEST, visibletext;

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

            ball = new Ball(Content.Load<Texture2D>("Untitled"), Color.Red, new Rectangle(200, 200, 50, 50),
                5, 5, WCBW, WCBH, true, true);

            platform1 = new Platform(Content.Load<Texture2D>("player1"),
                new Rectangle(0, 0, 50, 150), Color.Red, 0,
                WCBH, 0, WCBW, speed, 2, 5, 1, 0, 30, 0, 10, projectiles,
                Content.Load<Texture2D>("charge1"), Content.Load<Texture2D>("bullet1"), new Vector2(10, 10), new Vector2(10, 30));

            platform2 = new Platform(Content.Load<Texture2D>("player2"),
                new Rectangle(WCBW - 50, WCBH - 150, 50, 150), Color.Blue,
                0, WCBH, 0, WCBW, speed, 2, 5, 1, 0, 30, 0, 10, projectiles,
                Content.Load<Texture2D>("charge4"), Content.Load<Texture2D>("bullet1"), new Vector2(WCBW - 40, WCBH - 40), new Vector2(WCBW - 50, WCBH - 50));

            p1lifebar = new P1Lifebar(Content.Load<Texture2D>("box"),
                new Rectangle(200, 10, 100, 30), Color.White, 120, 40);
            p2lifebar = new P2Lifebar(Content.Load<Texture2D>("box"), 
                new Rectangle(500, 10, 100, 30), Color.White, 120, 40);

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ball.ballMovement();

            // Check collision with platforms
            ball.ballcollision(platform1.PlatRec);
            ball.ballcollision(platform2.PlatRec);

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
                    TEST = true; //create method that creates a save file
                    visibletext = !visibletext;

                }
                else if (cont.SelectionRectangle.Contains(mouseState.X, mouseState.Y))
                {
                    TEST = true; //create method that continues or reads a save file
                    visibletext = !visibletext;
                }
                else if (exit.SelectionRectangle.Contains(mouseState.X, mouseState.Y))
                {
                    Exit();

                }

            }


            //loop for lifebar and scoring
            //make a conditional statement where the bullet interesects to the platform and causes damage
            //Unable to integrate the bullets to this loop for the lifebar and score. Asking for help on this one...
            /*   if (bullets.DamageCheck.Intersects(platform1.PlatRec)) //player 2 scores 
                {

                        p1lifebar.LifebarWidth -= 15;
                        p1lifebar.LifebarNumber -= 5;

                        if (p1lifebar.LifebarWidth <= 0)
                        {
                            p1lifebar.lifebarReset();
                            p2lifebar.lifebarReset();
                            p2score.Updatescore();
                            rounds.ScoreCount += 1;

                            if (p2score.ScoreCount == 2)
                            {
                              
                                winner = "Player 2 Wins!";
                                rounds.CountReset();
                                rounds.ScoreCount += 1;
                                p2score.CountReset();
                                p1score.CountReset();

                            }
                        }

                }

                else if (bullets.DamageCheck.Intersects(platform1.PlatRec)) //player 1 scores
                {
                   
                        p2lifebar.LifebarWidth -= 15;
                        p2lifebar.LifebarNumber -= 5;

                        if (p2lifebar.LifebarWidth <= 0)
                        {
                            p1lifebar.lifebarReset();
                            p2lifebar.lifebarReset();
                            p1score.Updatescore(); //adds round score +1
                            rounds.ScoreCount += 1;

                            if (p1score.ScoreCount == 2) //Player wins 1 round of a score of 3
                            {
                              
                                winner = "Player 1 Wins!";
                                rounds.CountReset();
                            }

                        }
                    
                }*/

            foreach (var projectile in projectiles)
            {
                platform1.damageCheck(projectile);
                platform2.damageCheck(projectile);
            } 


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (!visibletext)
            {

                _spriteBatch.DrawString(Text, "         START", new Vector2(50, 250), Color.White);
                _spriteBatch.DrawString(Text, "         CONTINUE", new Vector2(50, 300), Color.White);
                _spriteBatch.DrawString(Text, "         EXIT", new Vector2(50, 350), Color.White);
                // spriteBatch.Draw(start.SelectionTexture, start.SelectionRectangle, start.SelectionColor);
                // spriteBatch.Draw(cont.SelectionTexture, cont.SelectionRectangle, cont.SelectionColor);
                // spriteBatch.Draw(exit.SelectionTexture, exit.SelectionRectangle, exit.SelectionColor);

            }

            if (TEST)
            {

                _spriteBatch.Draw(platform1.PlatText, platform1.PlatRec, platform1.PlatColor);
                _spriteBatch.Draw(platform2.PlatText, platform2.PlatRec, platform2.PlatColor);
                _spriteBatch.Draw(ball.BallTexture, ball.BallRec, ball.BallColor);

                platform1.DrawProjectiles(_spriteBatch, Content.Load<Texture2D>("projectile"));
                platform2.DrawProjectiles(_spriteBatch, Content.Load<Texture2D>("projectile"));

                platform1.DrawUI(_spriteBatch, 1);
                platform2.DrawUI(_spriteBatch, 2);

                _spriteBatch.Draw(p1lifebar.LifebarTexture, p1lifebar.LifebarRectangle, p1lifebar.LifebarColor);
                _spriteBatch.Draw(p2lifebar.LifebarTexture, p2lifebar.LifebarRectangle, p2lifebar.LifebarColor);

                _spriteBatch.DrawString(Text, p1score.ScoreCount.ToString(), new Vector2(200, 50), Color.Black);
                _spriteBatch.DrawString(Text, p2score.ScoreCount.ToString(), new Vector2(500, 50), Color.Black);
                _spriteBatch.DrawString(Text, "   ROUND", new Vector2(350, 10), Color.Black);
                _spriteBatch.DrawString(Text, rounds.ScoreCount.ToString(), new Vector2(400, 40), Color.Black);

                if (!string.IsNullOrEmpty(winner)) //checks winner
                {
                    _spriteBatch.DrawString(Text, winner.ToString(), new Vector2(350, 80), Color.Black);


                }

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
