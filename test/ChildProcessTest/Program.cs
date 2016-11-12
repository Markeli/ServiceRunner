using System;
using System.Threading.Tasks;

namespace ChildProcessTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hi!");
            var randomizer = new Random(123);
            Console.WriteLine("Starting delay");
            Task.Delay(TimeSpan.FromSeconds(randomizer.Next(2, 10))).GetAwaiter().GetResult();

            Console.WriteLine("Can I fault?");
            if (randomizer.Next(0, 10) > 5)
            {
                throw new Exception();
            }
            Console.WriteLine("Not now");

            Task.Delay(TimeSpan.FromSeconds(randomizer.Next(3, 10))).GetAwaiter().GetResult();
            Console.WriteLine("By!");
        }
    }
}
