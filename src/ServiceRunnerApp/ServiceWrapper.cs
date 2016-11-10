using System;
using System.Diagnostics;

namespace ServiceRunner
{
    internal class ServiceWrapper
    {
        private readonly string _servicePath;
        private readonly string _serviceArgs;

        private Process _osrmProcess;

        public ServiceWrapper(string servicePath, string serviceArgs)
        {
            if (servicePath == null) throw new ArgumentNullException(nameof(servicePath));
            if (serviceArgs == null) throw new ArgumentNullException(nameof(serviceArgs));

            _servicePath = servicePath;
            _serviceArgs = serviceArgs;
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
            //throw new NotImplementedException();
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            //dataReceivedEventArgs.Data;
        }


        public void Stop()
        {
            _osrmProcess?.Close();
        }
    }
}