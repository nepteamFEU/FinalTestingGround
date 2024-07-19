using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

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

        /*  public int scorePoint()
          {
              if (ballRec.X <= 0)
                  return 1;
              if (ballRec.X >= boundaryX - ballRec.Width)
                  return 2;
              return 0;
          } */

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

        /*   public void ballMovement()
           {
               if (ballDirX)
                   ballRec.X += speedX;
               else
                   ballRec.X -= speedX;

               if (ballDirY)
                   ballRec.Y += speedY;
               else
                   ballRec.Y -= speedY;

               // Handle boundary collision for X
               if (ballRec.X <= 0 || ballRec.X >= boundaryX - ballRec.Width)
                   ballDirX = !ballDirX;

               // Handle boundary collision for Y
               if (ballRec.Y <= 0 || ballRec.Y >= boundaryY - ballRec.Height)
                   ballDirY = !ballDirY;
           } */
    }
}
