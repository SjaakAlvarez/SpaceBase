using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceBaseMono
{
    public class Hole
    {
        public int x;
        public int y;
        public Boolean isBlack;
        public int nr;
        public Boolean small;

        public Hole()
        {
        }

        public Hole(int x, int y, Boolean isBlack = true, int nr=0, Boolean small=true)
        {
            this.x=x;
            this.y=y;
            this.isBlack=isBlack;
            this.nr = nr;
            this.small = small;
        }
    }
}
