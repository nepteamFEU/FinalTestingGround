using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace FinalTestingGround
{
    public class Tree
    {
        private Color treeColor = Color.Brown;
        private Texture2D fullTree;
        private Rectangle recTree;
        private int durability;
        private SoundEffect treeFallSFX;
        private bool treeBroken;

        public Tree(Texture2D fullTree, Rectangle recTree, int durability, SoundEffect treeFallSFX)
        {
            this.fullTree = fullTree;
            this.recTree = recTree;
            this.durability = durability;
            this.treeFallSFX = treeFallSFX;
        }

        public Texture2D FullTree
        {
            get => fullTree;
            set => fullTree = value;
        }

        public Rectangle RecTree
        {
            get => recTree;
            set => recTree = value;
        }

        public int Durability
        {
            get => durability;
            set => durability = value;
        }

        public void TreeHit(Rectangle rect)
        {
            if (rect.Intersects(recTree))
            {
                durability--;
            }
            else { return; }
        }

        public void TreeCrumble(Texture2D stumpTexture, Rectangle stumpRectangle)
        {
            if (durability == 0 && treeBroken == false)
            {
                fullTree = stumpTexture;
                recTree = stumpRectangle;
                treeFallSFX.Play();
                treeBroken = true;
            }
            else
            {
                treeBroken = false;
                return;
            }
        }
    }
}
