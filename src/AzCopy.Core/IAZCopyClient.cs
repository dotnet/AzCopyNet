using Microsoft.AzCopy.Contract;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AzCopy
{
    internal interface IAZCopyClient : IAZCopyChannel
    {
        /// <summary>
        /// azcopy copy from local to remote.
        /// </summary>
        /// <param name="src">copy src.</param>
        /// <param name="dst">copy dst.</param>
        /// <param name="option">copy option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns>empty task.</returns>
        Task CopyAsync(IAZCopyLocation src, IAZCopyLocation dst, AZCopyOption option, CancellationToken ct);

        /// <summary>
        /// azcopy delete blablabla.
        /// </summary>
        /// <param name="dst">delete src.</param>
        /// <param name="option">delete option.</param>
        /// <returns>empty task.</returns>
        Task DeleteAsync(IAZCopyLocation dst, AZDeleteOption option, CancellationToken ct);
    }
}
