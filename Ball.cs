using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FinalTestingGround
{
    public class Ball
    {
        Texture2D ballTexture;
        Color ballColor;
        Rectangle ballRec;
        int speedX, speedY;
        int boundaryX, boundaryY;
        bool ballDirX, ballDirY;
        List<projectile> projectiles;

        public Ball(Texture2D ballTexture, Color ballColor, Rectangle ballRec,
            int speedX, int speedY, int boundaryX, int boundaryY, bool ballDirX, bool ballDirY)
        {
            this.ballTexture = ballTexture;
            this.ballColor = ballColor;
            this.ballRec = ballRec;
            this.speedX = speedX;
            this.speedY = speedY;
            this.boundaryX = boundaryX;
            this.boundaryY = boundaryY;
            this.ballDirX = ballDirX;
            this.ballDirY = ballDirY;
        }

        public Texture2D BallTexture { get => ballTexture; set => ballTexture = value; }
        public Rectangle BallRec { get => ballRec; set => ballRec = value; }
        public Color BallColor { get => ballColor; set => ballColor = value; }

        public void ballcollision(Rectangle platform)
        {
            if (ballRec.Intersects(platform))
            {
                return;
            }
        }


        internal void ballcollision(Rectangle platRec, Vector2 platformspeed)
        {
            throw new NotImplementedException();
        }
        
    }
}
