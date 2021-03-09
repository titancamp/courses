using System;
using System.Threading;

namespace Threads3_003_SimpleWaitLock
{
    public class SimpleWaitLock : IDisposable
    {
        private readonly AutoResetEvent autoResetEvent;

        public void Enter()
        {
            this.autoResetEvent.WaitOne();
        }

        public void Leave()
        {
            this.autoResetEvent.Set();
        }

        public SimpleWaitLock()
        {
            this.autoResetEvent = new AutoResetEvent(true);
        }

        public void Dispose()
        {
            this.autoResetEvent?.Dispose();
        }
    }
}