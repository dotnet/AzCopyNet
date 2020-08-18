namespace Microsoft.AzCopy.Contract
{
    public sealed class AZCopyErrorMessage : AZCopyMessageBase
    {
        // For StreamJson-RPC only
        public AZCopyErrorMessage()
        {
        }

        public override string MessageType => "Error";
    }
}
