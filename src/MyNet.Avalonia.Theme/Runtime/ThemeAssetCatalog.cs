// -----------------------------------------------------------------------
// <copyright file="ThemeAssetCatalog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Avalonia resource URIs for optional theme modules loaded after core XAML.
/// </summary>
internal static class ThemeAssetCatalog
{
    public const string ColorPickerModule = "avares://MyNet.Avalonia.Theme/Controls/Modules/ColorPickerModule.axaml";

    public const string DataGridModule = "avares://MyNet.Avalonia.Theme/Controls/Modules/DataGridModule.axaml";

    public const string ExtendedDateTimeModule = "avares://MyNet.Avalonia.Theme/Controls/Modules/ExtendedDateTimeModule.axaml";
}
