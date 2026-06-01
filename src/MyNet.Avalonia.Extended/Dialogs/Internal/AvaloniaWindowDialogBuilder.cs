// -----------------------------------------------------------------------
// <copyright file="AvaloniaWindowDialogBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MyNet.Avalonia;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

internal static class AvaloniaWindowDialogBuilder
{
    public static WindowDialog Create(
        IDialog dialog,
        object view,
        UI.Dialogs.ContentDialogs.DialogOptions options)
    {
        var window = new WindowDialog();
        var contentDialog = view as ContentDialog;
        PrepareWindow(window, contentDialog, dialog, options);

        window.Content = view;
        window.DataContext = dialog;

        WireWindowLifetime(dialog, window);
        return window;
    }

    public static WindowMessageBox CreateMessageBox(MessageBoxViewModel messageBox, UI.Dialogs.ContentDialogs.DialogOptions options)
    {
        var window = new WindowMessageBox(messageBox.Buttons)
        {
            Content = messageBox.Message,
            Title = options.Title ?? messageBox.Title ?? string.Empty,
            Severity = messageBox.Severity,
            DataContext = messageBox
        };

        WireWindowLifetime(messageBox, window);
        return window;
    }

    private static void PrepareWindow(
        WindowDialog window,
        ContentDialog? content,
        IDialog dialog,
        UI.Dialogs.ContentDialogs.DialogOptions options)
    {
        window.WindowState = WindowState.Normal;

        if (!string.IsNullOrEmpty(options.Title))
            window.Title = options.Title;

        if (content is null) return;

        window.WindowStartupLocation = content.StartupLocation;
        window.Title = options.Title ?? dialog.Title ?? content.Header switch
        {
            string str => str,
            null => null,
            var header => header.ToString()
        };

        window.CanResize = content.CanResize;
        window.ShowInTaskbar = content.ShowInTaskBar;

        if (content.StartupLocation == WindowStartupLocation.Manual)
        {
            if (content.Position is not null)
                window.Position = content.Position.Value;
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        if (!string.IsNullOrWhiteSpace(content.ParentClasses))
            window.AddClasses(content.ParentClasses);
    }

    private static void WireWindowLifetime(IDialog dialog, Window window)
    {
        async void OnCloseRequested(object? sender, CloseRequestedEventArgs e)
        {
            if (!await dialog.CanCloseAsync().ConfigureAwait(true))
                return;

            window.Close(e.Force ? true : null);
        }

        dialog.CloseRequested += OnCloseRequested;

        window.Closing += onWindowClosingAsync;
        window.Closed += onWindowClosed;

        async void onWindowClosingAsync(object? sender, WindowClosingEventArgs e)
        {
            e.Cancel = !await dialog.CanCloseAsync().ConfigureAwait(true);
        }

        void onWindowClosed(object? sender, EventArgs e)
        {
            dialog.CloseRequested -= OnCloseRequested;
            window.Closing -= onWindowClosingAsync;
            window.Closed -= onWindowClosed;
        }
    }
}
