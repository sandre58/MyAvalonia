// -----------------------------------------------------------------------
// <copyright file="LabelsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Utilities;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class LabelsPage : AutoBuildPage
{
    public LabelsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Label { HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center, Content = data.Color.Or(data.Size.OrEmpty()).Or("Default") };

        if (data.Theme.ContainsAny("tag"))
            item.Classes.Add("CanAddIcon");
        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors).AddStyles("Secondary", "Tertiary")
                                                                                     .AddAllColors()
                                                                                     .AddAllSizes(),

           new ControlThemeData("Tag").AddLayouts("Circle")
                                      .AddStyles("Light", "Solid", "Outlined")
                                      .AddCartesianStyles("Solid", "Shadow")
                                      .AddCartesianStyles("Light", "Outlined")
                                      .AddAllColors()
                                      .AddSizes("Small", "Medium", "Large"),

           new("Code")
        ];

    private void Icon_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddIconOnChildren<Label>(Root, Icon?.SelectedIndex ?? 0, x => x.Classes.Contains("CanAddIcon"));
}
