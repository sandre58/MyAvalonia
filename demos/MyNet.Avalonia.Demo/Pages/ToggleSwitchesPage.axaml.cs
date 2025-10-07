// -----------------------------------------------------------------------
// <copyright file="ToggleSwitchesPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Demo.Resources;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class ToggleSwitchesPage : AutoBuildPage
{
    public ToggleSwitchesPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new ToggleSwitch
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            IsChecked = RandomGenerator.Bool(),
            OnContent = (data.Theme?.Contains("inner", StringComparison.OrdinalIgnoreCase)).IsTrue()
            || (data.Theme?.Contains("rounded", StringComparison.OrdinalIgnoreCase)).IsTrue()
            ? RandomGenerator.Enum<IconData>().ToIcon()
            : (data.Theme?.Contains("icon", StringComparison.OrdinalIgnoreCase)).IsTrue()
            ? RandomGenerator.Enum<IconData>().ToGeometry()
            : (data.Theme?.Contains("alternate", StringComparison.OrdinalIgnoreCase)).IsTrue()
            ? null
            : DemoResources.On,
            OffContent = (data.Theme?.Contains("inner", StringComparison.OrdinalIgnoreCase)).IsTrue()
            || (data.Theme?.Contains("rounded", StringComparison.OrdinalIgnoreCase)).IsTrue()
            ? RandomGenerator.Enum<IconData>().ToIcon()
            : (data.Theme?.Contains("icon", StringComparison.OrdinalIgnoreCase)).IsTrue()
            ? RandomGenerator.Enum<IconData>().ToGeometry()
            : (data.Theme?.Contains("alternate", StringComparison.OrdinalIgnoreCase)).IsTrue()
            ? null
            : DemoResources.Off
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
            new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddDefaultColors(false)
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeData("Inner", defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddDefaultColors(false)
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeData("Alternate", defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddDefaultColors(),

            new ControlThemeData("Button", defaultStyleDisplay: DefaultStyleDisplay.WithColors)
            .AddLayouts("Circle")
            .AddStyles("Light", "Solid", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Shadow").AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultColors(false)
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeData("Button.Rounded", DefaultStyleDisplay.WithColors)
            .AddStyles("Light", "Solid", "Outlined", "Text")
            .AddCartesianStyles("Solid", "Shadow").AddCartesianStyles("Light", "Outlined", "Text")
            .AddDefaultColors(false)
            .AddSizes("Small", "Medium", "Large"),

            new ControlThemeData("Button.Icon", DefaultStyleDisplay.WithColors)
            .AddDefaultColors(false)
            .AddSizes("ExtraSmall", "Small", "Medium", "Large", "ExtraLarge")
        ];
}
