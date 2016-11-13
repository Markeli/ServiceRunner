using System;
using System.Collections.Generic;
using ServiceRunner.Args;
using ServiceRunner.Logs;
using Topshelf;
using Topshelf.StartParameters;

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

        public void Start(ServiceInfo serviceInfo, List<Option> serviceOptions)
        {
            HostFactory.Run(x =>
            {
                x.EnableStartParameters();

                 // для поддержки вызова сервиса, чтобы нормально отрабатывал аргумент
                foreach (var option in serviceOptions)
                {
                    x.AddCommandLineDefinition(option.Name, service =>
                    {

                    });
                    if (option.IsFlag) continue;

                    x.WithStartParameter(option.Name, option.Value, a =>
                    {
                        _logManager.MainLog.Info($"Adding support for service parameter: {option.Name}, value: {option.Value}");
                    });
                }
                x.ApplyCommandLine();

                x.Service<ServiceRunner>(s =>
                {
                    s.ConstructUsing(name => new ServiceRunner(serviceInfo, _logManager));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalService();
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