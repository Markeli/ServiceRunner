using System;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using ServiceRunner.Args;
using ServiceRunner.Logs.NLog;
using ServiceRunner.Service;
using Topshelf;
using LogManager = ServiceRunner.Logs.LogManager;

namespace ServiceRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var logManager = new LogManager(NLogSystem.CreateByConfig("NLog.config"));
            var options = new ArgOptions();
            var serviceInfoPath = String.Empty;
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                //"Example/service.json"
                serviceInfoPath = options.ServiceInfoPath;
            }
            else
            {
                logManager.MainLog.Warning(options.GetUsage());
                return;
            }
            serviceInfoPath = "Example/service.json";

        var infoReader = new ServiceInfoReader();

            var serviceInfo = infoReader.ReadServiceInfo(serviceInfoPath);
            
            HostFactory.Run(x =>                                 
            {
                x.Service<Service.Service>(s =>                        
                {
                    s.ConstructUsing(name => new Service.Service(serviceInfo, logManager));     
                    s.WhenStarted(tc => tc.Start());              
                    s.WhenStopped(tc => tc.Stop());               
                });
                x.RunAsLocalSystem();                            

                x.SetDescription(serviceInfo.ServiceDescription);        
                x.SetDisplayName(serviceInfo.ServiceDisplayName);                       
                x.SetServiceName(serviceInfo.ServiceName);                       
                x.OnException(ex =>
                {
                    // log unhandled exception
                    logManager.ExceptionLog.Error(ex);
                });

                if (serviceInfo.RestartAfterCrash)
                {
                    x.EnableServiceRecovery(r =>
                    {
                        r.OnCrashOnly();
                        // позволяет перезапускать сервис только после первого падения
                        r.RestartService(serviceInfo.RestartTimeoutMin);
                    });
                }
                x.UseNLog();
            });
        }
    }
}
