using System;
using System.Collections.Generic;

namespace _00_Linq_Referencesource
{
    //https://github.com/microsoft/referencesource
    //http://referencesource.microsoft.com/
    class Program
    {
        static void Main(string[] args)
        {
            int[] arr = {  10, 20, 30 };
            var res1 = arr.Where(p => p > 10).ToList();

            var list = new List<int> { 10, 20, 30 };
            var res2 = list.Where(p => p > 10).ToList();

            var res3 = list.Select(p => p * 2).ToList();

            var res4 = arr.Skip(1).ToList();

            Console.ReadLine();
        }
    }
}
