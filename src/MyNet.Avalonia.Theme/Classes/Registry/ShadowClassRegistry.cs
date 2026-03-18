// -----------------------------------------------------------------------
// <copyright file="ShadowClassRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Reactive.Disposables;
using Avalonia;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes.Enums;

namespace MyNet.Avalonia.Theme.Classes.Registry;

/// <summary>
/// Provides utility methods for registering and managing opacity settings for visual elements.
/// </summary>
/// <remarks>The ShadowClassRegistry class offers static methods to streamline the process of applying and
/// registering shadow values for visual components. It is intended to centralize shadow configuration, making it
/// easier to maintain consistent visual appearance across an application.</remarks>
public static class ShadowClassRegistry
{
    /// <summary>
    /// Registers multiple shadow values for visual elements using a specified CSS prefix.
    /// </summary>
    /// <remarks>This method utilizes the UtilityRegistry to associate shadow values with visual elements,
    /// allowing for dynamic resource management based on theme resources. It is important to call this method during
    /// application initialization to ensure that shadow settings are correctly applied.</remarks>
    public static void Register()
    {
        ClassRegistry.RegisterMany<ShadowDepth, Visual>(CssPrefix.Shadow, (x, y) => new CompositeDisposable
        {
            x.SetProperty(Visual.ClipToBoundsProperty, false),
            x.SetProperty(ShadowAssist.ShadowDepthProperty, y)
        });
        ClassRegistry.Register<Visual>(CssClass.ShadowControl, x => new CompositeDisposable
        {
            x.SetProperty(Visual.ClipToBoundsProperty, false),
            x.SetProperty(ShadowAssist.ShadowDepthProperty, ThemeResources.Shadow.Control.Value)
        });
        ClassRegistry.Register<Visual>(CssClass.ShadowSurface, x => new CompositeDisposable
        {
            x.SetProperty(Visual.ClipToBoundsProperty, false),
            x.SetProperty(ShadowAssist.ShadowDepthProperty, ThemeResources.Shadow.Surface.Value)
        });
        ClassRegistry.Register<Visual>(CssClass.ShadowHeader, x => x.SetProperty(HeaderAssist.ShadowDepthProperty, ThemeResources.Shadow.Surface.Value));

        ClassRegistry.Register<Visual>(CssClass.ShadowItems, x => new CompositeDisposable
        {
            x.SetProperty(Visual.ClipToBoundsProperty, false),
            x.SetProperty(ItemsAssist.ShadowDepthProperty, ThemeResources.Shadow.Control.Value),
            x.SetProperty(ItemsAssist.MarginProperty, new Thickness(ThemeResources.Spacing.Get(SpacingSize.Sm).Value))
        });
    }
}
