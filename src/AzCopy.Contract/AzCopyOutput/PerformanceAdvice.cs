using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AzCopy.Contract
{
    /// <summary>
    /// https://github.com/Azure/azure-storage-azcopy/blob/2c30af330b681d3a1f31db32e11245eafce9c5c3/common/fe-ste-models.go#L1183
    /// </summary>
    [DataContract]
    public class PerformanceAdvice
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public bool PriorityAdvice { get; set; }
    }
}
