try {
    Push-Location "$PSScriptRoot/main"
    rm *
}
finally {
    Pop-Location
}

Copy-Item -Recurse "$PSScriptRoot/pr/*"  "$PSScriptRoot/main/"