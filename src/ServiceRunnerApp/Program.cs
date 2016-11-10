using System;
using System.Configuration;
using Topshelf;

namespace ServiceRunner
{
    class Program
    {
        static void Main()
        {
            var infoReader = new ServiceInfoReader();
            var serviceInfo = infoReader.ReadServiceInfo(ConfigurationManager.AppSettings);
            HostFactory.Run(x =>                                 
            {
                x.Service<ServiceWrapper>(s =>                        
                {
                    s.ConstructUsing(name => new ServiceWrapper(serviceInfo.ServicePath, serviceInfo.ServiceArguments));     
                    s.WhenStarted(tc => tc.Start());              
                    s.WhenStopped(tc => tc.Stop());               
                });
                x.RunAsLocalSystem();                            

                x.SetDescription(serviceInfo.ServiceDescription);        
                x.SetDisplayName(serviceInfo.ServiceDisplayName);                       
                x.SetServiceName(serviceInfo.ServiceName);                       
                x.OnException(ex =>
                {
                    Console.WriteLine(ex.Message);
                });

                if (serviceInfo.RestartAfterCrash)
                {
                    x.EnableServiceRecovery(r =>
                    {
                        r.OnCrashOnly();
                        r.RestartService(serviceInfo.RestartTimeoutMin);
                    });
                }
                x.UseNLog();
            });
        }
    }
}
