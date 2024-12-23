using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPP_lab
{
    internal class Utilities
    {
        public static int CountOnesInBinary(int number)
        {
            int count = 0;
            while (number > 0)
            {
                count += number & 1;
                number >>= 1;
            }
            return count;
        }

        public static void CalculateRowSums(int[][] matrix, int[] results)
        {
            int rows = matrix.Length;
            Parallel.For(0, rows, i =>
            {
                int sum = 0;
                int cols = matrix[i].Length;

                for (int j = 0; j < cols; j++)
                {
                    for (int k = j + 1; k < cols; k++)
                    {
                        int product = matrix[i][j] * matrix[i][k];
                        sum += CountOnesInBinary(product);
                    }
                }
                results[i] = sum;
            });
        }

        public static void CalculateRowSumsSequential(int[][] matrix, int[] results)
        {
            int rows = matrix.Length;

            for (int i = 0; i < rows; i++)
            {
                int sum = 0;
                int cols = matrix[i].Length;

                for (int j = 0; j < cols; j++)
                {
                    for (int k = j + 1; k < cols; k++)
                    {
                        int product = matrix[i][j] * matrix[i][k];
                        sum += CountOnesInBinary(product);
                    }
                }

                results[i] = sum;
            }
        }

        public static TimeSpan MeasureExecutionTime(int nmax, int limit, bool parallel)
        {
            float[,] a = new float[nmax, nmax];
            for (int i = 0; i < nmax; i++)
                for (int j = 0; j < nmax; j++)
                    a[i, j] = i + j;

            Stopwatch stopwatch = Stopwatch.StartNew();

            if (parallel && nmax > limit)
            {
                Parallel.For(0, nmax, i =>
                {
                    float sum = 0;
                    for (int j = 0; j < nmax; j++)
                        sum += a[i, j];
                });
            }
            else
            {
                for (int i = 0; i < nmax; i++)
                {
                    float sum = 0;
                    for (int j = 0; j < nmax; j++)
                        sum += a[i, j];
                }
            }

            stopwatch.Stop();
            return stopwatch.Elapsed;
            
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
            Console.WriteLine($"Время выполнения (многопоточно): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }

        /*public static double CalculateSumSingleThread(int n)
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
            Console.WriteLine($"Время выполнения (однопоточно): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }*/

        public static double CalculateSumWithAtomic(int n)
        {
            double[] A = new double[n];
            double[] B = new double[n];
            double[] C = new double[n];

            for (int i = 0; i < n; i++)
            {
                A[i] = i * 0.5;
                B[i] = i * 0.3;
            }

            long sumBits = 0;

            Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.For(0, n, i =>
            {
                C[i] = Math.Sin(A[i] + B[i]);
                Interlocked.Add(ref sumBits, BitConverter.DoubleToInt64Bits(C[i]));
            });

            double sum = BitConverter.Int64BitsToDouble(sumBits);
            stopwatch.Stop();
            Console.WriteLine($"Время выполнения (atomic): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }

        public static double CalculateSumWithCritical(int n)
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
            object lockObj = new object();

            Stopwatch stopwatch = Stopwatch.StartNew();

            Parallel.For(0, n, i =>
            {
                C[i] = Math.Sin(A[i] + B[i]);
                lock (lockObj)
                {
                    sum += C[i];
                }
            });

            stopwatch.Stop();
            Console.WriteLine($"Время выполнения (critical): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }

        public static double CalculateSumParallelWithBarrier(int n)
        {
            double[] A = new double[n];
            double[] B = new double[n];
            double[] C = new double[n];

            // Инициализация массивов
            for (int i = 0; i < n; i++)
            {
                A[i] = i * 0.5;
                B[i] = i * 0.3;
            }

            double sum = 0;

            // Количество потоков равно количеству логических процессоров
            int threadCount = Environment.ProcessorCount;

            // Создаем барьер с количеством участников, равным числу потоков
            Barrier barrier = new Barrier(threadCount,
                b => Console.WriteLine($"Все потоки завершили фазу {b.CurrentPhaseNumber}."));

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Распределяем задачи между потоками
            Parallel.For(0, threadCount, threadIndex =>
            {
                int chunkSize = n / threadCount;
                int start = threadIndex * chunkSize;
                int end = (threadIndex == threadCount - 1) ? n : start + chunkSize; // Последний поток обрабатывает остаток

                double localSum = 0;

                for (int i = start; i < end; i++)
                {
                    C[i] = Math.Sin(A[i] + B[i]);
                    localSum += C[i];
                }

                // Синхронизация между потоками
                barrier.SignalAndWait();

                lock (C) // Безопасное добавление результата
                {
                    sum += localSum;
                }
            });

            stopwatch.Stop();
            Console.WriteLine($"Время выполнения (многопоточно с барьером): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
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
            Console.WriteLine($"Время выполнения (однопоточно): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }


        public static double CalculateSumParallelWithoutBarrier(int n)
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
            Console.WriteLine($"Время выполнения (многопоточно без барьера): {stopwatch.ElapsedMilliseconds} ms");

            return sum;
        }
    }
}
