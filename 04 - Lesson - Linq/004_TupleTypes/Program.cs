using System;

namespace _004_TupleTypes
{
    //https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/value-tuples
    class Program
    {
        static void Main(string[] args)
        {
            DateTime dt = DateTime.Now;

            (string Name, int Year) o1 = ("A1", dt.Year);
            (string Name, int Year) o2 = ("A1", dt.Year);

            if (o1.Equals(o2))
            {

            }

            var o3 = o1;
            if(ReferenceEquals(o1, o3))
            {

            }

            var o4 = ("A1", dt.Year);
            var o5 = (Name: "A1", dt.Year);
        }
    }
}
