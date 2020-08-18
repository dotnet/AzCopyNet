using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AzCopy.Contract
{
    public interface IAZCopyService : IAZCopyServiceChannel, IDisposable
    {
        /// <summary>
        /// azcopy copy from source to destination.
        /// </summary>
        /// <param name="src">copy src.</param>
        /// <param name="dst">copy dst.</param>
        /// <param name="option">copy option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns>empty task.</returns>
        Task CopyLocalRemoteAsync(LocalLocation src, RemoteSasLocation dst, AZCopyOption option, CancellationToken ct);

        /// <summary>
        /// azcopy copy from source to destination.
        /// </summary>
        /// <param name="src">copy src.</param>
        /// <param name="dst">copy dst.</param>
        /// <param name="option">copy option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns>empty task.</returns>
        Task CopyRemoteLocalAsync(RemoteSasLocation src, LocalLocation dst, AZCopyOption option, CancellationToken ct);

        /// <summary>
        /// azcopy delete blablabla.
        /// </summary>
        /// <param name="dst">delete src.</param>
        /// <param name="option">delete option.</param>
        /// <param name="ct">cancallation token.</param>
        /// <returns>empty task.</returns>
        Task DeleteRemoteSASAsync(RemoteSasLocation dst, AZDeleteOption option, CancellationToken ct);
    }
}
