// -----------------------------------------------------------------------
// <copyright file="ControlIconViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Theme.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Observable;
using MyNet.Utilities.Generator;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

/// <summary>
/// View model for managing control icon settings.
/// </summary>
internal sealed class ControlIconViewModel : ObservableObject
{
    /// <summary>
    /// Gets or sets a value indicating whether to show an icon in the control. When set to true, an icon will be displayed based on the specified data or randomly generated if no data is provided. When set to false, no icon will be shown in the control preview.
    /// </summary>
    [AlsoNotifyFor(nameof(ComputedClasses), nameof(Icon))]
    public bool ShowIcon { get; set; }

    /// <summary>
    /// Gets or sets the icon data to provide. This property can be used to specify a specific icon, but it is not required for the random icon generation.
    /// </summary>
    [AlsoNotifyFor(nameof(Icon))]
    public IconData? Data { get; set; }

    /// <summary>
    /// Gets or sets the icon position.
    /// </summary>
    [AlsoNotifyFor(nameof(ComputedClasses))]
    public Position Position { get; set; }

    /// <summary>
    /// Gets the icon to display based on the ShowIcon property and the provided Data. If ShowIcon is true, it returns an icon object created from the Data property or a randomly generated icon if Data is null. If ShowIcon is false, it returns null, indicating that no icon should be displayed in the control preview.
    /// </summary>
    public PathIcon? Icon => ShowIcon ? (Data ?? RandomGenerator.Enum<IconData>()).ToIcon() : null;

    /// <summary>
    /// Gets the CSS classes name to apply to the control based on the ShowIcon property and the specified Position. If ShowIcon is true, it returns a class name corresponding to the selected Position (e.g., "icon-left", "icon-right", "icon-top", "icon-bottom"). If ShowIcon is false, it returns an empty string, indicating that no icon-related class should be applied to the control.
    /// </summary>
    public string[] ComputedClasses => ShowIcon ? Position switch
    {
        Position.Left => ["icon-left"],
        Position.Right => ["icon-right"],
        Position.Top => ["icon-top"],
        Position.Bottom => ["icon-bottom"],
        _ => [],
    } : [];
}
