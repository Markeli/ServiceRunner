using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json;

namespace ServiceRunner.Service
{
    internal class ServiceInfoReader
    {
        public ServiceInfo ReadServiceInfo(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<ServiceInfo>(json);
        }
    }
}
