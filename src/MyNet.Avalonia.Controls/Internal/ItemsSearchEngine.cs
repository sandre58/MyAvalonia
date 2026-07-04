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
using MyNet.Avalonia.Controls.Icons;
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

    public static string GetItemText(SelectingItemsControl control, object? item) =>
        GetItemText(control, item, cache: null);

    public static string GetItemText(SelectingItemsControl control, object? item, ItemsSearchTextCache? cache)
    {
        if (item is null)
            return string.Empty;

        var searchPath = ItemsSearchBehavior.GetSearchMemberPath(control);
        if (cache is not null)
        {
            cache.EnsureCurrent(searchPath);
            return cache.GetOrAdd(item, searchPath, () => ResolveItemText(control, item, searchPath));
        }

        return ResolveItemText(control, item, searchPath);
    }

    public static bool ShouldApplyFilter(string? text, int minimumLength) =>
        string.IsNullOrEmpty(text) || text.Length >= minimumLength;

    public static bool IsItemMatch(
        SelectingItemsControl control,
        object? item,
        string? text,
        bool applyFilter,
        ItemsSearchFilterMode filterMode,
        bool isCaseSensitive,
        ItemsSearchTextCache? cache)
    {
        if (!applyFilter || string.IsNullOrEmpty(text))
            return true;

        return IsMatch(
            text,
            GetItemText(control, item, cache),
            filterMode,
            isCaseSensitive);
    }

    private static string ResolveItemText(SelectingItemsControl control, object item, string? searchPath)
    {
        if (TryGetMaterialIconKindGroupText(item, searchPath, out var groupText))
            return groupText;

        var culture = GlobalizationServices.Current.CurrentCulture;

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

    private static bool TryGetMaterialIconKindGroupText(object item, string? searchPath, out string text)
    {
        if (item is not MaterialIconKindGroup group)
        {
            text = string.Empty;
            return false;
        }

        if (string.IsNullOrEmpty(searchPath) || searchPath is "Display" or "DisplayName" or ".")
        {
            text = group.DisplayName;
            return true;
        }

        if (searchPath is "Name")
        {
            text = group.Name;
            return true;
        }

        if (searchPath is "Kind")
        {
            text = group.Kind.ToString();
            return true;
        }

        text = string.Empty;
        return false;
    }

    private static bool IsDisplayAlignedSearchPath(string? path) =>
        string.IsNullOrEmpty(path) || path is "Name" or "Display" or ".";
}
