// -----------------------------------------------------------------------
// <copyright file="AvaloniaOverlayDialogBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs.Internal;
using MyNet.UI;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

internal static class AvaloniaOverlayDialogBuilder
{
    private static readonly OverlayDialogOptions DefaultOptions = new();

    public static OverlayDialog Create(
        IDialog dialog,
        object view,
        UI.Dialogs.ContentDialogs.DialogOptions options,
        DialogHostRequest request)
    {
        if (dialog is MessageBoxViewModel messageBox)
            return CreateMessageBox(messageBox, options, request.OverlayOptions);

        var overlay = new AvaloniaContentOverlayDialog();
        var overlayOptions = MergeOptions(GetOptions(view), request.OverlayOptions);
        PrepareOverlayDialog(overlay, overlayOptions, options);

        overlay.Content = view;
        overlay.DataContext = dialog;

        WireCloseRequested(dialog, overlay, result => overlay.CloseWithResult(result));
        return overlay;
    }

    public static OverlayDialog CreateMessageBox(
        MessageBoxViewModel messageBox,
        UI.Dialogs.ContentDialogs.DialogOptions options,
        OverlayDialogOptions? overlayOptions)
    {
        var messageBoxControl = new OverlayMessageBox
        {
            Content = messageBox.Message,
            Title = options.Title ?? messageBox.Title,
            Buttons = messageBox.Buttons,
            Severity = messageBox.Severity,
            DataContext = messageBox,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };

        ApplyMessageBoxOptions(messageBoxControl, overlayOptions);
        WireCloseRequested(messageBox, messageBoxControl, _ => messageBoxControl.Close());
        messageBoxControl.Closed += (_, args) => AvaloniaDialogResultMapper.ApplyMessageBoxResult(messageBox, args.Result);
        return messageBoxControl;
    }

    public static void PrepareOverlayDialog(
        OverlayDialog control,
        OverlayDialogOptions options,
        UI.Dialogs.ContentDialogs.DialogOptions dialogOptions)
    {
        control.IsFullScreen = options.FullScreen;
        if (options.FullScreen)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;
            control.VerticalAlignment = VerticalAlignment.Stretch;
        }

        control.HorizontalAnchor = options.HorizontalAnchor;
        control.VerticalAnchor = options.VerticalAnchor;
        control.ActualHorizontalAnchor = options.HorizontalAnchor;
        control.ActualVerticalAnchor = options.VerticalAnchor;
        control.HorizontalOffset = control.HorizontalAnchor == HorizontalPosition.Center ? null : options.HorizontalOffset;
        control.VerticalOffset = options.VerticalAnchor == VerticalPosition.Center ? null : options.VerticalOffset;
        control.IsCloseButtonVisible = options.IsCloseButtonVisible ?? true;
        control.CanLightDismiss = options.CanLightDismiss || dialogOptions.CloseOnOverlayClick;
        control.CanResize = options.CanResize;

        if (options.Width.HasValue) control.Width = options.Width.Value;
        if (options.Height.HasValue) control.Height = options.Height.Value;
        if (options.MinWidth.HasValue) control.MinWidth = options.MinWidth.Value;
        if (options.MinHeight.HasValue) control.MinHeight = options.MinHeight.Value;
        if (options.MaxWidth.HasValue) control.MaxWidth = options.MaxWidth.Value;
        if (options.MaxHeight.HasValue) control.MaxHeight = options.MaxHeight.Value;

        if (!string.IsNullOrWhiteSpace(options.StyleClass))
        {
            var styles = options.StyleClass.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            control.Classes.AddRange(styles);
        }
    }

    private static void WireCloseRequested(IDialog dialog, OverlayDialog overlay, Action<object?> close)
    {
        async void OnCloseRequested(object? sender, CloseRequestedEventArgs e)
        {
            if (!await dialog.CanCloseAsync().ConfigureAwait(true))
                return;

            close(e.Force ? true : null);
        }

        dialog.CloseRequested += OnCloseRequested;
        overlay.Closed += (_, _) => dialog.CloseRequested -= OnCloseRequested;
    }

    private static OverlayDialogOptions MergeOptions(OverlayDialogOptions baseOptions, OverlayDialogOptions? overrideOptions)
    {
        if (overrideOptions is null) return baseOptions;

        return new()
        {
            FullScreen = overrideOptions.FullScreen,
            HorizontalAnchor = overrideOptions.HorizontalAnchor,
            VerticalAnchor = overrideOptions.VerticalAnchor,
            HorizontalOffset = overrideOptions.HorizontalOffset ?? baseOptions.HorizontalOffset,
            VerticalOffset = overrideOptions.VerticalOffset ?? baseOptions.VerticalOffset,
            Width = overrideOptions.Width ?? baseOptions.Width,
            Height = overrideOptions.Height ?? baseOptions.Height,
            MinWidth = overrideOptions.MinWidth ?? baseOptions.MinWidth,
            MinHeight = overrideOptions.MinHeight ?? baseOptions.MinHeight,
            MaxWidth = overrideOptions.MaxWidth ?? baseOptions.MaxWidth,
            MaxHeight = overrideOptions.MaxHeight ?? baseOptions.MaxHeight,
            Severity = overrideOptions.Severity,
            Buttons = overrideOptions.Buttons,
            Title = overrideOptions.Title ?? baseOptions.Title,
            IsCloseButtonVisible = overrideOptions.IsCloseButtonVisible ?? baseOptions.IsCloseButtonVisible,
            CanLightDismiss = overrideOptions.CanLightDismiss,
            TopLevelHashCode = overrideOptions.TopLevelHashCode ?? baseOptions.TopLevelHashCode,
            CanResize = overrideOptions.CanResize || baseOptions.CanResize,
            StyleClass = overrideOptions.StyleClass ?? baseOptions.StyleClass
        };
    }

    private static OverlayDialogOptions GetOptions(object view) => view is not ContentDialog contentDialog
        ? DefaultOptions
        : new()
        {
            Title = contentDialog.Header switch
            {
                string str => str,
                null => null,
                var header => header.ToString()
            },
            IsCloseButtonVisible = contentDialog.ShowCloseButton,
            CanResize = contentDialog.CanResize
        };

    private static void ApplyMessageBoxOptions(OverlayMessageBox messageBox, OverlayDialogOptions? options)
    {
        options ??= OverlayDialogOptions.Default;
        messageBox.CanLightDismiss = options.CanLightDismiss;
        messageBox.CanResize = options.CanResize;

        if (options.Width.HasValue) messageBox.Width = options.Width.Value;
        if (options.Height.HasValue) messageBox.Height = options.Height.Value;
        if (options.MinWidth.HasValue) messageBox.MinWidth = options.MinWidth.Value;
        if (options.MinHeight.HasValue) messageBox.MinHeight = options.MinHeight.Value;
        if (options.MaxWidth.HasValue) messageBox.MaxWidth = options.MaxWidth.Value;
        if (options.MaxHeight.HasValue) messageBox.MaxHeight = options.MaxHeight.Value;
    }
}
