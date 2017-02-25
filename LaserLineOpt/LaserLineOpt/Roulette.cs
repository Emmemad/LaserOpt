using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserLineOpt
{
    class Roulette
    {
        double fitnessSum;
        double fitnessAverage;
        static Random rng = new Random();
        List<Plate> roulettePlates;
        List<Tuple<double, double>> platesCoords = new List<Tuple<double, double>>();

        public Roulette(List<Plate> plates)
        {
            roulettePlates = plates;

            fitnessSum = plates[0].FitnessValue;
            plates[0].Start = 0;
            plates[0].End = plates[0].FitnessValue;
            for (int i = 1; i < plates.Count; i++)
            {
                plates[i].Start = plates[i - 1].End;
                plates[i].End = plates[i - 1].End + plates[i].FitnessValue;
                fitnessSum += plates[i].FitnessValue;
            }

            fitnessAverage = fitnessSum / plates.Count;


            /*fitnessSum = plates[0].FitnessValue;
            platesCoords.Add(Tuple.Create(0.0, plates[0].FitnessValue));
            for (int i = 1; i < plates.Count; i++)
            {
                platesCoords.Add(Tuple.Create(platesCoords[i - 1].Item2, platesCoords[i - 1].Item2 + plates[i].FitnessValue));
                fitnessSum += plates[i].FitnessValue;
            }
            fitnessAverage = fitnessSum / plates.Count;
            */
        }

        public List<Plate> GetPlates(int num = 1)
        {
            List<Plate> selectedPlates = new List<Plate>();

            /* (int i = 0; i < num; i++)
            {
                
                double chosenFitness = fitnessSum * rng.NextDouble();
                int guessedPlateID = (int)(chosenFitness / fitnessAverage);

                if ((chosenFitness >= platesCoords[guessedPlateID].Item1) && (chosenFitness <= platesCoords[guessedPlateID].Item2))
                {
                    selectedPlates.Add(roulettePlates[guessedPlateID]);
                }
                else if (chosenFitness < platesCoords[guessedPlateID].Item1)
                {
                    for (int k = guessedPlateID - 1; k >= 0; k--)
                    {
                        if ((chosenFitness >= platesCoords[k].Item1) && (chosenFitness <= platesCoords[k].Item2))
                        {
                            selectedPlates.Add(roulettePlates[k]);
                            break;
                        }

                    }
                }
                else if (chosenFitness > platesCoords[guessedPlateID].Item2)
                {
                    for (int k = guessedPlateID + 1; k < roulettePlates.Count; k++)
                    {
                        if ((chosenFitness >= platesCoords[k].Item1) && (chosenFitness <= platesCoords[k].Item2))
                        {
                            selectedPlates.Add(roulettePlates[k]);
                            break;
                        }
                    }
                }*/

            double chosenFitness = fitnessSum * rng.NextDouble();
            int guessedPlateID = (int)(chosenFitness / fitnessAverage);

            if ((chosenFitness >= roulettePlates[guessedPlateID].Start) && (chosenFitness <= roulettePlates[guessedPlateID].End))
            {
                selectedPlates.Add(roulettePlates[guessedPlateID]);
            }
            else if (chosenFitness < roulettePlates[guessedPlateID].Start)
            {
                for (int k = guessedPlateID - 1; k >= 0; k--)
                {
                    if ((chosenFitness >= roulettePlates[k].Start) && (chosenFitness <= roulettePlates[k].End))
                    {
                        selectedPlates.Add(roulettePlates[k]);
                        break;
                    }

                }
            }
            else if (chosenFitness > roulettePlates[guessedPlateID].End)
            {
                for (int k = guessedPlateID + 1; k < roulettePlates.Count; k++)
                {
                    if ((chosenFitness >= roulettePlates[k].Start) && (chosenFitness <= roulettePlates[k].End))
                    {
                        selectedPlates.Add(roulettePlates[k]);
                        break;
                    }
                }
            }
            return selectedPlates;
        }

    }
}
