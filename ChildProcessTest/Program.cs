using System;
using System.Threading.Tasks;

namespace ChildProcessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var randomizer = new Random(123);
            Task.Delay(TimeSpan.FromSeconds(randomizer.Next(2, 10))).GetAwaiter().GetResult();

            if (randomizer.Next(0, 10) > 5)
            {
                throw new Exception();
            }

            Task.Delay(TimeSpan.FromSeconds(randomizer.Next(3, 10))).GetAwaiter().GetResult();
        }
    }
}
