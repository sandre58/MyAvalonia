// -----------------------------------------------------------------------
// <copyright file="OverlayDialogBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Threading;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

/// <summary>
/// Builder class responsible for creating and configuring overlay dialogs based on the provided dialog view models and options.
/// </summary>
internal static class OverlayDialogBuilder
{
    private static readonly OverlayDialogOptions DefaultOptions = new();

    /// <summary>
    /// Creates an instance of <see cref="OverlayDialog"/> based on the provided dialog, view, options, and request.
    /// </summary>
    /// <param name="dialog">The dialog view model.</param>
    /// <param name="view">The content view for the dialog.</param>
    /// <param name="options">The dialog options.</param>
    /// <param name="request">The dialog host request containing overlay options.</param>
    /// <returns>A configured <see cref="OverlayDialog"/> instance.</returns>
    public static OverlayDialog Create(IDialog dialog, object view, DialogOptions options, DialogHostRequest request)
    {
        if (dialog is MessageBoxViewModel messageBox)
            return CreateMessageBox(messageBox, options, request.OverlayOptions);

        var overlay = new ContentOverlayDialog();
        var overlayOptions = MergeOptions(GetOptions(view), request.OverlayOptions);
        PrepareOverlayDialog(overlay, overlayOptions, options);

        if (view is ContentDialog contentDialog)
            contentDialog.ShowHeader = false;

        overlay.Content = view;
        overlay.DataContext = dialog;

        WireCloseRequested(dialog, overlay, overlay.CloseWithResult);
        return overlay;
    }

    /// <summary>
    /// Creates an instance of <see cref="OverlayMessageBox"/> based on the provided message box view model, options, and overlay options.
    /// </summary>
    /// <param name="messageBox">The message box view model.</param>
    /// <param name="options">The dialog options.</param>
    /// <param name="overlayOptions">The overlay dialog options.</param>
    /// <returns>A configured <see cref="OverlayMessageBox"/> instance.</returns>
    public static OverlayDialog CreateMessageBox(MessageBoxViewModel messageBox, DialogOptions options, OverlayDialogOptions? overlayOptions)
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

        var mergedOverlayOptions = MergeOptions(OverlayDialogOptions.Default, overlayOptions);
        ApplyMessageBoxOptions(messageBoxControl, mergedOverlayOptions, options);
        WireCloseRequested(messageBox, messageBoxControl, _ => messageBoxControl.Close());
        messageBoxControl.Closed += (_, args) => DialogResultMapper.ApplyMessageBoxResult(messageBox, args.Result);
        return messageBoxControl;
    }

    /// <summary>
    /// Configures the properties of the provided <see cref="OverlayDialog"/> control based on the specified options and dialog options.
    /// </summary>
    /// <param name="control">The overlay dialog control to configure.</param>
    /// <param name="options">The overlay dialog options.</param>
    /// <param name="dialogOptions">The dialog options.</param>
    public static void PrepareOverlayDialog(OverlayDialog control, OverlayDialogOptions options, DialogOptions dialogOptions)
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
        OverlayDialog.SetCanDragMove(control, options.CanDragMove);
        control.Title = options.Title ?? dialogOptions.Title;

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
        dialog.CloseRequested += onCloseRequested;
        overlay.Closed += (_, _) => dialog.CloseRequested -= onCloseRequested;
        return;

        void onCloseRequested(object? sender, CloseRequestedEventArgs e) => Dispatcher.UIThread.Post(() => _ = handleCloseRequestedAsync(dialog, close, e));

        static async Task handleCloseRequestedAsync(IDialog target, Action<object?> closeAction, CloseRequestedEventArgs args)
        {
            if (!await target.CanCloseAsync().ConfigureAwait(true))
                return;

            closeAction(args.Force ? true : null);
        }
    }

    internal static OverlayDialogOptions MergeOptions(OverlayDialogOptions baseOptions, OverlayDialogOptions? overrideOptions) => overrideOptions is null
        ? baseOptions
        : new()
        {
            FullScreen = overrideOptions.FullScreen || baseOptions.FullScreen,
            HorizontalAnchor = overrideOptions.HorizontalAnchor != HorizontalPosition.Center
                ? overrideOptions.HorizontalAnchor
                : baseOptions.HorizontalAnchor,
            VerticalAnchor = overrideOptions.VerticalAnchor != VerticalPosition.Center
                ? overrideOptions.VerticalAnchor
                : baseOptions.VerticalAnchor,
            HorizontalOffset = overrideOptions.HorizontalOffset ?? baseOptions.HorizontalOffset,
            VerticalOffset = overrideOptions.VerticalOffset ?? baseOptions.VerticalOffset,
            Width = overrideOptions.Width ?? baseOptions.Width,
            Height = overrideOptions.Height ?? baseOptions.Height,
            MinWidth = overrideOptions.MinWidth ?? baseOptions.MinWidth,
            MinHeight = overrideOptions.MinHeight ?? baseOptions.MinHeight,
            MaxWidth = overrideOptions.MaxWidth ?? baseOptions.MaxWidth,
            MaxHeight = overrideOptions.MaxHeight ?? baseOptions.MaxHeight,
            Severity = overrideOptions.Severity != MessageSeverity.Custom
                ? overrideOptions.Severity
                : baseOptions.Severity,
            Buttons = overrideOptions.Buttons != MessageBoxResultOption.OkCancel
                ? overrideOptions.Buttons
                : baseOptions.Buttons,
            Title = overrideOptions.Title ?? baseOptions.Title,
            IsCloseButtonVisible = overrideOptions.IsCloseButtonVisible ?? baseOptions.IsCloseButtonVisible,
            CanLightDismiss = overrideOptions.CanLightDismiss || baseOptions.CanLightDismiss,
            TopLevelKey = overrideOptions.TopLevelKey ?? baseOptions.TopLevelKey,
            CanDragMove = overrideOptions.CanDragMove,
            StyleClass = overrideOptions.StyleClass ?? baseOptions.StyleClass
        };

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
            IsCloseButtonVisible = contentDialog.ShowCloseButton
        };

    private static void ApplyMessageBoxOptions(OverlayMessageBox messageBox, OverlayDialogOptions options, DialogOptions dialogOptions)
    {
        if (options.Severity != MessageSeverity.Custom)
            messageBox.Severity = options.Severity;

        if (options.Buttons != MessageBoxResultOption.OkCancel)
            messageBox.Buttons = options.Buttons;

        if (!string.IsNullOrWhiteSpace(options.Title))
            messageBox.Title = options.Title;

        PrepareOverlayDialog(messageBox, options, dialogOptions);
    }
}
