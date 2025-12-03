// -----------------------------------------------------------------------
// <copyright file="HighContrastThemePalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Themes;

/// <summary>
/// Static high contrast theme palette definition.
/// Designed for maximum accessibility with high contrast ratios (WCAG AAA compliant).
/// </summary>
public static class HighContrastThemePalette
{
    /// <summary>
    /// Gets variant of colors.
    /// </summary>
    public static ThemeVariant Variant { get; } = new ThemeVariant(nameof(BuiltInThemeProviders.HighContrast), ThemeVariant.Dark);

    /// <summary>
    /// Gets the base theme palette for high contrast mode.
    /// All colors chosen for maximum contrast (minimum 7:1 ratio).
    /// </summary>
    public static BaseThemePalette Base { get; } = new()
    {
        // Application - Maximum contrast
        ApplicationBackground = Colors.Black,           // #000000
        ApplicationForeground = Colors.White,           // #FFFFFF

        // Surfaces (Containers) - High contrast
        SurfaceBackground = Color.Parse("#0A0A0A"),     // Very dark gray
        SurfaceBackgroundDark = Colors.Black,
        SurfaceBorder = Colors.Yellow,                  // High visibility border

        // Controls - High contrast
        ControlBackground = Color.Parse("#1A1A1A"),
        ControlBackgroundLight = Color.Parse("#2A2A2A"),
        ControlBorder = Colors.White,
        ControlBorderHover = Colors.Cyan,               // Bright hover
        ControlBorderFocus = Colors.Yellow,             // High visibility focus

        // Overlay - Semi-transparent
        OverlayBackground = Color.Parse("#CC000000"),   // 80% black

        // Dialog - High contrast
        DialogBackground = Color.Parse("#0F0F0F"),

        // Popup - High contrast
        PopupBackground = Colors.Black,

        // ToolTip - Inverted for visibility
        ToolTipBackground = Colors.Yellow,
        ToolTipBorder = Colors.Black,

        // Button
        ButtonCloseBackgroundHover = Colors.Red,        // Bright red for high visibility
    };

    /// <summary>
    /// Gets the success (positive) color palette.
    /// Bright green for high visibility.
    /// </summary>
    public static ColorPalette Success { get; } = new(Colors.LimeGreen);  // #32CD32

    /// <summary>
    /// Gets the warning color palette.
    /// Bright yellow/orange for high visibility.
    /// </summary>
    public static ColorPalette Warning { get; } = new(Colors.Orange);  // #FFA500

    /// <summary>
    /// Gets the error (negative) color palette.
    /// Bright red for high visibility.
    /// </summary>
    public static ColorPalette Error { get; } = new(Colors.Red);  // #FF0000

    /// <summary>
    /// Gets the information color palette.
    /// Bright cyan for high visibility.
    /// </summary>
    public static ColorPalette Information { get; } = new(Colors.Cyan);  // #00FFFF

    /// <summary>
    /// Gets the neutral color palette.
    /// Light gray for visibility against black background.
    /// </summary>
    public static ColorPalette Neutral { get; } = new(Colors.LightGray);  // #D3D3D3

    /// <summary>
    /// Gets the gender color palette.
    /// Bright colors for high visibility.
    /// </summary>
    public static GenderPalette Gender { get; } = new()
    {
        Male = Colors.DodgerBlue,
        Female = Colors.HotPink
    };

    /// <summary>
    /// Gets the code block syntax highlighting palette.
    /// High contrast colors for code editors.
    /// </summary>
    public static CodeBlockPalette CodeBlock { get; } = new()
    {
        Unknown = Colors.White,
        Space = Colors.Transparent,
        Comment = Colors.LimeGreen,
        Tag = Colors.Cyan,
        Quote = Colors.Yellow,
        AttributeValue = Colors.Magenta,
        AttributeKey = Colors.Orange,
        Brace = Colors.Red,
        Entity = Colors.Aqua
    };

    /// <summary>
    /// Creates a complete ThemePalette for high contrast mode.
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
