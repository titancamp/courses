using System;
using System.Threading;

namespace Threads3_011_ReaderWriterLockSlim
{
    /// <summary>
    /// • When one thread is writing to the data,
    /// all other threads requesting access are blocked.
    /// • When one thread is reading from the data,
    /// other threads requesting read access are allowed to continue executing,
    /// but threads requesting write access are blocked.
    /// • When a thread writing to the data has completed,
    /// either a single writer thread is unblocked so it can access the data or all
    /// the reader threads are unblocked so that all of them can access the data concurrently.
    /// If no threads are blocked, then the lock is free and available for the next reader
    /// or writer thread that wants it.
    /// • When all threads reading from the data have completed,
    /// a single writer thread is unblocked so it can access the data.
    /// If no threads are blocked, then the lock is free and available for the next reader
    /// or writer thread that wants it. 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    internal sealed class Transaction : IDisposable
    {
        private readonly ReaderWriterLockSlim m_lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private DateTime m_timeOfLastTrans;

        public void PerformTransaction()
        {
            var sem = new SemaphoreSlim(1);
            sem.Wait();
            m_lock.EnterWriteLock();
            // This code has exclusive access to the data...
            m_timeOfLastTrans = DateTime.Now;
            m_lock.ExitWriteLock();
        }

        public DateTime LastTransaction {
            get {
                m_lock.EnterReadLock(); // This code has shared access to the data...
                DateTime temp = m_timeOfLastTrans;
                m_lock.ExitReadLock();
                return temp;
            }
        }

        public void Dispose()
        {
            m_lock.Dispose();
        }
    }
}