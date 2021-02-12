using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Threads2_006_Parallel
{
    class Program
    {
        private static int[] data;
        private static List<int> threads = new List<int>(1000000);
        static void MyTransform(int i)
        {
            threads.Add(Thread.CurrentThread.ManagedThreadId);
            data[i] = data[i] / 10;
            if (data[i] < 10000) data[i] = 0;
            if (data[i] > 10000 && data[i] < 20000) data[i] = 100;
            if (data[i] > 20000 && data[i] < 30000) data[i] = 200;
            if (data[i] > 30000) data[i] = 300;
        }

        static void MyTransform(int[] arr)
        {
            for (var i = 0; i < arr.Length; i++) {
                arr[i] = arr[i] / 10;
                if (arr[i] < 10000) arr[i] = 0;
                if (arr[i] > 10000 && arr[i] < 20000) arr[i] = 100;
                if (arr[i] > 20000 && arr[i] < 30000) arr[i] = 200;
                if (arr[i] > 30000) arr[i] = 300;
            }
        }
        
        static void Main()
        {
            Console.WriteLine("Starting Main Thread.");

            data = new int[100000000];

            // Data initialization
            for (var i = 0; i < data.Length; i++)
                data[i] = i;

            var sw = new Stopwatch();
            sw.Start();

            Parallel.For(0, data.Length, MyTransform);
            //MyTransform(data);
            
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            Console.WriteLine("Main Thread completed.");

            Console.ReadLine();
            
            Console.WriteLine(threads.Count);
            Console.WriteLine(threads.Distinct().Count());
            Console.ReadLine();
        }
    }
}