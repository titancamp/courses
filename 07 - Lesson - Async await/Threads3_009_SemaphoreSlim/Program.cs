using System;
using System.Threading;

namespace Threads3_009_SemaphoreSlim
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// E.g. Do you remember the covid example,
    /// related to the bodyguard letting in max 10 people at the same time?
    /// Now assume your cashier, is figuring out the situation, when only 1 person enters
    /// the supermarket. When the second person comes,
    /// the cashier asks to wait a little. (spin count)
    /// If the person inside does not leave in time, the cashier calls in the bodyguard,
    /// otherwise the second person enters, without the need to call that heavy resource :D.
    /// That's what approximately SemaphoreSlim does.
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        internal sealed class SimpleHybridLock : IDisposable
        {
            // The Int32 is used by the primitive user-mode constructs (Interlocked methods)
            private int m_waiters = 0;

            // The AutoResetEvent is the primitive kernel-mode construct
            private readonly AutoResetEvent m_waiterLock = new AutoResetEvent(false);

            public void Enter()
            {
                var sem = new SemaphoreSlim(1);
                sem.WaitAsync();
                // Indicate that this thread wants the lock
                if (Interlocked.Increment(ref m_waiters) == 1)
                    return; // Lock was free, no contention, just return 

                // Another thread has the lock (contention), make this thread wait
                m_waiterLock.WaitOne();
                // Bad performance hit here
                // When WaitOne returns, this thread now has the lock
            }

            public void Leave()
            {
                // This thread is releasing the lock
                if (Interlocked.Decrement(ref m_waiters) == 0) return; // No other threads are waiting, just return 

                // Other threads are waiting, wake 1 of them
                m_waiterLock.Set(); // Bad performance hit here
            }

            public void Dispose()
            {
                m_waiterLock.Dispose();
            }
        }
    }
}