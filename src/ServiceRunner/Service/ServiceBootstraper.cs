using System;
using ServiceRunner.Args;
using ServiceRunner.Logs;
using Topshelf;

namespace ServiceRunner.Service
{
    internal class ServiceBootstraper
    {
        private readonly LogManager _logManager;

        public ServiceBootstraper(LogManager logManager)
        {
            if (logManager == null) throw new ArgumentNullException(nameof(logManager));

            _logManager = logManager;
        }

        public void Start(ServiceInfo serviceInfo)
        {
            HostFactory.Run(x =>
            {
                 // для поддержки вызова сервиса, чтобы нормально отрабатывал аргумент
                var options = AppOptionsFactory.GetAppOptions();
                foreach (var option in options)
                {
                    x.AddCommandLineDefinition(option.Name, service =>
                    {

                    });
                }
                x.ApplyCommandLine();

                x.Service<Service>(s =>
                {
                    s.ConstructUsing(name => new Service(serviceInfo, _logManager));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription(serviceInfo.ServiceDescription);
                x.SetDisplayName(serviceInfo.ServiceSystemName);
                x.SetServiceName(serviceInfo.ServiceName);
                x.OnException(ex =>
                {
                    // log unhandled exception
                    _logManager.ExceptionLog.Error(ex);
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