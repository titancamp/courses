using System;
using System.Collections.Generic;
using System.Linq;

namespace _001_Linq
{
    class Program
    {
        static void Main(string[] args)
        {
            var studens = GenerateStudentsData(10).ToList();
        }

        static List<Student> Find(IEnumerable<Student> source, University university)
        {
            var list = new List<Student>();
            foreach (var item in source)
            {
                if (item.university == university)
                    list.Add(item);
            }
            return list;
        }

        static IEnumerable<Student> GenerateStudentsData(int count)
        {
            var rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                yield return new Student
                {
                    name = $"A{i + 1}",
                    surname = $"A{i + 1}yan",
                    mark = (byte)rnd.Next(1, 21),
                    age = (byte)rnd.Next(15, 35),
                    university = (University)rnd.Next(1, 4)
                };
            }
        }
    }
}
