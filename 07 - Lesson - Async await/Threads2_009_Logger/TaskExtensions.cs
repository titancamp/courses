using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threads2_009_Logger
{
    public static class TaskExtensions
    {
        private struct Void { } // Because there isn't a non-generic TaskCompletionSource class.

        public static async Task<TResult> WithCancellation<TResult>(this Task<TResult> originalTask,
            CancellationToken ct)
        {
            // Create a Task that completes when the CancellationToken is canceled
            var cancelTask = new TaskCompletionSource<Void>();
            
            // When the CancellationToken is cancelled, complete the Task
            using (ct.Register(t => ((TaskCompletionSource<Void>) t).TrySetResult(new Void()), cancelTask)) {
                // Create a Task that completes when either the original or CancellationToken Task completes
                Task any = await Task.WhenAny(originalTask, cancelTask.Task);
                
                // If any Task completes due to CancellationToken, throw OperationCanceledException
                if (any == cancelTask.Task) ct.ThrowIfCancellationRequested();
            }

            // await original task (synchronously); if it failed, awaiting it
            // throws 1st inner exception instead of AggregateException
            return await originalTask;
        }
        
        public static async Task WithCancellation(this Task originalTask,
            CancellationToken ct)
        {
            // Create a Task that completes when the CancellationToken is canceled
            var cancelTask = new TaskCompletionSource<Void>();
            
            // When the CancellationToken is cancelled, complete the Task
            using (ct.Register(t => ((TaskCompletionSource<Void>) t).TrySetResult(new Void()), cancelTask)) {
                // Create a Task that completes when either the original or CancellationToken Task completes
                Task any = await Task.WhenAny(originalTask, cancelTask.Task);
                
                // If any Task completes due to CancellationToken, throw OperationCanceledException
                if (any == cancelTask.Task) ct.ThrowIfCancellationRequested();
            }

            // await original task (synchronously); if it failed, awaiting it
            // throws 1st inner exception instead of AggregateException
            await originalTask;
        }
    }
}