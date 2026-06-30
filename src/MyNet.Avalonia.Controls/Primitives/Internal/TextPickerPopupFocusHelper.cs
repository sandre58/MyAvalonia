// -----------------------------------------------------------------------
// <copyright file="TextPickerPopupFocusHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace MyNet.Avalonia.Controls.Primitives.Internal;

internal static class TextPickerPopupFocusHelper
{
    public static bool TryHandleTextBoxTab(Control previewer, TextBox textBox, KeyEventArgs e, Action<Control>? focusPreviewerFromTextBox = null)
    {
        if (e.Key != Key.Tab || !ReferenceEquals(e.Source, textBox))
            return false;

        if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            FocusLast(previewer);
        else if (focusPreviewerFromTextBox is not null)
            focusPreviewerFromTextBox(previewer);
        else
            FocusFirst(previewer);

        return true;
    }

    public static bool TryHandlePreviewerTab(Control previewer, TextBox? textBox, KeyEventArgs e)
    {
        if (e.Key != Key.Tab || textBox is null)
            return false;

        var focusables = GetTabFocusables(previewer);
        if (focusables.Count == 0)
            return false;

        var index = GetFocusableIndex(focusables, e.Source);
        if (index < 0)
            return false;

        var shift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);

        if ((!shift && index == focusables.Count - 1) || (shift && index == 0))
        {
            textBox.Focus(NavigationMethod.Tab);
            return true;
        }

        return false;
    }

    public static void FocusFirst(Control previewer) =>
        GetTabFocusables(previewer).FirstOrDefault()?.Focus(NavigationMethod.Tab);

    public static void FocusLast(Control previewer) =>
        GetTabFocusables(previewer).LastOrDefault()?.Focus(NavigationMethod.Tab);

    internal static IReadOnlyList<Control> GetTabFocusables(Control root) =>
        [.. root.GetVisualDescendants()
            .OfType<Control>()
            .Where(static c => c is { Focusable: true, IsTabStop: true, IsEffectivelyEnabled: true, IsVisible: true })
            .OrderBy(static c => KeyboardNavigation.GetTabIndex(c))
            .ThenBy(static c => c, VisualTreeOrderComparer.Instance)];

    internal static int GetFocusableIndex(IReadOnlyList<Control> focusables, object? source)
    {
        if (source is not Visual visual)
            return -1;

        for (var i = focusables.Count - 1; i >= 0; i--)
        {
            var candidate = focusables[i];
            if (ReferenceEquals(candidate, visual) || candidate.IsVisualAncestorOf(visual))
                return i;
        }

        return -1;
    }

    private sealed class VisualTreeOrderComparer : IComparer<Control>
    {
        internal static VisualTreeOrderComparer Instance { get; } = new();

        public int Compare(Control? x, Control? y) => ReferenceEquals(x, y) ? 0 : x is null ? -1 : y is null ? 1 : x.IsVisualAncestorOf(y) ? -1 : y.IsVisualAncestorOf(x) ? 1 : 0;
    }
}
