using System;
using System.Diagnostics;
using ServiceRunner.Logs;

namespace ServiceRunner
{
    internal class ServiceWrapper
    {
        private readonly ServiceInfo _serviceInfo;
        private readonly LogManager _logManager;
        private readonly string _servicePath;
        private readonly string _serviceArgs;

        private Process _osrmProcess;

        public ServiceWrapper(ServiceInfo serviceInfo, LogManager logManager)
        {
            if (serviceInfo == null) throw new ArgumentNullException(nameof(serviceInfo));
            if (logManager == null) throw new ArgumentNullException(nameof(logManager));
            _serviceInfo = serviceInfo;
            _logManager = logManager;

            _servicePath = serviceInfo.ServicePath;
            _serviceArgs = serviceInfo.ServiceArguments;
        }

        public void Start()
        {
            _osrmProcess = new Process
            {
                StartInfo =
                {
                    FileName = _servicePath,
                    Arguments = _serviceArgs,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                },
                EnableRaisingEvents = true
            };



            _osrmProcess.OutputDataReceived += ProcessOnOutputDataReceived;
            _osrmProcess.ErrorDataReceived += ProcessOnErrorDataReceived;
            _osrmProcess.Exited += ProcessOnExited;

            _osrmProcess.Start();

            _logManager.ServiceMainMainLog.Info("Service started");
        }

        private void ProcessOnExited(object sender, EventArgs eventArgs)
        {
            if (_osrmProcess.ExitCode != 0)
            {
                throw new Exception();
            }
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs == null) return;

            _logManager.ServiceMainMainLog.Error(dataReceivedEventArgs.Data);
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs == null) return;

            _logManager.ServiceMainMainLog.Info(dataReceivedEventArgs.Data);
        }


        public void Stop()
        {
            _osrmProcess?.Close();
        }
    }
}