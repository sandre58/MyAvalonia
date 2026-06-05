// -----------------------------------------------------------------------
// <copyright file="LazyControlThemes.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Linq;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Factories;
using MyNet.Collections;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground;

/// <summary>
/// Defers creation of <see cref="ControlThemeViewModel"/> instances until first access.
/// </summary>
internal sealed class LazyControlThemes(string controlName, ICommandFactory commands, ControlThemeBuilder[] builders)
{
    private readonly string _controlName = controlName ?? throw new ArgumentNullException(nameof(controlName));
    private readonly ICommandFactory _commands = commands ?? throw new ArgumentNullException(nameof(commands));
    private readonly ControlThemeBuilder[] _builders = builders ?? throw new ArgumentNullException(nameof(builders));

    public ObservableCollection<ControlThemeViewModel> Themes
        => field ??= _builders
            .Select(x => new ControlThemeViewModelFactory(x, _commands).Create(_controlName))
            .ToList()
            .ToObservableCollection();
}
