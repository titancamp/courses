using System;
using System.Threading;

namespace Threads_013_Spin
{
    internal struct SimpleSpinLock
    {
        private Int32 m_ResourceInUse; // 0=false (default), 1=true 

        public void Enter()
        {
            while (true) {
                // Always set resource to in-use 
                // When this thread changes it from not in-use, return
                if (Interlocked.Exchange(ref m_ResourceInUse, 1) == 0) return; // Black magic goes here...
            }
        }

        public void Leave()
        {
            // Set resource to not in-use
            Volatile.Write(ref m_ResourceInUse, 0);
        }
    }

    public sealed class SomeResource
    {
        private SimpleSpinLock m_sl = new SimpleSpinLock();

        public void AccessResource()
        {
            m_sl.Enter();
            // Only one thread at a time can get in here to access the resource...
            m_sl.Leave();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}