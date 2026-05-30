// -----------------------------------------------------------------------
// <copyright file="FormItemsPanel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class FormItemsPanel : Panel
{
    public static readonly StyledProperty<int> ColumnsProperty = AvaloniaProperty.Register<FormItemsPanel, int>(nameof(Columns), 1);
    public static readonly StyledProperty<double> SpacingProperty = AvaloniaProperty.Register<FormItemsPanel, double>(nameof(Spacing), 16d);
    public static readonly StyledProperty<Position> LabelPositionProperty = AvaloniaProperty.Register<FormItemsPanel, Position>(nameof(LabelPosition));
    public static readonly StyledProperty<GridLength> LabelWidthProperty = AvaloniaProperty.Register<FormItemsPanel, GridLength>(nameof(LabelWidth), GridLength.Auto);
    public static readonly StyledProperty<Thickness> GroupMarginProperty = AvaloniaProperty.Register<FormItemsPanel, Thickness>(nameof(GroupMargin), new(0, 16, 0, 16));

    static FormItemsPanel() => AffectsMeasure<FormItemsPanel>(ColumnsProperty, SpacingProperty, LabelPositionProperty, LabelWidthProperty, GroupMarginProperty);

    public int Columns
    {
        get => Math.Max(1, GetValue(ColumnsProperty));
        set => SetValue(ColumnsProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public Position LabelPosition
    {
        get => GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    public GridLength LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public Thickness GroupMargin
    {
        get => GetValue(GroupMarginProperty);
        set => SetValue(GroupMarginProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        // Get all visible children (FormItemContainer and FormGroup)
        var visibleChildren = Children.Where(x => x.IsVisible).ToList();

        if (visibleChildren.Count == 0)
            return default;

        // Calculate max label width only for FormItemContainer items
        var formItemContainers = visibleChildren.OfType<FormItemContainer>().ToList();

        if (formItemContainers is { Count: <= 0 })
        {
            return Columns > 1
                ? MeasureGrid(visibleChildren, availableSize)
                : MeasureVertical(visibleChildren, availableSize);
        }

        computeMaxLabelWidth(Position.Left);
        computeMaxLabelWidth(Position.Right);

        // Final measurement with computed widths
        return Columns > 1
            ? MeasureGrid(visibleChildren, availableSize)
            : MeasureVertical(visibleChildren, availableSize);

        void computeMaxLabelWidth(Position position)
        {
            var itemsWithPosition = formItemContainers.Where(x => x.LabelPosition == position).ToList();

            // Pass 1: reset computed width and force a neutral measure to get natural label widths.
            foreach (var item in itemsWithPosition)
            {
                item.PanelComputedWidth = 0;
                item.InvalidateMeasure();
                item.Measure(Size.Infinity);
            }

            var maxWidth = itemsWithPosition
                .Select(item => item.MeasureLabelContainer().Width)
                .Prepend(0)
                .Max();

            // Pass 2: Apply computed width to all items
            // Each item will decide if it uses PanelComputedWidth or its own LabelWidth
            foreach (var item in itemsWithPosition)
            {
                item.PanelComputedWidth = maxWidth;
            }
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        // Get all visible children (FormItemContainer and FormGroup)
        var visibleChildren = Children.Where(x => x.IsVisible).ToList();

        return Columns > 1
            ? ArrangeGrid(visibleChildren, finalSize)
            : ArrangeVertical(visibleChildren, finalSize);
    }

    private Size MeasureVertical(List<Control> items, Size availableSize)
    {
        double width = 0;
        double height = 0;

        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            item.Measure(availableSize);
            width = Math.Max(width, item.DesiredSize.Width);
            height += item.DesiredSize.Height;

            // Add top margin for FormGroup if it's the first item
            if (i == 0 && item is FormGroup)
            {
                var (topMargin, _) = GetFormGroupMargins(item, items, i);
                height += topMargin;
            }

            // Add spacing or margins between items
            if (i < items.Count - 1)
            {
                var nextItem = items[i + 1];
                height += GetSpacingBetweenItems(item, nextItem, items, i);
            }

            // Add bottom margin if it's the last FormGroup
            else if (item is FormGroup)
            {
                var (_, bottomMargin) = GetFormGroupMargins(item, items, i);
                height += bottomMargin;
            }
        }

        return new(width, height);
    }

    private Size ArrangeVertical(List<Control> items, Size finalSize)
    {
        double y = 0;

        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];

            // Add top margin for FormGroup if it's the first item
            if (i == 0 && item is FormGroup)
            {
                var (topMargin, _) = GetFormGroupMargins(item, items, i);
                y += topMargin;
            }

            var h = item.DesiredSize.Height;
            item.Arrange(new(0, y, finalSize.Width, h));
            y += h;

            // Add spacing or margins between items
            if (i < items.Count - 1)
            {
                var nextItem = items[i + 1];
                y += GetSpacingBetweenItems(item, nextItem, items, i);
            }

            // Add bottom margin if it's the last FormGroup
            else if (item is FormGroup)
            {
                var (_, bottomMargin) = GetFormGroupMargins(item, items, i);
                y += bottomMargin;
            }
        }

        return finalSize;
    }

    private Size MeasureGrid(List<Control> items, Size availableSize)
    {
        var columns = Columns;
        var spacing = Spacing;
        var cellWidth = (availableSize.Width - ((columns - 1) * spacing)) / columns;

        double totalHeight = 0;
        var index = 0;
        var previousRowHadFormGroup = false;
        var previousBottomMargin = 0.0;

        while (index < items.Count)
        {
            double rowHeight = 0;
            var col = 0;
            var rowItems = new List<Control>();

            while (col < columns && index < items.Count)
            {
                var item = items[index];
                var span = Math.Min(FormItem.GetColumnSpan(item), columns - col);
                var width = (cellWidth * span) + (spacing * (span - 1));

                item.Measure(new(width, availableSize.Height));
                rowHeight = Math.Max(rowHeight, item.DesiredSize.Height);
                rowItems.Add(item);

                col += span;
                index++;
            }

            var currentRowHasFormGroup = rowItems is
            [
                FormGroup
            ];

            // Add spacing from previous row
            // Not first row
            if (index > rowItems.Count)
            {
                if (currentRowHasFormGroup)
                {
                    var groupIndex = items.IndexOf(rowItems[0]);
                    var (topMargin, _) = GetFormGroupMargins(rowItems[0], items, groupIndex);

                    if (!ShouldSkipTopMargin(rowItems[0], items, groupIndex))
                    {
                        // Take max between spacing and combined margins
                        totalHeight += Math.Max(spacing, previousBottomMargin + topMargin);
                    }
                    else
                    {
                        // Consecutive FormGroups: margin already handled
                        totalHeight += Math.Max(spacing, previousBottomMargin);
                    }
                }
                else if (previousRowHadFormGroup)
                {
                    // Previous was FormGroup, current is not
                    totalHeight += Math.Max(spacing, previousBottomMargin);
                }
                else
                {
                    // Normal spacing
                    totalHeight += spacing;
                }
            }

            // First row with FormGroup: add top margin
            else if (currentRowHasFormGroup)
            {
                var groupIndex = items.IndexOf(rowItems[0]);
                var (topMargin, _) = GetFormGroupMargins(rowItems[0], items, groupIndex);
                totalHeight += topMargin;
            }

            totalHeight += rowHeight;

            // Track bottom margin for next iteration
            if (currentRowHasFormGroup)
            {
                var groupIndex = items.IndexOf(rowItems[0]);
                var (_, bottomMargin) = GetFormGroupMargins(rowItems[0], items, groupIndex);
                previousBottomMargin = bottomMargin;
            }
            else
            {
                previousBottomMargin = 0;
            }

            previousRowHadFormGroup = currentRowHasFormGroup;
        }

        // Add final bottom margin if last row is FormGroup
        if (previousRowHadFormGroup && previousBottomMargin > 0)
            totalHeight += previousBottomMargin;

        return new(availableSize.Width, totalHeight);
    }

    private Size ArrangeGrid(List<Control> items, Size finalSize)
    {
        var columns = Columns;
        var spacing = Spacing;
        var cellWidth = (finalSize.Width - ((columns - 1) * spacing)) / columns;

        double y = 0;
        var index = 0;
        var previousRowHadFormGroup = false;
        var previousBottomMargin = 0.0;

        while (index < items.Count)
        {
            double rowHeight = 0;
            var rowStart = index;
            var col = 0;
            var rowItems = new List<Control>();

            // First pass: calculate row height
            while (col < columns && index < items.Count)
            {
                var item = items[index];
                var span = Math.Min(FormItem.GetColumnSpan(item), columns - col);
                rowHeight = Math.Max(rowHeight, item.Bounds.Height > 0 ? item.Bounds.Height : item.DesiredSize.Height);
                rowItems.Add(item);
                col += span;
                index++;
            }

            var currentRowHasFormGroup = rowItems is
            [
                FormGroup
            ];

            // Add spacing from previous row
            // Not first row
            if (index > rowItems.Count)
            {
                if (currentRowHasFormGroup)
                {
                    var groupIndex = items.IndexOf(rowItems[0]);
                    var (topMargin, _) = GetFormGroupMargins(rowItems[0], items, groupIndex);

                    if (!ShouldSkipTopMargin(rowItems[0], items, groupIndex))
                    {
                        y += Math.Max(spacing, previousBottomMargin + topMargin);
                    }
                    else
                    {
                        y += Math.Max(spacing, previousBottomMargin);
                    }
                }
                else if (previousRowHadFormGroup)
                {
                    y += Math.Max(spacing, previousBottomMargin);
                }
                else
                {
                    y += spacing;
                }
            }

            // First row with FormGroup: add top margin
            else if (currentRowHasFormGroup)
            {
                var groupIndex = items.IndexOf(rowItems[0]);
                var (topMargin, _) = GetFormGroupMargins(rowItems[0], items, groupIndex);
                y += topMargin;
            }

            // Second pass: arrange items
            col = 0;
            for (var arrangeIndex = rowStart; col < columns && arrangeIndex < items.Count; arrangeIndex++)
            {
                var item = items[arrangeIndex];
                var span = Math.Min(FormItem.GetColumnSpan(item), columns - col);
                var width = (cellWidth * span) + (spacing * (span - 1));
                var x = col * (cellWidth + spacing);

                var desiredHeight = item.DesiredSize.Height;
                item.Arrange(new(x, y, width, desiredHeight));
                col += span;
            }

            y += rowHeight;

            // Track bottom margin for next iteration
            if (currentRowHasFormGroup)
            {
                var groupIndex = items.IndexOf(rowItems[0]);
                var (_, bottomMargin) = GetFormGroupMargins(rowItems[0], items, groupIndex);
                previousBottomMargin = bottomMargin;
            }
            else
            {
                previousBottomMargin = 0;
            }

            previousRowHadFormGroup = currentRowHasFormGroup;
        }

        return finalSize;
    }

    private (double TopMargin, double BottomMargin) GetFormGroupMargins(Control item, List<Control> allItems, int index)
    {
        if (item is not FormGroup)
            return (0, 0);

        var (_, nextTopMargin, _, bottom) = GroupMargin;
        var isFirst = index == 0;
        var isLast = index == allItems.Count - 1;

        var topMargin = isFirst ? 0 : nextTopMargin;
        var bottomMargin = isLast ? 0 : bottom;

        // If next item is also a FormGroup, use max margin instead of both
        if (!isLast && index + 1 < allItems.Count && allItems[index + 1] is FormGroup)
        {
            // Use the larger of bottom margin of current and top margin of next
            // We'll apply it as bottom margin here and skip top margin for next
            bottomMargin = Math.Max(bottomMargin, nextTopMargin);
        }

        return (topMargin, bottomMargin);
    }

    private static bool ShouldSkipTopMargin(Control item, List<Control> allItems, int index)
    {
        if (item is not FormGroup || index == 0)
            return false;

        // Skip top margin if previous item is a FormGroup (margin was already applied)
        return allItems[index - 1] is FormGroup;
    }

    private double GetSpacingBetweenItems(Control currentItem, Control? nextItem, List<Control> allItems, int currentIndex)
    {
        if (nextItem == null)
            return 0;

        var spacing = Spacing;
        var (_, bottomMargin) = GetFormGroupMargins(currentItem, allItems, currentIndex);
        var (topMargin, _) = GetFormGroupMargins(nextItem, allItems, currentIndex + 1);

        // If next item should skip top margin (consecutive FormGroups), it's already handled in bottomMargin
        if (ShouldSkipTopMargin(nextItem, allItems, currentIndex + 1))
            topMargin = 0;

        // Take the max between spacing and the combined margins
        var totalMargin = bottomMargin + topMargin;
        return Math.Max(spacing, totalMargin);
    }
}
