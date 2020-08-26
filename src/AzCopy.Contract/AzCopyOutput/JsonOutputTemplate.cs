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
        public string MessageType { get; set; }

        [DataMember(IsRequired = true)]
        public string TimeStamp { get; set; }

        [DataMember(IsRequired = true)]
        public string MessageContent { get; set; }

        [DataMember(IsRequired = true)]
        public PromptDetails PromptDetails { get; set; }
    }

    public static class MessageType
    {
        public const string Init = "Init";
        public const string Info = "Info";
        public const string Progress = "Progress";
        public const string EndOfJob = "EndOfJob";
        public const string Error = "Error";
        public const string Prompt = "Prompt";
    }

    public static class PromptType
    {
        public const string Cancel = "Cancel";
        public const string Overwrite = "Overwrite";
        public const string DeleteDestination = "DeleteDestination";
    }

    [DataContract]
    public class PromptDetails
    {
        [DataMember]
        public string PromptType { get; set; }

        [DataMember]
        public string PromptTarget { get; set; }
    }
}
