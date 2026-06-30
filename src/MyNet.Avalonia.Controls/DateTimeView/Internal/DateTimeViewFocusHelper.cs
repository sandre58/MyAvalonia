// -----------------------------------------------------------------------
// <copyright file="DateTimeViewFocusHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Primitives.Internal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

internal static class DateTimeViewFocusHelper
{
    public static bool TryHandleTextBoxTab(DateTimeView view, TextBox textBox, KeyEventArgs e)
    {
        if (e.Key != Key.Tab || !ReferenceEquals(e.Source, textBox))
            return false;

        if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            FocusLastSection(view);
        else
            view.FocusSection(DateTimeViewSection.Calendar);

        return true;
    }

    public static bool TryHandlePreviewerTab(DateTimeView view, TextBox? textBox, KeyEventArgs e)
    {
        if (e.Key != Key.Tab || textBox is null)
            return false;

        var shift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);

        if (view.IsSourceInCalendarSection(e.Source) && view.CalendarPart is { } calendar)
        {
            var days = CalendarFocusHelper.GetTabFocusables(calendar);
            if (days.Count == 0)
                return false;

            var index = TextPickerPopupFocusHelper.GetFocusableIndex(days, e.Source);
            if (index < 0)
                return false;

            if (!shift && index == days.Count - 1)
            {
                view.FocusSection(DateTimeViewSection.Time);
                return true;
            }

            if (shift && index == 0)
            {
                textBox.Focus(NavigationMethod.Tab);
                return true;
            }

            return false;
        }

        if (view.IsSourceInTimeSection(e.Source) && view.TimeViewPart is { } timeView)
        {
            var spinners = TextPickerPopupFocusHelper.GetTabFocusables(timeView);
            if (spinners.Count == 0)
                return false;

            var index = TextPickerPopupFocusHelper.GetFocusableIndex(spinners, e.Source);
            if (index < 0)
                return false;

            if (!shift && index == spinners.Count - 1)
            {
                textBox.Focus(NavigationMethod.Tab);
                return true;
            }

            if (shift && index == 0)
            {
                view.FocusSection(DateTimeViewSection.Calendar);
                return true;
            }

            return false;
        }

        return false;
    }

    private static void FocusLastSection(DateTimeView view)
    {
        view.FocusSection(DateTimeViewSection.Time);
        if (view.TimeViewPart is { } timeView)
            TextPickerPopupFocusHelper.FocusLast(timeView);
    }
}
