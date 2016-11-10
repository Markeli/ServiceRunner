using System.Collections.Specialized;

namespace ServiceRunner
{
    internal class ServiceInfoReader
    {
        public ServiceInfo ReadServiceInfo(NameValueCollection appSettings)
        {
            var name = appSettings["ServiceName"];
            var displayName = appSettings["ServiceDisplayName"];
            var description = appSettings["ServiceDescription"];
            var path = appSettings["ServicePath"];
            var arguments = appSettings["ServiceArguments"];
            var restartTimeout = int.Parse(appSettings["RestartTimeout"]);
            var restartAfterCrash = bool.Parse(appSettings["RestartAfterCrash"]);
            return new ServiceInfo(name, displayName, description, path, arguments, restartAfterCrash, restartTimeout);
        }
    }
}
