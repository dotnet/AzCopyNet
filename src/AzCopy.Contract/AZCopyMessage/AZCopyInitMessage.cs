namespace Microsoft.AzCopy.Contract
{
    public class AZCopyInitMessage : AZCopyMessageBase
    {
        // For StreamJson-RPC only
        public AZCopyInitMessage()
        {
        }

        public override string MessageType => "Init";

        public string LogFileLocation { get; set; }

        // the only ID that can use to trace status in azcopy.exe, really not useful here though
        public string JobID { get; set; }

        // don't know what's that for
        public string IsCleanupJob { get; set; }
    }
}
