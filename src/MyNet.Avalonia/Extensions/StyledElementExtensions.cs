// -----------------------------------------------------------------------
// <copyright file="StyledElementExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;

namespace MyNet.Avalonia.Extensions;

public static class StyledElementExtensions
{
    public static void AddClasses(this StyledElement obj, params string[] classes)
        => obj.Classes.AddRange(classes.SelectMany(x => x.Split(" ", System.StringSplitOptions.RemoveEmptyEntries)));

    public static void RemoveClasses(this StyledElement obj, params string[] classes) => obj.Classes.RemoveAll(classes);
}
