// -----------------------------------------------------------------------
// <copyright file="ShowcaseViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground;

/// <summary>
/// Base view model for control playground pages (interactive preview + theme catalog).
/// </summary>
internal abstract class ShowcaseViewModel(string controlName, ICommandFactory commands, ControlThemeBuilder[] builders)
    : PageViewModel
{
    private readonly LazyControlThemes _themes = new(controlName, commands, builders);
    private PlaygroundViewModel? _playground;
    private ThemesCatalogViewModel? _catalog;

    /// <summary>Gets the interactive theme preview for the showcased control.</summary>
    public PlaygroundViewModel Playground => _playground ??= new(controlName, _themes.Themes, commands);

    /// <summary>Gets the available theme variants for the showcased control.</summary>
    public ThemesCatalogViewModel Catalog => _catalog ??= new(_themes.Themes);

    /// <inheritdoc/>
    protected override void DisposeManagedResources()
    {
        _playground?.Dispose();
        _catalog?.Dispose();
        base.DisposeManagedResources();
    }
}
