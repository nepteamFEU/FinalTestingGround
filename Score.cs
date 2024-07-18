using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTestingGround
{
    internal class Score
    {
        Color scoreColor;
        int scoreCount = 0;

        public int ScoreCount { get => scoreCount; set => scoreCount = value; }

        public Score(Color scoreColor, int scoreCount)
        {
            this.scoreColor = scoreColor;
            this.scoreCount = scoreCount;

        }

        public void Updatescore()
        {
            ScoreCount += 1;
        }

        public void CountReset()
        {
            ScoreCount = 0;
        }
    }

    internal class P1Score : Score
    {
        public P1Score(Color scoreColor, int scoreCount) : base(scoreColor, scoreCount)
        {

        }
    }


    internal class P2Score : Score
    {
        public P2Score(Color scoreColor, int scoreCount) : base(scoreColor, scoreCount)
        {

        }
    }

    internal class Rounds : Score
    {
        public Rounds(Color scoreColor, int scoreCount) : base(scoreColor, scoreCount)
        {

        }

    }

}