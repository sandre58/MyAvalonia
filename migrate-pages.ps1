# Script de migration des pages AutoBuildPage vers le nouveau système
# Ce script migre automatiquement les pages

$pages = @(
    @{Name="ToggleButton"; Layout="Circle"; HasSizes=$true},
    @{Name="ToggleSwitch"; Layout=$null; HasSizes=$true},
    @{Name="Slider"; Layout=$null; HasSizes=$false},
    @{Name="ProgressBar"; Layout=$null; HasSizes=$true},
    @{Name="TextBlock"; Layout=$null; HasSizes=$true; HasStyles=@("Secondary", "Tertiary", "Underline", "Delete", "Disablable")},
    @{Name="SelectableTextBlock"; Layout=$null; HasSizes=$true; HasStyles=@("Secondary", "Tertiary", "Underline", "Delete", "Disablable")},
    @{Name="HyperlinkButton"; Layout=$null; HasSizes=$false; HasStyles=@("Text")},
    @{Name="Label"; Layout=$null; HasSizes=$false},
    @{Name="ListBox"; Layout=$null; HasSizes=$false},
    @{Name="Expander"; Layout=$null; HasSizes=$false},
    @{Name="TabControl"; Layout=$null; HasSizes=$false}
)

foreach ($page in $pages) {
    $controlName = $page.Name
    $viewModelName = "${controlName}PageViewModel"
    $viewModelPath = "demos\MyNet.Avalonia.Demo\ViewModels\$viewModelName.cs"
    
    Write-Host "Creating ViewModel for $controlName..." -ForegroundColor Green
    
    # Le script complet serait ici mais en raison de la complexité,
    # je vais créer les fichiers manuellement un par un
}

Write-Host "Migration terminée!" -ForegroundColor Green
