// -----------------------------------------------------------------------
// <copyright file="TreeViewsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class TreeViewsPage : AutoBuildPage
{
    public TreeViewsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data) => new();

    protected override IEnumerable<ControlThemeData> ProvideThemes() => [];
}
