// -----------------------------------------------------------------------
// <copyright file="ItemsSearchEngine.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using MyNet.Avalonia.Controls.Behaviors;
using MyNet.Avalonia.Converters;
using MyNet.Globalization.Facade;

namespace MyNet.Avalonia.Controls.Internal;

internal static class ItemsSearchEngine
{
    public static bool IsMatch(
        string? query,
        string itemText,
        ItemsSearchFilterMode filterMode,
        bool isCaseSensitive)
    {
        if (string.IsNullOrEmpty(query))
            return true;

        var comparison = isCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        return filterMode switch
        {
            ItemsSearchFilterMode.StartsWith => itemText.StartsWith(query, comparison),
            ItemsSearchFilterMode.Equals => string.Equals(itemText, query, comparison),
            _ => itemText.Contains(query, comparison),
        };
    }

    public static string GetItemText(SelectingItemsControl control, object? item)
    {
        if (item is null)
            return string.Empty;

        var culture = GlobalizationServices.Current.CurrentCulture;
        var searchPath = ItemsSearchBehavior.GetSearchMemberPath(control);

        if (!string.IsNullOrEmpty(searchPath) && !IsDisplayAlignedSearchPath(searchPath))
        {
            var searchHost = new ContentControl { DataContext = item };
            using var searchExpression = searchHost.Bind(
                ContentControl.ContentProperty,
                new ReflectionBinding(searchPath) { Mode = BindingMode.OneWay });
            return searchHost.Content?.ToString() ?? string.Empty;
        }

        if (string.IsNullOrEmpty(searchPath) && control.DisplayMemberBinding is { } displayBinding)
        {
            var displayHost = new ContentControl { DataContext = item };
            using var displayExpression = displayHost.Bind(ContentControl.ContentProperty, displayBinding);
            return displayHost.Content?.ToString() ?? string.Empty;
        }

        if (IsDisplayAlignedSearchPath(searchPath)
            && DisplayTextResolver.TryConvertRegistered(item, culture, out var displayText))
            return displayText ?? string.Empty;

        if (!string.IsNullOrEmpty(searchPath))
        {
            var searchHost = new ContentControl { DataContext = item };
            using var searchExpression = searchHost.Bind(
                ContentControl.ContentProperty,
                new ReflectionBinding(searchPath) { Mode = BindingMode.OneWay });
            return searchHost.Content?.ToString() ?? string.Empty;
        }

        if (DisplayTextResolver.TryConvertRegistered(item, culture, out displayText))
            return displayText ?? string.Empty;

        return item.ToString() ?? string.Empty;
    }

    private static bool IsDisplayAlignedSearchPath(string? path) =>
        string.IsNullOrEmpty(path) || path is "Name" or "Display" or ".";

    public static bool ShouldApplyFilter(string? text, int minimumLength) =>
        string.IsNullOrEmpty(text) || text.Length >= minimumLength;

    public static int GetMatchCount(SelectingItemsControl control)
    {
        var text = ItemsSearchBehavior.GetText(control);
        var applyFilter = ShouldApplyFilter(text, ItemsSearchBehavior.GetMinimumLength(control));
        if (!applyFilter || string.IsNullOrEmpty(text))
            return control.ItemCount;

        var count = 0;
        for (var i = 0; i < control.ItemCount; i++)
        {
            if (IsItemMatch(control, control.Items[i], text, applyFilter))
                count++;
        }

        return count;
    }

    public static int? TryGetSingleMatchIndex(SelectingItemsControl control)
    {
        var text = ItemsSearchBehavior.GetText(control);
        var applyFilter = ShouldApplyFilter(text, ItemsSearchBehavior.GetMinimumLength(control));
        if (!applyFilter || string.IsNullOrEmpty(text))
            return null;

        int? matchIndex = null;
        for (var i = 0; i < control.ItemCount; i++)
        {
            if (!IsItemMatch(control, control.Items[i], text, applyFilter))
                continue;

            if (matchIndex is not null)
                return null;

            matchIndex = i;
        }

        return matchIndex;
    }

    private static bool IsItemMatch(SelectingItemsControl control, object? item, string? text, bool applyFilter) =>
        !applyFilter
        || string.IsNullOrEmpty(text)
        || IsMatch(
            text,
            GetItemText(control, item),
            ItemsSearchBehavior.GetFilterMode(control),
            ItemsSearchBehavior.GetIsCaseSensitive(control));
}
