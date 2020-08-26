using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AzCopy.Contract
{
    /// <summary>
    /// See https://github.com/Azure/azure-storage-azcopy/blob/2c30af330b681d3a1f31db32e11245eafce9c5c3/common/rpc-models.go#L214
    /// </summary>
    [DataContract]
    public class ListJobSummaryResponse
    {
        [DataMember]
        public string ErrorMsg { get; set; }

        [DataMember]
        public Guid JobID { get; set; }

        [DataMember(IsRequired = true)]
        public string JobStatus { get; set; }

        [DataMember]
        public long ActiveConnections { get; set; }

        [DataMember]
        public bool CompleteJobOrdered { get; set; }

        [DataMember]
        public int TotalTransfers { get; set; }

        [DataMember]
        public int FileTransfers { get; set; }

        [DataMember]
        public int FolderPropertyTransfers { get; set; }

        [DataMember]
        public int TransfersCompleted { get; set; }

        [DataMember]
        public int TransfersFailed { get; set; }

        [DataMember]
        public int TransfersSkipped { get; set; }

        [DataMember]
        public long BytesOverWire { get; set; }

        [DataMember]
        public long TotalBytesTransferred { get; set; }

        [DataMember]
        public long TotalBytesEnumerated { get; set; }

        [DataMember]
        public long TotalBytesExpected { get; set; }

        [DataMember]
        public float PercentComplete { get; set; }

        [DataMember]
        public int AverageIOPS { get; set; }

        [DataMember]
        public int AverageE2EMilliseconds { get; set; }

        [DataMember]
        public float ServerBusyPercentage { get; set; }

        [DataMember]
        public float NetworkErrorPercentage { get; set; }

        [DataMember]
        public TransferDetail[] FailedTransfers { get; set; }

        [DataMember]
        public TransferDetail[] SkippedTransfers { get; set; }

        [DataMember]
        public int PerfConstraint { get; set; }

        [DataMember]
        public string[] PerfStrings { get; set; }

        [DataMember]
        public bool IsCleanupJob { get; set; }

        [DataMember]
        public PerformanceAdvice[] PerformanceAdvice { get; set; }
    }

    public class JobStatus
    {
        public static readonly string InProgress = "InProgress";
        public static readonly string Paused = "Paused";
        public static readonly string Cancelling = "Cancelling";
        public static readonly string Cancelled = "Cancelled";
        public static readonly string Completed = "Completed";
        public static readonly string CompletedWithErrors = "CompletedWithErrors";
        public static readonly string CompletedWithSkipped = "CompletedWithSkipped";
        public static readonly string CompletedWithErrorsAndSkipped = "CompletedWithErrorsAndSkipped";
        public static readonly string Failed = "Failed";
    };
}
