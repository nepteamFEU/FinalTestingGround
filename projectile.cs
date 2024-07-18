
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace FinalTestingGround
{
    internal class projectile
    {
        public Rectangle Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int boundaryLeft { get; set; }
        public int boundaryRight { get; set; }

        Rectangle damageCheck;

        public projectile(Rectangle position, Vector2 velocity, Rectangle damageCheck)
        {
            Position = position;
            Velocity = velocity;
            boundaryLeft = -50;
            boundaryRight = 850;
            this.damageCheck = damageCheck;
        }

        public Rectangle DamageCheck { get => damageCheck; set => damageCheck = value; }

        public void Update()
        {
            Position = new Rectangle(Position.X + (int)Velocity.X, Position.Y + (int)Velocity.Y, Position.Width, Position.Height);
            damageCheck = Position;
        }
    }
}

