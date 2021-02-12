using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Threads2_009_Logger
{
    public static class TaskLogger
    {
        public enum TaskLogLevel
        {
            None,
            Pending
        }
        
        public static TaskLogLevel LogLevel { get; set; }

        public sealed class TaskLogEntry
        {
            public Task Task { get; internal set; }
            public string Tag { get; internal set; }
            public DateTime LogTime { get; internal set; }
            public string CallerMemberName { get; internal set; }
            public string CallerFilePath { get; internal set; }
            public int CallerLineNumber { get; internal set; }

            public override string ToString()
            {
                return string.Format("LogTime={0}, Tag={1}, Member={2}, File={3}({4})",
                    LogTime, Tag ?? "(none)", CallerMemberName, CallerFilePath, CallerLineNumber);
            }
        }

        private static readonly ConcurrentDictionary<Task, TaskLogEntry> s_log =
            new ConcurrentDictionary<Task, TaskLogEntry>();

        public static IEnumerable<TaskLogEntry> GetLogEntries()
        {
            return s_log.Values;
        }

        public static Task<TResult> Log<TResult>(this Task<TResult> task, string tag = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return (Task<TResult>) Log((Task) task, tag, callerMemberName, callerFilePath, callerLineNumber);
        }

        public static Task Log(this Task task, string tag = null,
            [CallerMemberName] string callerMemberName = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = -1)
        {
            if (LogLevel == TaskLogLevel.None) return task;
            var logEntry = new TaskLogEntry
            {
                Task = task,
                LogTime = DateTime.Now,
                Tag = tag,
                CallerMemberName = callerMemberName,
                CallerFilePath = callerFilePath,
                CallerLineNumber = callerLineNumber
            };
            s_log[task] = logEntry;
            task.ContinueWith(t =>
                {
                    TaskLogEntry entry;
                    s_log.TryRemove(t, out entry);
                },
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }
    } 
    
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                await Go();
            });

            Console.ReadLine();
        }

        public static async Task Go()
        {
#if DEBUG
            // Using TaskLogger incurs a memory and performance hit; so turn it on in debug builds
            TaskLogger.LogLevel = TaskLogger.TaskLogLevel.Pending;
#endif
            // Initiate 3 task; for testing the TaskLogger, we control their duration explicitly
            var tasks = new List<Task>
            {
                Task.Delay(2000).Log("2s op"),
                Task.Delay(5000).Log("5s op"),
                Task.Delay(6000).Log("6s op"),
                Task.Run(() => Task.Delay(4000)).Log("Test"),
            };
            try {
                // Wait for all tasks but cancel after 3 seconds; only 1 task above should complete in time
                await Task.WhenAll(tasks).WithCancellation(new CancellationTokenSource(3000).Token);
            }
            catch (OperationCanceledException) { }

            // Ask the logger which tasks have not yet completed and sort
            // them in order from the one that’s been waiting the longest
            foreach (var op in TaskLogger.GetLogEntries().OrderBy(tle => tle.LogTime))
                Console.WriteLine(op);
        }
    }
}