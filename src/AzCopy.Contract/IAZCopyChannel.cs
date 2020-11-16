using AzCopy.Contract;
using System;

namespace AzCopy.Contract
{
    public interface IAZCopyChannel
    {
        /// <summary>
        /// event handler for output message.
        /// </summary>
        event EventHandler<JsonOutputTemplate> OutputMessageHandler;

        /// <summary>
        /// event handler for init message.
        /// </summary>
        event EventHandler<InitMsgJsonTemplate> InitMessageHandler;

        /// <summary>
        /// event handler for error message.
        /// </summary>
        event EventHandler<JsonOutputTemplate> ErrorMessageHanlder;

        /// <summary>
        /// event handler for info message.
        /// </summary>
        event EventHandler<JsonOutputTemplate> InfoMessageHanlder;

        /// <summary>
        /// event handler for job status message.
        /// </summary>
        event EventHandler<ListJobSummaryResponse> JobStatusMessageHandler;
    }
}
