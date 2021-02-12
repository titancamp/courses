using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace Threads2_001_EC
{
    class Program
    {
        static void Main(string[] args)
        {
            CallContext.LogicalSetData("Data", "Some Data");
            Console.WriteLine($"Main Thread: {Thread.CurrentThread.ManagedThreadId}");
            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine(
                    $"Child thread: {Thread.CurrentThread.ManagedThreadId}," +
                    $" with data: {CallContext.LogicalGetData("Data")}");
            });

            Console.ReadLine();

            ExecutionContext.SuppressFlow();
            
            ThreadPool.QueueUserWorkItem(state =>
            {
                Console.WriteLine(
                    $"Child thread: {Thread.CurrentThread.ManagedThreadId}," +
                    $" with data: {CallContext.LogicalGetData("Data")}");
            });
            
            Console.ReadLine();
        }
    }
}