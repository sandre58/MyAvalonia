// -----------------------------------------------------------------------
// <copyright file="ThemePalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Observable;
using MyNet.Utilities;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Represents a complete set of color palettes for a theme variant (Dark, Light, HighContrast, etc.).
/// Contains all color palettes needed to style an application: base colors, semantic colors, and specialized palettes.
/// Implements INotifyPropertyChanged via ObservableObject to support reactive bindings.
/// </summary>
public class ThemePalette(ThemeVariant variant) : ObservableObject
{
    public ThemeVariant Variant { get; } = variant;

    /// <summary>
    /// Gets or sets the base theme palette containing application, surface, and control colors.
    /// </summary>
    public required BaseThemePalette Base { get; set; }

    /// <summary>
    /// Gets or sets the success (positive) color palette with shades.
    /// </summary>
    public required ColorPalette Success { get; set; }

    /// <summary>
    /// Gets or sets the warning color palette with shades.
    /// </summary>
    public required ColorPalette Warning { get; set; }

    /// <summary>
    /// Gets or sets the error (negative) color palette with shades.
    /// </summary>
    public required ColorPalette Error { get; set; }

    /// <summary>
    /// Gets or sets the information color palette with shades.
    /// </summary>
    public required ColorPalette Information { get; set; }

    /// <summary>
    /// Gets or sets the neutral color palette with shades.
    /// </summary>
    public required ColorPalette Neutral { get; set; }

    /// <summary>
    /// Gets or sets the gender-specific color palette.
    /// </summary>
    public required GenderPalette Gender { get; set; }

    /// <summary>
    /// Gets or sets the code block syntax highlighting color palette.
    /// </summary>
    public required CodeBlockPalette CodeBlock { get; set; }

    /// <summary>
    /// Converts the entire theme palette to a read-only dictionary suitable for use as resource dictionary keys and values.
    /// </summary>
    /// <returns>A dictionary containing all theme colors from all palettes with their corresponding resource keys.</returns>
    public IReadOnlyDictionary<string, Color> ToResourceDictionary()
    {
        var dictionary = new Dictionary<string, Color>();

        dictionary.AddRange(Base.ToResourceDictionary());
        dictionary.AddRange(Success.ToResourceDictionary(nameof(Success)));
        dictionary.AddRange(Warning.ToResourceDictionary(nameof(Warning)));
        dictionary.AddRange(Error.ToResourceDictionary(nameof(Error)));
        dictionary.AddRange(Information.ToResourceDictionary(nameof(Information)));
        dictionary.AddRange(Neutral.ToResourceDictionary(nameof(Neutral)));
        dictionary.AddRange(Gender.ToResourceDictionary());
        dictionary.AddRange(CodeBlock.ToResourceDictionary());

        return dictionary;
    }
}
