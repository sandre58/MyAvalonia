// -----------------------------------------------------------------------
// <copyright file="ThemeResourceStore.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia.Styling;

namespace MyNet.Avalonia.Theme.Runtime;

/// <summary>
/// Per-variant cache for theme resource lookups to avoid repeated dictionary walks.
/// </summary>
internal sealed class ThemeResourceStore
{
    private static readonly object NotFoundSentinel = new();

    private Dictionary<object, object?>? _cache;
    private ThemeVariant? _cachedThemeVariant;

    public bool IsLoaded { get; private set; }

    public void EnsureLoaded(Action load)
    {
        if (IsLoaded)
            return;

        IsLoaded = true;
        load();
    }

    public void Invalidate() => _cache?.Clear();

    public bool TryGetResource(
        object key,
        ThemeVariant? theme,
        Action onFirstAccess,
        Func<object, ThemeVariant?, (bool Found, object? Value)> lookup,
        out object? value)
    {
        if (!IsLoaded)
        {
            IsLoaded = true;
            onFirstAccess();
        }

        if (_cache is not null && theme == _cachedThemeVariant && _cache.TryGetValue(key, out var cached))
        {
            if (ReferenceEquals(cached, NotFoundSentinel))
            {
                value = null;
                return false;
            }

            value = cached;
            return true;
        }

        var (found, resolved) = lookup(key, theme);

        if (theme != _cachedThemeVariant)
        {
            _cache?.Clear();
            _cachedThemeVariant = theme;
        }

        _cache ??= new(256);
        _cache[key] = found ? resolved : NotFoundSentinel;
        value = resolved;
        return found;
    }
}
