using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRunner.Args
{
    class AppOptionsFactory
    {
        public static List<Option> GetAppOptions()
        {
            var options = new List<Option>();

            options.Add(new Option
            {
                IsRequired = true,
                Name = SupportedOptions.Service
            });

            return options;
        }

        public  static string GetUsage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ServiceRunner.Args.Usage.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
