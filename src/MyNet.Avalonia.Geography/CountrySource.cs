// -----------------------------------------------------------------------
// <copyright file="CountrySource.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using MyNet.Geography;

namespace MyNet.Avalonia.Geography;

/// <summary>
/// Provides country collections for Avalonia bindings and markup extensions.
/// </summary>
public static class CountrySource
{
    /// <summary>
    /// Returns all countries sorted by localized display name.
    /// </summary>
    public static IReadOnlyList<Country> GetAllOrderedByDisplay()
        => [.. Country.All.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase)];
}
