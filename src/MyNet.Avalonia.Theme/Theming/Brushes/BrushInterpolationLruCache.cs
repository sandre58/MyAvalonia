// -----------------------------------------------------------------------
// <copyright file="BrushInterpolationLruCache.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using MyNet.Avalonia.Colors;
using MyNet.Avalonia.Theme.Diagnostics;

namespace MyNet.Avalonia.Theme.Theming.Brushes;

/// <summary>
/// LRU cache of transformed <see cref="SolidColorBrush"/> instances keyed by <see cref="ColorInterpolation"/>.
/// </summary>
internal sealed class BrushInterpolationLruCache
{
    private readonly int _capacity;
    private readonly Action<ISolidColorBrush>? _onEvicted;
    private readonly Dictionary<ColorInterpolation, LinkedListNode<Entry>> _entries = [];
    private readonly LinkedList<Entry> _order = [];

    public BrushInterpolationLruCache(int capacity, Action<ISolidColorBrush>? onEvicted = null)
    {
        if (capacity < 1)
            throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Capacity must be at least 1.");

        _capacity = capacity;
        _onEvicted = onEvicted;
    }

    public int Count => _entries.Count;

    public bool TryGet(ColorInterpolation key, out SolidColorBrush brush)
    {
        if (_entries.TryGetValue(key, out var node))
        {
            Touch(node);
            brush = node.Value.Brush;
            return true;
        }

        brush = null!;
        return false;
    }

    public void Set(ColorInterpolation key, SolidColorBrush brush)
    {
        if (_entries.TryGetValue(key, out var existing))
        {
            existing.Value = new(key, brush);
            Touch(existing);
            return;
        }

        var entry = new Entry(key, brush);
        var node = _order.AddFirst(entry);
        _entries[key] = node;
        EvictOverflow();
    }

    public IEnumerable<KeyValuePair<ColorInterpolation, SolidColorBrush>> EnumerateEntries() => _order.Select(entry => new KeyValuePair<ColorInterpolation, SolidColorBrush>(entry.Key, entry.Brush));

    private void Touch(LinkedListNode<Entry> node)
    {
        _order.Remove(node);
        _order.AddFirst(node);
    }

    private void EvictOverflow()
    {
        while (_entries.Count > _capacity)
        {
            var last = _order.Last;
            if (last is null)
                break;

            _order.RemoveLast();
            _entries.Remove(last.Value.Key);

            if (PerformanceMonitor.IsEnabled(PerformanceCategory.Brushes))
                PerformanceMonitor.Debug($"[BrushSet] Evicted transformed brush ({last.Value.Key})", PerformanceCategory.Brushes);

            _onEvicted?.Invoke(last.Value.Brush);
        }
    }

    private sealed record Entry(ColorInterpolation Key, SolidColorBrush Brush);
}
