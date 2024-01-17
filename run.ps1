$root = (Resolve-Path ..)
$env:Path = "$root\runtime\.dotnet\;${env:Path}"
Set-Location $PSScriptRoot
dotnet run -c Release --filter "*"  --coreRun (
    20000,
    4932,
    3200,
    2466,
    1233,
    616,
    308,
    200 | ForEach-Object { "$root\coreruns\threshold-$_\corerun.exe" })