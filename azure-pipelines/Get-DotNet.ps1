$path = "$PSScriptRoot\..\.dotnet\dotnet.exe"

if (!(Test-Path $path)) {
    Write-Error "Can't find dotnet.exe, please run init.cmd first"
    throw "Can't find dotnet.exe"
}

(Resolve-Path $path).Path
