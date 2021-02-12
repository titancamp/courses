using System;

namespace Threads_001
{
    using System;
    using System.Threading;

    public class ThreadExample
    {
        public static void Main()
        {
            Console.WriteLine($"Main thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine("Main thread: Start a second thread.");
            var t = new Thread(ThreadProc);

            // How will this behave on uniprocessor?
            t.Start();
            Thread.Sleep(20);

            for (int i = 0; i < 4; i++) {
                Console.WriteLine("Main thread: Do some work.");
                Thread.Sleep(0);
            }

            Console.WriteLine("Main thread: Call Join(), to wait until ThreadProc ends.");
            t.Join();
            Console.WriteLine("Main thread: ThreadProc.Join has returned.  Press Enter to end program.");
            Console.ReadLine();
        }
        
        public static void ThreadProc()
        {
            Console.WriteLine($"Thread: {Thread.CurrentThread.GetHashCode()}");
            for (var i = 0; i < 10; i++) {
                Console.WriteLine("ThreadProc: {0}", i);
                // Yield the rest of the time slice.
                Thread.Sleep(0);
            }
        }
    }
}