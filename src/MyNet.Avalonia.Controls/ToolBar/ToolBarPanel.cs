// -----------------------------------------------------------------------
// <copyright file="ToolBarPanel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130

/// <summary>
/// Custom <see cref="Panel"/> that executes the three-phase toolbar layout pipeline
/// (Measure → Overflow → Arrange) entirely through the injected <see cref="ToolBarLayoutContext"/>.
/// </summary>
/// <remarks>
/// <para>This panel has no knowledge of <see cref="ToolBar"/>. Its only external dependency
/// is <see cref="ToolBarLayoutContext"/>, which is pushed by <see cref="ToolBar.OnApplyTemplate"/>.</para>
/// <para>Rules enforced here to match project conventions:</para>
/// <list type="bullet">
///   <item>IsVisible is never mutated in layout passes — overflow items are arranged off-screen.</item>
///   <item>InvalidateMeasure is never called from ArrangeOverride.</item>
///   <item>ClipToBounds = true ensures overflow items (at position 0,0 / 0×0) are invisible.</item>
/// </list>
/// </remarks>
public class ToolBarPanel : Panel
{
    private static readonly Rect HiddenOverflowRect = new(-10000, -10000, 0, 0);

    private ToolBarLayoutContext? _layoutContext;
    private ToolBarOverflowResult? _pendingOverflowResult;

    public ToolBarPanel() => ClipToBounds = true;

    /// <summary>
    /// Gets or sets the context pushed by <see cref="ToolBar"/>. Setting this property triggers
    /// <see cref="Panel.InvalidateMeasure"/> so the panel re-runs the full pipeline
    /// with the new configuration.
    /// </summary>
    internal ToolBarLayoutContext? LayoutContext
    {
        get => _layoutContext;
        set
        {
            _layoutContext = value;
            InvalidateMeasure();
        }
    }

    /// <summary>
    /// Consumes the overflow result produced by the latest arrange pass.
    /// Called from <see cref="ToolBar"/> after layout completes.
    /// </summary>
    internal bool TryTakePendingOverflowResult([NotNullWhen(true)] out ToolBarOverflowResult? result)
    {
        result = _pendingOverflowResult;
        _pendingOverflowResult = null;
        return result is not null;
    }

    /// <summary>
    /// Phase 1 — Measure: measures all children and asks the engine
    /// for the total desired size (no overflow decisions yet).
    /// </summary>
    protected override Size MeasureOverride(Size availableSize)
    {
        var ctx = _layoutContext;
        var measureConstraint = GetMeasureConstraint(availableSize);

        foreach (var child in Children)
            child.Measure(measureConstraint);

        if (ctx is null)
        {
            var width = 0.0;
            var height = 0.0;
            foreach (var child in Children)
            {
                width += child.DesiredSize.Width;
                height = Math.Max(height, child.DesiredSize.Height);
            }

            if (Children.Count > 1)
                width += (Children.Count - 1) * 2;
            return new(width, height);
        }

        var allStates = BuildChildStates();
        return ctx.Engine.Measure(allStates, ctx.Orientation, ctx.ItemSpacing);
    }

    /// <summary>
    /// Phases 2 + 3 — Overflow then Arrange.
    /// <para>Phase 2: the overflow engine splits children into visible and overflow sets.</para>
    /// <para>Phase 3: the layout engine assigns an explicit <see cref="Rect"/> to every visible child.</para>
    /// <para>Overflow children are arranged off-screen — IsVisible is never mutated.</para>
    /// </summary>
    protected override Size ArrangeOverride(Size finalSize)
    {
        var ctx = _layoutContext;
        if (ctx is null)
        {
            ArrangeChildrenWithoutContext();
            return finalSize;
        }

        var allStates = BuildChildStates();

        // Phase 2: resolve overflow
        var overflowInput = new ToolBarOverflowInput(
            allStates,
            finalSize,
            ctx.ItemSpacing,
            ctx.OverflowMode,
            ctx.OverflowButtonReserveWidth);
        var overflowResult = ctx.OverflowEngine.Resolve(overflowInput);

        // Phase 3: arrange visible items
        var arrangeInput = new ToolBarArrangeInput(overflowResult.VisibleItems, finalSize, ctx.Orientation, ctx.ItemSpacing);
        var layoutResult = ctx.Engine.Arrange(arrangeInput);

        // Apply explicit rects to visible items
        foreach (var arranged in layoutResult.ArrangedItems)
            arranged.Element.Arrange(arranged.Rect);

        // Arrange overflow items off-screen — IsVisible is never changed.
        var visibleElements = new HashSet<Control>(
            layoutResult.ArrangedItems.Select(a => a.Element),
            ReferenceEqualityComparer.Instance);

        foreach (var child in Children)
        {
            if (!visibleElements.Contains(child))
                child.Arrange(HiddenOverflowRect);
        }

        _pendingOverflowResult = overflowResult;

        return finalSize;
    }

    private void ArrangeChildrenWithoutContext()
    {
        var x = 0.0;
        const double y = 0.0;
        const double fallbackSpacing = 2;

        foreach (var child in Children)
        {
            var size = child.DesiredSize;
            child.Arrange(new(x, y, size.Width, size.Height));
            x += size.Width + fallbackSpacing;
        }
    }

    private List<ToolBarChildState> BuildChildStates()
    {
        var states = new List<ToolBarChildState>(Children.Count);
        var resolvePopupItem = _layoutContext?.ResolvePopupItem;
        foreach (var child in Children)
        {
            var priority = child is ToolBarItem item
                ? item.OverflowPriority
                : ToolBarOverflowPriority.Normal;
            var isSeparator = child is ToolBarSeparator;
            var popupItem = resolvePopupItem?.Invoke(child);
            states.Add(new(child, child.DesiredSize, priority, isSeparator, popupItem));
        }

        return states;
    }

    private static Size GetMeasureConstraint(Size availableSize)
    {
        var width = double.IsPositiveInfinity(availableSize.Width) || availableSize.Width <= 0
            ? double.PositiveInfinity
            : availableSize.Width;
        var height = double.IsPositiveInfinity(availableSize.Height) || availableSize.Height <= 0
            ? double.PositiveInfinity
            : availableSize.Height;

        return new(width, height);
    }
}
