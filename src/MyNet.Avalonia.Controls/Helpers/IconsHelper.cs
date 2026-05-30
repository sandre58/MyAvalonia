// -----------------------------------------------------------------------
// <copyright file="IconsHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Material.Icons;
using MyNet.Humanizer;

namespace MyNet.Avalonia.Controls.Helpers;

public static class IconsHelper
{
    public static ICollection<MaterialIconKindGroup> Groups { get; } = [.. Enum.GetNames<MaterialIconKind>().GroupBy(Enum.Parse<MaterialIconKind>).Select(x => new MaterialIconKindGroup([.. x])).ToList().OrderBy(x => x.Name)];

    public static ICollection<MaterialIconKind> Kinds { get; } = [.. Groups.Select(x => x.Kind)];
}

public sealed record MaterialIconKindGroup(string[] Aliases)
{
    public string Name { get; } = Aliases[0];

    public MaterialIconKind Kind { get; } = Enum.Parse<MaterialIconKind>(Aliases[0]);

    public string DisplayName { get; } = Aliases[0].Humanize().ToTitle();
}
