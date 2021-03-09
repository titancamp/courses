using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Threads3_001_InterlockedAny
{
    class Program
    {
        private static List<int> store = new List<int>();
        private static Random random = new Random();
        
        static void Main(string[] args)
        {
            InitializeData(store, 1000000);

            var max = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //max = store.Max();
            Parallel.For(0, store.Count - 2, i => Maximum(ref max, store[i]));
            stopwatch.Stop();
            
            Console.WriteLine($"Max: {max}, time: {stopwatch.ElapsedMilliseconds}");
            Console.ReadLine();
        }

        private static void InitializeData(List<int> store, int count)
        {
            for (var i = 0; i < count; i++) {
                store.Add(random.Next(1, count));
            }
        }

        public static int Maximum(ref int target, int value)
        {
            int currentVal = target, startVal, desiredVal;

            // Don't access target in the loop except in an attempt
            // to change it because another thread may be touching it
            do {
                // Record this iteration's starting value
                startVal = currentVal;

                // Calculate the desired value in terms of startVal and value
                desiredVal = Math.Max(startVal, value);

                // NOTE: the thread could be preempted here! 

                // if (target == startVal) target = desiredVal
                // Value prior to potential change is returned
                currentVal = Interlocked.CompareExchange(ref target, desiredVal, startVal);

                // If the starting value changed during this iteration, repeat
            } while (startVal != currentVal);

            // Return the maximum value when this thread tried to set it
            return desiredVal;
        }
    }
}