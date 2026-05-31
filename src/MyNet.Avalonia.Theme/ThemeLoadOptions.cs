// -----------------------------------------------------------------------
// <copyright file="ThemeLoadOptions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme;

/// <summary>
/// Controls which optional theme asset modules are merged at startup.
/// </summary>
public sealed class ThemeLoadOptions
{
    /// <summary>
    /// Gets loads every optional module (default, preserves full control catalog).
    /// </summary>
    public static ThemeLoadOptions Full { get; } = new();

    /// <summary>
    /// Gets skips heavy optional modules (color pickers, DataGrid, extended date/time controls).
    /// </summary>
    public static ThemeLoadOptions CoreOnly { get; } = new()
    {
        IncludeColorPicker = false,
        IncludeDataGrid = false,
        IncludeExtendedDateTime = false
    };

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether color-picker control themes are merged.
    /// </summary>
    public bool IncludeColorPicker { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether the DataGrid control theme is merged.
    /// </summary>
    public bool IncludeDataGrid { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets whether extended calendar/time picker control themes are merged.
    /// </summary>
    public bool IncludeExtendedDateTime { get; set; } = true;
}
