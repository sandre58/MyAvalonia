// -----------------------------------------------------------------------
// <copyright file="ControlCatalogViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Linq;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

/// <summary>
/// View model for the control catalog page, which displays available control themes and provides a playground for testing.
/// </summary>
internal class ControlCatalogViewModel : PageViewModel
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
    public ControlCatalogViewModel(string controlName, ControlThemeBuilder[] builders)
    {
        var definitions = builders.Select(x => x.Build(controlName)).ToList();
        Themes = new(definitions);
        Playground = new(controlName, definitions);
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
