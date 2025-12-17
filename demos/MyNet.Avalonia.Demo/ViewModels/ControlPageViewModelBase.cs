// -----------------------------------------------------------------------
// <copyright file="ControlPageViewModelBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MyNet.Avalonia.Demo.Controls;
using MyNet.UI.ViewModels.Workspace;

namespace MyNet.Avalonia.Demo.ViewModels;

internal class ControlPageViewModelBase : NavigableWorkspaceViewModel
{
    public ObservableCollection<ControlThemeDescription> ControlThemes { get; }

    protected ControlPageViewModelBase(string controlName, IList<ControlThemeBuilder> builders) => ControlThemes = new(builders.Select(x => x.Build(controlName)));
}
