using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinalTestingGround
{
    internal class Lifebar
    {

        Texture2D lifebarTexture;
        Rectangle lifebarRectangle;
        Color lifebarColor;
        int lifebarWidth = 120;
        int lifebarNumber = 40;

        public Texture2D LifebarTexture { get => lifebarTexture; set => lifebarTexture = value; }
        public Rectangle LifebarRectangle { get => lifebarRectangle; set => lifebarRectangle = value; }
        public Color LifebarColor { get => lifebarColor; set => lifebarColor = value; }
        public int LifebarWidth { get => lifebarRectangle.Width; set => lifebarRectangle.Width = value; }
        public int LifebarNumber { get => lifebarNumber; set => lifebarNumber = value; }

        public Lifebar(Texture2D lifebarTexture, Rectangle lifebarRectangle, Color lifebarColor, int lifebarWidth, int lifebarNumber)
        {
            this.LifebarTexture = lifebarTexture;
            this.LifebarRectangle = lifebarRectangle;
            this.LifebarColor = lifebarColor;
            this.LifebarWidth = lifebarWidth;
            this.lifebarNumber = lifebarNumber;
        }

        public void lifebarReset()
        {
            LifebarWidth = 120;
            LifebarNumber = 40;
            LifebarColor = Color.White;
        }

    }

    internal class P1Lifebar : Lifebar
    {
        public P1Lifebar(Texture2D lifebarTexture, Rectangle lifebarRectangle, Color lifebarColor, int lifebarWidth, int lifebarNumber)
            : base(lifebarTexture, lifebarRectangle, lifebarColor, lifebarWidth, lifebarNumber)
        {

        }

        //should return to full width
        /*     public void lifebarReset() 
             {
                 LifebarWidth = 120;
                 LifebarNumber = 40;
                 LifebarColor = Color.White;
             } */


    }

    internal class P2Lifebar : Lifebar
    {
        public P2Lifebar(Texture2D lifebarTexture, Rectangle lifebarRectangle, Color lifebarColor, int lifebarWidth, int lifebarNumber)
            : base(lifebarTexture, lifebarRectangle, lifebarColor, lifebarWidth, lifebarNumber)
        {

        }

        /*    public void lifebarReset()
            {  
                LifebarWidth = 120;
                LifebarNumber = 40;
                LifebarColor = Color.White;
            } */


    }




}