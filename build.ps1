#!/usr/bin/env bash
<#
.SYNOPSIS
Build project
.DESCRIPTION
.PARAMETER
#>

[CmdletBinding(SupportsShouldProcess=$false)]
Param (
  [Parameter()]
  [switch]$BinaryLog,
  [Parameter()]
  [switch]$Pack,
  [ValidateSet('Debug', 'Release')]
  [string]$Configuration='Debug',
  [Parameter()]
  [string]$Projects,
  [Parameter()]
  [switch]$Clean,
  [Parameter()]
  [switch]$Test,
  [Parameter()]
  [string]$BuildNumber
)


function Create-Directory([string[]] $path) {
  if (!(Test-Path $path)) {
    New-Item -path $path -force -itemType 'Directory' | Out-Null
  }
}


$HeaderColor = 'Green'
$BuildNumber = $BuildNumber -replace "['.']",''
$DotNet = & "$PSScriptRoot\azure-pipelines\Get-Dotnet.ps1"
$RepoRoot = $PSScriptRoot
$ArtifactsRoot = Join-Path $RepoRoot 'artifacts'
$LogDir = Join-Path (Join-Path $ArtifactsRoot 'log') $Configuration
$TestResultRoot = Join-Path (Join-Path $ArtifactsRoot 'TestResults') $Configuration

if ($Clean){
  if ((Test-Path $ArtifactsRoot)){
    Write-Host "Clean Artifacts..." -ForegroundColor $HeaderColor
    Remove-Item $ArtifactsRoot -Recurse -Force -Confirm:$false
  }
  
  exit $lastexitcode
}


Create-Directory $LogDir

$MsBuildArgs = "-property:Configuration=$Configuration"

if ($Projects){
  $Projects = ($Projects.Split(';').ForEach({Resolve-Path $_}) -join ';')
  Write-Host "Build projects $Projects" -ForegroundColor $HeaderColor
  $MsBuildArgs = $Projects + " " + $MsBuildArgs
}else{
  $sln = 'AzCopy.Net.sln '
  $MsBuildArgs = $sln + $MsBuildArgs
}

if ($BinaryLog){
  Write-Host "Write BinaryLog to $LogDir" -ForegroundColor $HeaderColor
  $MsBuildArgs += ' -bl:' + (Join-Path $LogDir 'Build.binlog')
}

if ($Pack){
  $MsBuildArgs += ' -target:Build,Pack'
}



Push-Location $PSScriptRoot

try{
  Write-Host "MsBuildArgs $MsBuildArgs" -ForegroundColor $HeaderColor
  $p = Start-Process $DotNet ('msbuild','-restore', $MsBuildArgs, "-property:BUILDNUMBER=$BuildNumber") -NoNewWindow -Wait -PassThru

  if ($p.ExitCode -ne 0){
    throw "Build task fail"
  }

  if ($Test){
    Write-Host "Start Testing..." $HeaderColor
    $p = Start-Process $DotNet "test $sln -v n -r $TestResultRoot -l trx --no-build -c $Configuration --collect ""Code Coverage"""  -NoNewWindow -Wait -PassThru
  }

}
catch{
  Write-Error $error[0]
  throw $p.ExitCode
}
finally {
  Pop-Location
}
