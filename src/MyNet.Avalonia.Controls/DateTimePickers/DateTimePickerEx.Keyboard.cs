// -----------------------------------------------------------------------
// <copyright file="DateTimePickerEx.Keyboard.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Internals;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public partial class DateTimePickerEx
{
    protected override void TryFocusPopupContent()
    {
        if (Previewer is { } dateTimeView)
        {
            dateTimeView.FocusSection(DateTimeViewSection.Calendar);
            return;
        }

        base.TryFocusPopupContent();
    }

    protected override bool ProcessKey(KeyEventArgs e)
    {
        if (IsDropDownOpen && Previewer is { } dateTimeView)
        {
            if (ReferenceEquals(e.Source, TextBox) && DateTimePickerExPopupFocusHelper.TryHandleTextBoxTab(dateTimeView, TextBox!, e))
                return true;

            if (DateTimePickerExPopupFocusHelper.TryHandlePreviewerTab(dateTimeView, TextBox, e))
                return true;
        }

        return base.ProcessKey(e);
    }

    public override void Rollback()
    {
        base.Rollback();

        if (!IsDropDownOpen)
            return;

        ClosePopup();
        TextBox?.Focus();
    }

    protected override void AddPreviewerHandlers() => Previewer?.OnLoading<DateTimeView>(AttachPreviewerHandlers, DetachPreviewerHandlers);

    protected override void RemovePreviewerHandlers()
    {
        if (Previewer is { } view)
            DetachPreviewerHandlers(view);
    }

    private void AttachPreviewerHandlers(DateTimeView view)
    {
        view.SelectedValueChanged += OnDateTimeChanged;
        view.AddHandler(KeyDownEvent, OnPreviewerKeyDown, RoutingStrategies.Tunnel, handledEventsToo: true);
    }

    private void DetachPreviewerHandlers(DateTimeView view)
    {
        view.SelectedValueChanged -= OnDateTimeChanged;
        view.RemoveHandler(KeyDownEvent, OnPreviewerKeyDown);
    }

    private void OnPreviewerKeyDown(object? sender, KeyEventArgs e)
    {
        if (!IsDropDownOpen || Previewer is not { } dateTimeView)
            return;

        if (DateTimePickerExPopupFocusHelper.TryHandlePreviewerTab(dateTimeView, TextBox, e))
            e.Handled = true;
    }
}
