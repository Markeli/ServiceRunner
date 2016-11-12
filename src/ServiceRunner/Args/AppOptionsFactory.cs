using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
                IsFlag = false,
                Name = SupportedOptions.Service
            });
            options.Add(new Option
            {
                IsRequired = false,
                IsFlag = true,
                Name = SupportedOptions.Help
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
