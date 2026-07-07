// -----------------------------------------------------------------------
// <copyright file="StandardToolBarLayoutEngine.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Layout;

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130

/// <summary>
/// Default layout engine: items are placed left-to-right (horizontal) or
/// top-to-bottom (vertical) with uniform spacing between them.
/// Used as-is for Standard mode and as a fallback for Compact/Fluent/Ribbon stubs.
/// </summary>
internal sealed class StandardToolBarLayoutEngine : IToolBarLayoutEngine
{
    public Size Measure(IReadOnlyList<ToolBarChildState> allItems, Orientation orientation, double spacing)
    {
        if (allItems.Count == 0)
            return default;

        if (orientation == Orientation.Horizontal)
        {
            var totalWidth = 0.0;
            var maxHeight = 0.0;
            for (var i = 0; i < allItems.Count; i++)
            {
                totalWidth += allItems[i].DesiredSize.Width;
                if (i < allItems.Count - 1) totalWidth += spacing;
                maxHeight = Math.Max(maxHeight, allItems[i].DesiredSize.Height);
            }

            return new(totalWidth, maxHeight);
        }
        else
        {
            var maxWidth = 0.0;
            var totalHeight = 0.0;
            for (var i = 0; i < allItems.Count; i++)
            {
                maxWidth = Math.Max(maxWidth, allItems[i].DesiredSize.Width);
                totalHeight += allItems[i].DesiredSize.Height;
                if (i < allItems.Count - 1) totalHeight += spacing;
            }

            return new(maxWidth, totalHeight);
        }
    }

    public ToolBarLayoutResult Arrange(ToolBarArrangeInput input)
    {
        var items = input.VisibleItems;
        var arranged = new List<ArrangedItem>(items.Count);

        if (input.Orientation == Orientation.Horizontal)
        {
            var x = 0.0;
            foreach (var state in items)
            {
                arranged.Add(new(state.Element, new(x, 0, state.DesiredSize.Width, input.FinalSize.Height)));
                x += state.DesiredSize.Width + input.Spacing;
            }

            var usedWidth = items.Count > 0 ? x - input.Spacing : 0;
            return new(arranged, new(Math.Max(0, usedWidth), input.FinalSize.Height));
        }
        else
        {
            var y = 0.0;
            foreach (var state in items)
            {
                arranged.Add(new(state.Element, new(0, y, input.FinalSize.Width, state.DesiredSize.Height)));
                y += state.DesiredSize.Height + input.Spacing;
            }

            var usedHeight = items.Count > 0 ? y - input.Spacing : 0;
            return new(arranged, new(input.FinalSize.Width, Math.Max(0, usedHeight)));
        }
    }
}
