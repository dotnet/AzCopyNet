using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AzCopy.Contract
{
    /// <summary>
    /// https://github.com/Azure/azure-storage-azcopy/blob/25635976913d156222cffec8ca3693fe6a0afb65/common/output.go#L77    
    /// </summary>
    [DataContract]
    public class JsonOutputTemplate
    {
        [DataMember(IsRequired = true)]
        public MessageType MessageType { get; set; }

        [DataMember(IsRequired = true)]
        public string TimeStamp { get; set; }

        [DataMember(IsRequired = true)]
        public string MessageContent { get; set; }

        [DataMember(IsRequired = true)]
        internal PromptDetails PromptDetails { get; set; }
    }
}
