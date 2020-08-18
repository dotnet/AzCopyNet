using Microsoft.AzCopy.Contract;
using System;

namespace Microsoft.AzCopy
{
    internal interface IAZCopyChannel
    {
        event EventHandler<AZCopyMessageBase> JobStatusHandler;
    }
}
