// -----------------------------------------------------------------------
// <copyright file="ThemeExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace MyNet.Avalonia.Theme.MarkupExtensions;

public class ThemeExtension(string path) : MarkupExtension
{
    [ConstructorArgument("path")]
    public string Path { get; set; } = path;

    public override object ProvideValue(IServiceProvider serviceProvider) => new StaticResourceExtension(ThemeResourceKeyFactory.Brush(Path)).ProvideValue(serviceProvider)!;
}
