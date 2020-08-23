using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AzCopy;
using Microsoft.AzCopy.Contract;
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
            this.resourceUri = @"https://test1storageafyok6j49.blob.core.windows.net";
            var sasToken = Environment.GetEnvironmentVariable("GRANTSETFIXSASTOKEN");
            this.sasToken = sasToken;
            this.container = "grantsetfix";
        }

        [Fact]
        public void TestAZCopyOption()
        {
            var copyOption = new AZCopyOption();
            Assert.Equal(string.Empty, copyOption.ToCommandLineString());

            copyOption.BlobType = "BlockBlob";
            Assert.Equal("--blob-type \"BlockBlob\"", copyOption.ToCommandLineString());

            // set flag as true
            copyOption.Recursive = string.Empty;
            Assert.Equal("--blob-type \"BlockBlob\" --recursive", copyOption.ToCommandLineString());
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

            var option = new AZCopyOption();
            var client = new AZCopyClient();
            client.JobStatusHandler += (object sender, AZCopyMessageBase e) =>
            {
                this.output.WriteLine(e.MessageType + e.MessageContent);
                if (e.MessageType == "Info")
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == "Init")
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == "Progress")
                {
                    hasProgressMessage = true;
                }

                if (e.MessageType == "EndOfJob")
                {
                    hasEndOfJobMessage = true;
                    jobCompleted = ((AZCopyEndOfJobMessage)e).JobStatus == "Completed";
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

            Assert.True(hasInfoMessage);
            Assert.True(hasInitMessage);
            Assert.True(hasProgressMessage);
            Assert.True(hasEndOfJobMessage);
            Assert.True(jobCompleted);

            hasInitMessage = false;
            hasProgressMessage = false;
            hasEndOfJobMessage = false;
            jobCompleted = false;

            var deleteOption = new AZDeleteOption();
            await client.DeleteAsync(sasLocation, deleteOption);
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

            var option = new AZCopyOption();
            var client = new AZCopyClient();
            client.JobStatusHandler += (object sender, AZCopyMessageBase e) =>
            {
                this.output.WriteLine(e.MessageContent);
                if (e.MessageType == "Info")
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == "Init")
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == "Progress")
                {
                    hasProgressMessage = true;
                }

                if (e.MessageType == "EndOfJob")
                {
                    hasEndOfJobMessage = true;
                    jobCompleted = ((AZCopyEndOfJobMessage)e).JobStatus == "Completed";
                }

                if (e.MessageType == "Error")
                {
                    hitError = true;
                }
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

            var option = new AZCopyOption();
            var client = new AZCopyClient();
            client.JobStatusHandler += (object sender, AZCopyMessageBase e) =>
            {
                this.output.WriteLine(e.MessageType + e.MessageContent);
                if (e.MessageType == "Info")
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == "Init")
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == "Progress")
                {
                    hasProgressMessage = true;
                }

                if (e.MessageType == "EndOfJob")
                {
                    hasEndOfJobMessage = true;
                    jobCanceled = ((AZCopyEndOfJobMessage)e).JobStatus == "Failed";
                    errorCode = ((AZCopyEndOfJobMessage)e).FailedTransfers[0].ErrorCode;
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

            var option = new AZCopyOption();
            var client = new AZCopyClient();
            client.JobStatusHandler += (object sender, AZCopyMessageBase e) =>
            {
                this.output.WriteLine(e.MessageContent);
                if (e.MessageType == "Info")
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == "Init")
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == "EndOfJob")
                {
                    hasEndOfJobMessage = true;
                    jobCancelled = ((AZCopyEndOfJobMessage)e).JobStatus == "Cancelled";
                }
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

            var option = new AZCopyOption()
            {
                Recursive = string.Empty,
                IncludePattern = "*.jpg;*.png",
            };

            var client = new AZCopyClient();
            client.JobStatusHandler += (object sender, AZCopyMessageBase e) =>
            {
                Console.WriteLine(e.MessageContent);
                if (e.MessageType == "Info")
                {
                    hasInfoMessage = true;
                }

                if (e.MessageType == "Init")
                {
                    hasInitMessage = true;
                }

                if (e.MessageType == "Progress")
                {
                    hasProgressMessage = true;
                }

                if (e.MessageType == "EndOfJob")
                {
                    hasEndOfJobMessage = true;
                    jobCompleted = ((AZCopyEndOfJobMessage)e).JobStatus == "Completed";
                    totalFiles = ((AZCopyEndOfJobMessage)e).TotalTransfers;
                }
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

            var deleteOption = new AZDeleteOption()
            {
                Recursive = string.Empty,
            };
            await client.DeleteAsync(sasLocation, deleteOption);
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

            var option = new AZCopyOption();
            option.Overwrite = "ifSourceNewer";
            option.CapMbps = "4";

            var client = new AZCopyClient();
            client.JobStatusHandler += (object sender, AZCopyMessageBase e) =>
            {
                this.output.WriteLine(e.MessageContent);

                if (e.MessageType == "EndOfJob")
                {
                    if (((AZCopyEndOfJobMessage)e).JobStatus == "CompletedWithSkipped")
                    {
                        isSkip = true;
                    }
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

            var deleteOption = new AZDeleteOption()
            {
                Recursive = string.Empty,
            };
            await client.DeleteAsync(sasLocation, deleteOption);

            Assert.True(isSkip);
        }

        [Fact]
        public void TestParseAZCopyMessages()
        {
            // AZCopyEndOfJobMessage
            var json = @"{
    ""TimeStamp"": ""2019-11-19T13:58:16.5202825-08:00"",
    ""MessageType"": ""EndOfJob"",
    ""MessageContent"": ""{\""ErrorMsg\"":\""\"",\""ActiveConnections\"":0,\""CompleteJobOrdered\"":true,\""JobStatus\"":\""Completed\"",\""TotalTransfers\"":1,\""TransfersCompleted\"":1,\""TransfersFailed\"":0,\""TransfersSkipped\"":0,\""BytesOverWire\"":1073741824,\""TotalBytesTransferred\"":1073741824,\""TotalBytesEnumerated\"":1073741824,\""TotalBytesExpected\"":1073741824,\""PercentComplete\"":100,\""AverageIOPS\"":7,\""AverageE2EMilliseconds\"":7544,\""ServerBusyPercentage\"":0,\""NetworkErrorPercentage\"":0,\""FailedTransfers\"":[],\""SkippedTransfers\"":null,\""PerfConstraint\"":0,\""PerformanceAdvice\"":[],\""IsCleanupJob\"":false}"",
    ""PromptDetails"": {
        ""PromptType"": """",
        ""ResponseOptions"": null,
        ""PromptTarget"": """"
    }
}";
            var azCopyEndOfJobMessage = AZCopyMessageFactory.CreateFromJson(json) as AZCopyEndOfJobMessage;
            Assert.Equal("Completed", azCopyEndOfJobMessage.JobStatus);
            Assert.Equal(100, azCopyEndOfJobMessage.PercentComplete);
            Assert.Equal(0, azCopyEndOfJobMessage.SkippedTransfers.Count);
            Assert.Equal(0, azCopyEndOfJobMessage.FailedTransfers.Count);
        }

        private string GetRandomFileName()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[AZCopyClientTests.random.Next(s.Length)]).ToArray()) + ".bin";
        }
    }
}
