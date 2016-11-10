namespace ServiceRunner
{
    internal class ServiceInfo
    {
        public ServiceInfo(
            string serviceName, 
            string serviceDisplayName, 
            string serviceDescription, 
            string servicePath, 
            string serviceArguments,
            bool restartAfterCrash,
            int restartTimeout)
        {
            ServiceName = serviceName;
            ServiceDisplayName = serviceDisplayName;
            ServiceDescription = serviceDescription;
            ServicePath = servicePath;
            ServiceArguments = serviceArguments;
            RestartAfterCrash = restartAfterCrash;
            RestartTimeout = restartTimeout;
        }

        public string ServiceName { get; private set; }

        public string ServiceDisplayName { get; private set; }

        public string ServiceDescription { get; private set; }

        public string ServicePath { get; private set; }

        public string ServiceArguments { get; private set; }

        public bool RestartAfterCrash { get; private set; }

        public int RestartTimeout { get; private set; }
    }
}