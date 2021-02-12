using System;
using System.Threading;

namespace Threads_005_Join
{
    internal class Program
    {
        private static void Main()
        {
            var thread = new Thread(() => ThreadFunc(ConsoleColor.Yellow));
            var thread1 = new Thread(() => ThreadFunc(ConsoleColor.Red));
            var thread2 = new Thread(ParameterizedThreadFunc);

            Console.WriteLine("Main thread id: {0} \n", Thread.CurrentThread.GetHashCode());

            thread.Start();
            thread.Join();

            thread1.Start();
            thread1.Join();
            
            thread2.Start(ConsoleColor.DarkCyan);
            thread2.Join();

            Console.ForegroundColor = ConsoleColor.Green;

            for (var i = 0; i < 160; i++) {
                Thread.Sleep(20);
                Console.Write("_");
            }

            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("\nMain thread ended.");

            Console.ReadLine();
        }

        private static void ThreadFunc(ConsoleColor color)
        {
            Console.WriteLine("Secondary thread id: {0}", Thread.CurrentThread.ManagedThreadId);
            Console.ForegroundColor = color;

            for (var i = 0; i < 160; i++) {
                Thread.Sleep(20);
                Console.Write(".");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nSecondary thread ended.");
        }

        private static void ParameterizedThreadFunc(object state)
        {
            ConsoleColor color = default;
            if (state is ConsoleColor consoleColor) {
                color = consoleColor;
            }
            
            Console.WriteLine("Secondary thread id: {0}", Thread.CurrentThread.ManagedThreadId);
            Console.ForegroundColor = color;

            for (var i = 0; i < 160; i++) {
                Thread.Sleep(20);
                Console.Write(".");
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\nSecondary thread ended.");
        }
    }
}