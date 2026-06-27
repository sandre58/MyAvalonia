// -----------------------------------------------------------------------
// <copyright file="DateTimePickerExPopupFocusHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Internals;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class DateTimePickerExPopupFocusHelper
{
    public static bool TryHandleTextBoxTab(DateTimeView previewer, TextBox textBox, KeyEventArgs e)
    {
        if (e.Key != Key.Tab || !ReferenceEquals(e.Source, textBox))
            return false;

        if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            FocusLastElement(previewer);
        else
            previewer.FocusSection(DateTimeViewSection.Calendar);

        return true;
    }

    public static bool TryHandlePreviewerTab(DateTimeView previewer, TextBox? textBox, KeyEventArgs e)
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

    public static void FocusLastElement(DateTimeView previewer)
    {
        var last = GetTabFocusables(previewer).LastOrDefault();
        last?.Focus(NavigationMethod.Tab);
    }

    private static List<Control> GetTabFocusables(Control root) =>
        [.. root.GetVisualDescendants()
            .OfType<Control>()
            .Where(static c => c is { Focusable: true, IsEffectivelyEnabled: true, IsVisible: true })
            .OrderBy(static c => KeyboardNavigation.GetTabIndex(c))
            .ThenBy(static c => c, VisualTreeOrderComparer.Instance)];

    private static int GetFocusableIndex(List<Control> focusables, object? source)
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
