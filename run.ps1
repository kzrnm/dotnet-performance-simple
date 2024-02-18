param (
    [string]$Filter = "*"
)
$root = (Resolve-Path ..)
$env:Path = "$root\runtime\.dotnet\;${env:Path}"

# Set-Location "$root\runtime"
# ./build.cmd clr+libs+libs.tests -rc release -lc release

Set-Location $PSScriptRoot
dotnet run -c Release --filter $Filter --coreRun ('main', 'pr' | ForEach-Object { "$PSScriptRoot\coreruns\$_\corerun.exe" }) -m 
exit

dotnet run -c Release --filter "*Parse*" -d --coreRun (
    20000,
    4932,
    3200,
    2466,
    1233,
    616,
    308,
    200 | ForEach-Object { "$root\coreruns\threshold-$_\corerun.exe" }) -j short -m
