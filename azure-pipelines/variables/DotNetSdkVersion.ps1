$globalJson = Get-Content -Path "$PSScriptRoot\..\..\global.json" | ConvertFrom-Json
$globalJson.tools.dotnet
