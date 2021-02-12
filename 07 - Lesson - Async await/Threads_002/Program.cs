namespace Threads_002
{
    using System;
    using System.Threading;

    // Foreground vs Background
    public class Example
    {
        static readonly object lockObject = new object();
   
        public static void Main()
        {
            ThreadPool.QueueUserWorkItem(ShowThreadInformation);
            var th1 = new Thread(ShowThreadInformation);
            th1.Start();
            var th2 = new Thread(ShowThreadInformation) {IsBackground = true};
            th2.Start();
            Thread.Sleep(500);
            ShowThreadInformation(null); 
        }
   
        private static void ShowThreadInformation(object state)
        {
            lock (lockObject) {
                var th = Thread.CurrentThread;
                Console.WriteLine("Managed thread #{0}: ", th.GetHashCode());
                Console.WriteLine("   Background thread: {0}", th.IsBackground);
                Console.WriteLine("   Thread pool thread: {0}", th.IsThreadPoolThread);
                Console.WriteLine("   Priority: {0}", th.Priority);
                Console.WriteLine("   Culture: {0}", th.CurrentCulture.Name);
                Console.WriteLine("   UI culture: {0}", th.CurrentUICulture.Name);
                Console.WriteLine();
            }
        }
    }
}