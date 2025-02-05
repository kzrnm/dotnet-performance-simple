param (
    [string]$Filter = "*",
    [ValidateSet("Dry", "Short", "Medium", "Long", "Default")]
    [string]$Job = "Short",
    [string[]]$Corerun = @('main', 'pr'),
    [switch]$Build
)

Write-Output "Build: $Build"
Write-Output "Job: $Job"
Write-Output "Coreruns: $Corerun"
Write-Output "Filter: $Filter"

$root = (Resolve-Path ..)
$env:Path = "$root\runtime\.dotnet\;${env:Path}"

if ($Build) {
    Set-Location "$root\runtime"
    ./build.cmd clr+libs+libs.tests -rc release -lc release
}
Set-Location $PSScriptRoot

$Coreruns = $Corerun | ForEach-Object { "$PSScriptRoot\coreruns\$_\corerun.exe" }
dotnet run -c Release --filter $Filter --coreRun $Coreruns -m -j $Job
exit

$thresholdCoreruns = @(
    20000,
    4932,
    3200,
    2466,
    1233,
    616,
    308,
    200
) | ForEach-Object { "$root\coreruns\threshold-$_\corerun.exe" }
dotnet run -c Release --filter "*Parse*" -d --coreRun $thresholdCoreruns -m -j $Job
