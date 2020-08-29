using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AzCopy.Contract
{
    /// <summary>
    /// https://github.com/Azure/azure-storage-azcopy/blob/25635976913d156222cffec8ca3693fe6a0afb65/common/output.go#L15:6
    /// </summary>
    [DataContract]
    public enum MessageType
    {
        [EnumMember]
        Init = 0,

        [EnumMember]
        Info = 1,

        [EnumMember]
        Progress = 2,

        [EnumMember]
        EndOfJob = 3,

        [EnumMember]
        Error = 4,

        [EnumMember]
        Prompt = 5,
    }
}
