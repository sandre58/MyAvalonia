# Regenerates src/*/README.md via the shared MyNet script.
# Requires ../MyNet cloned next to MyAvalonia (same parent folder).
# Usage (repo root): powershell -File tools/update-package-readmes.ps1

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path $PSScriptRoot -Parent
$myNetScript = Join-Path $repoRoot '..\MyNet\tools\update-package-readmes.ps1'
$packagesConfig = Join-Path $PSScriptRoot 'package-readmes.json'

if (-not (Test-Path $myNetScript)) {
    throw "Shared script not found at '$myNetScript'. Clone MyNet as a sibling of MyAvalonia."
}

& $myNetScript `
    -RepoRoot $repoRoot `
    -GitHubRepo 'sandre58/MyAvalonia' `
    -PackagesConfig $packagesConfig

exit $LASTEXITCODE
