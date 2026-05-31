// -----------------------------------------------------------------------
// <copyright file="OpacityClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Theming;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for registering and managing opacity settings for visual elements.
/// </summary>
/// <remarks>The OpacityClassRegistry class offers static methods to streamline the process of applying and
/// registering opacity values for visual components. It is intended to centralize opacity configuration, making it
/// easier to maintain consistent visual appearance across an application.</remarks>
public static class OpacityClassRegistry
{
    /// <summary>
    /// Registers multiple opacity values for visual elements using a specified CSS prefix.
    /// </summary>
    /// <remarks>This method utilizes the UtilityRegistry to associate opacity values with visual elements,
    /// allowing for dynamic resource management based on theme resources. It is important to call this method during
    /// application initialization to ensure that opacity settings are correctly applied.</remarks>
    public static void Register()
    {
        ClassRegistry.RegisterMany<Opacity, Visual>(CssPrefix.Opacity, (x, y) => x.SetProperty(Visual.OpacityProperty, x.GetResourceObservable(ThemeResourceKeyFactory.Opacity(y.ToString()))));
        ClassRegistry.Register<Visual>(CssClass.Hidden, x => x.SetProperty(Visual.OpacityProperty, 0));
        ClassRegistry.Register<Visual>(CssClass.Visible, x => x.SetProperty(Visual.OpacityProperty, 1));
    }
}
