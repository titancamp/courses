using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Threads_012_Interlocked
{
    internal enum CoordinationStatus
    {
        AllDone,
        Timeout,
        Cancel
    };

    internal sealed class MultiWebRequests
    {
        // This helper class coordinates all the asynchronous operations
        private AsyncCoordinator m_ac = new AsyncCoordinator();

        // Set of Web servers we want to query & their responses (Exception or Int32)
        // NOTE: Even though multiple could access this dictionary simultaneously,
        // there is no need to synchronize access to it since the keys are
        // read-only after construction
        private Dictionary<string, object> m_servers = new Dictionary<string, object>
        {
            {"http://Wintellect.com/", null},
            {"https://www.microsoft.com/", null},
            {"http://1.1.1.0/", null}
        };

        public MultiWebRequests(int timeout = Timeout.Infinite)
        {
            // Asynchronously initiate all the requests all at once
            var httpClient = new HttpClient();
            foreach (var server in m_servers.Keys) {
                m_ac.AboutToBegin(1);
                httpClient.GetByteArrayAsync(server)
                    .ContinueWith(task => ComputeResult(server, task));
            }

            // Tell AsyncCoordinator that all operations have been initiated and to call
            // AllDone when all operations complete, Cancel is called, or the timeout occurs
            m_ac.AllBegun(AllDone, timeout);
        }

        private void ComputeResult(string server, Task<byte[]> task)
        {
            object result;
            if (task.Exception != null) {
                result = task.Exception.InnerException;
            }
            else {
                // Process I/O completion here on thread pool thread(s)
                // Put your own compute-intensive algorithm here...
                result = task.Result.Length; // This example just returns the length
            }

            // Save result (exception/sum) and indicate that 1 operation completed
            m_servers[server] = result;
            m_ac.JustEnded();
        }

        // Calling this method indicates that the results don't matter anymore
        public void Cancel()
        {
            m_ac.Cancel();
        }

        // This method is called after all Web servers respond,
        // Cancel is called, or the timeout occurs
        private void AllDone(CoordinationStatus status)
        {
            switch (status) {
                case CoordinationStatus.Cancel:
                    Console.WriteLine("Operation canceled.");
                    break;
                case CoordinationStatus.Timeout:
                    Console.WriteLine("Operation timed-out.");
                    break;
                case CoordinationStatus.AllDone:
                    Console.WriteLine("Operation completed; results below:");
                    foreach (var server in m_servers) {
                        Console.Write("{0} ", server.Key);
                        var result = server.Value;
                        if (result is Exception) {
                            Console.WriteLine("failed due to {0}.", result.GetType().Name);
                        }
                        else {
                            Console.WriteLine("returned {0:N0} bytes.", result);
                        }
                    }

                    break;
            }
        }
    }

    internal sealed class AsyncCoordinator
    {
        private int m_opCount = 1; // Decremented when AllBegun calls JustEnded
        private int m_statusReported = 0; // 0=false, 1=true
        private Action<CoordinationStatus> m_callback;

        private Timer m_timer;

        // This method MUST be called BEFORE initiating an operation
        public void AboutToBegin(int opsToAdd = 1)
        {
            Interlocked.Add(ref m_opCount, opsToAdd);
        }

        // This method MUST be called AFTER an operation’s result has been processed
        public void JustEnded()
        {
            if (Interlocked.Decrement(ref m_opCount) == 0)
                ReportStatus(CoordinationStatus.AllDone);
        }

        // This method MUST be called AFTER initiating ALL operations
        public void AllBegun(Action<CoordinationStatus> callback,
            int timeout = Timeout.Infinite)
        {
            m_callback = callback;
            if (timeout != Timeout.Infinite)
                m_timer = new Timer(TimeExpired, null, timeout, Timeout.Infinite);
            JustEnded();
        }

        private void TimeExpired(object o)
        {
            ReportStatus(CoordinationStatus.Timeout);
        }

        public void Cancel()
        {
            ReportStatus(CoordinationStatus.Cancel);
        }

        private void ReportStatus(CoordinationStatus status)
        {
            // If status has never been reported, report it; else ignore it
            if (Interlocked.Exchange(ref m_statusReported, 1) == 0)
                m_callback(status);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var mwr = new MultiWebRequests(2000);
            Console.ReadLine();
        }
    }
}