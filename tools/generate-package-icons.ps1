# Generates NuGet package icons (128x128 PNG) via the shared MyNet generator.
# Requires ../MyNet cloned next to MyAvalonia (same parent folder).
# Usage (repo root): powershell -File tools/generate-package-icons.ps1

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path $PSScriptRoot -Parent
$myNetRoot = Join-Path $repoRoot '..\MyNet'
$generatorProject = Join-Path $myNetRoot 'tools\MyNet.Tools.PackageIconGenerator\MyNet.Tools.PackageIconGenerator.csproj'

if (-not (Test-Path $generatorProject)) {
    throw "Shared generator not found at '$generatorProject'. Clone MyNet as a sibling of MyAvalonia."
}

$manifest = Join-Path $PSScriptRoot 'package-icons.json'
$svgDir = Join-Path $myNetRoot 'tools\icon-svgs'
$output = Join-Path $repoRoot 'assets'

Push-Location $repoRoot
try {
    dotnet run --project $generatorProject -c Release -- `
        --manifest $manifest `
        --svg-dir $svgDir `
        --output $output
    exit $LASTEXITCODE
}
finally {
    Pop-Location
}
