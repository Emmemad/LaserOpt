using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserLineOpt
{
    public class Segment
    {
        public int ID { get; set; }
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }

        // True означает направление от 1 точки ко 2
        public bool Direction { get; set; }

        public Segment(Segment copy)
        {
            ID = copy.ID;
            X1 = copy.X1;
            Y1 = copy.Y1;
            X2 = copy.X2;
            Y2 = copy.Y2;
            Direction = copy.Direction;
        }

        public Segment(int ID, int X1, int Y1, int X2, int Y2)
        {
            this.ID = ID;
            this.X1 = X1;
            this.Y1 = Y1;
            this.X2 = X2;
            this.Y2 = Y2;
            Direction = true;
        }

        public override string ToString()
        {
            if (Direction)
            {
                return "[" + X1 + ", " + Y1 + ";" + X2 + ", " + Y2 + "]";
            }
            else
            {
                return "[" + X2 + ", " + Y2 + ";" + X1 + ", " + Y1 + "]";
            }
        }

        public void ReverseDirection()
        {
            Direction = !Direction;
        }


    }
}
