// -----------------------------------------------------------------------
// <copyright file="RandomImageExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Demo.MarkupExtensions;

/// <summary>
/// Markup extension for creating and binding to a themed icon in XAML.
/// Allows specifying the icon data (geometry key), size category, or explicit size for consistent icon rendering in the UI.
/// </summary>
internal sealed class RandomImageExtension : MarkupExtension
{
    public bool AllowNull { get; set; } = true;

    public override object ProvideValue(IServiceProvider serviceProvider)
        => !AllowNull || RandomGenerator.Bool() ? new Bitmap(AssetLoader.Open(new Uri($"avares://MyNet.Avalonia.Demo/Assets/Images/avatar_{RandomGenerator.Int(1, 7)}.png"))) : null!;
}
