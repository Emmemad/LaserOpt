using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaserLineOpt;

namespace LaserOptTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int NumberOfTestSegments = 20;

            TSPSolver tspSolver = new TSPSolver();

            Plate targetPlate = new Plate();

            for (int i = 0; i < NumberOfTestSegments; i++)
            {
                targetPlate.AddSegment(new Segment(i, i, i, i + 1, i + 1));
            }

            tspSolver.sizeOfPopulation = 4000;  //4000
            tspSolver.NumberOfCycles = 40000;   //40000
            tspSolver.MutationProbability = 0.02; //0.01
            tspSolver.SetTargetPlate(targetPlate);

            /*
			 * Ожидаемое время работы
			 * sizeOfPopulation * NumberOfCycles * NumberOfTestPlates
			 */

            tspSolver.Solve();

            Plate bestPlate = tspSolver.GetBestPlate();

            Console.WriteLine(targetPlate.ToString());
            Console.WriteLine(targetPlate.CalcSumIdlingLine());
            Console.WriteLine(TSPSolver.Fitness(targetPlate));
            Console.WriteLine("\n");
            Console.WriteLine(bestPlate.ToString());
            Console.WriteLine(bestPlate.CalcSumIdlingLine());
            Console.WriteLine(TSPSolver.Fitness(bestPlate));

            Console.ReadLine();
        }

    }
}
