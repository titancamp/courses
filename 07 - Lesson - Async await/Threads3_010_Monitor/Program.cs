using System;
using System.Threading;

namespace Threads3_010_Monitor
{
    /// <summary>
    /// Monitor and Sync Block (owning thread’s ID, a recursion count, and awaiting threads count)
    /// Issues of static Monitor
    /// Issues of lock(obj)
    /// lock taken
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");
        }
        
        public static void SomeMethod()
        {
            var t = new Transaction();
            Monitor.Enter(t);
            // This thread takes the object's public lock 

            // Have a thread pool thread display the LastTransaction time
            // // NOTE: The thread pool thread blocks until SomeMethod calls Monitor.Exit!
            ThreadPool.QueueUserWorkItem(o => Console.WriteLine(t.LastTransaction));

            // Execute some other code here...
            Monitor.Exit(t);
        }
        
        private void SomeLockMethod() {
            lock (this) {
                // This code has exclusive access to the data...
            }
        }
        
        private void SomeLockMethod2() {
            Boolean lockTaken = false;
            try {
                //
                Monitor.Enter(this, ref lockTaken);
                // This code has exclusive access to the data...
            }
            finally {
                if (lockTaken) Monitor.Exit(this);
            }
        } 
    }

    internal sealed class Transaction
    {
        private DateTime m_timeOfLastTrans;
        private object _lock = new object(); 

        public void PerformTransaction()
        {
            Monitor.Enter(this); // This code has exclusive access to the data...
            m_timeOfLastTrans = DateTime.Now;
            Monitor.Exit(this);
        }

        public DateTime LastTransaction {
            get {
                Monitor.Enter(this);

                // This code has exclusive access to the data...
                DateTime temp = m_timeOfLastTrans;
                Monitor.Exit(this);
                return temp;
            }
        }
    }
}