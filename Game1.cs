using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        int WCBW, WCBH;
        List<projectile> projectiles;
        Texture2D chargeSprite;
        Texture2D ammoSprite;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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
            _spriteBatch.Draw(platform1.PlatText, platform1.PlatRec, platform1.PlatColor);
            _spriteBatch.Draw(platform2.PlatText, platform2.PlatRec, platform2.PlatColor);
            _spriteBatch.Draw(ball.BallTexture, ball.BallRec, ball.BallColor);

            platform1.DrawProjectiles(_spriteBatch, Content.Load<Texture2D>("projectile"));
            platform2.DrawProjectiles(_spriteBatch, Content.Load<Texture2D>("projectile"));

            platform1.DrawUI(_spriteBatch,1);
            platform2.DrawUI(_spriteBatch,2);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
