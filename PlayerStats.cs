using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTestingGround
{
    public class PlayerStats
    {
        public int p1score { get; set; }
        public int p2score { get; set; }
        public int p1life { get; set; }
        public int p2life { get; set; }
        public int round { get; set; }

        public string winner { get; set; }
    }
}
