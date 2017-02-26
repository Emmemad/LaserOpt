using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserLineOpt
{
    public class TSPSolver
    {
        public double PlatesToRemove = 0.7;
        public int sizeOfPopulation = 0;
        public int NumberOfCycles = 1;
        public double MutationProbability = 0.01;

        Plate TargetPlate;
        List<Plate> Plates = new List<Plate>();     // Текущая популяция 

        private static Random rng = new Random();

        /* ///Основной метод/// */
        public void Solve()
        {
            if (TargetPlate == null)
            {
                throw new ArgumentException("Отсутствует целевая пластина (TargetPlate)");
            }

            if (sizeOfPopulation == 0)
            {
                throw new ArgumentException("Некорректный размер популяции");
            }

            GenerateFirstPopulation(); //Порождение первой популяции путём случайного перемешивания сегментов "идеальной" пластины

            for (int i = 0; i < NumberOfCycles; i++)
            {
                PerformSelection(); // Отбор
                Mutate();           // Мутация

                if ((i % 100) == 0) // Вывод информации на экран
                {
                    Plate bestPlate = GetBestPlate();

                    Console.WriteLine("Iteration № " + i);
                    WriteInfo(bestPlate);
                }

            }
        }

        public void WriteInfo(Plate bestPlate)
        {
            Console.WriteLine("Best plate is: \n" + bestPlate);
            Console.WriteLine("Best fitness is: " + bestPlate.FitnessValue);
            Console.WriteLine("Best idling is: " + bestPlate.CalcSumIdlingLine());
            Console.WriteLine();
        }

        public Plate GetBestPlate() // Возвращает особь с наибольшим значением fitness-функции
        {
            double fitnessMax = 0;
            int maxIndex = 0;
            for (int i = 0; i < Plates.Count; i++)
            {
                if (Plates[i].FitnessValue > fitnessMax)
                {
                    fitnessMax = Plates[i].FitnessValue;
                    maxIndex = i;
                }
            }
            return Plates[maxIndex];
        }

        public void SetTargetPlate(Plate plate) // Устанавливает "идеальную" особь
        {
            TargetPlate = plate;
        }

        public void GenerateFirstPopulation() // Создание новых особей путём перемешивания сегментов и их направлений
        {
            for (int i = 0; i < sizeOfPopulation; i++)
            {
                Plate newPlate = new Plate(TargetPlate);
                newPlate.ShuffleSegments();
                newPlate.SetRandomDirectionsToSegments();
                Plates.Add(newPlate);
            }
        }

        void PerformSelection() // Отбор
        {
            RouletteSelection();

            //SortPlatesByFitness();  // Отбор усечением: сортировка
            //LeaveBestOfPlates();    // Отбор усечением: усечение популяции
            //ProduceNewPlates();     // Отбор усечением: производство потомков для заполнения популяции
        }

        void Mutate() // Мутация
        {
            for (int i = 0; i < Plates.Count; i++)
            {
                Mutator.ReverseSegmentMutation(Plates[i], MutationProbability);
            }
        }


        void SortPlatesByFitness() // Сортировка главного массива особей по возрастанию значения фитнесс-функции
        {
            Plates = Plates.OrderBy(o => Fitness(o)).ToList();
        }

        void SortPlatesByFitness(List<Plate> list) // Сортировка переданного массива особей по возрастанию значения фитнесс-функции
        {
            list = list.OrderBy(o => Fitness(o)).ToList();
        }

        /* Функции отбора усечением */

        void LeaveBestOfPlates()
        {
            int amountToDelete = (int)Math.Floor(Plates.Count * PlatesToRemove);

            Plates.RemoveRange(0, amountToDelete);
        }

        void LeaveBestOfPlates(List<Plate> list, double percentPlatesToRemove)
        {
            int amountToDelete = (int)Math.Floor(list.Count * percentPlatesToRemove);

            list.RemoveRange(0, amountToDelete);
        }

        void LeaveBestOfPlates(List<Plate> list, int platesToSurvive)
        {
            list.RemoveRange(0, list.Count - platesToSurvive);
        }


        void ProduceNewPlates()
        {
            List<Plate> NewPopulation = new List<Plate>();
            NewPopulation.AddRange(Plates);

            while (NewPopulation.Count < sizeOfPopulation)
            {
                int n1 = rng.Next(Plates.Count);
                int n2 = rng.Next(Plates.Count);

                List<Plate> children = Crossover(Plates[n1], Plates[n2]);

                if (NewPopulation.Count == sizeOfPopulation - 1)
                {
                    double f1 = Fitness(children[0]);
                    double f2 = Fitness(children[1]);

                    if (f1 > f2)
                    {
                        NewPopulation.Add(children[0]);
                    }
                    else
                    {
                        NewPopulation.Add(children[1]);
                    }
                }
                else
                {
                    NewPopulation.AddRange(children);
                }
            }

            Plates.Clear();
            Plates = NewPopulation;
        }

        /* Отбор рулеткой */

        void RouletteSelection()
        {
            List<Plate> selectedPlates = new List<Plate>();
            List<Plate> currentPair;
            Roulette roulette = new Roulette(Plates);

            for (int i = 0; i < Plates.Count / 4; i++) // Plates.Count/2 пар особей, каждая пара порождает по 2 потомка
            {
                currentPair = roulette.GetPlates(2);
                selectedPlates.AddRange(currentPair); // Добавление выбранных рулеткой особей в следующее поколение (2 особи)
                selectedPlates.AddRange(Crossover(currentPair));  // Добавление их "детей" (2 особи)
            }

            if (Plates.Count % 4 != 0) // Если в популяции не делящееся на 4 число особей, добавляем ту, у которой больше значение фитнесс-функции
            {
                List<Plate> tempList = roulette.GetPlates(2);
                Plate lastPlate = (Fitness(tempList[0]) > Fitness(tempList[1])) ? tempList[0] : tempList[1];
                selectedPlates.Add(lastPlate);
            }

            Plates.Clear();
            Plates = selectedPlates;

        }

        /*Кроссинговер*/

        static List<Plate> Crossover(Plate plate1, Plate plate2)
        {
            List<Plate> pairOfChildren = new List<Plate>();

            pairOfChildren.Add(Crosser.CyclicCrossover(plate1, plate2));
            pairOfChildren.Add(Crosser.CyclicCrossover(plate2, plate1));

            return pairOfChildren;
        }

        static List<Plate> Crossover(List<Plate> pairOfPlates)
        {
            List<Plate> pairOfChildren = new List<Plate>();

            pairOfChildren.Add(Crosser.CyclicCrossover(pairOfPlates[0], pairOfPlates[1]));
            pairOfChildren.Add(Crosser.CyclicCrossover(pairOfPlates[1], pairOfPlates[0]));

            return pairOfChildren;
        }

        public static double Fitness(Plate plate)
        {
            return 1.0 / plate.CalcSumIdlingLine();
        }

    }

}
