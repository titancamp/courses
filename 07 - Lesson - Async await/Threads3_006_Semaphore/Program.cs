using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threads_006_Semaphore
{
    // We want to simulate a case, when only 10 concurrent calls are allowed to one server, not more.
    // But you have 100 calls, that have to go. No we need to implement somehow, this concurrent execution,
    // allowing only 10 calls to reach the server at a time.
    
    // E.g. Corona time -> entering a supermarket. The guard lets in only 10 people at a time :D 
    internal class Program
    {
        private static readonly Semaphore semaphore = new Semaphore(10, 10);
        
        static void Main(string[] args)
        {
            for (var i = 0; i < 100; i++) {
                Task.Run(CallToServer);
            }

            Console.ReadLine();
        }

        private static void CallToServer()
        {
            semaphore.WaitOne();
            
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} calling the server");
            Thread.Sleep(3000);
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} completed!!!");

            semaphore.Release();
        }
    }
}