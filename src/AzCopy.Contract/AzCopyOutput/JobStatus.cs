using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AzCopy.Contract
{
    /// <summary>
    /// https://github.com/Azure/azure-storage-azcopy/blob/2c30af330b681d3a1f31db32e11245eafce9c5c3/common/fe-ste-models.go#L335
    /// </summary>
    [DataContract]
    public enum JobStatus
    {
        [EnumMember]
        InProgress = 0,

        [EnumMember]
        Paused = 1,

        [EnumMember]
        Cancelling = 2,

        [EnumMember]
        Cancelled = 3,

        [EnumMember]
        Completed = 4,

        [EnumMember]
        CompletedWithErrors = 5,

        [EnumMember]
        CompletedWithSkipped = 6,

        [EnumMember]
        CompletedWithErrorsAndSkipped = 7,

        [EnumMember]
        Failed = 8,
    }
}
