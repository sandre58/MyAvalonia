// -----------------------------------------------------------------------
// <copyright file="IconClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Assists;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for registering and managing icon settings for visual elements.
/// </summary>
/// <remarks>The IconClassRegistry class offers static methods to streamline the process of applying and
/// registering icon values for visual components. It is intended to centralize icon configuration, making it
/// easier to maintain consistent visual appearance across an application.</remarks>
public static class IconClassRegistry
{
    /// <summary>
    /// Registers multiple icon values for visual elements using a specified CSS prefix.
    /// </summary>
    /// <remarks>This method utilizes the UtilityRegistry to associate icon values with visual elements,
    /// allowing for dynamic resource management based on theme resources. It is important to call this method during
    /// application initialization to ensure that icon settings are correctly applied.</remarks>
    public static void Register() => ClassRegistry.RegisterMany<Position, Visual>(CssPrefix.Icon, (x, y) => x.SetProperty(IconAssist.AlignmentProperty, y));
}
