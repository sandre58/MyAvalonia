// -----------------------------------------------------------------------
// <copyright file="ColorPalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Media;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Extensions;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Represents a color palette with shades from lightest (50) to darkest (900).
/// Provides access to material design style shades and common aliases for easy usage.
/// Implements INotifyPropertyChanged for reactive binding support via ObservableObject.
/// </summary>
public class ColorPalette
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPalette"/> class with a base color and an optional foreground color.
    /// Shades are generated automatically from the base color.
    /// </summary>
    /// <param name="baseColor">The base color of the palette (typically shade 500).</param>
    /// <param name="foregroundColor">The foreground color that contrasts with the base color. If not specified, it is computed automatically.</param>
    public ColorPalette(Color baseColor, Color? foregroundColor = null)
    {
        Base = baseColor;
        Foreground = foregroundColor ?? baseColor.ContrastingForegroundColor();

        // Generate shades automatically from base color
        GenerateShades();
    }

    /// <summary>
    /// Gets the base color of the palette (typically shade 500).
    /// </summary>
    public Color Base { get; }

    /// <summary>
    /// Gets the foreground color that contrasts with the base color.
    /// </summary>
    public Color Foreground { get; }

    /// <summary>
    /// Gets or sets the lightest shade (shade 50).
    /// </summary>
    public Color Shade50 { get; set; }

    /// <summary>
    /// Gets or sets shade 100.
    /// </summary>
    public Color Shade100 { get; set; }

    /// <summary>
    /// Gets or sets shade 200.
    /// </summary>
    public Color Shade200 { get; set; }

    /// <summary>
    /// Gets or sets shade 300.
    /// </summary>
    public Color Shade300 { get; set; }

    /// <summary>
    /// Gets or sets shade 400.
    /// </summary>
    public Color Shade400 { get; set; }

    /// <summary>
    /// Gets or sets the base shade (shade 500).
    /// </summary>
    public Color Shade500 { get; set; }

    /// <summary>
    /// Gets or sets shade 600.
    /// </summary>
    public Color Shade600 { get; set; }

    /// <summary>
    /// Gets or sets shade 700.
    /// </summary>
    public Color Shade700 { get; set; }

    /// <summary>
    /// Gets or sets shade 800.
    /// </summary>
    public Color Shade800 { get; set; }

    /// <summary>
    /// Gets or sets the darkest shade (shade 900).
    /// </summary>
    public Color Shade900 { get; set; }

    /// <summary>
    /// Gets the lightest color alias (shade 50).
    /// </summary>
    public Color Lightest => Shade50;

    /// <summary>
    /// Gets the lighter color alias (shade 200).
    /// </summary>
    public Color Lighter => Shade200;

    /// <summary>
    /// Gets the light color alias (shade 300).
    /// </summary>
    public Color Light => Shade300;

    /// <summary>
    /// Gets the default color alias (shade 500).
    /// </summary>
    public Color Default => Shade500;

    /// <summary>
    /// Gets the dark color alias (shade 700).
    /// </summary>
    public Color Dark => Shade700;

    /// <summary>
    /// Gets the darker color alias (shade 800).
    /// </summary>
    public Color Darker => Shade800;

    /// <summary>
    /// Gets the darkest color alias (shade 900).
    /// </summary>
    public Color Darkest => Shade900;

    /// <summary>
    /// Generates all palette shades based on the base color using HSL transformations.
    /// </summary>
    private void GenerateShades()
    {
        Shade500 = Base;

        var hsl = Base.ToHsl();

        Shade50 = HslColor.FromHsl(hsl.H, hsl.S * .15, 0.97).ToRgb();
        Shade100 = HslColor.FromHsl(hsl.H, hsl.S * .20, 0.92).ToRgb();
        Shade200 = HslColor.FromHsl(hsl.H, hsl.S * .30, 0.84).ToRgb();
        Shade300 = HslColor.FromHsl(hsl.H, hsl.S * .40, 0.70).ToRgb();
        Shade400 = HslColor.FromHsl(hsl.H, hsl.S * .50, 0.58).ToRgb();

        Shade600 = HslColor.FromHsl(hsl.H, hsl.S * .70, 0.40).ToRgb();
        Shade700 = HslColor.FromHsl(hsl.H, hsl.S * .85, 0.32).ToRgb();
        Shade800 = HslColor.FromHsl(hsl.H, hsl.S * .95, 0.22).ToRgb();
        Shade900 = HslColor.FromHsl(hsl.H, hsl.S, 0.14).ToRgb();
    }

    /// <summary>
    /// Gets a shade by its index (50, 100, 200, ..., 900).
    /// Returns the base color if the index is not recognized.
    /// </summary>
    /// <param name="shade">The shade index to retrieve.</param>
    /// <returns>The color corresponding to the specified shade index.</returns>
    public Color GetShade(int shade) => shade switch
    {
        50 => Shade50,
        100 => Shade100,
        200 => Shade200,
        300 => Shade300,
        400 => Shade400,
        500 => Shade500,
        600 => Shade600,
        700 => Shade700,
        800 => Shade800,
        900 => Shade900,
        _ => Base
    };

    /// <summary>
    /// Converts the color palette to a read-only dictionary suitable for use as resource dictionary keys and values.
    /// </summary>
    /// <param name="paletteName">The name of the palette to use as prefix for resource keys.</param>
    /// <returns>A dictionary containing all palette colors with their corresponding resource keys.</returns>
    public IReadOnlyDictionary<string, Color> ToResourceDictionary(string paletteName) => new Dictionary<string, Color>
        {
            { paletteName, Base },
            { nameof(Foreground).WithPrefix(paletteName), Foreground },
            { nameof(Shade50).WithPrefix(paletteName), Shade50 },
            { nameof(Shade100).WithPrefix(paletteName), Shade100 },
            { nameof(Shade200).WithPrefix(paletteName), Shade200 },
            { nameof(Shade300).WithPrefix(paletteName), Shade300 },
            { nameof(Shade400).WithPrefix(paletteName), Shade400 },
            { nameof(Shade500).WithPrefix(paletteName), Shade500 },
            { nameof(Shade600).WithPrefix(paletteName), Shade600 },
            { nameof(Shade700).WithPrefix(paletteName), Shade700 },
            { nameof(Shade800).WithPrefix(paletteName), Shade800 },
            { nameof(Shade900).WithPrefix(paletteName), Shade900 },
            { nameof(Lightest).WithPrefix(paletteName), Lightest },
            { nameof(Lighter).WithPrefix(paletteName), Lighter },
            { nameof(Light).WithPrefix(paletteName), Light },
            { nameof(Default).WithPrefix(paletteName), Default },
            { nameof(Dark).WithPrefix(paletteName), Dark },
            { nameof(Darker).WithPrefix(paletteName), Darker },
            { nameof(Darkest).WithPrefix(paletteName), Darkest }
        };
}
