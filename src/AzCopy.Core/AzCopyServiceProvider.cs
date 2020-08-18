using System;
using System.IO;
using Microsoft.AzCopy.Contract;
using Microsoft.VisualStudio.Threading;
using StreamJsonRpc;

namespace Microsoft.AzCopy
{
    public class AzCopyServiceProvider
    {
        private Stream stream;
        private JsonRpc rpc;

        public AzCopyServiceProvider(Stream stream, IServiceProvider serviceProvider)
        {
            this.stream = stream;
            var azCopyClient = new AzCopyService();
            azCopyClient.AZCopyEndOfJobHandler += this.AzCopyClient_AZCopyEndOfJobHandler;
            azCopyClient.AZCopyErrorMessageHandler += this.AzCopyClient_AZCopyErrorMessageHandler;
            azCopyClient.AZCopyInfoMessageHandler += this.AzCopyClient_AZCopyInfoMessageHandler;
            azCopyClient.AzCopyInitMessageHandler += this.AzCopyClient_AzCopyInitMessageHandler;
            azCopyClient.AzCopyProgressMessageHandler += this.AzCopyClient_AzCopyProgressMessageHandler;
            this.rpc = JsonRpc.Attach(this.stream, azCopyClient);
        }

        private void AzCopyClient_AzCopyProgressMessageHandler(object sender, AZCopyProgressMessage e)
        {
            this.rpc.NotifyAsync(nameof(IAZCopyServiceChannel.AzCopyProgressMessageHandler), e).Forget();
        }

        private void AzCopyClient_AzCopyInitMessageHandler(object sender, AZCopyInitMessage e)
        {
            this.rpc.NotifyAsync(nameof(IAZCopyServiceChannel.AzCopyInitMessageHandler), e).Forget();
        }

        private void AzCopyClient_AZCopyInfoMessageHandler(object sender, AZCopyInfoMessage e)
        {
            this.rpc.NotifyAsync(nameof(IAZCopyServiceChannel.AZCopyInfoMessageHandler), e).Forget();
        }

        private void AzCopyClient_AZCopyErrorMessageHandler(object sender, AZCopyErrorMessage e)
        {
            this.rpc.NotifyAsync(nameof(IAZCopyServiceChannel.AZCopyErrorMessageHandler), e).Forget();
        }

        private void AzCopyClient_AZCopyEndOfJobHandler(object sender, AZCopyEndOfJobMessage e)
        {
            this.rpc.NotifyAsync(nameof(IAZCopyServiceChannel.AZCopyEndOfJobHandler), e).Forget();
        }
    }
}
