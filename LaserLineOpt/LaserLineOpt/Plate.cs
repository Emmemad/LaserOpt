using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserLineOpt
{
    public class Plate
    {
        private List<Segment> _Segments;

        private static Random rng = new Random();

        public double Start { get; set; }
        public double End { get; set; }

        public double FitnessValue
        {
            get { return 1.0 / CalcSumIdlingLine(); }

        }

        public Plate()
        {
            _Segments = new List<Segment>();
        }

        public Plate(List<Segment> segments)
        {
            _Segments = segments;
        }

        public List<Segment> Segments
        {
            get { return _Segments; }
        }

        public Plate(Plate copy)
        {
            _Segments = new List<Segment>();
            foreach (Segment segment in copy.Segments)
            {
                this.AddSegment(new Segment(segment));
            }
        }

        public override string ToString()
        {
            String str = "";

            for (int i = 0; i < _Segments.Count; i++)
            {
                str += _Segments[i].ToString();
                str += "\n";
            }

            return str;
        }

        public void AddSegment(Segment segment)
        {
            Segments.Add(segment);
        }

        public int Size()
        {
            return Segments.Count();
        }

        public void ShuffleSegments()
        {
            FisherYatesShuffle();
        }

        private void FisherYatesShuffle()
        {
            int n = Segments.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Segment value = new Segment(Segments[k]);
                Segments[k] = new Segment(Segments[n]);
                Segments[n] = value; ;
            }
        }

        public void SetRandomDirectionsToSegments()
        {

            foreach (Segment segment in Segments)
            {
                int n = rng.Next(2);

                if (n == 1)
                {
                    segment.Direction = true;
                }
                else
                {
                    segment.Direction = false;
                }
            }
        }

        public double CalcSumIdlingLine()
        {
            double length = 0;
            int size = Segments.Count;

            for (int i = 0; i < size - 1; i++)
            {
                length += CalcIdling(Segments[i], Segments[i + 1]);
            }
            return length;
        }

        private double CalcIdling(Segment segment1, Segment segment2)
        {
            if (segment1.Direction && segment2.Direction)        //True и True
            {
                return CalcIdlingLine(segment1.X2, segment1.Y2, segment2.X1, segment2.Y1);
            }
            else if (!segment1.Direction && segment2.Direction)  //False и True
            {
                return CalcIdlingLine(segment1.X1, segment1.Y1, segment2.X1, segment2.Y1);
            }
            else if (segment1.Direction && !segment2.Direction) //True и False
            {
                return CalcIdlingLine(segment1.X2, segment1.Y2, segment2.X2, segment2.Y2);
            }
            else                                                //False и False
            {
                return CalcIdlingLine(segment1.X1, segment1.Y1, segment2.X2, segment2.Y2);
            }
        }

        private double CalcIdlingLine(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }
    }
}
