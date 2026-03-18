// -----------------------------------------------------------------------
// <copyright file="RandomContentExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.MarkupExtensions;

internal sealed class RandomContentExtension : MarkupExtension
{
    public bool AllowNull { get; set; } = true;

    public bool AllowImage { get; set; } = true;

    public bool AllowNumber { get; set; } = true;

    public bool AllowChar { get; set; } = true;

    public bool AllowText { get; set; } = true;

    public bool AllowIcon { get; set; } = true;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var type = new List<ContentType>();
        if (AllowImage) type.Add(ContentType.Image);
        if (AllowNumber) type.Add(ContentType.Number);
        if (AllowText) type.Add(ContentType.Text);
        if (AllowIcon) type.Add(ContentType.Icon);
        if (AllowNull) type.Add(ContentType.Null);

        var value = RandomGenerator.ListItem(type);

        return value switch
        {
            ContentType.Image => new Bitmap(AssetLoader.Open(new Uri($"avares://MyNet.Avalonia.Demo/Assets/Images/avatar_{RandomGenerator.Int(1, 7)}.png"))),
            ContentType.Icon => RandomGenerator.Enum<IconData>().ToIcon(),
            ContentType.Number => RandomGenerator.Int(0, 200),
            ContentType.Text => RandomGenerator.String(RandomGenerator.Int(1, 2)),
            ContentType.Char => RandomGenerator.Char(),
            _ => null!
        };
    }

    private enum ContentType
    {
        Null,

        Image,

        Icon,

        Number,

        Text,

        Char
    }
}
