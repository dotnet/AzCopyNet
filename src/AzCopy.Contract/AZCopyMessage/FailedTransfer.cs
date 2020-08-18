namespace Microsoft.AzCopy.Contract
{
    /// <summary>
    /// type for MessageContent.FailedTransfers and MessageContent.SkippedTransfers.
    /// </summary>
    public class FailedTransfer
    {
        public string Src { get; set; }

        public string Dst { get; set; }

        public string TransferStatus { get; set; }

        public int ErrorCode { get; set; }
    }
}
