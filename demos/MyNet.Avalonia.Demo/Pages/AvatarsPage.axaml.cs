// -----------------------------------------------------------------------
// <copyright file="AvatarsPage.axaml.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Demo.Helpers;
using MyNet.Avalonia.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.Pages;

internal sealed partial class AvatarsPage : AutoBuildPage
{
    public AvatarsPage() => InitializeComponent();

    protected override Control CreateControl(ControlData data)
    {
        var item = new Avatar
        {
            HorizontalAlignment = global::Avalonia.Layout.HorizontalAlignment.Center,
            Content = RandomGenerator.Bool() ? data.Color.Or(data.Size.OrEmpty()).Or("Default").GetInitials() : RandomGenerator.Enum<IconData>().ToIcon(),
            Source = RandomGenerator.Bool() ? new Bitmap(AssetLoader.Open(new Uri($"avares://MyNet.Avalonia.Demo/Assets/Images/avatar_{RandomGenerator.Int(1, 7)}.png"))) : null,
            [!Avatar.ShowBackgroundProperty] = ShowBackground[!global::Avalonia.Controls.Primitives.ToggleButton.IsCheckedProperty],
        };

        return item;
    }

    protected override IEnumerable<ControlThemeData> ProvideThemes()
        => [
           new ControlThemeData(defaultStyleDisplay: DefaultStyleDisplay.WithColors)
                                      .AddLayouts("Circle")
                                      .AddStyles("Shadow")
                                      .AddAllColors()
                                      .AddSizes("ExtraSmall", "Small", "Medium", "Large", "ExtraLarge")
        ];
}
