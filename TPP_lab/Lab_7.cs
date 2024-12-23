using System;
using MPI;

namespace TPP_lab
{
    class Lab_7
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
                int N = size - 1;       // Количество "крайних" процессов
                int M = 5;              // Количество итераций обмена сообщениями

                if (rank == centralProcess)
                {
                    // Логика для центрального процесса
                    for (int iteration = 0; iteration < M; iteration++)
                    {
                        Console.WriteLine($"Центральный процесс: начало итерации {iteration + 1}.");

                        // Прием сообщений от всех крайних процессов
                        for (int i = 1; i <= N; i++)
                        {
                            string message = communicator.Receive<string>(source: i, tag: 0);
                            Console.WriteLine($"Центральный процесс получил сообщение от процесса {i}: {message}");
                        }

                        // Отправка сообщений всем крайним процессам
                        for (int i = 1; i <= N; i++)
                        {
                            communicator.Send($"Ответ от центрального процесса на итерацию {iteration + 1}.", dest: i, tag: 1);
                        }
                    }
                }
                else
                {
                    // Логика для крайних процессов
                    for (int iteration = 0; iteration < M; iteration++)
                    {
                        // Отправка сообщения центральному процессу
                        communicator.Send($"Сообщение от процесса {rank} на итерацию {iteration + 1}.", dest: centralProcess, tag: 0);

                        // Получение ответа от центрального процесса
                        string response = communicator.Receive<string>(source: centralProcess, tag: 1);
                        Console.WriteLine($"Процесс {rank} получил ответ от центрального процесса: {response}");
                    }
                }
            }
        }
    }
}
