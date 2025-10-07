
<div id="top"></div>

<!-- PROJECT INFO -->
<br />
<div align="center">
  <img src="../../assets/MyAvalonia.png" width="128" alt="MyAvalonia">
</div>

<h1 align="center">My .NET Avalonia Controls</h1>

[![MIT License](https://img.shields.io/github/license/sandre58/MyAvalonia?style=for-the-badge)](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Controls?style=for-the-badge)](https://www.nuget.org/packages/MyNet.Avalonia.Controls)

Advanced controls and UI components for Avalonia applications: color pickers, data grids, custom cursors, and integration with MyNet libraries.

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-purple)](#)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-purple)](#)
[![.NET 10.0](https://img.shields.io/badge/.NET-10.0-purple)](#)
[![C#](https://img.shields.io/badge/language-C%23-blue)](#)
[![Cross Platform](https://img.shields.io/badge/platform-Windows%20%7C%20macOS%20%7C%20Linux-lightgrey)](#)

---

## Installation

Install via NuGet:

```bash
dotnet add package MyNet.Avalonia.Controls
```

## Features

- **Avatar** - User profile picture display with fallback options
- **Badge** - Notification badges and status indicators
- **Banner** - Informational banners and alerts
- **Clock** - Digital and analog clock components
- **CodeBlock** - Syntax highlighted code display
- **ColorPicker** - Advanced color selection and palette controls
- **ColorPalettes** - Pre-defined color schemes and themes
- **Cursors** - Custom cursor definitions and management
- **DataGrid** - Enhanced data grid with sorting, filtering, and selection
- **DateTimePickers** - Date and time input controls
- **Divider** - Visual separators and spacers
- **ElasticWrapPanel** - Flexible layout panel with elastic wrapping
- **Forms** - Form controls and validation helpers
- **MultiComboBox** - Multi-selection combo box control
- **NavigationMenu** - Navigation and menu components
- **OutlinedIcon** - Icon controls with outline styles
- **OverflowStackPanel** - Stack panel with overflow handling
- **Pagination** - Page navigation and data pagination controls
- **TagBox** - Tag input and management control
- **Cross-platform compatibility** - Works on Windows, macOS, and Linux


## Styling & Themes

MyNet.Avalonia.Controls integrates seamlessly with the MyNet.Avalonia.Theme package to provide consistent styling across all controls.

### Using with MyNet Theme

To apply consistent theming to all controls, reference the MyNet theme in your `App.axaml`:

```xml
<Application.Resources>
  <ResourceDictionary>
    <ResourceDictionary.MergedDictionaries>
      <ResourceDictionary Source="avares://MyNet.Avalonia.Theme/MyTheme.axaml" />
    </ResourceDictionary.MergedDictionaries>
  </ResourceDictionary>
</Application.Resources>
```

### Custom Styling

Controls can be styled individually using Avalonia's styling system:

```xml
<Style Selector="controls|Avatar">
  <Setter Property="BorderThickness" Value="2" />
  <Setter Property="BorderBrush" Value="Blue" />
</Style>
```

### Color Palette Integration

Use the built-in color palettes for consistent theming:

```csharp
// Access predefined color palettes
var primaryPalette = ColorPalettes.Primary;
var accentColor = primaryPalette.Color500;
```


## Example Usage

### Avatar Control
```xml
<controls:Avatar Source="/Assets/user-avatar.png" 
                 Size="64" 
                 Initials="JD" 
                 Background="Blue" />
```

### Color Picker
```xml
<controls:ColorPicker x:Name="colorPicker" 
                      SelectedColor="Red" 
                      ShowAlphaChannel="True" />
```

### Multi ComboBox
```xml
<controls:MultiComboBox x:Name="multiCombo"
                        ItemsSource="{Binding AvailableItems}"
                        SelectedItems="{Binding SelectedItems}" />
```

### Navigation Menu
```xml
<controls:NavigationMenu>
  <controls:NavigationMenuItem Header="Home" Icon="Home" />
  <controls:NavigationMenuItem Header="Settings" Icon="Settings" />
  <controls:NavigationMenuItem Header="About" Icon="Info" />
</controls:NavigationMenu>
```

### Badge with Counter
```xml
<Grid>
  <Button Content="Messages" />
  <controls:Badge Count="5" 
                  Background="Red" 
                  HorizontalAlignment="Right" 
                  VerticalAlignment="Top" />
</Grid>
```

### Tag Box
```xml
<controls:TagBox x:Name="tagBox"
                 Tags="{Binding Tags}"
                 NewTagCommand="{Binding AddTagCommand}"
                 RemoveTagCommand="{Binding RemoveTagCommand}" />
```

## License

Copyright © Stéphane ANDRE.

Distributed under the MIT License. See [LICENSE](../../LICENSE) for details.