using System.Collections.Generic;
using Microsoft.AzCopy.Contract;
using Newtonsoft.Json.Linq;

namespace Microsoft.AzCopy
{
    internal static class AZCopyMessageExtension
    {
        public static AZCopyProgressMessage FromJson(this AZCopyProgressMessage msg, JToken jToken)
        {
            msg.MessageContent = jToken.Value<string>(nameof(msg.MessageContent));
            msg.TimeStamp = jToken.Value<string>(nameof(msg.TimeStamp));

            var messageContentJToken = JValue.Parse(msg.MessageContent);

            msg.ErrorMsg = messageContentJToken.Value<string>(nameof(msg.ErrorMsg));
            msg.JobStatus = messageContentJToken.Value<string>(nameof(msg.JobStatus));
            msg.TotalTransfers = messageContentJToken.Value<int>(nameof(msg.TotalTransfers));
            msg.TransfersCompleted = messageContentJToken.Value<int>(nameof(msg.TransfersCompleted));
            msg.TransfersFailed = messageContentJToken.Value<int>(nameof(msg.TransfersFailed));
            msg.TransfersSkipped = messageContentJToken.Value<int>(nameof(msg.TransfersSkipped));
            msg.BytesOverWire = messageContentJToken.Value<double>(nameof(msg.BytesOverWire));
            msg.TotalBytesTransferred = messageContentJToken.Value<double>(nameof(msg.TotalBytesTransferred));
            msg.TotalBytesEnumerated = messageContentJToken.Value<double>(nameof(msg.TotalBytesEnumerated));
            msg.TotalBytesExpected = messageContentJToken.Value<double>(nameof(msg.TotalBytesExpected));
            msg.PercentComplete = messageContentJToken.Value<float>(nameof(msg.PercentComplete));

            msg.FailedTransfers = messageContentJToken[nameof(msg.FailedTransfers)].ToObject<List<FailedTransfer>>() ?? new List<FailedTransfer>();
            msg.SkippedTransfers = messageContentJToken[nameof(msg.SkippedTransfers)].ToObject<List<FailedTransfer>>() ?? new List<FailedTransfer>();
            return msg;
        }

        public static AZCopyInfoMessage FromJson(this AZCopyInfoMessage msg, JToken jToken)
        {
            msg.MessageContent = jToken.Value<string>(nameof(msg.MessageContent));
            msg.TimeStamp = jToken.Value<string>(nameof(msg.TimeStamp));
            return msg;
        }

        public static AZCopyInitMessage FromJson(this AZCopyInitMessage msg, JToken jToken)
        {
            msg.MessageContent = jToken.Value<string>(nameof(msg.MessageContent));
            msg.TimeStamp = jToken.Value<string>(nameof(msg.TimeStamp));
            var messageContentJToken = JValue.Parse(msg.MessageContent);
            msg.LogFileLocation = messageContentJToken.Value<string>(nameof(msg.LogFileLocation));
            msg.JobID = messageContentJToken.Value<string>(nameof(msg.JobID));
            msg.IsCleanupJob = messageContentJToken.Value<string>(nameof(msg.JobID));
            return msg;
        }

        public static AZCopyErrorMessage FromJson(this AZCopyErrorMessage msg, JToken jToken)
        {
            msg.MessageContent = jToken.Value<string>(nameof(msg.MessageContent));
            msg.TimeStamp = jToken.Value<string>(nameof(msg.TimeStamp));
            return msg;
        }

        public static AZCopyEndOfJobMessage FromJson(this AZCopyEndOfJobMessage msg, JToken jToken)
        {
            msg.MessageContent = jToken.Value<string>(nameof(msg.MessageContent));
            msg.TimeStamp = jToken.Value<string>(nameof(msg.TimeStamp));

            var messageContentJToken = JValue.Parse(msg.MessageContent);

            msg.ErrorMsg = messageContentJToken.Value<string>(nameof(msg.ErrorMsg));
            msg.JobStatus = messageContentJToken.Value<string>(nameof(msg.JobStatus));
            msg.TotalTransfers = messageContentJToken.Value<int>(nameof(msg.TotalTransfers));
            msg.TransfersCompleted = messageContentJToken.Value<int>(nameof(msg.TransfersCompleted));
            msg.TransfersFailed = messageContentJToken.Value<int>(nameof(msg.TransfersFailed));
            msg.TransfersSkipped = messageContentJToken.Value<int>(nameof(msg.TransfersSkipped));
            msg.BytesOverWire = messageContentJToken.Value<double>(nameof(msg.BytesOverWire));
            msg.TotalBytesTransferred = messageContentJToken.Value<double>(nameof(msg.TotalBytesTransferred));
            msg.TotalBytesEnumerated = messageContentJToken.Value<double>(nameof(msg.TotalBytesEnumerated));
            msg.TotalBytesExpected = messageContentJToken.Value<double>(nameof(msg.TotalBytesExpected));
            msg.PercentComplete = messageContentJToken.Value<float>(nameof(msg.PercentComplete));

            msg.FailedTransfers = messageContentJToken[nameof(msg.FailedTransfers)].ToObject<List<FailedTransfer>>() ?? new List<FailedTransfer>();
            msg.SkippedTransfers = messageContentJToken[nameof(msg.SkippedTransfers)].ToObject<List<FailedTransfer>>() ?? new List<FailedTransfer>();
            return msg;
        }
    }
}
