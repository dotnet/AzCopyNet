using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AzCopy.Contract
{
    /// <summary>
    /// https://github.com/Azure/azure-storage-azcopy/blob/2c30af330b681d3a1f31db32e11245eafce9c5c3/common/rpc-models.go#L288:6
    /// </summary>
    [DataContract]
    public class TransferDetail
    {
        [DataMember]
        public string Src { get; set; }

        [DataMember]
        public string Dst { get; set; }

        [DataMember]
        public bool IsFolderProperties { get; set; }

        [DataMember]
        public string TransferStatus { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }
    }

    public class TransferStatus
    {
        public static readonly string NotStarted = "NotStarted";
        public static readonly string Started = "Started";
        public static readonly string Success = "Success";
        public static readonly string Failed = "Failed";
        public static readonly string BlobTierFailure = "BlobTierFailure";
        public static readonly string SkippedEntityAlreadyExists = "SkippedEntityAlreadyExists";
        public static readonly string SkippedBlobHasSnapshots = "SkippedBlobHasSnapshots";
        public static readonly string TierAvailabilityCheckFailure = "TierAvailabilityCheckFailure";
        public static readonly string Cancelled = "Cancelled";
    }
}
