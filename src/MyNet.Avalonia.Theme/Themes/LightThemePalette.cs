// -----------------------------------------------------------------------
// <copyright file="LightThemePalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Themes;

/// <summary>
/// Static light theme palette definition.
/// This is the primary source of truth for light theme colors, loaded directly from C# for performance.
/// Light.axaml serves as an optional fallback/override.
/// </summary>
public static class LightThemePalette
{
    /// <summary>
    /// Gets variant of colors.
    /// </summary>
    public static ThemeVariant Variant { get; } = ThemeVariant.Light;

    /// <summary>
    /// Gets the base theme palette for light mode.
    /// </summary>
    public static BaseThemePalette Base { get; } = new()
    {
        // Application
        ApplicationBackground = Color.Parse("#EBEBEB"),
        ApplicationForeground = Color.Parse("#242424"),

        // Surfaces (Containers)
        SurfaceBackground = Color.Parse("#F6F6F6"),
        SurfaceBackgroundDark = Color.Parse("#CACACA"),
        SurfaceBorder = Color.Parse("#DADADA"),

        // Controls
        ControlBackground = Color.Parse("#E0E0E0"),
        ControlBackgroundLight = Color.Parse("#E2E2E2"),
        ControlBorder = Color.Parse("#C5C5C5"),
        ControlBorderHover = Color.Parse("#999999"),
        ControlBorderFocus = Color.Parse("#646464"),

        // Overlay
        OverlayBackground = Color.Parse("#22000000"),

        // Dialog
        DialogBackground = Color.Parse("#f6f6f6"),

        // Popup
        PopupBackground = Color.Parse("#FAFAFA"),

        // ToolTip
        ToolTipBackground = Color.Parse("#41464C"),
        ToolTipBorder = Colors.Transparent,

        // Button
        ButtonCloseBackgroundHover = Color.Parse("#E81123"),
    };

    /// <summary>
    /// Gets the success (positive) color palette.
    /// </summary>
    public static ColorPalette Success { get; } = new(Color.Parse("#388A34"));

    /// <summary>
    /// Gets the warning color palette.
    /// </summary>
    public static ColorPalette Warning { get; } = new(Color.Parse("#DD6E00"));

    /// <summary>
    /// Gets the error (negative) color palette.
    /// </summary>
    public static ColorPalette Error { get; } = new(Color.Parse("#A1260D"));

    /// <summary>
    /// Gets the information color palette.
    /// </summary>
    public static ColorPalette Information { get; } = new(Color.Parse("#1BA1E2"));

    /// <summary>
    /// Gets the neutral color palette.
    /// </summary>
    public static ColorPalette Neutral { get; } = new(Color.Parse("#828282"));

    /// <summary>
    /// Gets the gender color palette.
    /// </summary>
    public static GenderPalette Gender { get; } = new()
    {
        Male = Color.Parse("#2986cc"),
        Female = Color.Parse("#c90076")
    };

    /// <summary>
    /// Gets the code block syntax highlighting palette.
    /// </summary>
    public static CodeBlockPalette CodeBlock { get; } = new()
    {
        Unknown = Colors.DarkSlateGray,
        Space = Colors.Transparent,
        Comment = Colors.DarkGreen,
        Tag = Colors.Lime,
        Quote = Colors.RoyalBlue,
        AttributeValue = Colors.RoyalBlue,
        AttributeKey = Colors.CornflowerBlue,
        Brace = Colors.Chocolate,
        Entity = Colors.LightSeaGreen
    };

    /// <summary>
    /// Creates a complete ThemePalette for light mode.
    /// </summary>
    public static ThemePalette Create() => new(Variant)
    {
        Base = Base,
        Success = Success,
        Warning = Warning,
        Error = Error,
        Information = Information,
        Neutral = Neutral,
        Gender = Gender,
        CodeBlock = CodeBlock
    };
}
