using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPP_lab
{
    class Lab_2
    {

        const int NMAX = 100;
        const int LIMIT = 10;

        public static void ex_1()
        {
            {
                int[] nmaxValues = { 10, 50, 100, 500, 1000 };
                int[] limitValues = { 10, 50, 100 };

                foreach (int limit in limitValues)
                {
                    Console.WriteLine($"\nLIMIT = {limit}");
                    foreach (int nmax in nmaxValues)
                    {
                        Console.WriteLine($"NMAX = {nmax}");

                        var singleThreadTime = Utilities.MeasureExecutionTime(nmax, limit, false);
                        var multiThreadTime = Utilities.MeasureExecutionTime(nmax, limit, true);

                        Console.WriteLine($"Однопоточное время: {singleThreadTime.TotalMilliseconds} ms");
                        Console.WriteLine($"Многопоточное время: {multiThreadTime.TotalMilliseconds} ms");

                        if (Math.Abs(singleThreadTime.TotalMilliseconds - multiThreadTime.TotalMilliseconds) < 0.01)
                        {
                            Console.WriteLine($"Для NMAX = {nmax} и LIMIT = {limit} время выполнения однопоточной и многопоточной версии примерно совпадает.");
                        }
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
