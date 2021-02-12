using System;
using System.Threading;

namespace Threads
{
    public class Example 
    {
        public static void Main() 
        {
            // Queue the task.
            
            Console.WriteLine("Main thread does some work, then sleeps.");
            //Thread.Sleep(1000);

            Console.WriteLine("Main thread exits.");
            ThreadPool.QueueUserWorkItem(ThreadProc);
        }

        // This thread procedure performs the task.
        static void ThreadProc(Object stateInfo) 
        {
            // No state object was passed to QueueUserWorkItem, so stateInfo is null.
            Console.WriteLine("Hello from the thread pool.");
        }
    }
}
// The example displays output like the following:
//       Main thread does some work, then sleeps.
//       Hello from the thread pool.
//       Main thread exits.