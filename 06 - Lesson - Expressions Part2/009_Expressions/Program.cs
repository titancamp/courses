using _000_DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace _009_Expressions
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();

            List<Blog> blogs1 = Dapper.Execute<Blog>("select * from Blog").ToList();
            Console.WriteLine(sw.Elapsed);
            sw = Stopwatch.StartNew();

            List<Blog> blogs2 = Dapper.Execute<Blog>("select * from Blog").ToList();
            Console.WriteLine(sw.Elapsed);
            sw = Stopwatch.StartNew();

            List<Blog> blogs3 = Dapper.Execute<Blog>("select * from Blog").ToList();
            Console.WriteLine(sw.Elapsed);
            sw = Stopwatch.StartNew();

            List<Blog> blogs4 = Dapper.Execute<Blog>("select * from Blog").ToList();
            Console.WriteLine(sw.Elapsed);

            Console.ReadLine();
        }
    }
}
