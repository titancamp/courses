using System;
using System.Collections.Generic;
using System.Threading;

namespace Threads_008_Reader
{
    class Program
    {
        private static Dictionary<int, int> dic = new Dictionary<int, int>();
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var creater = new Thread(Creater);
            creater.Start();

            var remover = new Thread(Remover);
            remover.Start();

            var reader = new Thread(Reader);
            reader.Start();

            Console.ReadLine();
        }

        private static void Reader()
        {
            while (true) {
                int number = 10;

                // if (dic.ContainsKey(number))
                //     Console.WriteLine(dic[number]);

                dic.TryGetValue(number, out var num);
                Console.WriteLine(num);
            }
        }

        private static void Creater()
        {
            while (true) {
                int number = 10;
                try {
                    dic[10] = 10;
                }
                catch { }
            }
        }

        private static void Remover()
        {
            while (true) {
                int number = 10;
                dic.Remove(number);
            }
        }
    }
}