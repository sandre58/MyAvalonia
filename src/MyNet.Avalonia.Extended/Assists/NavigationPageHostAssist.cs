// -----------------------------------------------------------------------
// <copyright file="NavigationPageHostAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Extended.Navigation;

namespace MyNet.Avalonia.Extended.Assists;

/// <summary>
/// Attached property that binds an Avalonia <see cref="NavigationPage"/> to the navigation host.
/// </summary>
public static class NavigationPageHostAssist
{
    /// <summary>
    /// Identifies the <see cref="IsHost"/> attached property.
    /// </summary>
    public static readonly AttachedProperty<bool> IsHostProperty = AvaloniaProperty.RegisterAttached<NavigationPage, bool>("IsHost", typeof(NavigationPageHostAssist));

    static NavigationPageHostAssist() => IsHostProperty.Changed.AddClassHandler<NavigationPage>(OnIsHostChanged);

    /// <summary>
    /// Sets whether the target <see cref="NavigationPage"/> hosts application navigation.
    /// </summary>
    /// <param name="navigationPage">The navigation page control.</param>
    /// <param name="value">Whether the control is the navigation host.</param>
    public static void SetIsHost(NavigationPage navigationPage, bool value) => navigationPage.SetValue(IsHostProperty, value);

    /// <summary>
    /// Gets whether the target <see cref="NavigationPage"/> hosts application navigation.
    /// </summary>
    /// <param name="navigationPage">The navigation page control.</param>
    /// <returns><see langword="true"/> when the control is the navigation host.</returns>
    public static bool GetIsHost(NavigationPage navigationPage) => navigationPage.GetValue(IsHostProperty);

    private static void OnIsHostChanged(NavigationPage navigationPage, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is true)
            AvaloniaNavigationBootstrap.AttachNavigationPage(navigationPage);
    }
}
