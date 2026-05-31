// -----------------------------------------------------------------------
// <copyright file="ThemeBindingHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;

namespace MyNet.Avalonia.Theme.MarkupExtensions.Helpers;

/// <summary>
/// Creates standard <see cref="Binding"/> instances for theme markup extensions (avoids <see cref="ReflectionBinding"/>).
/// </summary>
internal static class ThemeBindingHelper
{
    public static Binding Create(string path, RelativeSource relativeSource, IServiceProvider? serviceProvider = null)
        => new(path)
        {
            Mode = BindingMode.OneWay,
            RelativeSource = relativeSource,
            TypeResolver = serviceProvider is null ? null : (x, y) => ThemeBrushExtensionBase.ResolveType(serviceProvider, x, y)
        };

    public static Binding CreateParentForeground(IServiceProvider serviceProvider)
        => Create("Parent.(TextElement.Foreground)", new(RelativeSourceMode.Self), serviceProvider);

    public static Binding CreateConstantSource(object source)
        => new()
        {
            Mode = BindingMode.OneTime,
            Source = source
        };
}
