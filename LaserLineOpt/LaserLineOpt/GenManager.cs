using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserLineOpt
{
    class GenManager
    {
        List<Plate> currentPopulation, newPopulation;

        public GenManager(int sizeOfPopulation)
        {
            currentPopulation = new List<Plate>();
            newPopulation = new List<Plate>();
        }

        public void MakeNewGenerationCurrent()
        {
            List<Plate> tmp;
        }
    }
}
