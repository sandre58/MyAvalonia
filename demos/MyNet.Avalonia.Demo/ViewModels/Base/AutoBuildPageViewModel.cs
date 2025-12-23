// -----------------------------------------------------------------------
// <copyright file="AutoBuildPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MyNet.Avalonia.Demo.Controls;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Demo.ViewModels;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal class AutoBuildPageViewModel : PageViewModel
{
    private readonly string _controlName;

    public ObservableCollection<ControlThemeDescription> ControlThemes { get; }

    protected AutoBuildPageViewModel(string controlName, IList<ControlThemeBuilder> builders)
    {
        _controlName = controlName;
        ControlThemes = new(builders.Select(x => x.Build(controlName)));
        UpdateTitle();
    }

    protected override string CreateTitle() => _controlName.OrEmpty().Translate();
}
