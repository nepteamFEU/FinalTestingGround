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
        Tree obstacle4;
        Lifebar p1lifebar, p2lifebar, TreeLifebar;
        Score p1score, p2score, rounds;
        int WCBW, WCBH, treeHP;
        List<projectile> projectiles, projectilesToRemove;
        Texture2D chargeSprite;
        Texture2D ammoSprite;
        SpriteFont Text;
        Song bgm;
        List<SoundEffect> soundEffects;
        SoundEffect hitsfx, winsfx, shootsfx, treefallsfx;
        Selection start, cont, exit;
        bool StartGame, visibletext;
        


        string winner = "";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            // soundEffects = new List<SoundEffect>();
            

        }

        protected override void Initialize()
        {
            Window.Title = "Prey";
            WCBW = Window.ClientBounds.Width;
            WCBH = Window.ClientBounds.Height;
            speed = 9; // Initialize speed before creating platform instances

            

            projectiles = new List<projectile>();
            projectilesToRemove = new List<projectile>();

            //still need to fix the collission
            obstacle1 = new Ball(Content.Load<Texture2D>("obstacle2"), Color.White, new Rectangle(500, 200, 91, 57),
                5, 5, WCBW, WCBH, true, true);
            obstacle2 = new Ball(Content.Load<Texture2D>("obstacle1"), Color.White, new Rectangle(150, 300, 67, 35),
                5, 5, WCBW, WCBH, true, true);
            obstacle3 = new Ball(Content.Load<Texture2D>("obstacle2"), Color.White, new Rectangle(250, 50, 91, 57),
                5, 5, WCBW, WCBH, true, true);
            obstacle4 = new Tree(Content.Load<Texture2D>("obstacle3_interact"), new Rectangle(295, 175, 210, 274), 10, Content.Load<SoundEffect>("treefall"));



            platform1 = new Platform(Content.Load<Texture2D>("bunny_R"),
                new Rectangle(0, 150, 63, 59), Color.White, 0,
                WCBH, 0, WCBW, speed, 2, 5, 1, 0, 30, 0, 10, projectiles,
                Content.Load<Texture2D>("charge1"), Content.Load<Texture2D>("bullet_healthbar"),
                new Vector2(10, 10), new Vector2(138, -45),
                Content.Load<SoundEffect>("hit_sfx"), Content.Load<SoundEffect>("shoot_sfx"),Content.Load<SoundEffect>("win"));

            platform2 = new Platform(Content.Load<Texture2D>("squirrel_L"),
                new Rectangle(WCBW - 100, WCBH - 350, 67, 56), Color.White,
                0, WCBH, 0, WCBW, speed, 2, 5, 1, 0, 30, 0, 10, projectiles,
                Content.Load<Texture2D>("charge4"), Content.Load<Texture2D>("bullet_healthbar"),
                new Vector2(WCBW - 40, WCBH - 40), new Vector2(WCBW - 344, WCBH - 525),
                Content.Load<SoundEffect>("hit_sfx"), Content.Load<SoundEffect>("shoot_sfx"),Content.Load<SoundEffect>("win"));

            p1lifebar = new P1Lifebar(Content.Load<Texture2D>("box"),
                new Rectangle(200, 10, 100, 30), Color.Green, 120, 40);
            p2lifebar = new P2Lifebar(Content.Load<Texture2D>("box"),
                new Rectangle(500, 10, 100, 30), Color.Green, 120, 40);
            TreeLifebar = new TreeLifebar(Content.Load<Texture2D>("box"),
                new Rectangle(335, 435, 120, 10), Color.YellowGreen, 150, 40);
            treeHP = TreeLifebar.LifebarWidth;

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

            //treefallsfx = Content.Load<SoundEffect>("treefall");

            //hitsfx = Content.Load<SoundEffect>("hit_sfx");
            //shootsfx = Content.Load<SoundEffect>("shoot_sfx");
            //winsfx = Content.Load<SoundEffect>("win");

            bgm = Content.Load<Song>("bgm");
            menuBG = Content.Load<Texture2D>("menu_bg");
            BG = Content.Load<Texture2D>("background");
            rabbitLifebar = Content.Load<Texture2D>("bunny_healthbar1");
            squirrelLifebar = Content.Load<Texture2D>("squirrel_healthbar");

          
            MediaPlayer.Play(bgm);
            MediaPlayer.Volume = 0.8f;
            MediaPlayer.IsRepeating = true;
            
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
            // bullets projectiles
            foreach (var projectile in projectiles)
            {
        if (projectile.Position.Intersects(obstacle1.BallRec) ||
            projectile.Position.Intersects(obstacle2.BallRec) ||
            projectile.Position.Intersects(obstacle3.BallRec))
        {
            projectilesToRemove.Add(projectile);
        }
        if (projectile.Position.Intersects(obstacle4.RecTree))
                {
                    obstacle4.TreeHit(projectile.Position);
                    projectilesToRemove.Add(projectile);
                    TreeLifebar.LifebarWidth = TreeLifebar.LifebarWidth - (treeHP/10);
                    obstacle4.TreeCrumble(Content.Load<Texture2D>("obstacle3_collide"), new Rectangle(335, 325, 132, 79));
                }
        platform1.damageCheck(1, projectile, p1lifebar, p2lifebar, p1score, p2score, rounds, pstats); //pstats prone to failure
        platform2.damageCheck(2, projectile, p2lifebar, p1lifebar, p2score, p1score, rounds, pstats);

        if (projectile.Position.Intersects(platform1.platRec) || projectile.Position.Intersects(platform2.platRec))
                {
                    projectilesToRemove.Add(projectile);
                }

            }

            foreach (var projectile in projectilesToRemove)
            {
                projectiles.Remove(projectile);
            }

            if (p1score.ScoreCount == 2)
            {
                Save((platform1.WinnerCheck(1, p1lifebar, p2lifebar, p1score, p2score, rounds, "", pstats)));
                winner = "Player 1 Wins!";

                //winsfx.Play();
            }

            if (p2score.ScoreCount == 2)
            {
                Save((platform2.WinnerCheck(2, p2lifebar, p1lifebar, p2score, p1score, rounds, "", pstats)));
                winner = "Player 2 Wins!";

                //winsfx.Play();
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
                _spriteBatch.Draw(obstacle4.FullTree, obstacle4.RecTree, Color.White);

                platform1.DrawProjectiles(_spriteBatch, Content.Load<Texture2D>("bullet_L")); //doesn't show any bullets???
                platform2.DrawProjectiles(_spriteBatch, Content.Load<Texture2D>("bullet_R"));

                platform1.DrawUI(_spriteBatch, 1);
                platform2.DrawUI(_spriteBatch, 2);

                _spriteBatch.Draw(rabbitLifebar, new Vector2(110, -79), Color.White);
                _spriteBatch.Draw(squirrelLifebar, new Vector2(470, -79), Color.White);

                _spriteBatch.Draw(p1lifebar.LifebarTexture, p1lifebar.LifebarRectangle, p1lifebar.LifebarColor);
                _spriteBatch.Draw(p2lifebar.LifebarTexture, p2lifebar.LifebarRectangle, p2lifebar.LifebarColor);
                _spriteBatch.Draw(TreeLifebar.LifebarTexture,TreeLifebar.LifebarRectangle, TreeLifebar.LifebarColor);

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