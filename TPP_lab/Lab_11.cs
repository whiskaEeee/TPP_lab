using System;
using MPI;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPP_lab
{
    class Lab_11
    {
        public static void ex_1(string[] args)
        {
            MPI.Environment.Run(ref args, communicator =>
            {
                int rank = communicator.Rank;
                int size = communicator.Size;

                const int N = 100; // Размер массива (больше 10,000)

                int[] createdArray = new int[N];

                if (rank == 0)
                {
                    Random rand = new Random();
                    int[] array = Enumerable.Range(0, N).Select(_ => rand.Next(0, 100001)).ToArray();
                    createdArray = array;
                    Console.WriteLine("Master process generated the array: {0}", string.Join(", ", array.Take(10)) + "...");
                }


                if (rank == 0)
                {
                    Console.WriteLine("Master process is merging sorted arrays.");
                    int[] finalSortedArray = ParallelMergeSort(createdArray);
                    Console.WriteLine("Array sorted successfully: ");
                    Console.WriteLine("Sorted array: {0}", string.Join(", ", finalSortedArray.Take(10)) + "...");
                }





            });
        }

        static int[] ParallelMergeSort(int[] array)
        {
            if (array.Length <= 1)
                return array;

            int mid = array.Length / 2;

            int[] left;
            int[] right;

            // Параллельное разделение массива
            Parallel.Invoke(
                () => left = ParallelMergeSort(array.Take(mid).ToArray()),
                () => right = ParallelMergeSort(array.Skip(mid).ToArray())
            );

            left = ParallelMergeSort(array.Take(mid).ToArray());
            right = ParallelMergeSort(array.Skip(mid).ToArray());

            // Слияние отсортированных массивов
            return Merge(left, right);
        }

        // Метод для слияния двух отсортированных массивов
        static int[] Merge(int[] left, int[] right)
        {
            int[] result = new int[left.Length + right.Length];
            int i = 0, j = 0, k = 0;

            while (i < left.Length && j < right.Length)
            {
                if (left[i] <= right[j])
                    result[k++] = left[i++];
                else
                    result[k++] = right[j++];
            }

            while (i < left.Length)
                result[k++] = left[i++];

            while (j < right.Length)
                result[k++] = right[j++];

            return result;
        }
    }

}
