
<div id="top"></div>

<!-- PROJECT INFO -->
<br />
<div align="center">
  <img src="../../assets/MyAvalonia.png" width="128" alt="MyAvalonia">
</div>

<h1 align="center">My .NET Avalonia</h1>

[![MIT License](https://img.shields.io/github/license/sandre58/MyAvalonia?style=for-the-badge)](https://github.com/sandre58/MyAvalonia/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia?style=for-the-badge)](https://www.nuget.org/packages/MyNet.Avalonia)

Extensions and helpers for Avalonia UI development: controls, theming, reactive programming, and integration with MyNet libraries.

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-purple)](#)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-purple)](#)
[![.NET 10.0](https://img.shields.io/badge/.NET-10.0-purple)](#)
[![C#](https://img.shields.io/badge/language-C%23-blue)](#)
[![Cross Platform](https://img.shields.io/badge/platform-Windows%20%7C%20macOS%20%7C%20Linux-lightgrey)](#)

---

## Installation

Install via NuGet:

```bash
dotnet add package MyNet.Avalonia
```

## Features

- **Clipboard** - Enhanced clipboard operations and data handling
- **Commands** - Extended command implementations and helpers
- **Converters** - Value converters for data binding and UI display
- **Extensions** - Extension methods for Avalonia controls and framework
- **MarkupExtensions** - Custom XAML markup extensions for enhanced functionality
- **ResourceLocator** - Resource management and localization utilities
- **Templates** - Reusable control templates and data templates
- **Theming** - Base theming infrastructure and color management
- **Reactive Programming** - Integration with System.Reactive for reactive UI patterns
- **MyNet Integration** - Seamless integration with MyNet.Observable, MyNet.Utilities, and MyNet.Humanizer
- **Cross-platform compatibility** - Works on Windows, macOS, and Linux


## Core Extensions & Utilities

MyNet.Avalonia provides foundational extensions and utilities for Avalonia application development.

### Resource Management

Use the ResourceLocator to manage application resources:

```csharp
// Access application resources
var resource = ResourceLocator.GetResource("MyResource");
var localizedString = ResourceLocator.GetLocalizedString("WelcomeMessage");
```

### Value Converters

Built-in converters for common data binding scenarios:

```xml
<TextBlock Text="{Binding DateValue, Converter={x:Static converters:DateToStringConverter.Default}}" />
<Border IsVisible="{Binding IsEnabled, Converter={x:Static converters:BooleanToVisibilityConverter.Default}}" />
```

### Markup Extensions

Enhanced XAML markup extensions:

```xml
<Button Content="{markup:Localize 'ButtonText'}" />
<Image Source="{markup:Resource 'IconName'}" />
```

### Reactive Programming

Integration with System.Reactive for modern UI patterns:

```csharp
// Observable property changes
this.WhenAnyValue(x => x.SearchText)
    .Throttle(TimeSpan.FromMilliseconds(300))
    .ObserveOnDispatcher()
    .Subscribe(text => FilterResults(text));
```


## Example Usage

### Enhanced Commands
```csharp
// Create commands with enhanced functionality
var saveCommand = new RelayCommand(Save, CanSave);
var asyncCommand = new AsyncRelayCommand(SaveAsync);
```

### Clipboard Operations
```csharp
// Enhanced clipboard operations
await ClipboardHelper.SetTextAsync("Hello World");
var text = await ClipboardHelper.GetTextAsync();
var hasImage = await ClipboardHelper.ContainsImageAsync();
```

### Extension Methods
```csharp
// Extension methods for controls
myButton.SetBinding(Button.ContentProperty, "Title");
myGrid.AddChild(myControl, row: 1, column: 2);
```

### Theming Infrastructure
```csharp
// Access theme resources
var primaryBrush = ThemeManager.GetResource("PrimaryBrush");
var accentColor = ThemeManager.GetAccentColor();

// Theme change notifications
ThemeManager.ThemeChanged += (sender, args) => {
    // React to theme changes
};
```

### Data Templates
```xml
<!-- Use predefined templates -->
<ContentControl ContentTemplate="{StaticResource MyNet.DataTemplate.Card}"
                Content="{Binding MyData}" />
```

### MyNet Integration
```csharp
// Leverage MyNet.Observable for reactive properties
public class MyViewModel : EditableObject
{
    [CanBeValidated]
    public string Name { get; set; } = string.Empty;
    
    [CanSetIsModified]
    public DateTime LastModified { get; set; }
}

// Use MyNet.Utilities extensions
var formattedDate = DateTime.Now.ToShortDateString(CultureInfo.CurrentCulture);
var humanizedTime = TimeSpan.FromMinutes(30).Humanize();
```

## License

Copyright © Stéphane ANDRE.

Distributed under the MIT License. See [LICENSE](../../LICENSE) for details.