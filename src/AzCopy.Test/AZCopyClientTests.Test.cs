using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzCopy.Client;
using AzCopy.Contract;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AzCopy.Test
{

    public class AZCopyClientTests
    {
        private static Random random = new Random();
        private readonly string resourceUri;
        private readonly string container;
        private readonly string sasToken;

        private readonly ITestOutputHelper output;

        public AZCopyClientTests(ITestOutputHelper output)
        {
            this.output = output;
            this.resourceUri = @"https://azcopynettest.blob.core.windows.net";
            var sasToken = Environment.GetEnvironmentVariable("GRANTSETFIXSASTOKEN2");
            this.sasToken = sasToken;
            this.container = "grantsetfix";
        }

        [Fact]
        public void CommandArgsBase_should_build_arguments_if_use_quote_is_true()
        {
            var copyOption = new CopyOption();
            copyOption.ToString().Should().BeEmpty();

            // BlobType is string type.
            copyOption.BlobType = "BlockBlob";
            copyOption.ToString().Should().Be("--blob-type=\"BlockBlob\"");
        }

        [Fact]
        public void CommandArgsBase_should_build_arguments_if_arg_type_is_float()
        {
            var copyOption = new CopyOption();
            copyOption.ToString().Should().BeEmpty();

            copyOption.CapMbps = 100;
            copyOption.ToString().Should().Be("--cap-mbps=100");
        }

        [Fact]
        public void CommandArgsBase_should_build_arguments_if_arg_type_is_bool()
        {
            var copyOption = new CopyOption();
            copyOption.ToString().Should().BeEmpty();

            copyOption.CheckLength = false;
            copyOption.ToString().Should().Be("--check-length=false");

            copyOption.CheckLength = true;
            copyOption.ToString().Should().Be("--check-length=true");
        }

        [Fact]
        public void CommandArgsBase_should_build_arguments_if_is_flag_is_true()
        {
            var copyOption = new CopyOption();
            Assert.Equal(string.Empty, copyOption.ToString());

            copyOption.Recursive = true;
            copyOption.ToString().Should().Be("--recursive=true");

            copyOption.Recursive = false;
            copyOption.ToString().Should().Be("--recursive=false");

        }

        [Fact]
        public async Task TestUploadAndDeleteLocalFileToSASAsync()
        {
            var hasInfoMessage = false;
            var hasInitMessage = false;
            var hasProgressMessage = false;
            var hasEndOfJobMessage = false;
            var jobCompleted = false;

            var localFile = new LocalLocation()
            {
                UseWildCard = false,
            };

            var fileName = this.GetRandomFileName();

            var sasLocation = new RemoteSasLocation()
            {
                ResourceUri = this.resourceUri,
                Container = this.container,
                Path = fileName,
                SasToken = this.sasToken,
            };

            var option = new CopyOption();
            var client = new AZCopyClient();
            client.OutputMessageHandler += (object sender, JsonOutputTemplate e) =>
            {
                this.output.WriteLine(e.MessageContent);
                if (e.MessageType == MessageType.Info)
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == MessageType.Init)
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == MessageType.Progress)
                {
                    hasProgressMessage = true;
                }

                if (e.MessageType == MessageType.EndOfJob)
                {
                    hasEndOfJobMessage = true;
                }
            };

            client.JobStatusMessageHandler += (object sender, ListJobSummaryResponse e) =>
            {
                jobCompleted = e.JobStatus == JobStatus.Completed;
            };

            // create random file
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(1024 * 1024); // 1MB
                await fs.FlushAsync();
                localFile.Path = fileName;
            }

            await client.CopyAsync(localFile, sasLocation, option);

            Assert.True(hasInfoMessage);
            Assert.True(hasInitMessage);
            Assert.True(hasProgressMessage);
            Assert.True(hasEndOfJobMessage);
            Assert.True(jobCompleted);

            hasInitMessage = false;
            hasProgressMessage = false;
            hasEndOfJobMessage = false;
            jobCompleted = false;

            var deleteOption = new RemoveOption();
            await client.RemoveAsync(sasLocation, deleteOption);
            Assert.True(hasInitMessage);
            Assert.True(hasProgressMessage);
            Assert.True(hasEndOfJobMessage);
            Assert.True(jobCompleted);
        }

        [Fact]
        public async Task TestUploadNonExistLocalFileAsync()
        {
            var hasInfoMessage = false;
            var hasInitMessage = false;
            var hasProgressMessage = false;
            var hasEndOfJobMessage = false;
            var jobCompleted = false;
            var hitError = false;

            var localFile = new LocalLocation()
            {
                UseWildCard = false,
                Path = @"not/exist/file.bin",
            };

            var sasLocation = new RemoteSasLocation()
            {
                ResourceUri = this.resourceUri,
                Container = this.container,
                Path = @"some-random-file.bin",
                SasToken = this.sasToken,
            };

            var option = new CopyOption();
            var client = new AZCopyClient();
            client.OutputMessageHandler += (object sender, JsonOutputTemplate e) =>
            {
                this.output.WriteLine(e.MessageContent);
                if (e.MessageType == MessageType.Info)
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == MessageType.Init)
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == MessageType.Progress)
                {
                    hasProgressMessage = true;
                }

                if (e.MessageType == MessageType.EndOfJob)
                {
                    hasEndOfJobMessage = true;
                }

                if (e.MessageType == MessageType.Error)
                {
                    hitError = true;
                }
            };

            client.JobStatusMessageHandler += (object sender, ListJobSummaryResponse e) =>
            {
                jobCompleted = e.JobStatus == JobStatus.Completed;
            };

            await client.CopyAsync(localFile, sasLocation, option);
            Assert.True(hasInfoMessage);
            Assert.False(hasInitMessage);
            Assert.False(hasProgressMessage);
            Assert.False(hasEndOfJobMessage);
            Assert.False(jobCompleted);
            Assert.True(hitError);
        }

        [Fact]
        public async Task TestUploadToNonExistContainerAsync()
        {
            var hasInfoMessage = false;
            var hasInitMessage = false;
            var hasProgressMessage = false;
            var hasEndOfJobMessage = false;
            var jobCanceled = false;
            var errorCode = 0;

            var localFile = new LocalLocation()
            {
                UseWildCard = false,
            };

            var fileName = this.GetRandomFileName();

            var sasLocation = new RemoteSasLocation()
            {
                ResourceUri = this.resourceUri,
                Container = "not-exist",
                Path = fileName,
                SasToken = this.sasToken,
            };

            var option = new CopyOption();
            var client = new AZCopyClient();
            client.OutputMessageHandler += (object sender, JsonOutputTemplate e) =>
            {
                this.output.WriteLine(e.MessageContent);
                if (e.MessageType == MessageType.Info)
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == MessageType.Init)
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == MessageType.Progress)
                {
                    hasProgressMessage = true;
                }

                if (e.MessageType == MessageType.EndOfJob)
                {
                    hasEndOfJobMessage = true;

                }
            };

            client.JobStatusMessageHandler += (object sender, ListJobSummaryResponse e) =>
            {
                jobCanceled = e.JobStatus == JobStatus.Failed;
                if (jobCanceled)
                {
                    errorCode = e.FailedTransfers[0].ErrorCode;
                }
            };

            // create random file
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(1024); // 1KB
                await fs.FlushAsync();
                localFile.Path = fileName;
            }

            await client.CopyAsync(localFile, sasLocation, option);

            Assert.True(hasInfoMessage);
            Assert.True(hasInitMessage);
            Assert.True(hasProgressMessage);
            Assert.True(hasEndOfJobMessage);
            Assert.True(jobCanceled);
            Assert.Equal(404, errorCode);
        }

        [Fact]
        public async Task TestUploadLocalSingleFileToSASCancellationAsync()
        {
            var hasInfoMessage = false;
            var hasInitMessage = false;
            var hasEndOfJobMessage = false;
            var jobCancelled = false;

            var localFile = new LocalLocation()
            {
                UseWildCard = false,
            };

            var fileName = this.GetRandomFileName();
            var sasLocation = new RemoteSasLocation()
            {
                ResourceUri = this.resourceUri,
                Container = this.container,
                Path = fileName,
                SasToken = this.sasToken,
            };

            var option = new CopyOption();
            var client = new AZCopyClient();
            client.OutputMessageHandler += (object sender, JsonOutputTemplate e) =>
            {
                this.output.WriteLine(e.MessageContent);
                if (e.MessageType == MessageType.Info)
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == MessageType.Init)
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == MessageType.EndOfJob)
                {
                    hasEndOfJobMessage = true;
                }
            };

            client.JobStatusMessageHandler += (object sender, ListJobSummaryResponse e) =>
            {
                jobCancelled = e.JobStatus == JobStatus.Cancelled;
            };

            // create cancellation Token
            var cts = new CancellationTokenSource();

            // create random file
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(1024L * 1024); // 1MB
                await fs.FlushAsync();
                localFile.Path = fileName;
            }

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            cts.CancelAfter(5);
            await client.CopyAsync(localFile, sasLocation, option, cts.Token);
            stopWatch.Stop();
            this.output.WriteLine("time elapes: " + stopWatch.ElapsedMilliseconds.ToString());

            Assert.True(hasInfoMessage);
            Assert.True(hasInitMessage);
            Assert.True(hasEndOfJobMessage);
            Assert.True(jobCancelled);
        }

        [Fact]
        public async Task TestUploadAndDeleteLocalFolderWithPatternToSASAsync()
        {
            var hasInfoMessage = false;
            var hasInitMessage = false;
            var hasProgressMessage = false;
            var hasEndOfJobMessage = false;
            var jobCompleted = false;
            var totalFiles = 0;

            var localFile = new LocalLocation()
            {
                UseWildCard = true,
                Path = @"TestData/fruits",
            };

            var sasLocation = new RemoteSasLocation()
            {
                ResourceUri = this.resourceUri,
                Container = this.container,
                Path = @"fruits",
                SasToken = this.sasToken,
            };

            var option = new CopyOption()
            {
                Recursive = true,
                IncludePattern = "*.jpg;*.png",
            };

            var client = new AZCopyClient();
            client.OutputMessageHandler += (object sender, JsonOutputTemplate e) =>
            {
                Console.WriteLine(e.MessageContent);
                if (e.MessageType == MessageType.Info)
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == MessageType.Init)
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == MessageType.Progress)
                {
                    hasProgressMessage = true;
                }

                if (e.MessageType == MessageType.EndOfJob)
                {
                    hasEndOfJobMessage = true;
                }
            };

            client.JobStatusMessageHandler += (object sender, ListJobSummaryResponse e) =>
            {
                jobCompleted = e.JobStatus == JobStatus.Completed;
                totalFiles = e.TotalTransfers;
            };

            await client.CopyAsync(localFile, sasLocation, option);
            Assert.True(hasInfoMessage);
            Assert.True(hasInitMessage);
            Assert.True(hasProgressMessage);
            Assert.True(hasEndOfJobMessage);
            Assert.True(jobCompleted);
            Assert.Equal(6, totalFiles);

            hasInitMessage = false;
            hasProgressMessage = false;
            hasEndOfJobMessage = false;
            jobCompleted = false;

            var deleteOption = new RemoveOption()
            {
                Recursive = true,
            };
            await client.RemoveAsync(sasLocation, deleteOption);
            Assert.True(hasInitMessage);
            Assert.True(hasProgressMessage);
            Assert.True(hasEndOfJobMessage);
            Assert.True(jobCompleted);
        }

        [Fact]
        public async Task TestUploadAndDeleteExistingLocalFile()
        {
            var isSkip = false;
            var localFile = new LocalLocation()
            {
                UseWildCard = false,
            };

            var fileName = this.GetRandomFileName();

            var sasLocation = new RemoteSasLocation()
            {
                ResourceUri = this.resourceUri,
                Container = this.container,
                Path = fileName,
                SasToken = this.sasToken,
            };

            var option = new CopyOption();
            option.Overwrite = "ifSourceNewer";
            option.CapMbps = 4;

            var client = new AZCopyClient();
            client.OutputMessageHandler += (object sender, JsonOutputTemplate e) =>
            {
                this.output.WriteLine(e.MessageContent);
            };

            client.JobStatusMessageHandler += (object sender, ListJobSummaryResponse e) =>
            {
                if (e.JobStatus == JobStatus.CompletedWithSkipped)
                {
                    isSkip = true;
                }
            };

            // create random file
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(1024 * 1024); // 1MB
                await fs.FlushAsync();
                localFile.Path = fileName;
            }

            await client.CopyAsync(localFile, sasLocation, option);

            await Task.Delay(3 * 1000); // delay 3 s

            // upload again
            await client.CopyAsync(localFile, sasLocation, option);

            var deleteOption = new RemoveOption()
            {
                Recursive = true,
            };
            await client.RemoveAsync(sasLocation, deleteOption);

            Assert.True(isSkip);
        }

        private string GetRandomFileName()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[AZCopyClientTests.random.Next(s.Length)]).ToArray()) + ".bin";
        }
    }
}
