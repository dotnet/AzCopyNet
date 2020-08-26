using AzCopy.Contract;
using System;

namespace AzCopy.Contract
{
    public interface IAZCopyChannel
    {
        /// <summary>
        /// event handler for output msg.
        /// </summary>
        event EventHandler<JsonOutputTemplate> OutputMsgHandler;

        /// <summary>
        /// event handler for init msg.
        /// </summary>
        event EventHandler<InitMsgJsonTemplate> InitMsgHandler;

        /// <summary>
        /// event handler for error msg.
        /// </summary>
        event EventHandler<JsonOutputTemplate> ErrorMsgHanlder;

        /// <summary>
        /// event handler for info msg.
        /// </summary>
        event EventHandler<JsonOutputTemplate> InfoMsgHanlder;

        /// <summary>
        /// event handler for job status.
        /// </summary>
        event EventHandler<ListJobSummaryResponse> JobStatusMsgHandler;
    }
}
