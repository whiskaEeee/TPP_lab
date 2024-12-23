using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPP_lab
{
    class Lab_6_1
    {

        private static SemaphoreSlim semaphore;

        public static void ex_1()
        {
            int[] nValues = { 1000, 5000, 10000, 50000 };
            Console.WriteLine("Многопоточная реализация с семафорами:");

            foreach (int n in nValues)
            {
                double parallelResult = CalculateSumParallelWithSemaphore(n);
                Console.WriteLine($"N = {n}, Сумма = {parallelResult}");
            }
            Console.ReadKey();
        }

        public static double CalculateSumParallelWithSemaphore(int n)
        {
            double[] A = new double[n];
            double[] B = new double[n];
            double[] C = new double[n];

            for (int i = 0; i < n; i++)
            {
                A[i] = i * 0.5;
                B[i] = i * 0.3;
            }

            double sum = 0;
            semaphore = new SemaphoreSlim(1, 1);
            Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.For(0, n, i =>
            {
                C[i] = Math.Sin(A[i] + B[i]);
                semaphore.Wait();
                try
                {
                    sum += C[i];
                }
                finally
                {
                    semaphore.Release();
                }
            });

            stopwatch.Stop();
            Console.WriteLine($"Время выполнения (с семафором, многопоточно): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }

        public static void ex_2()
        {
            int[] nValues = { 100000, 500000, 1000000, 5000000 };
            Console.WriteLine("Однопоточная реализация с семафором:");

            foreach (int n in nValues)
            {
                double singleThreadResult = CalculateSumSingleThreadWithSemaphore(n);
                Console.WriteLine($"N = {n}, Сумма = {singleThreadResult}");
            }
            Console.ReadKey();
        }

        public static double CalculateSumSingleThreadWithSemaphore(int n)
        {
            double[] A = new double[n];
            double[] B = new double[n];
            double[] C = new double[n];

            for (int i = 0; i < n; i++)
            {
                A[i] = i * 0.5;
                B[i] = i * 0.3;
            }

            double sum = 0;
            semaphore = new SemaphoreSlim(1, 1);
            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < n; i++)
            {
                C[i] = Math.Sin(A[i] + B[i]);
                semaphore.Wait();
                try
                {
                    sum += C[i];
                }
                finally
                {
                    semaphore.Release();
                }
            }

            stopwatch.Stop();
            Console.WriteLine($"Время выполнения (с семафором, однопоточно): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }

    }
}
