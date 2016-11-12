using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
