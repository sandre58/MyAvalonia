// -----------------------------------------------------------------------
// <copyright file="ShowcaseViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Factories;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground;

/// <summary>
/// Base view model for control playground pages (interactive preview + theme catalog).
/// </summary>
internal abstract class ShowcaseViewModel : PageViewModel
{
    /// <summary>Gets the interactive theme preview for the showcased control.</summary>
    public PlaygroundViewModel Playground { get; }

    /// <summary>Gets the available theme variants for the showcased control.</summary>
    public ThemesCatalogViewModel Catalog { get; }

    protected ShowcaseViewModel(string controlName, ICommandFactory commands, ControlThemeBuilder[] builders)
    {
        var themes = builders.Select(x => new ControlThemeViewModelFactory(x, commands).Create(controlName)).ToList().ToObservableCollection();
        Playground = new(controlName, themes);
        Catalog = new(themes);
    }

    /// <inheritdoc/>
    protected override void DisposeManagedResources()
    {
        Playground.Dispose();
        Catalog.Dispose();
        base.DisposeManagedResources();
    }
}
