<div id="top"></div>

<!-- PROJECT INFO -->
<br />
<div align="center">
  <img src="assets/MyAvalonia.png" width="128" alt="MyAvalonia">
</div>

<h1 align="center">My .NET Avalonia</h1>

[![MIT License][license-shield]][license-url]
[![GitHub Stars](https://img.shields.io/github/stars/sandre58/MyAvalonia?style=for-the-badge)](https://github.com/sandre58/MyAvalonia/stargazers)
[![GitHub Forks](https://img.shields.io/github/forks/sandre58/MyAvalonia?style=for-the-badge)](https://github.com/sandre58/MyAvalonia/network/members)
[![GitHub Issues](https://img.shields.io/github/issues/sandre58/MyAvalonia?style=for-the-badge)](https://github.com/sandre58/MyAvalonia/issues)
[![Last Commit](https://img.shields.io/github/last-commit/sandre58/MyAvalonia?style=for-the-badge)](https://github.com/sandre58/MyAvalonia/commits/main)
[![Contributors](https://img.shields.io/github/contributors/sandre58/MyAvalonia?style=for-the-badge)](https://github.com/sandre58/MyAvalonia/graphs/contributors)
[![Repo Size](https://img.shields.io/github/repo-size/sandre58/MyAvalonia?style=for-the-badge)](https://github.com/sandre58/MyAvalonia)

A comprehensive collection of Avalonia UI libraries and extensions for modern cross-platform .NET development (8.0, 9.0, and 10.0). This repository provides specialized Avalonia controls, theming, UI components, and utilities to enhance desktop applications across Windows, macOS, and Linux platforms.

## ✨ Key Features

- 🎨 **Modern Theming & Styling** - Complete theming system with customizable design components
- 🪟 **Rich Avalonia Controls** - Custom controls, color pickers, data grids, and UI helpers
- 🌈 **Advanced Color Management** - Color palettes, pickers, and theming utilities
- � **Cross-Platform Ready** - Works seamlessly on Windows, macOS, and Linux
- 🏗️ **MVVM Architecture** - Reactive programming and MVVM pattern support with observables
- 🔄 **Reactive Extensions** - Built-in System.Reactive integration for modern UI patterns
- 🚀 **Production Ready** - Battle-tested libraries with robust component architecture

## Packages

[![Build][build-shield]][build-url]
[![Coverage](https://codecov.io/gh/sandre58/MyAvalonia/branch/main/graph/badge.svg)](https://codecov.io/gh/sandre58/MyAvalonia)
[![C#](https://img.shields.io/badge/language-C%23-blue)](#)
[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-purple)](#)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-purple)](#)
[![.NET 10.0](https://img.shields.io/badge/.NET-10.0-purple)](#)

| Package | Description | NuGet |
|---|---|---|
| [**MyNet.Avalonia**](src/MyNet.Avalonia) | 🎨 Core Avalonia library with extensions, helpers, theming, reactive programming, and integration with MyNet libraries. | [![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia)](https://www.nuget.org/packages/MyNet.Avalonia) |
| [**MyNet.Avalonia.Controls**](src/MyNet.Avalonia.Controls) | 🪟 Advanced controls and UI components including color pickers, data grids, navigation menus, and custom cursors for Avalonia applications. | [![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Controls)](https://www.nuget.org/packages/MyNet.Avalonia.Controls) |
| [**MyNet.Avalonia.Theme**](src/MyNet.Avalonia.Theme) | 🎭 Comprehensive theming system with custom styles, control templates, and visual resources for consistent UI design. | [![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Theme)](https://www.nuget.org/packages/MyNet.Avalonia.Theme) |
| [**MyNet.Avalonia.Extended**](src/MyNet.Avalonia.Extended) | 🖼️ High-level UI components and composite controls for rapid application development with Avalonia. | [![NuGet](https://img.shields.io/nuget/v/MyNet.Avalonia.Extended)](https://www.nuget.org/packages/MyNet.Avalonia.Extended) |

## 🚀 Getting Started

### Prerequisites

- **.NET 8.0, 9.0, or 10.0** - The libraries support the latest .NET versions
- **JetBrains Rider**, **Visual Studio 2022**, or **VS Code** (recommended for Avalonia development)
- **Windows, macOS, or Linux** - Avalonia supports all major desktop platforms
- **NuGet** package manager

### Installation

Install any Avalonia package via NuGet Package Manager, .NET CLI, or PackageReference:

**Using .NET CLI:**
```bash
# Core Avalonia library with extensions and theming
dotnet add package MyNet.Avalonia

# Advanced controls and UI components
dotnet add package MyNet.Avalonia.Controls

# Comprehensive theming system
dotnet add package MyNet.Avalonia.Theme

# High-level UI components
dotnet add package MyNet.Avalonia.Extended
```

**Using Package Manager Console:**
```powershell
Install-Package MyNet.Avalonia
Install-Package MyNet.Avalonia.Controls
Install-Package MyNet.Avalonia.Theme
Install-Package MyNet.Avalonia.Extended
```

**Using PackageReference:**
```xml
<PackageReference Include="MyNet.Avalonia" Version="1.0.*" />
<PackageReference Include="MyNet.Avalonia.Controls" Version="1.0.*" />
<PackageReference Include="MyNet.Avalonia.Theme" Version="1.0.*" />
<PackageReference Include="MyNet.Avalonia.Extended" Version="1.0.*" />
```

### Quick Examples

**Basic Avalonia Theming:**
```xaml
<!-- App.axaml -->
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="avares://MyNet.Avalonia.Theme/MyTheme.axaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

**Using Custom Controls:**
```csharp
// Color picker integration
var colorPicker = new ColorPicker()
{
    SelectedColor = Colors.Blue
};

// Avatar control
var avatar = new Avatar()
{
    Source = new Bitmap("user-avatar.png"),
    Size = 64
};
```

**Reactive Extensions:**
```csharp
// Observable property binding
this.WhenAnyValue(x => x.SearchText)
    .Throttle(TimeSpan.FromMilliseconds(300))
    .Subscribe(text => FilterResults(text));
```

For detailed usage examples and API references, explore the documentation in each package's directory under `src/`.

## 📁 Repository Structure

- **`src/`** — Source code for all Avalonia packages:
  - **`MyNet.Avalonia/`** — Core Avalonia library with extensions, theming, and reactive programming
  - **`MyNet.Avalonia.Controls/`** — Advanced controls, color pickers, data grids, and UI components
  - **`MyNet.Avalonia.Theme/`** — Comprehensive theming system and visual resources
  - **`MyNet.Avalonia.Extended/`** — High-level UI components and composite controls

- **`demos/`** — Example Avalonia applications showcasing library features
  - **`MyNet.Avalonia.Showcase/`** — Comprehensive cross-platform demo application
  - **`MyNet.Avalonia.Showcase.Desktop/`** — Desktop-specific demo implementation
  - **`MyNet.Avalonia.Showcase.Browser/`** — WebAssembly browser demo
  - **`MyNet.Avalonia.Showcase.Android/`** — Android mobile demo
  - **`MyNet.Avalonia.Showcase.iOS/`** — iOS mobile demo
  
- **`assets/`** — Project logos, icons, and visual assets
- **`build/`** — MSBuild configuration and shared properties
- **`.github/`** — CI/CD workflows and GitHub automation

## 🛠️ Development

### Building from Source

```bash
# Clone the repository
git clone https://github.com/sandre58/MyAvalonia.git
cd MyAvalonia

# Build all projects
dotnet build

# Run tests (if available)
dotnet test

# Create NuGet packages
dotnet pack
```

### Demo Application

Explore the comprehensive demo application to see all Avalonia libraries in action across different platforms:

```bash
# Run the desktop demo
dotnet run --project demos/MyNet.Avalonia.Showcase.Desktop

# Run the browser demo (WebAssembly)
dotnet run --project demos/MyNet.Avalonia.Showcase.Browser

# Build and run mobile demos (requires platform-specific setup)
dotnet build demos/MyNet.Avalonia.Showcase.Android
dotnet build demos/MyNet.Avalonia.Showcase.iOS
```

### Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) and [Code of Conduct](CODE_OF_CONDUCT.md) for details on how to get involved.

## 📄 License

Copyright © 2016-2025 Stéphane ANDRE.

Distributed under the MIT License. See [LICENSE](./LICENSE) for complete details.

<!-- MARKDOWN LINKS & IMAGES -->
[license-shield]: https://img.shields.io/github/license/sandre58/MyAvalonia?style=for-the-badge
[license-url]: https://github.com/sandre58/MyAvalonia/blob/main/LICENSE
[build-shield]: https://img.shields.io/github/actions/workflow/status/sandre58/MyAvalonia/ci.yml?logo=github&label=CI
[build-url]: https://github.com/sandre58/MyAvalonia/actions