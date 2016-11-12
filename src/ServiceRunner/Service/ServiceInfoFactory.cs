using System;
using System.IO;
using Newtonsoft.Json;

namespace ServiceRunner.Service
{
    internal class ServiceInfoFactory
    {
        private readonly string _servicesFolder;

        public ServiceInfoFactory(string servicesFolder)
        {
            _servicesFolder = servicesFolder;
            if (String.IsNullOrEmpty(servicesFolder)) throw new ArgumentNullException(nameof(servicesFolder));
        }

        public ServiceInfo GetServiceInfo(string servicePath)
        {
            if (String.IsNullOrEmpty(servicePath)) throw new ArgumentNullException(nameof(servicePath));

            var path = !IsServicePath(servicePath)
                ? Path.Combine(_servicesFolder, $"{servicePath}.service")
                : servicePath;
            
            return ReadServiceInfo(path);
        }

        private bool IsServicePath(string servicePath)
        {
            return servicePath.EndsWith(".service");
        }

        private ServiceInfo ReadServiceInfo(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<ServiceInfo>(json);
        }
    }
}
