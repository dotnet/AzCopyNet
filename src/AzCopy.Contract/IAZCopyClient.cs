using System.Threading;
using System.Threading.Tasks;

namespace AzCopy.Contract
{
    public interface IAZCopyClient : IAZCopyChannel
    {
        /// <summary>
        /// Copies source data to a destination location.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy copy
        /// </code>
        /// </summary>
        /// <param name="src">copy src.</param>
        /// <param name="dst">copy dst.</param>
        /// <param name="option">copy option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns>task</returns>
        Task CopyAsync(LocationBase src, LocationBase dst, CopyOption option, CancellationToken ct);

        /// <summary>
        /// Delete blobs or files from an Azure storage account.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy remove
        /// </code>
        /// </summary>
        /// <param name="dst">delete src.</param>
        /// <param name="option">delete option.</param>
        /// <returns>task.</returns>
        Task RemoveAsync(LocationBase dst, RemoveOption option, CancellationToken ct);

        /// <summary>
        /// azcopy jobs clean.
        /// Remove all log and plan files for all jobs.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy jobs clean
        /// </code>
        /// </summary>
        /// <param name="option">Jobs clean option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task JobsCleanAsync(JobsCleanOption option, CancellationToken ct);

        /// <summary>
        /// Displays information on all jobs.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy jobs list
        /// </code>
        /// </summary>
        /// <param name="option">Jobs list option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task JobsListAsync(JobsListOption option, CancellationToken ct);

        /// <summary>
        /// Remove all files associated with the given job ID.
        /// Note that you can customize the location where log and plan files are saved.See the env command to learn more.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy jobs remove
        /// </code>
        /// </summary>
        /// <param name="jobId">job id.</param>
        /// <param name="option">Jobs remove option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task JobsRemoveAsync(string jobId, JobsRemoveOption option, CancellationToken ct);

        /// <summary>
        /// Resume the existing job with the given job ID.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy jobs resume
        /// </code>
        /// </summary>
        /// <param name="jobId">job id.</param>
        /// <param name="option">Jobs resume option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task JobsResumeAsync(string jobId, JobsResumeOption option, CancellationToken ct);

        /// <summary>
        /// Show detailed information for the given job ID.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy jobs show
        /// </code>
        /// </summary>
        /// <param name="jobId">job id.</param>
        /// <param name="option">Jobs show option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task JobsShowAsync(string jobId, JobsShowOption option, CancellationToken ct);

        /// <summary>
        /// Performs a performance benchmark.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy bench
        /// </code>
        /// </summary>
        /// <param name="destination">benchmark destination.</param>
        /// <param name="option">bench option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task BenchAsync(LocationBase destination, BenchOption option, CancellationToken ct);

        /// <summary>
        /// Shows the environment variables that you can use to configure the behavior of AzCopy.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy env
        /// </code>
        /// </summary>
        /// <param name="option">env option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task EnvAsync(EnvOption option, CancellationToken ct);

        /// <summary>
        /// List the entities in a given resource.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy list
        /// </code>
        /// </summary>
        /// <param name="location">location.</param>
        /// <param name="option">list option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task ListAsync(LocationBase location, ListOption option, CancellationToken ct);

        /// <summary>
        /// Log in to Azure Active Directory (AD) to access Azure Storage resources.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy login
        /// </code>
        /// </summary>
        /// <param name="option">login option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task LoginAsync(LoginOption option, CancellationToken ct);

        /// <summary>
        /// Log out to terminate access to Azure Storage resources.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy logout
        /// </code>
        /// </summary>
        /// <param name="option">logout option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task LogoutAsync(LogoutOption option, CancellationToken ct);

        /// <summary>
        /// Create a container or file share represented by the given resource URL.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy make
        /// </code>
        /// </summary>
        /// <param name="dst">remote location.</param>
        /// <param name="option">make option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task MakeAsync(LocationBase dst, MakeOption option, CancellationToken ct);

        /// <summary>
        /// Replicate source to the destination location.
        /// <para>
        /// <b>Command</b>
        /// </para>
        /// <code>
        ///     azcopy sync
        /// </code>
        /// </summary>
        /// <param name="src">src location.</param>
        /// <param name="dst">dst location.</param>
        /// <param name="option">sync option.</param>
        /// <param name="ct">cancellation token.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task SyncAsync(LocationBase src, LocationBase dst, SyncOption option, CancellationToken ct);
    }
}
