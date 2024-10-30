using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPP_lab
{
    class Lab_4
    {
        public static void ex_1()
        {
            int[] nValues = { 1000, 5000, 10000, 50000 };
            Console.WriteLine("Реализация с атомарной операцией (Interlocked):");

            foreach (int n in nValues)
            {
                double atomicResult = Utilities.CalculateSumWithAtomic(n);
                Console.WriteLine($"N = {n}, Сумма = {atomicResult}");
            }
            Console.ReadKey();
        }

        public static void ex_2()
        {
            int[] nValues = { 100000, 500000, 1000000, 5000000 };
            Console.WriteLine("Реализация с критической секцией (lock):");

            foreach (int n in nValues)
            {
                double criticalResult = Utilities.CalculateSumWithCritical(n);
                Console.WriteLine($"N = {n}, Сумма = {criticalResult}");
            }
            Console.ReadKey();
        }
    }
}
