using System.IO;
using System.Reflection;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace ServiceRunner.Args
{
    public class ArgOptions
    {
        [Option('s', "service", 
            HelpText = "service info config")]
        public string ServiceInfoPath { get; set; }
        
        public string GetUsage()
        {
            var usage = new StringBuilder();
            var serviceRunnerOptions = HelpText.AutoBuild(this);
            usage.AppendLine(serviceRunnerOptions);
            usage.AppendLine(GetTopShelfUsage());

            return usage.ToString();
        }

        private string GetTopShelfUsage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ServiceRunner.Args.TopShelfUsage.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}