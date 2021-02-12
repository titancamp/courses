using System;
using System.Threading;

namespace Threads2_002_Cancellation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Main Thread Started: {Thread.CurrentThread.ManagedThreadId}");
            
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            token.Register(() =>
            {
                Console.WriteLine($"I am called from thread: {Thread.CurrentThread.ManagedThreadId}");
            }, true);
            
            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine($"Inside Thread: {Thread.CurrentThread.ManagedThreadId}");
                Count(token, 1000);
            });

            Console.WriteLine("Press enter to cancel the operation");
            Console.ReadLine();
            
            cts.Cancel();
            
            Console.ReadLine();
        }

        static void Count(CancellationToken token, int count)
        {
            for (int i = 0; i < count; i++) {
                if (token.IsCancellationRequested) {
                    Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} is being cancelled.");
                    break;
                }
                
                Console.WriteLine(i);
                Thread.Sleep(200);
            }
            
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} completed.");
        }
    }
}