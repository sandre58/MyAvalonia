// -----------------------------------------------------------------------
// <copyright file="ItemsSearchFocusHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Behaviors;
using MyNet.Avalonia.Controls.Primitives.Internal;

namespace MyNet.Avalonia.Controls.Internal;

internal static class ItemsSearchFocusHelper
{
    public static bool TryHandleSearchBoxKeyDown(
        SelectingItemsControl control,
        TextBox searchBox,
        Control itemsRoot,
        KeyEventArgs e,
        Action clearSearchText,
        Action closePopup)
    {
        if (!ReferenceEquals(e.Source, searchBox))
            return false;

        switch (e.Key)
        {
            case Key.Escape:
                if (!string.IsNullOrEmpty(ItemsSearchBehavior.GetText(control)))
                {
                    clearSearchText();
                    searchBox.Focus();
                    e.Handled = true;
                    return true;
                }

                closePopup();
                e.Handled = true;
                return true;

            case Key.Enter:
                if (TryCommitSearchFromSearchBox(control, itemsRoot))
                {
                    e.Handled = true;
                    return true;
                }

                break;

            case Key.Down:
                if (FocusFirstVisibleItem(control, itemsRoot))
                {
                    e.Handled = true;
                    return true;
                }

                break;

            case Key.Up:
                searchBox.Focus(NavigationMethod.Directional);
                e.Handled = true;
                return true;

            case Key.Tab:
                if (TextPickerPopupFocusHelper.TryHandleTextBoxTab(itemsRoot, searchBox, e, _ => FocusFirstVisibleItem(control, itemsRoot)))
                {
                    e.Handled = true;
                    return true;
                }

                break;
        }

        return false;
    }

    public static bool TryHandleItemKeyDown(
        SelectingItemsControl control,
        TextBox? searchBox,
        Control itemsRoot,
        KeyEventArgs e)
    {
        if (searchBox is null)
            return false;

        if (e.Key == Key.F && e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            FocusSearchBox(searchBox, selectAll: true);
            e.Handled = true;
            return true;
        }

        if (e.Key is not Key.Up and not Key.Tab)
            return false;

        var focusables = GetVisibleItemFocusables(itemsRoot);
        if (focusables.Count == 0)
            return false;

        if (e.Key == Key.Up && focusables[0].IsFocused)
        {
            searchBox.Focus(NavigationMethod.Directional);
            e.Handled = true;
            return true;
        }

        if (e.Key == Key.Tab && TextPickerPopupFocusHelper.TryHandlePreviewerTab(itemsRoot, searchBox, e))
        {
            e.Handled = true;
            return true;
        }

        return false;
    }

    public static void FocusSearchBox(TextBox searchBox, bool selectAll = false)
    {
        searchBox.Focus(NavigationMethod.Directional);

        if (selectAll)
            searchBox.SelectAll();
    }

    public static bool FocusFirstVisibleItem(SelectingItemsControl control, Control itemsRoot)
    {
        ItemsSearchBehavior.FlushApplyFilter(control);
        return GetVisibleItemFocusables(itemsRoot).FirstOrDefault()?.Focus(NavigationMethod.Directional) == true;
    }

    public static bool TryCommitSearchFromSearchBox(SelectingItemsControl control, Control itemsRoot)
    {
        ItemsSearchBehavior.FlushApplyFilter(control);

        var matchCount = ItemsSearchBehavior.GetMatchCount(control);
        if (matchCount == 0)
            return false;

        if (matchCount == 1)
        {
            var matchIndex = ItemsSearchEngine.TryGetSingleMatchIndex(control);
            if (matchIndex is null)
                return false;

            switch (control)
            {
                case ComboBox comboBox:
                    comboBox.SelectedIndex = matchIndex.Value;
                    comboBox.ClosePopup();
                    return true;
                case MultiComboBox multiComboBox:
                    multiComboBox.Selection.Select(matchIndex.Value);
                    return true;
            }
        }

        return FocusFirstVisibleItem(control, itemsRoot);
    }

    private static IReadOnlyList<Control> GetVisibleItemFocusables(Control itemsRoot) =>
        TextPickerPopupFocusHelper.GetTabFocusables(itemsRoot)
            .Where(static c => c.IsVisible)
            .ToList();
}
