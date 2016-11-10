using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using ServiceRunner.Logs.NLog;
using ServiceRunner.Service;
using Topshelf;
using LogManager = ServiceRunner.Logs.LogManager;

namespace ServiceRunner
{
    class Program
    {
        static void Main()
        {
            var logManager = new LogManager(NLogSystem.CreateByConfig("NLog.config"));

            var infoReader = new ServiceInfoReader();

            var serviceInfo = infoReader.ReadServiceInfo("Example/service.json");
            
            HostFactory.Run(x =>                                 
            {
                x.Service<ServiceWrapper>(s =>                        
                {
                    s.ConstructUsing(name => new ServiceWrapper(serviceInfo, logManager));     
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
