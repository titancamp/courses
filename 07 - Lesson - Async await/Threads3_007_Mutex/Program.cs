using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threads3_007_Mutex
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// E.g. Taking a school class to a swimming pool. Let's say 10 pupils are in the class.
    /// The teacher (thread) takes the pupils to the swimming pool, and provides the
    /// passing cards (.WaitOne) of the pupils to the administrator.
    /// When the second class comes, it's blocked, until the first teacher takes his/her class
    /// out of the pool. While getting back the passing cards,
    /// the teacher counts the kids (ReleaseMutex).
    /// If the number is correct, the second class can enter the pool.
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            var schoolClass1 = new SchoolClass();
            var schoolClass2 = new SchoolClass();
            var schoolClass3 = new SchoolClass();

            Task.Run(() => schoolClass1.Teacher.TakeClassToThePool());
            Task.Run(() => schoolClass2.Teacher.TakeClassToThePool());
            Task.Run(() => schoolClass3.Teacher.TakeClassToThePool());

            Console.ReadLine();
        }
    }

    internal interface IPerson
    {
        string Name { get; set; }
    }

    static class Pool
    {
        public static readonly Mutex ShirakatsiSchoolMutex = new Mutex();
    }

    class Teacher : IPerson
    {
        public string Name { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public Teacher(string name)
        {
            Name = name;
        }

        public void TakeClassToThePool()
        {
            Pool.ShirakatsiSchoolMutex.WaitOne();
            
            Console.WriteLine($"{Name}'s class is in the pool.");

            foreach (var pupil in SchoolClass.Pupils) {
                Pool.ShirakatsiSchoolMutex.WaitOne();
                Console.WriteLine($"{pupil.Name} is in the pool");
            }
            
            // Waiting for the kids to finish
            Thread.Sleep(5000);

            foreach (var pupil in SchoolClass.Pupils) {
                Pool.ShirakatsiSchoolMutex.ReleaseMutex();
            }
            
            Console.WriteLine("All kids are done");
            Pool.ShirakatsiSchoolMutex.ReleaseMutex();
        }
    }

    class Pupil : IPerson
    {
        public string Name { get; set; }

        public Pupil(string name)
        {
            Name = name;
        }
    }

    class SchoolClass
    {
        private static int counter = 0;
        public Teacher Teacher { get; set; }
        public Pupil[] Pupils { get; set; }

        public SchoolClass()
        {
            Teacher = new Teacher("Teacher: " + counter++);

            Pupils = new[]
            {
                new Pupil("Pupil 1"),
                new Pupil("Pupil 2"),
                new Pupil("Pupil 3"),
            };

            Teacher.SchoolClass = this;
        }
    }
}