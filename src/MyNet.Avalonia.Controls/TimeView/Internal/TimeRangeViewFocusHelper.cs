// -----------------------------------------------------------------------
// <copyright file="TimeRangeViewFocusHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Primitives.Internal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class TimeRangeViewFocusHelper
{
    public static bool TryHandleTextBoxTab(TimeRangeView view, TextBox textBox, KeyEventArgs e)
    {
        if (e.Key != Key.Tab || !ReferenceEquals(e.Source, textBox))
            return false;

        if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            FocusLastSpinner(view);
        else
            view.FocusStartHour();

        return true;
    }

    public static bool TryHandlePreviewerTab(TimeRangeView view, TextBox? textBox, KeyEventArgs e)
    {
        if (e.Key != Key.Tab || textBox is null)
            return false;

        var shift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);

        if (view.IsSourceInStartSection(e.Source) && view.StartTimeViewPart is { } startView)
            return TryHandleBoundaryTab(view, startView, textBox, e, shift, isStart: true);

        if (view.IsSourceInEndSection(e.Source) && view.EndTimeViewPart is { } endView)
            return TryHandleBoundaryTab(view, endView, textBox, e, shift, isStart: false);

        return false;
    }

    private static bool TryHandleBoundaryTab(
        TimeRangeView view,
        TimeView timeView,
        TextBox textBox,
        KeyEventArgs e,
        bool shift,
        bool isStart)
    {
        var spinners = TextPickerPopupFocusHelper.GetTabFocusables(timeView);
        if (spinners.Count == 0)
            return false;

        var index = TextPickerPopupFocusHelper.GetFocusableIndex(spinners, e.Source);
        if (index < 0)
            return false;

        if (!shift && index == spinners.Count - 1)
        {
            if (isStart)
            {
                view.SwitchToEnd(autoAdvance: false);
                return true;
            }

            textBox.Focus(NavigationMethod.Tab);
            return true;
        }

        if (shift && index == 0)
        {
            if (isStart)
            {
                textBox.Focus(NavigationMethod.Tab);
                return true;
            }

            view.SwitchBoundary(TimeRangeBoundary.Start);
            if (view.StartTimeViewPart is { } startView)
                TextPickerPopupFocusHelper.FocusLast(startView);

            return true;
        }

        return false;
    }

    private static void FocusLastSpinner(TimeRangeView view)
    {
        view.SwitchBoundary(TimeRangeBoundary.End);
        if (view.EndTimeViewPart is { } endView)
            TextPickerPopupFocusHelper.FocusLast(endView);
    }
}
