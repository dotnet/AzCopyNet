using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AzCopy.Contract;

namespace Microsoft.AzCopy
{
    public class AzCopyService : IAZCopyService
    {
        private IAZCopyClient azCopyClient;

        public AzCopyService()
        {
            this.azCopyClient = new AZCopyClient();

            this.azCopyClient.JobStatusHandler += this.AzCopyClient_JobStatusHandler;
        }

        public event EventHandler<AZCopyEndOfJobMessage> AZCopyEndOfJobHandler;

        public event EventHandler<AZCopyErrorMessage> AZCopyErrorMessageHandler;

        public event EventHandler<AZCopyInfoMessage> AZCopyInfoMessageHandler;

        public event EventHandler<AZCopyInitMessage> AzCopyInitMessageHandler;

        public event EventHandler<AZCopyProgressMessage> AzCopyProgressMessageHandler;

        public Task CopyLocalRemoteAsync(LocalLocation src, RemoteSasLocation dst, AZCopyOption option, CancellationToken ct)
        {
            return this.azCopyClient.CopyAsync(src, dst, option, ct);
        }

        public Task CopyRemoteLocalAsync(RemoteSasLocation src, LocalLocation dst, AZCopyOption option, CancellationToken ct)
        {
            return this.azCopyClient.CopyAsync(src, dst, option, ct);
        }

        public Task DeleteRemoteSASAsync(RemoteSasLocation dst, AZDeleteOption option, CancellationToken ct)
        {
            return this.azCopyClient.DeleteAsync(dst, option, ct);
        }

        public void Dispose()
        {
        }

        private void AzCopyClient_JobStatusHandler(object sender, AZCopyMessageBase e)
        {
            switch (e)
            {
                case AZCopyInfoMessage msg:
                    this.AZCopyInfoMessageHandler?.Invoke(sender, msg);
                    break;
                case AZCopyEndOfJobMessage msg:
                    this.AZCopyEndOfJobHandler?.Invoke(sender, msg);
                    break;
                case AZCopyErrorMessage msg:
                    this.AZCopyErrorMessageHandler?.Invoke(sender, msg);
                    break;
                case AZCopyInitMessage msg:
                    this.AzCopyInitMessageHandler?.Invoke(sender, msg);
                    break;
                case AZCopyProgressMessage msg:
                    this.AzCopyProgressMessageHandler?.Invoke(sender, msg);
                    break;
                default:
                    break;
            }
        }
    }
}
