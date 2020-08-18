namespace Microsoft.AzCopy.Contract
{
    public class AZCopyInfoMessage : AZCopyMessageBase
    {
        // For StreamJson-RPC only
        public AZCopyInfoMessage()
        {
        }

        public override string MessageType { get => "Info"; }
    }
}
