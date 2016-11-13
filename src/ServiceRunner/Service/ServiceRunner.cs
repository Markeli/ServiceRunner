using System;
using System.Diagnostics;
using ServiceRunner.Logs;
using ServiceRunner.Properties;
using Topshelf;

namespace ServiceRunner.Service
{
    internal class ServiceRunner : IDisposable
    {
        private readonly ServiceInfo _serviceInfo;
        private readonly LogManager _logManager;

        private Process _osrmProcess;

        private int _failsCount;
        private HostControl _hostControl;


        public ServiceRunner(ServiceInfo serviceInfo, LogManager logManager)
        {
            if (serviceInfo == null) throw new ArgumentNullException(nameof(serviceInfo));
            if (logManager == null) throw new ArgumentNullException(nameof(logManager));
            _serviceInfo = serviceInfo;
            _logManager = logManager;
            
        }

        public void Start(HostControl hostControl)
        {
            if (hostControl == null) throw new ArgumentNullException(nameof(hostControl));
            _hostControl = hostControl;
            Start();
        }

        private void Start()
        {
            _osrmProcess = new Process
            {
                StartInfo =
                {
                    FileName = _serviceInfo.ServicePath,
                    Arguments = _serviceInfo.ServiceArguments,
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
            _osrmProcess.BeginErrorReadLine();
            _osrmProcess.BeginOutputReadLine();
        }

        private void ProcessOnExited(object sender, EventArgs eventArgs)
        {
            // штатное завершение
            if (_osrmProcess.ExitCode == 0)
            {
                _logManager.MainLog.Info(Resource.ServiceRunner_ServiceNormalyTerminated);
                _hostControl?.Stop();
                return;
            }

            _logManager.MainLog.Error(Resource.ServiceRunner_ServiceCrashed);
            if (_serviceInfo.RestartAfterCrash)
            {
                if (_failsCount < _serviceInfo.RestartCountOnFail)
                {
                    _failsCount++;
                    _logManager.MainLog.Info(string.Format(Resource.ServiceRunner_TryingRestartMessageMask, _failsCount, _serviceInfo.RestartCountOnFail));
                    Stop();
                    Start();
                    _logManager.MainLog.Info(Resource.ServiceRunner_ServiceSuccessfullyRestarted);
                    return;
                }
            }
            _logManager.MainLog.Fatal(Resource.ServiceRunner_FailedToRestartService);
            throw new Exception(Resource.ServiceRunner_FailedToRestartService);
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (String.IsNullOrEmpty(dataReceivedEventArgs?.Data)) return;

            _logManager.ServiceExceptionLog.Error(dataReceivedEventArgs.Data);
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (String.IsNullOrEmpty(dataReceivedEventArgs?.Data)) return;
            
            _logManager.ServiceMainMainLog.Info(dataReceivedEventArgs.Data);
        }


        public void Stop()
        {
            _osrmProcess?.Close();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}