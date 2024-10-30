using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPP_lab
{
    class Lab_1
    {

        public static void ex_1()
        {
            int M = 100;
            int N = 100;

            Random rand = new Random();

            int[][] matrix = new int[M][];
            for (int i = 0; i < M; i++)
            {
                matrix[i] = new int[N];
                for (int j = 0; j < N; j++)
                {
                    matrix[i][j] = rand.Next(1, 100);
                }
            }

            int[] results = new int[M];

            Stopwatch stopwatch = Stopwatch.StartNew();
            Utilities.CalculateRowSums(matrix, results);
            stopwatch.Stop();

            Console.WriteLine("Результаты (сумма единиц для каждой строки):");
            for (int i = 0; i < M; i++)
            {
                Console.WriteLine($"Строка {i}: {results[i]}");
            }
            Console.WriteLine($"Время выполнения с параллелизмом: {stopwatch.Elapsed.TotalMilliseconds} мс");

            stopwatch.Restart();
            for (int i = 0; i < M; i++)
            {
                int sum = 0;
                for (int j = 0; j < N; j++)
                {
                    for (int k = j + 1; k < N; k++)
                    {
                        int product = matrix[i][j] * matrix[i][k];
                        sum += Utilities.CountOnesInBinary(product);
                    }
                }
                results[i] = sum;
            }
            stopwatch.Stop();

            Console.WriteLine($"Время выполнения без параллелизма: {stopwatch.Elapsed.TotalMilliseconds} мс");
            Console.ReadKey();
        }

        public static void ex_2()
        {
            int M = 100;
            int N = 100;

            Random rand = new Random();

            int[][] matrix = new int[M][];
            for (int i = 0; i < M; i++)
            {
                matrix[i] = new int[N];
                for (int j = 0; j < N; j++)
                {
                    matrix[i][j] = rand.Next(1, 100);
                }
            }

            int[] results = new int[M];

            Stopwatch stopwatch = Stopwatch.StartNew();
            Utilities.CalculateRowSumsSequential(matrix, results);
            stopwatch.Stop();

            Console.WriteLine("Результаты (сумма единиц для каждой строки):");
            for (int i = 0; i < M; i++)
            {
                Console.WriteLine($"Строка {i}: {results[i]}");
            }

            Console.WriteLine($"Время выполнения без параллелизма: {stopwatch.Elapsed.TotalMilliseconds} мс");
            Console.ReadKey();
        }
    }
}
