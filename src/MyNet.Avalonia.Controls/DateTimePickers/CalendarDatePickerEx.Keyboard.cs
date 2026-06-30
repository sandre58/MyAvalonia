// -----------------------------------------------------------------------
// <copyright file="CalendarDatePickerEx.Keyboard.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Input;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public partial class CalendarDatePickerEx
{
    protected override bool ProcessKey(KeyEventArgs e)
    {
        if (IsDropDownOpen && Previewer is Calendar calendar && TextBox is { } textBox)
        {
            if (ReferenceEquals(e.Source, textBox) && CalendarFocusHelper.TryHandleTextBoxTab(calendar, textBox, e))
                return true;

            if (CalendarFocusHelper.TryHandlePreviewerTab(calendar, TextBox, e))
                return true;
        }

        return base.ProcessKey(e);
    }

    protected override void OnPreviewerKeyDown(object? sender, KeyEventArgs e)
    {
        if (IsDropDownOpen && Previewer is Calendar calendar && CalendarFocusHelper.TryHandlePreviewerTab(calendar, TextBox, e))
        {
            e.Handled = true;
            return;
        }

        base.OnPreviewerKeyDown(sender, e);
    }

    protected override void TryFocusPopupContent()
    {
        if (Previewer is { } calendar)
        {
            calendar.FocusSelectedDay();
            return;
        }

        base.TryFocusPopupContent();
    }

    protected override void FocusPreviewerOnTabFromTextBox(Control previewer)
    {
        if (previewer is Calendar calendar)
            calendar.FocusSelectedDay();
        else
            base.FocusPreviewerOnTabFromTextBox(previewer);
    }
}
