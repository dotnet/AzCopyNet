using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AzCopy.Contract
{
    /// <summary>
    /// https://github.com/Azure/azure-storage-azcopy/blob/25635976913d156222cffec8ca3693fe6a0afb65/common/output.go#L89
    /// </summary>
    [DataContract]
    public class InitMsgJsonTemplate
    {
        [DataMember]
        public string LogFileLocation { get; set; }

        [DataMember]
        public string JobID { get; set; }

        [DataMember]
        public bool IsCleanupJob { get; set; }
    }
}
