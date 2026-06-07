// -----------------------------------------------------------------------
// <copyright file="WindowDialogBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Theme.Controls.Assists;
using MyNet.UI;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

internal static class WindowDialogBuilder
{
    public static WindowDialog Create(
        IDialog dialog,
        object view,
        UI.Dialogs.ContentDialogs.DialogOptions options)
    {
        var request = DialogOptions.Resolve(options);
        var window = new WindowDialog { Content = view, DataContext = dialog };

        var windowOptions = MergeWindowOptions(GetWindowOptions(view as ContentDialog), request.WindowOptions);
        if (view is ContentDialog contentDialog)
            contentDialog.ShowHeader = false;

        ApplyWindowOptions(window, windowOptions, dialog, options);

        WireWindowLifetime(dialog, window);
        return window;
    }

    public static WindowMessageBox CreateMessageBox(MessageBoxViewModel messageBox, UI.Dialogs.ContentDialogs.DialogOptions options)
    {
        var request = DialogOptions.Resolve(options);
        var windowOptions = request.WindowOptions ?? WindowDialogOptions.Default;

        var window = new WindowMessageBox(messageBox.Buttons)
        {
            Title = options.Title ?? messageBox.Title ?? string.Empty,
            Severity = messageBox.Severity,
            Message = messageBox.Message,
            DataContext = messageBox
        };

        ApplyWindowOptions(window, windowOptions, messageBox, options);
        WireWindowLifetime(messageBox, window);
        return window;
    }

    internal static WindowDialogOptions MergeWindowOptions(
        WindowDialogOptions baseOptions,
        WindowDialogOptions? overrideOptions)
        => overrideOptions is null
            ? baseOptions
            : new()
            {
                CanDragMove = overrideOptions.CanDragMove,
                CanResize = overrideOptions.CanResize || baseOptions.CanResize,
                ShowInTaskbar = overrideOptions.ShowInTaskbar,
                StartupLocation = overrideOptions.StartupLocation != WindowStartupLocation.CenterOwner
                    ? overrideOptions.StartupLocation
                    : baseOptions.StartupLocation,
                Position = overrideOptions.Position ?? baseOptions.Position,
                Width = overrideOptions.Width ?? baseOptions.Width,
                Height = overrideOptions.Height ?? baseOptions.Height,
                MinWidth = overrideOptions.MinWidth ?? baseOptions.MinWidth,
                MinHeight = overrideOptions.MinHeight ?? baseOptions.MinHeight,
                StyleClass = overrideOptions.StyleClass ?? baseOptions.StyleClass,
                Title = overrideOptions.Title ?? baseOptions.Title
            };

    private static WindowDialogOptions GetWindowOptions(ContentDialog? content) => content is null
        ? WindowDialogOptions.Default
        : new()
        {
            Title = content.Header switch
            {
                string str => str,
                null => null,
                var header => header.ToString()
            },
            CanResize = content.CanResize,
            ShowInTaskbar = content.ShowInTaskBar,
            StartupLocation = content.StartupLocation,
            Position = content.Position,
            StyleClass = content.ParentClasses
        };

    private static void ApplyWindowOptions(
        Window window,
        WindowDialogOptions windowOptions,
        IDialog dialog,
        UI.Dialogs.ContentDialogs.DialogOptions options)
    {
        window.WindowState = WindowState.Normal;

        var title = options.Title ?? windowOptions.Title ?? dialog.Title;
        if (!string.IsNullOrEmpty(title))
            window.Title = title;

        window.CanResize = windowOptions.CanResize;
        window.SizeToContent = window is WindowMessageBox ? SizeToContent.Height : SizeToContent.WidthAndHeight;
        window.ShowInTaskbar = windowOptions.ShowInTaskbar;
        window.WindowStartupLocation = windowOptions.StartupLocation;

        WindowAssist.SetShowMinimizeButton(window, windowOptions.CanResize);
        WindowAssist.SetShowMaximizeButton(window, windowOptions.CanResize);

        if (windowOptions.StartupLocation == WindowStartupLocation.Manual)
        {
            if (windowOptions.Position is not null)
                window.Position = windowOptions.Position.Value;
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        if (windowOptions.Width.HasValue)
            window.Width = windowOptions.Width.Value;

        if (windowOptions.Height.HasValue)
            window.Height = windowOptions.Height.Value;

        if (windowOptions.MinWidth.HasValue)
            window.MinWidth = windowOptions.MinWidth.Value;

        if (windowOptions.MinHeight.HasValue)
            window.MinHeight = windowOptions.MinHeight.Value;

        if (!string.IsNullOrWhiteSpace(windowOptions.StyleClass))
            window.AddClasses(windowOptions.StyleClass.Split(' ', StringSplitOptions.RemoveEmptyEntries));

        if (windowOptions.CanResize)
        {
            window.Opened += onOpened;

            void onOpened(object? sender, EventArgs e)
            {
                window.Opened -= onOpened;
                var width = window.Width;
                var height = window.Height;
                window.SizeToContent = SizeToContent.Manual;
                if (width > 0)
                    window.Width = width;
                if (height > 0)
                    window.Height = height;
            }
        }
    }

    private static void WireWindowLifetime(IDialog dialog, Window window)
    {
        dialog.CloseRequested += onCloseRequested;

        window.Closing += onWindowClosingAsync;
        window.Closed += onWindowClosed;
        return;

        async void onCloseRequested(object? sender, CloseRequestedEventArgs e)
        {
            if (!await dialog.CanCloseAsync().ConfigureAwait(true))
                return;

            if (window is WindowDialog windowDialog)
                windowDialog.CloseWithResult(e.Force ? true : null);
            else
                window.Close(e.Force ? true : null);
        }

        async void onWindowClosingAsync(object? sender, WindowClosingEventArgs e)
            => e.Cancel = !await dialog.CanCloseAsync().ConfigureAwait(true);

        void onWindowClosed(object? sender, EventArgs e)
        {
            dialog.CloseRequested -= onCloseRequested;
            window.Closing -= onWindowClosingAsync;
            window.Closed -= onWindowClosed;
        }
    }
}
