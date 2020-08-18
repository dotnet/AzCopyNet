using System;

namespace Microsoft.AzCopy.Contract
{
    public interface IAZCopyServiceChannel
    {
        event EventHandler<AZCopyEndOfJobMessage> AZCopyEndOfJobHandler;

        event EventHandler<AZCopyErrorMessage> AZCopyErrorMessageHandler;

        event EventHandler<AZCopyInfoMessage> AZCopyInfoMessageHandler;

        event EventHandler<AZCopyInitMessage> AzCopyInitMessageHandler;

        event EventHandler<AZCopyProgressMessage> AzCopyProgressMessageHandler;
    }
}