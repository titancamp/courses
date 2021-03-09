using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Threads3_013_AsyncEnumerable
{
    internal class Program
    {
        private static async Task Main()
        {
            var mode = Console.ReadLine();

            if (mode?.ToLowerInvariant() == "async") {
                await foreach (var dataPoint in FetchDataStream()) {
                    Console.WriteLine(dataPoint);
                }
            }
            else {
                foreach (var dataPoint in await FetchData()) {
                    Console.WriteLine(dataPoint);
                }
            }

            Console.ReadLine();
        }

        private static async Task<IEnumerable<int>> FetchData()
        {
            var dataPoints = new List<int>();
            for (var i = 1; i <= 10; i++) {
                await Task.Delay(1000); //Simulate waiting for data to come through. 
                dataPoints.Add(i);
            }

            return dataPoints;
        }

        private static async IAsyncEnumerable<int> FetchDataStream()
        {
            for (var i = 1; i <= 10; i++) {
                await Task.Delay(500);
                yield return i;
            }
        }
    }
}