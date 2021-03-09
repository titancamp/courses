using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threads3_005_AutoResetEvent
{
    /// <summary>
    /// Assume we need to load the same resource from the server, from different pages.
    /// The loading takes around 15 seconds to load.
    /// The user can navigate from page to page during that time,
    /// causing the same call, for the same resource done couple of times.
    ///
    /// Now we will have couple of calls and the data will be loaded in couple of places.
    /// </summary>
    /// <remarks>
    /// E.g. Club face control on the entrance, the guard let's in one by one, once someone let's the
    /// guard know that he can let one in.
    /// </remarks>
    internal class Program
    {
        private static readonly AutoResetEvent AutoResetEvent = new AutoResetEvent(true);
        
        private static int dataFromServer = -1;
        
        private static void Main()
        {
            Task.Run(ChangeLocalNumberInThread);
            Task.Run(ChangeLocalNumberInThread);
            Task.Run(ChangeLocalNumberInThread);
            Task.Run(ChangeLocalNumberInThread);
            Console.ReadLine();
        }
        
        private static void ChangeLocalNumberInThread()
        {
            AutoResetEvent.WaitOne();

            if (dataFromServer == -1) {
                Console.WriteLine("Loading data...");
                Thread.Sleep(2000);
                dataFromServer = 4444;
                Console.WriteLine("Data loaded and cached, no extra loading needed...");
            }
            
            Console.WriteLine($"I changed the local from thread: {Thread.CurrentThread.ManagedThreadId}, data: {dataFromServer}");
            AutoResetEvent.Set();
        }
    }
}