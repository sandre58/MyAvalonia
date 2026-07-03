// -----------------------------------------------------------------------
// <copyright file="MaterialIconCatalog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Material.Icons;

namespace MyNet.Avalonia.Controls.Icons;

public static class MaterialIconCatalog
{
    public static ICollection<MaterialIconKindGroup> Groups { get; } = [.. Enum.GetNames<MaterialIconKind>().GroupBy(Enum.Parse<MaterialIconKind>).Select(x => new MaterialIconKindGroup([.. x])).ToList().OrderBy(x => x.Name)];

    public static ICollection<MaterialIconKind> Kinds { get; } = [.. Groups.Select(x => x.Kind)];
}

public sealed partial record MaterialIconKindGroup(string[] Aliases)
{
    public string Name { get; } = Aliases[0];

    public MaterialIconKind Kind { get; } = Enum.Parse<MaterialIconKind>(Aliases[0]);

    public string DisplayName { get; } = Aliases.Length > 1 ? string.Join(", ", Aliases.Select(HumanizeName)) : HumanizeName(Aliases[0]);

    private static string HumanizeName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        var result = MyRegex().Replace(name, "$1 $2");
        return MyRegex1().Replace(result, "$1 $2");
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();

    [GeneratedRegex("([A-Z]+)([A-Z][a-z])")]
    private static partial Regex MyRegex1();
}
