param (
    $Filter = "*",
    [ValidateSet("Dry", "Short", "Medium", "Long", "Default")]
    [string]$Job = "Short",
    [string[]]$Corerun = @('main', 'pr'),
    [switch]$Dryrun,
    [switch]$Build,
    [switch]$NoCrossgen
)

function NoCrossgen {
    param ([Parameter(Mandatory, Position = 0)]$Dir)

    $orig = (Get-Item "$Dir/System.Runtime.Numerics.dll")
    $backup = (Get-Item "$Dir/System.Runtime.NumericsOrig.dll")

    if (($null -eq $backup) -or ($backup.Length -eq $orig.Length)) {
        # System.Runtime.Numerics2 is updated
        return
    }

    Copy-Item "$Dir/System.Runtime.NumericsOrig.dll" "$orig" -Force
    Get-Item "$orig"
}

function Crossgen {
    param ([Parameter(Mandatory, Position = 0)]$Dir)

    $orig = (Get-Item "$Dir/System.Runtime.Numerics.dll")

    $OutPath = "$Dir/System.Runtime.Numerics2.dll"
    $out = (Get-Item "$OutPath" -ErrorAction SilentlyContinue)
    if (($null -ne $out) -and ($out.Length -eq $orig.Length)) {
        # System.Runtime.Numerics2 is updated
        return
    }

    dotnet $Root\runtime\artifacts\bin\coreclr\windows.x64.Release\crossgen2\crossgen2.dll (
        "System.Runtime.Numerics", "System.Memory", "System.Private.CoreLib", "System.Runtime" | ForEach-Object {
            "$dir/$_.dll"
        }) --out "$OutPath" --verbose

    $out = Get-Item "$OutPath"
    Copy-Item "$orig" "$Dir/System.Runtime.NumericsOrig.dll" -Force
    Copy-Item "$out" "$orig" -Force
    Get-Item "$orig"
}

Write-Output "Build: $Build"
Write-Output "Job: $Job"
Write-Output "Coreruns: $Corerun"
Write-Output "Filter: $Filter"

$root = (Resolve-Path ..)
$env:Path = "$root\runtime\.dotnet\;${env:Path}"

if ($Build) {
    Set-Location "$root\runtime"
    ./build.cmd clr+libs -rc release -lc release
}
Set-Location $PSScriptRoot

$CorerunsDir = $Corerun | ForEach-Object { "$PSScriptRoot\coreruns\$_" }
foreach ($dir in $CorerunsDir) {
    if ($NoCrossgen) {
        NoCrossgen $dir
    }
    else {
        Crossgen $dir
    }
}

if ($Dryrun) {
    Write-Host dotnet run -c Release -- --filter $Filter --coreRun ($CorerunsDir | ForEach-Object { "$_\corerun.exe" }) -m -j $Job
}
else {
    dotnet run -c Release -- --filter $Filter --coreRun ($CorerunsDir | ForEach-Object { "$_\corerun.exe" }) -m -j $Job
}