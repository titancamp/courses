using System;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    static async Task MainAsync()
    { 
        using (var reader = new Reader(new FileStream("Threads2_010_ValueTask.exe", FileMode.Open, FileAccess.Read)))
        { 
            long sum = 0;
            while (true)
            {
                var b = await reader.GetNextByteAsync();
                if (!b.HasValue) break;
                sum += b.Value;
            }
            Console.WriteLine($"Sum of bytes: {sum}");
        }
    }
}


class Reader : IDisposable
{
    byte[] buf = new byte[1024];
    int pos = 0, len=0;
    Stream stream;

    public async ValueTask<byte?> GetNextByteAsync()
    {
        // If we've exhausted the current buffer, fetch another buffer
        if (pos >= len)
        {
            len = await stream.ReadAsync(buf, 0, buf.Length);
            pos = 0;
        }

        // If there's anything left in the current buffer, return it
        if (pos < len)
        {
            return buf[pos++];
        }

        // Otherwise signal EOF
        return null;
    }

    public Reader(Stream stream)
    {
        this.stream = stream;
    }

    public void Dispose()
    {
        stream.Dispose();
    }
}