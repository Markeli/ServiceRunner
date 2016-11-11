using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        private const int FailureExitCode = -1;

        static void Main(string[] args)
        {
            var logManager = new LogManager(NLogSystem.CreateByConfig("NLog.config"));
            var options = ArgumentParser.Parse(args);
            string serviceInfoPath;
            if (ArgumentParser.IsValid(options.Values.ToList()))
            {
                var serviceOption = options[SupportedOptions.Service];
                serviceInfoPath = serviceOption.Value;
            }
            else
            {
                logManager.MainLog.Warning("Incorrect call. See usage");
                logManager.MainLog.Warning(AppOptionsFactory.GetUsage());
                return;
            }

            try
            {
                var serficeInfoFolder = ConfigurationManager.AppSettings["ServiceInfoFolder"];
                var infoReader = new ServiceInfoFactory(serficeInfoFolder);
                var serviceInfo = infoReader.GetServiceInfo(serviceInfoPath);

                var bootstraper = new ServiceBootstraper(logManager);
                bootstraper.Start(serviceInfo);

            }
            catch (Exception ex)
            {
                logManager.ExceptionLog.Error(ex);
                Environment.ExitCode = FailureExitCode;
            }

        }
    }
}
