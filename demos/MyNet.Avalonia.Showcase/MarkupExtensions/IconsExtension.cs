// -----------------------------------------------------------------------
// <copyright file="IconsExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Controls.Helpers;

namespace MyNet.Avalonia.Showcase.MarkupExtensions;

internal sealed class IconsExtension : MarkupExtension
{
    private static readonly List<MaterialIconKindGroup> Icons = [.. IconsHelper.Groups];

    public override object ProvideValue(IServiceProvider serviceProvider) => Icons;
}
