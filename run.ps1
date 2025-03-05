param (
    $Filter = "*",
    [ValidateSet("Dry", "Short", "Medium", "Long", "Default")]
    [string]$Job = "Short",
    [string[]]$Corerun = @('main', 'pr'),
    [switch]$Build
)

function Crossgen {
    param ([Parameter(Mandatory, Position = 0)]$Dir)

    $orig = (Get-ITem "$Dir/System.Runtime.Numerics.dll")

    $OutPath = "$Dir/System.Runtime.Numerics2.dll"
    $out = (Get-ITem "$OutPath" -ErrorAction SilentlyContinue)
    if (($null -ne $out) -and ($out.Length -eq $orig.Length)) {
        # System.Runtime.Numerics2 is updated
        return
    }

    dotnet $Root\runtime\artifacts\bin\coreclr\windows.x64.Release\crossgen2\crossgen2.dll (
        "System.Runtime.Numerics", "System.Memory", "System.Private.CoreLib", "System.Runtime" | ForEach-Object {
            "$dir/$_.dll"
        }) --out "$OutPath" --verbose

    $out = Get-Item "$OutPath"
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
    Crossgen $dir
}
dotnet run -c Release -- --filter $Filter --coreRun ($CorerunsDir | ForEach-Object { "$_\corerun.exe" }) -m -j $Job