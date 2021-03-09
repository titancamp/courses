using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Threads3_003_SimpleWaitLock;

namespace Threads3_004_SimpleWaitLockBenchmark
{
    class Program
    {
        public static void Main()
        {
            var x = 0;
            const int iterations = 10000000; // 10 million 

            // How long does it take to increment x 10 million times?
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < iterations; i++) {
                x++;
            }

            Console.WriteLine("Incrementing x: {0:N0}", sw.ElapsedMilliseconds);

            // How long does it take to increment x 10 million times
            // adding the overhead of calling a method that does nothing?
            sw.Restart();
            for (var i = 0; i < iterations; i++) {
                M();
                x++;
                M();
            }

            Console.WriteLine("Incrementing x in M: {0:N0}", sw.ElapsedMilliseconds);

            // How long does it take to increment x 10 million times
            // adding the overhead of calling an uncontended SimpleSpinLock?
            var sl = new SpinLock(false);
            sw.Restart();
            for (var i = 0; i < iterations; i++) {
                var taken = false;
                sl.Enter(ref taken);
                x++;
                sl.Exit();
            }

            Console.WriteLine("Incrementing x in SpinLock: {0:N0}", sw.ElapsedMilliseconds);

            // How long does it take to increment x 10 million times
            // adding the overhead of calling an uncontended SimpleWaitLock?
            using (var swl = new SimpleWaitLock()) {
                sw.Restart();
                for (var i = 0; i < iterations; i++) {
                    swl.Enter();
                    x++;
                    swl.Leave();
                }

                Console.WriteLine("Incrementing x in SimpleWaitLock: {0:N0}", sw.ElapsedMilliseconds);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void M()
        {
             /* This method does nothing but return */
        } 
    }
}