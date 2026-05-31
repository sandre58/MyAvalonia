// -----------------------------------------------------------------------
// <copyright file="ThemeVariantPalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Avalonia.Styling;
using MyNet.Collections;
using MyNet.Observable;

namespace MyNet.Avalonia.Theme.Theming.Palettes;

/// <summary>
/// Represents a complete set of color palettes and opacity levels for a theme variant (e.g., Dark, Light, HighContrast).
/// Contains all color shades, semantic colors, and specialized palettes needed to style an application for a specific theme variant.
/// </summary>
public class ThemeVariantPalette(ThemeVariant variant) : ObservableObject
{
    /// <summary>
    /// Gets the theme variant (e.g., Dark, Light, HighContrast) associated with this set of colors.
    /// </summary>
    public ThemeVariant Variant { get; } = variant;

    /// <summary>
    /// Gets or sets the base theme palette containing application, surface, and control colors.
    /// </summary>
    public required ControlPalette Base { get; set; }

    /// <summary>
    /// Gets or sets the success (positive) color shades.
    /// </summary>
    public required ColorShades Success { get; set; }

    /// <summary>
    /// Gets or sets the warning color shades.
    /// </summary>
    public required ColorShades Warning { get; set; }

    /// <summary>
    /// Gets or sets the error (negative) color shades.
    /// </summary>
    public required ColorShades Error { get; set; }

    /// <summary>
    /// Gets or sets the information color shades.
    /// </summary>
    public required ColorShades Information { get; set; }

    /// <summary>
    /// Gets or sets the neutral color shades.
    /// </summary>
    public required ColorShades Neutral { get; set; }

    /// <summary>
    /// Gets or sets the gender-specific color palette.
    /// </summary>
    public required GenderPalette Gender { get; set; }

    /// <summary>
    /// Gets or sets the code block syntax highlighting color palette.
    /// </summary>
    public required CodeBlockPalette CodeBlock { get; set; }

    /// <summary>
    /// Gets or sets the opacity levels used for rendering operations in this theme variant.
    /// </summary>
    public required OpacityLevels Opacity { get; set; }

    /// <summary>
    /// Converts the entire theme variant colors to a read-only dictionary suitable for use as resource dictionary keys and values.
    /// </summary>
    /// <returns>A dictionary containing all theme colors and opacities from all palettes with their corresponding resource keys.</returns>
    public IReadOnlyDictionary<string, object> ToResourceDictionary()
    {
        var dictionary = new Dictionary<string, object>();

        dictionary.AddRange(Base.ToResourceDictionary());
        dictionary.AddRange(Gender.ToResourceDictionary());
        dictionary.AddRange(CodeBlock.ToResourceDictionary());
        dictionary.AddRange(Opacity.ToResourceDictionary());
        addColorShades(Success, nameof(Success));
        addColorShades(Warning, nameof(Warning));
        addColorShades(Error, nameof(Error));
        addColorShades(Information, nameof(Information));
        addColorShades(Neutral, nameof(Neutral));

        return dictionary;

        void addColorShades(ColorShades shades, string name) => dictionary.AddRange(shades.ToResourceDictionary(name).ToDictionary(x => x.Key, x => (object)x.Value));
    }

    /// <summary>
    /// Creates a ThemeVariantPalette instance from a resource dictionary.
    /// </summary>
    /// <param name="variant">The theme variant (e.g., Dark, Light, HighContrast).</param>
    /// <param name="resourceDictionary">The resource dictionary containing color definitions.</param>
    /// <returns>A new ThemeVariantPalette instance.</returns>
    public static ThemeVariantPalette FromResourceDictionary(ThemeVariant variant, IReadOnlyDictionary<string, object> resourceDictionary) => new(variant)
    {
        Base = ControlPalette.FromResourceDictionary(resourceDictionary),
        Success = ColorShades.FromResourceDictionary(resourceDictionary, nameof(Success)),
        Warning = ColorShades.FromResourceDictionary(resourceDictionary, nameof(Warning)),
        Error = ColorShades.FromResourceDictionary(resourceDictionary, nameof(Error)),
        Information = ColorShades.FromResourceDictionary(resourceDictionary, nameof(Information)),
        Neutral = ColorShades.FromResourceDictionary(resourceDictionary, nameof(Neutral)),
        Gender = GenderPalette.FromResourceDictionary(resourceDictionary),
        CodeBlock = CodeBlockPalette.FromResourceDictionary(resourceDictionary),
        Opacity = OpacityLevels.FromResourceDictionary(resourceDictionary)
    };
}
