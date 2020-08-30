# AzCopy.Net

AzCopy.Net is a .net standard library for [AzCopy](https://github.com/Azure/azure-storage-azcopy). It is a thin wrapper out of AzCopy v10 cli.

[![Build Status](https://dev.azure.com/xiaoyuz0315/BigMiao/_apis/build/status/LittleLittleCloud.AzCopy.Net?branchName=refs%2Fpull%2F1%2Fmerge)](https://dev.azure.com/xiaoyuz0315/BigMiao/_build/latest?definitionId=2&branchName=refs%2Fpull%2F1%2Fmerge) ![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/xiaoyuz0315/BigMiao/2) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

# Quick start

First, add nuget reference to `AzCopy.Client`. It provides straight forward API to call into AzCopy v10 cli.

```
<PackageReference Include="AzCopy.Client" Version="1.0.0" />
```

Then add reference to one of the four nuget asset packages based on your OS and platform. The version of these packages are corresponded to the version of azcopy v10 cli it carries with.
- AzCopy.WinX64
- AzCopy.WinX86
- AzCopy.OsxX64
- AzCopy.LinuxX64

For example:
```
<PackageReference Include="AzCopy.WinX64" Version="1.0.0" />
```

If azcopy v10 cli has already installed on your machine, you can also add its full path to `$AZCOPYPATH`. When running the app, `AzCopy.Client` will use the azcopy cli pointed by `$AZCOPYPATH`.

Finally, use AzCopy.Client the same way of using azcopy in cmd!

```
var localFile = new LocalLocation()
{
  UseWildCard = true,
  Path = @"src",
};

var sasLocation = new RemoteSasLocation()
{
    ResourceUri = @"uri",
    Container = @"container"
    Path = @"dest",
    SasToken = @"sastoken",
};

var option = new AZCopyOption()
{
    IncludePattern = "*.jpg;*.png",
};

var client = new AZCopyClient();

// subscribe output event hander to get output from azcopy v10 cli
client.OutputMsgHandler += (object sender, JsonOutputTemplate e) =>
            {
                Console.WriteLine(e.MessageContent);
            };
await client.CopyAsync(localFile, sasLocation, option);
```

# Supported command

`AzCopy.Net` supports all commands that azcopy v10 cli has. The interface of supported command is `AzCopy.Contract.IAZCopyClient`, and is implemented by `AzCopy.Client.AzCopyClient` class.

The supported command list is presented below.
- `bench`
- `copy`
- `doc`
- `env`
- `jobs clean`
- `jobs list`
- `jobs remove`
- `jobs resume`
- `jobs show`
- `list`
- `login`
- `logout`
- `make`
- `remove`
- `sync`

# Tested senarios
The following scenarios have been validated on windows, mac os and ubuntu.

- [x] Upload files and directories to azure blob container (use SAS only)
- [x] Delete files and directories from azure blob container (use SAS only)
- [x] Download files and directories from azure blob container (use SAS only)

# Nightly build
> https://pkgs.dev.azure.com/xiaoyuz0315/BigMiao/_packaging/AzCopy.Net/nuget/v3/index.json

## Pre-released package
|[AzCopy.Net](https://dev.azure.com/xiaoyuz0315/BigMiao/_packaging?_a=feed&feed=AzCopy.Net)|Local|
|-|-|
|AzCopy.Contract|[![AzCopy.Contract package in AzCopy.Net feed in Azure Artifacts](https://feeds.dev.azure.com/xiaoyuz0315/1bf31d68-811d-4872-ae8b-cdd289c934f1/_apis/public/Packaging/Feeds/d3e592c1-5c13-4f04-8516-8c9562a2537a/Packages/1be9697e-cef9-4a2f-bfb4-47abe958e3e6/Badge)](https://dev.azure.com/xiaoyuz0315/BigMiao/_packaging?_a=package&feed=d3e592c1-5c13-4f04-8516-8c9562a2537a&package=1be9697e-cef9-4a2f-bfb4-47abe958e3e6&preferRelease=true)|
|AzCopy.Client|[![AzCopy.Client package in AzCopy.Net feed in Azure Artifacts](https://feeds.dev.azure.com/xiaoyuz0315/1bf31d68-811d-4872-ae8b-cdd289c934f1/_apis/public/Packaging/Feeds/d3e592c1-5c13-4f04-8516-8c9562a2537a/Packages/159cc454-0282-4a52-8338-8e115a45f642/Badge)](https://dev.azure.com/xiaoyuz0315/BigMiao/_packaging?_a=package&feed=d3e592c1-5c13-4f04-8516-8c9562a2537a&package=159cc454-0282-4a52-8338-8e115a45f642&preferRelease=true)|
|AzCopy.WinX64| [![AzCopy.WinX64 package in AzCopy.Net@Local feed in Azure Artifacts](https://feeds.dev.azure.com/xiaoyuz0315/1bf31d68-811d-4872-ae8b-cdd289c934f1/_apis/public/Packaging/Feeds/d3e592c1-5c13-4f04-8516-8c9562a2537a%404458679a-8715-4070-9dbb-dd189a22d11d/Packages/5dbe4d8f-3562-4c81-8929-e8a507695049/Badge)](https://dev.azure.com/xiaoyuz0315/BigMiao/_packaging?_a=package&feed=d3e592c1-5c13-4f04-8516-8c9562a2537a%404458679a-8715-4070-9dbb-dd189a22d11d&package=5dbe4d8f-3562-4c81-8929-e8a507695049&preferRelease=true)|
|AzCopy.WinX86| [![AzCopy.WinX86 package in AzCopy.Net@Local feed in Azure Artifacts](https://feeds.dev.azure.com/xiaoyuz0315/1bf31d68-811d-4872-ae8b-cdd289c934f1/_apis/public/Packaging/Feeds/d3e592c1-5c13-4f04-8516-8c9562a2537a%404458679a-8715-4070-9dbb-dd189a22d11d/Packages/60103c70-9123-476c-9383-fa39b2d49d36/Badge)](https://dev.azure.com/xiaoyuz0315/BigMiao/_packaging?_a=package&feed=d3e592c1-5c13-4f04-8516-8c9562a2537a%404458679a-8715-4070-9dbb-dd189a22d11d&package=60103c70-9123-476c-9383-fa39b2d49d36&preferRelease=true)|
|AzCopy.OsxX64|[![AzCopy.OsxX64 package in AzCopy.Net@Local feed in Azure Artifacts](https://feeds.dev.azure.com/xiaoyuz0315/1bf31d68-811d-4872-ae8b-cdd289c934f1/_apis/public/Packaging/Feeds/d3e592c1-5c13-4f04-8516-8c9562a2537a%404458679a-8715-4070-9dbb-dd189a22d11d/Packages/29bb57d0-26bc-4167-b6df-fc2629d0a6e8/Badge)](https://dev.azure.com/xiaoyuz0315/BigMiao/_packaging?_a=package&feed=d3e592c1-5c13-4f04-8516-8c9562a2537a%404458679a-8715-4070-9dbb-dd189a22d11d&package=29bb57d0-26bc-4167-b6df-fc2629d0a6e8&preferRelease=true)|
|AzCopy.LinuxX64| [![AzCopy.LinuxX64 package in AzCopy.Net@Local feed in Azure Artifacts](https://feeds.dev.azure.com/xiaoyuz0315/1bf31d68-811d-4872-ae8b-cdd289c934f1/_apis/public/Packaging/Feeds/d3e592c1-5c13-4f04-8516-8c9562a2537a%404458679a-8715-4070-9dbb-dd189a22d11d/Packages/44b44414-e8df-491c-9106-f4423f3859a3/Badge)](https://dev.azure.com/xiaoyuz0315/BigMiao/_packaging?_a=package&feed=d3e592c1-5c13-4f04-8516-8c9562a2537a%404458679a-8715-4070-9dbb-dd189a22d11d&package=44b44414-e8df-491c-9106-f4423f3859a3&preferRelease=true)|

# Examples

Check [AzCopy.Test](https://github.com/LittleLittleCloud/AzCopy.Net/blob/41856b39ff710cf0f9844d00b73c1ab9bfbb919b/src/AzCopy.Test/AZCopyClientTests.Test.cs#L15) for more examples.
