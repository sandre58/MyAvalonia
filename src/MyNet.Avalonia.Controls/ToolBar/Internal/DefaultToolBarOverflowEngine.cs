// -----------------------------------------------------------------------
// <copyright file="DefaultToolBarOverflowEngine.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Single-pass overflow engine.
/// Resolution order:
/// 1. <see cref="ToolBarOverflowPriority.AlwaysOverflow"/> items are always moved to the overflow menu.
/// 2. <see cref="ToolBarOverflowPriority.NeverOverflow"/> items are always kept visible.
/// 3. Remaining items are removed in descending priority order (Low before Normal)
///    until the remaining items fit within <see cref="ToolBarOverflowInput.AvailableSize"/>.
/// </summary>
internal sealed class DefaultToolBarOverflowEngine : IToolBarOverflowEngine
{
    public ToolBarOverflowResult Resolve(ToolBarOverflowInput input)
    {
        if (input.OverflowMode == ToolBarOverflowMode.None)
            return new(input.AllItems, [], false);

        var overflowSet = new HashSet<ToolBarChildState>(ReferenceEqualityComparer.Instance);

        // Step 1: AlwaysOverflow items are always removed from the visible set.
        foreach (var item in input.AllItems)
        {
            if (item.IsSeparator)
                continue;

            if (item.OverflowPriority == ToolBarOverflowPriority.AlwaysOverflow)
                overflowSet.Add(item);
        }

        // Step 2: Compute total width of remaining candidates.
        var candidates = input.AllItems.Where(s => !overflowSet.Contains(s)).ToList();
        var currentExtent = ComputeTotalWidth(candidates, input.Spacing);
        var availableExtent = GetAvailableExtent(input);

        // Step 3: If Adaptive and everything fits, return early.
        if (input.OverflowMode == ToolBarOverflowMode.Adaptive && currentExtent <= availableExtent && overflowSet.Count == 0)
            return new(input.AllItems, [], false);

        // Step 4: Greedily remove items (Low priority first, then Normal) until they fit.
        var removable = candidates
            .Where(s => !s.IsSeparator && s.OverflowPriority is ToolBarOverflowPriority.Low or ToolBarOverflowPriority.Normal)
            .OrderByDescending(s => (int)s.OverflowPriority)
            .ToList();

        foreach (var item in removable.TakeWhile(_ => currentExtent > availableExtent))
        {
            overflowSet.Add(item);
            currentExtent -= GetFiniteWidth(item.DesiredSize) + input.Spacing;
        }

        var visibleItems = input.AllItems.Where(s => !overflowSet.Contains(s)).ToList();
        var overflowItems = input.AllItems.Where(overflowSet.Contains).ToList();
        var hasOverflow = overflowItems.Any(s => !s.IsSeparator);

        return new(visibleItems, overflowItems, hasOverflow);
    }

    private static double GetAvailableExtent(ToolBarOverflowInput input)
    {
        var extent = input.AvailableSize.Width;

        // First layout pass with an unknown strip size — do not overflow everything.
        if (double.IsNaN(extent) || extent <= 0)
            return double.PositiveInfinity;

        if (input.OverflowMode != ToolBarOverflowMode.None && input.OverflowButtonReserveWidth > 0)
            extent = Math.Max(0, extent - input.OverflowButtonReserveWidth);

        return extent;
    }

    private static double ComputeTotalWidth(List<ToolBarChildState> items, double spacing)
    {
        if (items.Count == 0)
            return 0;

        var total = 0.0;
        for (var i = 0; i < items.Count; i++)
        {
            total += GetFiniteWidth(items[i].DesiredSize);
            if (i < items.Count - 1)
                total += spacing;
        }

        return total;
    }

    private static double GetFiniteWidth(Size desiredSize)
    {
        var width = desiredSize.Width;
        return double.IsFinite(width) && width > 0 ? width : 0;
    }
}
