namespace Microsoft.AzCopy.Contract
{
    public class AZCopyStringMessage : AZCopyMessageBase
    {
        public AZCopyStringMessage(string message)
        {
            this.MessageContent = message;
        }

        public override string MessageType => "StringError";
    }
}
