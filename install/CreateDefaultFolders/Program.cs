using System;
using System.Diagnostics;

namespace CreateDefaultFolders
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Process.Start("cmd", "mkdir %Public%/ServiceRunner/services/");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
