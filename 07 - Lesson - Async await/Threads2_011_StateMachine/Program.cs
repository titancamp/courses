using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Threads2_011_StateMachine
{
    class Program
    {
        internal sealed class Type1 { }

        internal sealed class Type2 { }

        private static async Task<Type1> Method1Async()
        {
            /* Does some async thing that returns a Type1 object */
            return await Task.FromResult(new Type1());
        }

        private static async Task<Type2> Method2Async()
        {
            /* Does some async thing that returns a Type2 object */
            return await Task.FromResult(new Type2());
        }

        private static async Task<string> MyMethodAsync(int argument)
        {
            var local = argument;
            try {
                var result1 = await Method1Async();
                for (var x = 0; x < 3; x++) {
                    var result2 = await Method2Async();
                }
            }
            catch (Exception) {
                Console.WriteLine("Catch");
            }
            finally {
                Console.WriteLine("Finally");
            }

            return "Done";
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        // AsyncStateMachine attribute indicates an async method (good for tools using reflection);
        // the type indicates which structure implements the state machine
        [DebuggerStepThrough, AsyncStateMachine(typeof(StateMachine))]
        private static Task<String> MyMethodAsync(Int32 argument)
        {
            // Create state machine instance & initialize it
            StateMachine stateMachine = new StateMachine()
            {
                // Create builder returning Task<String> from this stub method
                // State machine accesses builder to set Task completion/exception
                m_builder = AsyncTaskMethodBuilder<String>.Create(),
                m_state = -1, // Initialize state machine location
                m_argument = argument // Copy arguments to state machine fields
            };

            // Start executing the state machine
            stateMachine.m_builder.Start(ref stateMachine);
            return stateMachine.m_builder.Task; // Return state machine's Task
        }

        // This is the state machine structure
        [CompilerGenerated, StructLayout(LayoutKind.Auto)]
        private struct StateMachine : IAsyncStateMachine
        {
            // Fields for state machine's builder (Task) & its location
            public AsyncTaskMethodBuilder<String> m_builder;
            public Int32 m_state;

            // Argument and local variables are fields now:
            public Int32 m_argument, m_local, m_x;
            public Type1 m_resultType1;
            public Type2 m_resultType2;

            // There is 1 field per awaiter type.
// Only 1 of these fields is important at any time. That field refers
            // to the most recently executed await that is completing asynchronously:
            private TaskAwaiter<Type1> m_awaiterType1;
            private TaskAwaiter<Type2> m_awaiterType2;
            private IAsyncStateMachine asyncStateMachineImplementation;

            // This is the state machine method itself
            void IAsyncStateMachine.MoveNext()
            {
                String result = null; // Task's result value 

                // Compiler-inserted try block ensures the state machine’s task completes
                try {
                    Boolean executeFinally = true; // Assume we're logically leaving the 'try' block
                    if (m_state == -1) {
                        // If 1st time in state machine method,
                        m_local = m_argument;
                        // execute start of original method
                    }

                    // Try block that we had in our original code
                    try {
                        TaskAwaiter<Type1> awaiterType1;
                        TaskAwaiter<Type2> awaiterType2;
                        switch (m_state) {
                            case -1: // Start execution of code in 'try'
                                // Call Method1Async and get its awaiter
                                awaiterType1 = Method1Async().GetAwaiter();
                                if (!awaiterType1.IsCompleted) {
                                    m_state = 0;
                                    // 'Method1Async' is completing asynchronously
                                    m_awaiterType1 = awaiterType1; // Save the awaiter for when we come back 
                                    // Tell awaiter to call MoveNext when operation completes
                                    m_builder.AwaitUnsafeOnCompleted(ref awaiterType1, ref this);
                                    // The line above invokes awaiterType1's OnCompleted which approximately
                                    // calls ContinueWith(t => MoveNext()) on the Task being awaited.
                                    // When the Task completes, the ContinueWith task calls MoveNext 
                                    executeFinally = false;
// We're not logically leaving the 'try' block
                                    return;
// Thread returns to caller
                                }

                                // 'Method1Async' completed synchronously
                                break;
                            case 0: // 'Method1Async' completed asynchronously
                                awaiterType1 = m_awaiterType1; // Restore most-recent awaiter
                                break;
                            case 1: // 'Method2Async' completed asynchronously
                                awaiterType2 = m_awaiterType2; // Restore most-recent awaiter
                                goto ForLoopEpilog;
                        }

// After the first await, we capture the result & start the 'for' loop
                        m_resultType1 = awaiterType1.GetResult(); // Get awaiter's result 

                        ForLoopPrologue:
                        m_x = 0; // 'for' loop initialization
                        goto ForLoopBody; // Skip to 'for' loop body 

                        ForLoopEpilog:
                        m_resultType2 = awaiterType2.GetResult();
                        m_x++; // Increment x after each loop iteration
// Fall into the 'for' loop’s body 

                        ForLoopBody:
                        if (m_x < 3) {
                            // 'for' loop test
                            // Call Method2Async and get its awaiter
                            awaiterType2 = Method2Async().GetAwaiter();
                            if (!awaiterType2.IsCompleted) {
                                m_state = 1;
                                // 'Method2Async' is completing asynchronously
                                m_awaiterType2 = awaiterType2; // Save the awaiter for when we come back 


                                // Tell awaiter to call MoveNext when operation completes (see above)
                                m_builder.AwaitUnsafeOnCompleted(ref awaiterType2, ref this);
                                executeFinally = false; // We're not logically leaving the 'try' block
                                return;
                                // Thread returns to caller
                            }

                            // 'Method2Async' completed synchronously
                            goto ForLoopEpilog; // Completed synchronously, loop around
                        }
                    }
                    catch (Exception) {
                        Console.WriteLine("Catch");
                    }
                    finally {
// Whenever a thread physically leaves a 'try', the 'finally' executes
// We only want to execute this code when the thread logically leaves the 'try'
                        if (executeFinally) {
                            Console.WriteLine("Finally");
                        }
                    }

                    result = "Done"; // What we ultimately want to return from the async function
                }
                catch (Exception exception) {
                    // Unhandled exception: complete state machine's Task with exception
                    m_builder.SetException(exception);
                    return;
                }

                // No exception: complete state machine's Task with result
                m_builder.SetResult(result);
            }

            public void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.asyncStateMachineImplementation.SetStateMachine(stateMachine);
            }
        }
    }
}