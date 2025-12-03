// -----------------------------------------------------------------------
// <copyright file="DarkThemePalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Themes;

/// <summary>
/// Static dark theme palette definition.
/// This is the primary source of truth for dark theme colors, loaded directly from C# for performance.
/// Dark.axaml serves as an optional fallback/override.
/// </summary>
public static class DarkThemePalette
{
    /// <summary>
    /// Gets variant of colors.
    /// </summary>
    public static ThemeVariant Variant { get; } = ThemeVariant.Dark;

    /// <summary>
    /// Gets the base theme palette for dark mode.
    /// </summary>
    public static BaseThemePalette Base { get; } = new()
    {
        // Application
        ApplicationBackground = Color.Parse("#202020"),
        ApplicationForeground = Color.Parse("#FFFFFF"),

        // Surfaces (Containers)
        SurfaceBackground = Color.Parse("#252525"),
        SurfaceBackgroundDark = Color.Parse("#151515"),
        SurfaceBorder = Color.Parse("#373737"),

        // Controls
        ControlBackground = Color.Parse("#3F3F3F"),
        ControlBackgroundLight = Color.Parse("#E2E2E2"),
        ControlBorder = Color.Parse("#525252"),
        ControlBorderHover = Color.Parse("#777777"),
        ControlBorderFocus = Color.Parse("#BFBFBF"),

        // Overlay
        OverlayBackground = Color.Parse("#66999999"),

        // Dialog
        DialogBackground = Color.Parse("#292929"),

        // Popup
        PopupBackground = Color.Parse("#151515"),

        // ToolTip
        ToolTipBackground = Color.Parse("#EBEBEB"),
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
        Unknown = Colors.WhiteSmoke,
        Space = Colors.Transparent,
        Comment = Colors.Green,
        Tag = Colors.Aquamarine,
        Quote = Colors.CornflowerBlue,
        AttributeValue = Colors.CornflowerBlue,
        AttributeKey = Colors.LightSteelBlue,
        Brace = Colors.Goldenrod,
        Entity = Colors.Aqua
    };

    /// <summary>
    /// Creates a complete ThemePalette for dark mode.
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
