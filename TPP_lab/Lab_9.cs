using System;
using MPI;

namespace TPP_lab
{
    class Lab_9
    {
        public static void ex_1(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                var communicator = Communicator.world;
                int rank = communicator.Rank; // Номер текущего процесса
                int size = communicator.Size; // Общее количество процессов

                if (size < 2)
                {
                    Console.WriteLine("Для работы программы необходимо как минимум 2 процесса.");
                    return;
                }

                int centralProcess = 0; // Центральный процесс всегда имеет ранг 0
                int M = 5;              // Количество итераций

                for (int iteration = 0; iteration < M; iteration++)
                {
                    if (rank == centralProcess)
                    {
                        // Центральный процесс: получение сообщений от крайних процессов
                        Console.WriteLine($"Центральный процесс: начало итерации {iteration + 1}.");

                        for (int i = 1; i < size; i++) // Получение сообщений от процессов 1, 2, ...
                        {
                            string message = communicator.Receive<string>(source: i, tag: iteration);
                            Console.WriteLine($"Центральный процесс получил сообщение от процесса {i}: {message}");
                        }

                        // Центральный процесс: отправка ответа всем крайним процессам
                        for (int i = 1; i < size; i++)
                        {
                            string response = $"Ответ от центрального процесса на итерацию {iteration + 1}.";
                            communicator.Send(response, dest: i, tag: iteration);
                        }
                    }
                    else
                    {
                        // Крайние процессы: отправка сообщения центральному процессу
                        string message = $"Сообщение от процесса {rank} на итерацию {iteration + 1}.";
                        communicator.Send(message, dest: centralProcess, tag: iteration);

                        // Крайние процессы: прием ответа от центрального процесса
                        string response = communicator.Receive<string>(source: centralProcess, tag: iteration);
                        Console.WriteLine($"Процесс {rank} получил сообщение от центрального процесса: {response}");
                    }
                }
            }
        }
    }
}
