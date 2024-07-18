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
    internal class Selection
    {

        bool clicked;
        Rectangle selectionRectangle;
        Texture2D selectionTexture;
        Color selectionColor;

        public bool Clicked { get => clicked; set => clicked = value; }
        public Rectangle SelectionRectangle { get => selectionRectangle; set => selectionRectangle = value; }
        public Texture2D SelectionTexture { get => selectionTexture; set => selectionTexture = value; }
        public Color SelectionColor { get => selectionColor; set => selectionColor = value; }

        public Selection(bool clicked, Rectangle selectionRectangle, Texture2D selectionTexture, Color selectionColor)
        {
            this.Clicked = clicked;
            this.SelectionRectangle = selectionRectangle;
            this.SelectionTexture = selectionTexture;
            this.SelectionColor = selectionColor;
        }


    }

    internal class Start : Selection
    {
        public Start(bool clicked, Rectangle selectionRectangle, Texture2D selectionTexture, Color selectionColor)
            : base(clicked, selectionRectangle, selectionTexture, selectionColor)
        {

        }
    }

    internal class Continue : Selection
    {
        public Continue(bool clicked, Rectangle selectionRectangle, Texture2D selectionTexture, Color selectionColor)
            : base(clicked, selectionRectangle, selectionTexture, selectionColor)
        {

        }
    }

    internal class Exit : Selection
    {
        public Exit(bool clicked, Rectangle selectionRectangle, Texture2D selectionTexture, Color selectionColor)
            : base(clicked, selectionRectangle, selectionTexture, selectionColor)
        {

        }
    }


}