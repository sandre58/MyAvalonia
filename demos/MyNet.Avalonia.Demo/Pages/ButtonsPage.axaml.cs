// -----------------------------------------------------------------------
// <copyright file="ButtonsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class ButtonsPage : AutoBuildPage
{
    public ButtonsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Button
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            Content = data.Theme.ContainsAny("rounded")
                        ? RandomGenerator.Enum<IconData>().ToIcon()
                        : data.Theme.ContainsAny("icon", "tool")
                        ? RandomGenerator.Enum<IconData>().ToGeometry()
                        : data.Role.ToString().Or(data.Size.OrEmpty()).Or("Default")
        };

        if (data.Theme.NotContainsAny("icon", "tool", "rounded"))
            item.Classes.Add("CanAddIcon");

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithRoles)
            .AddLayouts("Circle")
            .AddStyles("Light", "Outlined", "Text", "Shadow")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeData("Rounded", defaultStyleDisplay: DefaultStyleDisplay.WithRoles)
            .AddStyles("Light", "Outlined", "Text", "Shadow")
            .AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultRoles()
            .AddSizes("Small", "Medium", "Large")
            .AddCustomControls(() =>
            {
                var control = new Button
                {
                    HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
                    Content = "Top"
                };
                control.Classes.Add("IconTop");
                IconAssist.SetIcon(control, RandomGenerator.Enum<IconData>().ToIcon());
                control.Width = 50;
                control.Height = 50;

                var control1 = new Button
                {
                    HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
                    Content = "Bottom"
                };
                control1.AddClasses("IconBottom Outlined Primary");
                IconAssist.SetIcon(control1, RandomGenerator.Enum<IconData>().ToIcon());
                control1.Width = 60;
                control1.Height = 60;
                return [control, control1];
            }),

            new ControlThemeData("Icon", DefaultStyleDisplay.WithRoles)
            .AddDefaultRoles()
            .AddSizes("ExtraSmall", "Small", "Medium", "Large", "ExtraLarge"),

            new("Embedded.Tool")
        ];

    private void Icon_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        => BuildHelper.AddIconOnChildren<Button>(Root, Icon?.SelectedIndex ?? 0, x => x.Classes.Contains("CanAddIcon"));
}
