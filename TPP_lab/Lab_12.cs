using System;
using MPI;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TPP_lab
{
    class Lab_12
    {
        public static void ex_1(string[] args)
        {
            using (new MPI.Environment(ref args))
            {
                Intracommunicator comm = Communicator.world;
                int rank = comm.Rank;
                int size = comm.Size;

                // Проверяем, достаточно ли процессов для двух групп
                if (size < 2)
                {
                    if (rank == 0)
                    {
                        Console.WriteLine("Необходимо как минимум 2 процесса.");
                    }
                    return;
                }

                // Разделяем процессы на две группы
                int color = rank % 2; // 0 - кольцевая группа, 1 - звезда
                Intracommunicator groupComm = (Intracommunicator)comm.Split(color, rank);

                if (color == 0)
                {
                    // Кольцевая топология
                    RingCommunication(groupComm);
                }
                else
                {
                    // Звезда (master-slave)
                    StarCommunication(groupComm);
                }
            }
        }

        static void RingCommunication(Intracommunicator comm)
        {
            int rank = comm.Rank;
            int size = comm.Size;

            // Сдвиг информации в кольце
            int sendValue = rank; // Отправляем значение, равное номеру процесса
            int recvValue = -1;

            int left = (rank - 1 + size) % size;
            int right = (rank + 1) % size;

            // Обмен сообщениями: отправка и получение без ref
            comm.Send(sendValue, right, 0);
            recvValue = comm.Receive<int>(left, 0);

            Console.WriteLine($"[Ring] Процесс {rank} получил {recvValue} от процесса {left}");
        }


        static void StarCommunication(Intracommunicator comm)
        {
            int rank = comm.Rank;
            int size = comm.Size;

            if (rank == 0)
            {
                // Master
                for (int i = 1; i < size; i++)
                {
                    string message = $"Привет от мастера к процессу {i}";
                    comm.Send(message, i, 0);
                    string response = comm.Receive<string>(i, 0);
                    Console.WriteLine($"[Star] Мастер получил ответ: {response}");
                }
            }
            else
            {
                // Slave
                string message = comm.Receive<string>(0, 0);
                Console.WriteLine($"[Star] Процесс {rank} получил сообщение: {message}");
                comm.Send($"Ответ от процесса {rank}", 0, 0);
            }
        }
    }

}
