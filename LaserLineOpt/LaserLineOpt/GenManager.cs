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
        int currentPlateIndex, newPlateIndex;

        public int CurrentPopulationCount
        {
            get { return currentPopulation.Count; }
        }

        public int NewPopulationCount
        {
            get { return newPopulation.Count; }
        }

        public GenManager(List<Plate> firstPopulation)
        {
            currentPopulation = firstPopulation;
            newPopulation = new List<Plate>();
            currentPlateIndex = 0;
            newPlateIndex = 0;
        }

        public void MakeNewGenerationCurrent()
        {
            List<Plate> tmp;
            tmp = currentPopulation;
            currentPopulation = newPopulation;
        }

        public Plate GetEmptyPlate()
        {
            if (newPlateIndex < newPopulation.Count)
            {
                Plate outputPlate = newPopulation[newPlateIndex];
                newPlateIndex++;
                return outputPlate;
            }
            else
            {
                throw new IndexOutOfRangeException("Requested index in newPopulation is out of range");
            }
        }

        public Plate GetCurrentPlate(int index)
        {
            if (index < currentPopulation.Count)
            {
                return currentPopulation[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Requested index in currentPopulation is out of range");
            }
        }

    }
}
