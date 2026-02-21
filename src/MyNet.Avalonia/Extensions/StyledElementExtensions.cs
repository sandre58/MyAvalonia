// -----------------------------------------------------------------------
// <copyright file="StyledElementExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia;

namespace MyNet.Avalonia.Extensions;

[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class StyledElementExtensions
{
    extension(StyledElement obj)
    {
        public void AddClasses(params string[] classes)
            => obj.Classes.AddRange(classes.SelectMany(x => x.Split(" ", System.StringSplitOptions.RemoveEmptyEntries)));

        public void RemoveClasses(params string[] classes) => obj.Classes.RemoveAll(classes);
    }
}
