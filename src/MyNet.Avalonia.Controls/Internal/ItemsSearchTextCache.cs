// -----------------------------------------------------------------------
// <copyright file="ItemsSearchTextCache.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Globalization.Facade;

namespace MyNet.Avalonia.Controls.Internal;

internal sealed class ItemsSearchTextCache
{
    private readonly Dictionary<(object Item, string? Path), string> _entries = [];

    private string? _cultureName;

    private string? _searchPath;

    public void Invalidate()
    {
        _entries.Clear();
        _cultureName = null;
        _searchPath = null;
    }

    public void EnsureCurrent(string? searchPath)
    {
        var cultureName = GlobalizationServices.Current.CurrentCulture.Name;
        if (_cultureName == cultureName && _searchPath == searchPath)
            return;

        _cultureName = cultureName;
        _searchPath = searchPath;
        _entries.Clear();
    }

    public string GetOrAdd(object item, string? searchPath, Func<string> factory)
    {
        var key = (item, searchPath);
        if (!_entries.TryGetValue(key, out var text))
        {
            text = factory();
            _entries[key] = text;
        }

        return text;
    }
}
