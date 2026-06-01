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
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Generator.Facade;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

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

        var value = RandomGenerator.Current.Item(type);

        return value switch
        {
            ContentType.Image => new Bitmap(AssetLoader.Open(new($"avares://MyNet.Avalonia.Showcase/Assets/Images/avatar_{RandomGenerator.Current.Int(1, 7)}.png"))),
            ContentType.Icon => RandomGenerator.Current.Enum<MaterialIconKind>().ToIcon(),
            ContentType.Number => RandomGenerator.Current.Int(0, 200),
            ContentType.Text => RandomGenerator.Current.String(RandomGenerator.Current.Int(1, 2)),
            ContentType.Char => RandomGenerator.Current.Char(),
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
