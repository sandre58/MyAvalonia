// -----------------------------------------------------------------------
// <copyright file="IconsExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Classes.Enums;
using MyNet.Avalonia.Theme.Extensions;
using MyNet.Observable.Translatables;

namespace MyNet.Avalonia.Demo.MarkupExtensions;

internal sealed class IconsExtension : MarkupExtension
{
    private static readonly List<IconDataWrapper> Icons = [.. Enum.GetValues<IconData>().Order().Select(x => new IconDataWrapper(x, x.ToGeometry(), x.ToString()))];

    public override object ProvideValue(IServiceProvider serviceProvider) => Icons;
}

public class IconDataWrapper(IconData data, Geometry geometry, string key) : DisplayWrapper<IconData>(data, key)
{
       public Geometry Geometry { get; } = geometry;
}
