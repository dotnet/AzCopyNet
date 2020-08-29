using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AzCopy.Contract
{

    [DataContract]
    public class PromptDetails
    {
        public PromptType PromptType { get; set; }

        [DataMember]
        public string PromptTarget { get; set; }

        [DataMember(Name = nameof(PromptType))]
        internal string PromptTypeString
        {
            get
            {
                return this.PromptType.ToString();
            }

            set
            {
                if (value == string.Empty)
                {
                    this.PromptType = PromptType.Empty;
                }
                else
                {
                    this.PromptType = (PromptType)Enum.Parse(typeof(PromptType), value);
                }
            }
        }
    }
}
