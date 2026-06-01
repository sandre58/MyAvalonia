// -----------------------------------------------------------------------
// <copyright file="AvaloniaExtendedThemes.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Extended.Theming;

/// <summary>
/// Avalonia resource URIs for MyNet.Avalonia.Extended styles.
/// </summary>
public static class AvaloniaExtendedThemes
{
    /// <summary>
    /// Styles entry point merging Extended control themes (toast notifications, dialogs, etc.).
    /// </summary>
    /// <remarks>
    /// Include in <c>App.axaml</c> under <c>Application.Styles</c>:
    /// <c>&lt;StyleInclude Source="avares://MyNet.Avalonia.Extended/Themes/Generic.axaml" /&gt;</c>.
    /// </remarks>
    public const string GenericStyles = "avares://MyNet.Avalonia.Extended/Themes/Generic.axaml";
}
