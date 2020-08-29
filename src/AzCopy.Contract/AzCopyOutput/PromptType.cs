using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace AzCopy.Contract
{
    /// <summary>
    /// https://github.com/Azure/azure-storage-azcopy/blob/25635976913d156222cffec8ca3693fe6a0afb65/common/output.go#L60
    /// </summary>
    public enum PromptType
    {
        Empty,
        Cancel,
        Overwrite,
        DeleteDestination,
    }
}
