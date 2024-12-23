using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPP_lab
{
    class Lab_6_2
    {
        public static void ex_1()
        {
            int[] nValues = { 1000, 5000, 10000, 50000 };
            Console.WriteLine("Многопоточная реализация с барьерной синхронизацией:");

            foreach (int n in nValues)
            {
                double parallelResult = Utilities.CalculateSumParallelWithBarrier(n);
                Console.WriteLine($"N = {n}, Сумма = {parallelResult}");
            }
            Console.ReadKey();
        }

        public static void ex_2()
        {
            int[] nValues = { 100000, 500000, 1000000, 5000000 };
            Console.WriteLine("Однопоточная реализация:");

            foreach (int n in nValues)
            {
                double singleThreadResult = Utilities.CalculateSumSingleThread(n);
                Console.WriteLine($"N = {n}, Сумма = {singleThreadResult}");
            }
            Console.ReadKey();
        }
    }

}
