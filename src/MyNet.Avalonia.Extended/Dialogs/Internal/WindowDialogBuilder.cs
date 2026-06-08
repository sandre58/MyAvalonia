// -----------------------------------------------------------------------
// <copyright file="WindowDialogBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Theme.Controls.Assists;
using MyNet.UI;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Dialogs.Internal;

/// <summary>
/// Provides methods to create and configure window dialogs based on the specified dialog view models and options.
/// </summary>
internal static class WindowDialogBuilder
{
    /// <summary>
    /// Creates a new instance of a window dialog based on the specified dialog view model, content view, and dialog options.
    /// </summary>
    /// <param name="dialog">The dialog view model.</param>
    /// <param name="view">The content view for the dialog.</param>
    /// <param name="options">The dialog options.</param>
    /// <returns>A configured <see cref="WindowDialog"/> instance.</returns>
    public static WindowDialog Create(IDialog dialog, object view, DialogOptions options)
    {
        var request = DialogOptionsFactory.Resolve(options);
        var window = new WindowDialog { Content = view, DataContext = dialog };

        var windowOptions = MergeWindowOptions(GetWindowOptions(view as ContentDialog), request.WindowOptions);
        if (view is ContentDialog contentDialog)
            contentDialog.ShowHeader = false;

        ApplyWindowOptions(window, windowOptions, dialog, options);

        WireWindowLifetime(dialog, window);
        return window;
    }

    /// <summary>
    /// Creates a new instance of a window message box based on the specified message box view model and dialog options.
    /// </summary>
    /// <param name="messageBox">The message box view model.</param>
    /// <param name="options">The dialog options.</param>
    /// <returns>A configured <see cref="WindowMessageBox"/> instance.</returns>
    public static WindowMessageBox CreateMessageBox(MessageBoxViewModel messageBox, DialogOptions options)
    {
        var request = DialogOptionsFactory.Resolve(options);
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

    /// <summary>
    /// Merges the base window options with the override options, giving precedence to the override options when specified.
    /// </summary>
    /// <param name="baseOptions">The base window options.</param>
    /// <param name="overrideOptions">The override window options.</param>
    /// <returns>A new <see cref="WindowDialogOptions"/> instance with the merged options.</returns>
    internal static WindowDialogOptions MergeWindowOptions(WindowDialogOptions baseOptions, WindowDialogOptions? overrideOptions)
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

    /// <summary>
    /// Extracts window options from the given content dialog, if available, to configure the appearance and behavior of the window dialog.
    /// </summary>
    /// <param name="content">The content dialog from which to extract window options.</param>
    /// <returns>A <see cref="WindowDialogOptions"/> instance with the extracted options, or the default options if the content dialog is null.</returns>
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

    /// <summary>
    /// Applies the specified window options to the given window dialog, configuring its properties such as title, size, resize behavior, and startup location based on the provided options and dialog information.
    /// </summary>
    /// <param name="window">The window dialog to which the options will be applied.</param>
    /// <param name="windowOptions">The window options to apply.</param>
    /// <param name="dialog">The dialog associated with the window.</param>
    /// <param name="options">The dialog options.</param>
    private static void ApplyWindowOptions(
        Window window,
        WindowDialogOptions windowOptions,
        IDialog dialog,
        DialogOptions options)
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
