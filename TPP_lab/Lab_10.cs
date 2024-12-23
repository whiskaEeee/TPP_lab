using System;
using MPI;
using System.Numerics;
using System.Collections.Generic;

namespace TPP_lab
{
    class Lab_10
    {
        public static void ex_1(string[] args)
        {
            MPI.Environment.Run(ref args, communicator =>
            {
                int rank = communicator.Rank;
                int size = communicator.Size;

                int[] polynomials = { 2, 3, 1, 2, 1, 4, 0, 1};

                // Example polynomials
                Polynomial p1 = new Polynomial();
                p1.AddTerm(2, 3); // 3x^2
                p1.AddTerm(1, 2); // 2x^1

                Polynomial p2 = new Polynomial();
                p2.AddTerm(1, 4); // 4x^1
                p2.AddTerm(0, 1); // 1x^0

                // Each process will compute the product of all polynomials
                List<int> results = new List<int>();
                for (int i = 0; i < polynomials.Length; i++)
                {
                    results.Add(Polynomial.Multiplly(polynomials[i]));
                }

                // Gather results at the root process
                Polynomial result = p1.Multiply(p2);

                // Combine results at root
                if (rank == 0)
                {
                    Polynomial finalResult = p2;
                    for (int i = 1; i < results.Count; i++)
                    {
                        finalResult = Polynomial.MultiplyFinaly(result, results[i]);
                    }
                    Console.WriteLine("Final Result: " + finalResult);
                }
            });
        }
    }
}
