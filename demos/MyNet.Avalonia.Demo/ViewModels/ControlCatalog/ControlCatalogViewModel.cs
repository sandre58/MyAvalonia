// -----------------------------------------------------------------------
// <copyright file="ControlCatalogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using MyNet.Avalonia.Theme;
using MyNet.Avalonia.Theme.Theming;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Avalonia.Theme.Theming.Palettes;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

/// <summary>
/// View model for the control catalog page, which displays available control themes and provides a playground for testing.
/// </summary>
internal abstract class ControlCatalogViewModel : PageViewModel
{
    /// <summary>
    /// Gets the collection of available theme definitions for the control.
    /// </summary>
    public ObservableCollection<ControlThemeDefinition> Themes { get; }

    /// <summary>
    /// Gets the playground view model for interactive control testing and preview.
    /// </summary>
    public ControlPlaygroundViewModel Playground { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlCatalogViewModel"/> class.
    /// </summary>
    /// <param name="controlName">The name of the control being cataloged.</param>
    /// <param name="builders">The array of theme builders to generate theme definitions.</param>
    protected ControlCatalogViewModel(string controlName, ControlThemeBuilder[] builders)
    {
        var definitions = builders.Select(x => x.Build(controlName)).ToList();
        Themes = new(definitions);
        Playground = new(controlName, definitions, CreateBackgroundContexts());
    }

    /// <summary>
    /// Retrieves a collection of background contexts representing different visual themes for the application UI.
    /// </summary>
    /// <remarks>Each <see cref="BackgroundContext"/> in the returned collection is initialized with brushes
    /// corresponding to a particular theme element, ensuring consistent theming across various UI components. Override
    /// this method to customize or extend the available background contexts for derived classes.</remarks>
    /// <returns>An enumerable collection of <see cref="BackgroundContext"/> instances, each configured with specific theme
    /// brushes for accent, primary, and surface backgrounds.</returns>
    private static IEnumerable<BackgroundContext> CreateBackgroundContexts()
    {
        var theme = MyTheme.Current;
        var accent = theme.GetBrush(nameof(MyTheme.Accent));
        var accentForeground = theme.GetBrush($"{nameof(MyTheme.Accent)}.{nameof(ColorShades.Foreground)}");
        var primary = theme.GetBrush(nameof(MyTheme.Primary));
        var primaryForeground = theme.GetBrush($"{nameof(MyTheme.Primary)}.{nameof(ColorShades.Foreground)}");
        var surface = theme.GetBrush("Surface.Level2");
        var surfaceForeground = theme.GetBrush(ThemeResourceKeyFactory.PrimaryForeground);
        return [
            new BackgroundContext(surface, surfaceForeground, ThemeContext.Default),
            new BackgroundContext(primary, primaryForeground, ThemeContext.Contrast),
            new BackgroundContext(accent, accentForeground, ThemeContext.Contrast),
        ];
    }

    /// <summary>
    /// Cleans up resources and disposes the playground view model.
    /// </summary>
    protected override void Cleanup()
    {
        base.Cleanup();

        Playground.Dispose();
    }
}
