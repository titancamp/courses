using System;
using System.Threading;

namespace Threads_010_SharingData
{
    class Program
    {
        private int flag = 0;
        private int @value = 0;

        public void Thread1()
        {
            // This can execute it reverse order
            this.@value = 5;
            //this.flag = 1;
            Volatile.Write(ref this.flag, 1);
        }
        
        public void Thread2()
        {
            // NOTE: @value can be read before flag
            if (Volatile.Read(ref this.flag) == 1) {
                Console.WriteLine(this.@value);
            }
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var program = new Program();
            
            var thread1 = new Thread(program.Thread1);
            var thread2 = new Thread(program.Thread2);
            
            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Console.ReadLine();
        }
    }
}