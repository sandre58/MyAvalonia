// -----------------------------------------------------------------------
// <copyright file="ColorViewFocusHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace MyNet.Avalonia.Controls.Primitives.Internal;

internal static class ColorViewFocusHelper
{
    public static void FocusDefaultContent(ColorView view)
    {
        if (TryFocusSpectrum(view) || TryFocusPalette(view) || TryFocusSelectedTabContent(view))
            return;

        TextPickerPopupFocusHelper.FocusFirst(view);
    }

    private static bool TryFocusSpectrum(ColorView view)
    {
        var spectrum = view.GetVisualDescendants()
            .OfType<ColorSpectrum>()
            .FirstOrDefault(static c => c.IsVisible && c.IsEffectivelyEnabled);

        return spectrum is { Focusable: true } && spectrum.Focus();
    }

    private static bool TryFocusPalette(ColorView view)
    {
        var palette = view.GetVisualDescendants()
            .OfType<ListBox>()
            .FirstOrDefault(static c => c.IsVisible && c.IsEffectivelyEnabled && c.ItemCount > 0);

        return palette is { Focusable: true } && palette.Focus();
    }

    private static bool TryFocusSelectedTabContent(ColorView view)
    {
        var tabControl = view.GetVisualDescendants()
            .OfType<TabControl>()
            .FirstOrDefault(static c => c.IsVisible);

        if (tabControl?.SelectedContent is not Control content)
            return false;

        TextPickerPopupFocusHelper.FocusFirst(content);
        return true;
    }
}
