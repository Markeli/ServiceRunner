using System;
using System.Configuration;
using System.Linq;
using ServiceRunner.Args;
using ServiceRunner.Logs.NLog;
using ServiceRunner.Properties;
using ServiceRunner.Service;
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

            var helpOption = options[SupportedOptions.Help];
            if (helpOption.IsSetted)
            {
                logManager.MainLog.Info(AppOptionsFactory.GetUsage());
                return;
            }

            string serviceInfoPath;
            if (ArgumentParser.IsValid(options.Values.ToList()))
            {
                var serviceOption = options[SupportedOptions.Service];
                serviceInfoPath = serviceOption.Value;
            }
            else
            {
                logManager.MainLog.Warning(Resource.Program_IncorrectArgumentsNeedUsageMessage);
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
