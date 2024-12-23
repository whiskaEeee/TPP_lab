using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPP_lab
{
    class Lab_5
    {
        public static void ex_1()
        {
            int[] nValues = { 1000, 5000, 10000, 50000 };
            Console.WriteLine("Многопоточная реализация с секциями:");

            foreach (int n in nValues)
            {
                Console.WriteLine($"N = {n}");

                double parallelResult = CalculateSumParallel(n);
                Console.WriteLine($"Оригинал: Сумма = {parallelResult}");

                double twoSectionResult = CalculateSumParallelSections(n, 2);
                Console.WriteLine($"2 секции: Сумма = {twoSectionResult}");

                double fourSectionResult = CalculateSumParallelSections(n, 4);
                Console.WriteLine($"4 секции: Сумма = {fourSectionResult}");
            }
            Console.ReadKey();
        }

        public static double CalculateSumParallel(int n)
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
            Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.For(0, n, () => 0.0, (i, loopState, localSum) =>
            {
                C[i] = Math.Sin(A[i] + B[i]);
                return localSum + C[i];
            },
            localSum => { lock (C) sum += localSum; });

            stopwatch.Stop();
            Console.WriteLine($"Время выполнения (оригинал, многопоточно): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }

        public static double CalculateSumParallelSections(int n, int sections)
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
            Stopwatch stopwatch = Stopwatch.StartNew();

            int chunkSize = n / sections;
            Parallel.For(0, sections, section =>
            {
                double localSum = 0;
                int start = section * chunkSize;
                int end = (section == sections - 1) ? n : start + chunkSize;

                for (int i = start; i < end; i++)
                {
                    C[i] = Math.Sin(A[i] + B[i]);
                    localSum += C[i];
                }

                lock (C)
                {
                    sum += localSum;
                }
            });

            stopwatch.Stop();
            Console.WriteLine($"Время выполнения ({sections} секции): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }

        public static void ex_2()
        {
            int[] nValues = { 100000, 500000, 1000000, 5000000 };
            Console.WriteLine("Однопоточная реализация с секциями:");

            foreach (int n in nValues)
            {
                Console.WriteLine($"N = {n}");

                double singleThreadResult = CalculateSumSingleThread(n);
                Console.WriteLine($"Оригинал: Сумма = {singleThreadResult}");

                double twoSectionResult = CalculateSumSingleThreadSections(n, 2);
                Console.WriteLine($"2 секции: Сумма = {twoSectionResult}");

                double fourSectionResult = CalculateSumSingleThreadSections(n, 4);
                Console.WriteLine($"4 секции: Сумма = {fourSectionResult}");
            }
            Console.ReadKey();
        }

        public static double CalculateSumSingleThread(int n)
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
            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < n; i++)
            {
                C[i] = Math.Sin(A[i] + B[i]);
                sum += C[i];
            }

            stopwatch.Stop();
            Console.WriteLine($"Время выполнения (оригинал, однопоточно): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }

        public static double CalculateSumSingleThreadSections(int n, int sections)
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
            Stopwatch stopwatch = Stopwatch.StartNew();

            int chunkSize = n / sections;

            for (int section = 0; section < sections; section++)
            {
                double localSum = 0;
                int start = section * chunkSize;
                int end = (section == sections - 1) ? n : start + chunkSize;

                for (int i = start; i < end; i++)
                {
                    C[i] = Math.Sin(A[i] + B[i]);
                    localSum += C[i];
                }

                sum += localSum;
            }

            stopwatch.Stop();
            Console.WriteLine($"Время выполнения ({sections} секции, однопоточно): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }

    }
}
