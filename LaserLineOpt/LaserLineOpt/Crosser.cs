using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserLineOpt
{
    public class Crosser
    {
        static Random rng = new Random();


        public static Plate OrderedCrossover(Plate plate1, Plate plate2)
        {

            List<Segment> child = new List<Segment>();

            for(int i = 0; i < plate1.Size(); i++)
            {
                child.Add(null);
            }

            int start = rng.Next(0, plate1.Size());
            int end = rng.Next(0, plate1.Size());

            start = Math.Min(start, end);
            end = Math.Max(start, end);

            HashSet<Segment> set = new HashSet<Segment>();

            for (int i = start; i <= end; i++)
            {
                child[i] = plate1.Segments[i];
                set.Add(child[i]);
            }

            int ci = end + 1;
            ci %= plate1.Size();

            for (int i = ci; i < 2 * plate2.Size(); i++)
            {
                int ri = i % plate2.Size();

                if (!set.Contains(plate2.Segments[ri]))
                {
                    child[ci] = plate2.Segments[ri];
                    set.Add(plate2.Segments[ri]);
                    ci++;

                    ci %= plate1.Size();
                }
            }

            return new Plate(child);
        }


        public static Plate CyclicCrossover (Plate plate1, Plate plate2)
        {

            int size = plate1.Size();
            List<Segment> segments = new List<Segment>();

            for (int i = 0; i < size; i++)
            {
                segments.Add(null);
            }

            HashSet<int> seenIndexes = new HashSet<int>();

            int currentIndex = 0;
            while (!seenIndexes.Contains(currentIndex))
            {
                seenIndexes.Add(currentIndex);
                int IDToFind = plate2.Segments[currentIndex].ID;
                currentIndex = plate1.Segments.FindIndex(a => {
                    return a.ID == IDToFind;
                });
            }

            foreach (int index in seenIndexes)
            {
                segments[index] = new Segment(plate1.Segments[index]);
            }

            for (int i = 0; i < size; i++)
            {
                if (segments[i] == null)
                {
                    segments[i] = new Segment(plate2.Segments[i]);
                }
            }

            return new Plate(segments);
        }



    }
}
