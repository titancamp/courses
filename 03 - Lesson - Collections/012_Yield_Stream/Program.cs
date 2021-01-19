using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace _012_Yield_Stream
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream stream = new FileStream("test.txt", FileMode.Open, FileAccess.Read);

            var aaaaa = Environment.NewLine;
            var aa = stream.FirstLine().ToList();
            string text = ToString(aa);
            //foreach (byte item in stream.AsEnumerable())
            //{
            //    Console.Write((char)item);
            //}

            //foreach (var item in stream.Lines())
            //{
            //    Console.WriteLine(ToString(item));
            //}

            stream.Dispose();

            Console.ReadLine();
        }

        static string ToString(List<byte> source)
        {
            var builder = new StringBuilder(source.Count);
            foreach (var item in source)
            {
                builder.Append((char)item);
            }
            return builder.ToString();
        }

    }
}
