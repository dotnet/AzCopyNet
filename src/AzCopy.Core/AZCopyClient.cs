using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AzCopy.Contract;

namespace Microsoft.AzCopy
{
    public class AZCopyClient : IAZCopyClient
    {
        private Process process = default;

        public event EventHandler<AZCopyMessageBase> JobStatusHandler;

        public async Task CopyAsync(IAZCopyLocation src, IAZCopyLocation dst, AZCopyOption option, CancellationToken ct = default)
        {
            // Locations must be in quotes. It could have spaces in the name and the CLI would interpret as separate parameters.
            var args = $"copy \"{src.LocationToString()}\" \"{dst.LocationToString()}\" {option.ToCommandLineString()} --output-type=json --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task DeleteAsync(IAZCopyLocation dst, AZDeleteOption option, CancellationToken ct = default)
        {
            // Lcations must be in quotes. It could have spaces in the name and the CLI would interpret as separate parameters.
            var args = $"rm \"{dst.LocationToString()}\" {option.ToCommandLineString()} --output-type=json --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        private static string GetAzCopyPath()
        {
            try
            {
                // First check if $env.AzCopyPath exists
                if (Environment.GetEnvironmentVariable("AZCOPYPATH") != null)
                {
                    return Environment.GetEnvironmentVariable("AZCOPYPATH");
                }

                var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string azCopyPath = Path.Combine(assemblyFolder, "azcopy");
                if (!File.Exists(azCopyPath))
                {
                    throw new FileNotFoundException();
                }

                return azCopyPath;
            }
            catch (FileNotFoundException)
            {
                throw new Exception(@"Can't find azcopy. Make sure you install azcopy and set its path to $AZCOPYPATH on your system, or use one of the following nuget package: AzCopy.WinX64, AzCopy.LinuxX64, AzCopy.OsxX64.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task StartAZCopyAsync(string args, CancellationToken ct = default, Dictionary<string, string> envs = default)
        {
            string azCopyPath = GetAzCopyPath();
            var procInfo = new ProcessStartInfo(azCopyPath);
            procInfo.Arguments = args;
            procInfo.RedirectStandardOutput = true;
            procInfo.RedirectStandardError = true;
            procInfo.RedirectStandardInput = true;

            if (envs != null)
            {
                foreach (var kv in envs)
                {
                    procInfo.Environment.Add(kv);
                }
            }

            // set Environment Info for OAuth Location
            // only for test output
            procInfo.UseShellExecute = false;
            await Task.Run(() =>
            {
                this.process = Process.Start(procInfo);

                // cancellation
                ct.Register(() => this.process.StandardInput.WriteLine("cancel"));

                this.process.OutputDataReceived += this.Process_OutputDataReceived;
                this.process.ErrorDataReceived += this.Process_OutputDataReceived;

                this.process.BeginOutputReadLine();
                this.process.BeginErrorReadLine();

                this.process.WaitForExit();
            });
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                var message = AZCopyMessageFactory.CreateFromJson(e.Data);
                this.JobStatusHandler?.Invoke(sender, message);
            }
        }
    }
}
