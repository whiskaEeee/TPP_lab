using System;
using MPI;

namespace TPP_lab
{
    class Lab_8
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
                    Console.WriteLine($"Процесс {rank}: начало итерации {iteration + 1}.");

                    // Коллективная передача сообщения от всех процессов к центральному процессу
                    string localMessage = $"Сообщение от процесса {rank} на итерацию {iteration + 1}.";
                    string[] gatheredMessages = communicator.Gather(localMessage, centralProcess);

                    if (rank == centralProcess)
                    {
                        // Центральный процесс обрабатывает полученные сообщения
                        Console.WriteLine($"Центральный процесс: получил все сообщения на итерации {iteration + 1}.");
                        for (int i = 0; i < gatheredMessages.Length; i++)
                        {
                            Console.WriteLine($"Центральный процесс: сообщение от процесса {i}: {gatheredMessages[i]}");
                        }
                    }

                    // Центральный процесс формирует ответ
                    string broadcastMessage = $"Ответ от центрального процесса на итерацию {iteration + 1}.";
                    if (rank == centralProcess)
                    {
                        Console.WriteLine("Центральный процесс: рассылает ответное сообщение.");
                    }

                    // Рассылка ответа от центрального процесса всем процессам
                    communicator.Broadcast(ref broadcastMessage, centralProcess);
                    Console.WriteLine($"Процесс {rank}: получил сообщение: {broadcastMessage}");
                }
            }
        }
    }
}
