using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Threads3_012_AsyncEvent
{
    /// <summary>
    /// Assume we want to collect event subscriptions and invoke them at some point.
    /// The subscriptions to event can be done in parallel as well as the invocation.
    /// So we need to somehow keep the subscriptions avoiding race conditions,
    /// and allow read and execution.
    ///
    /// ReaderWriterLockSlim will help with this.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
    
    public class AsyncEvent<TEventArgs> where TEventArgs : EventArgs
    {
        private readonly List<Func<object, TEventArgs, ValueTask>> _invocationList = new List<Func<object, TEventArgs, ValueTask>>();
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private readonly bool _parallelInvoke;

        public IDisposable AddHandler(Func<object, TEventArgs, ValueTask> callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            locker.EnterWriteLock();
            try {
                _invocationList.Add(callback);
            }
            finally {
                locker.ExitWriteLock();
            }

            return new EventHandlerDisposable<TEventArgs>(this, callback);
        }

        public IDisposable AddSyncHandler(Action<object, TEventArgs> callback) =>
            AddHandler((sender, args) => {
                callback(sender, args);
                return new ValueTask();
            });

        public void RemoveHandler(Func<object, TEventArgs, ValueTask> callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            locker.EnterWriteLock();
            try {
                _invocationList.Remove(callback);
            }
            finally {
                locker.ExitWriteLock();
            }
        }

        public async ValueTask InvokeAsync(object sender, TEventArgs eventArgs)
        {
            Func<object, TEventArgs, ValueTask>[] tmpInvocationList;

            locker.EnterReadLock();
            try {
                if (_invocationList.Count == 0)
                    return;
                tmpInvocationList = _invocationList.ToArray();
            }
            finally {
                locker.ExitReadLock();
            }

            if (_parallelInvoke) {
                await Task.WhenAll(tmpInvocationList.Select(callback => callback(sender, eventArgs).AsTask()));
            }
            else {
                foreach (var callback in tmpInvocationList)
                    await callback(sender, eventArgs);
            }
        }

        private class EventHandlerDisposable<T> : IDisposable
            where T : EventArgs
        {
            private readonly AsyncEvent<T> _event;
            private readonly Func<object, T, ValueTask> _callback;

            public EventHandlerDisposable(AsyncEvent<T> @event, Func<object, T, ValueTask> callback)
            {
                _event = @event;
                _callback = callback;
            }

            public void Dispose()
            {
                _event.RemoveHandler(_callback);
            }
        }

        public AsyncEvent(bool parallelInvoke = false)
        {
            _parallelInvoke = parallelInvoke;
        }
    }
}