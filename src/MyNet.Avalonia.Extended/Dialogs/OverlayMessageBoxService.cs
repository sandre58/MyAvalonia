// -----------------------------------------------------------------------
// <copyright file="OverlayMessageBoxService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;
using Avalonia.Input;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Dialogs;

/// <summary>
/// Service for showing message boxes as overlay dialogs.
/// </summary>
public class OverlayMessageBoxService
{
    /// <summary>
    /// Shows a message box as a modal overlay dialog and returns the user's choice.
    /// </summary>
    public Task<MessageBoxResult> ShowAsync(
        string message,
        string? title = null,
        MessageBoxResultOption buttons = MessageBoxResultOption.Ok,
        MessageSeverity severity = MessageSeverity.Information,
        string? hostId = null,
        OverlayDialogOptions? options = null,
        CancellationToken? token = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(MessageBoxResult.None);

        var messageBox = new OverlayMessageBox
        {
            Content = message,
            Title = title,
            Buttons = buttons,
            Severity = severity,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };

        ApplyOptions(messageBox, options);
        host.AddModalDialog(messageBox);
        return messageBox.ShowAsync<MessageBoxResult>(token);
    }

    /// <summary>
    /// Shows a message box with custom content as a modal overlay dialog.
    /// </summary>
    public Task<MessageBoxResult> ShowAsync(
        object content,
        string? title = null,
        MessageBoxResultOption buttons = MessageBoxResultOption.Ok,
        MessageSeverity severity = MessageSeverity.Information,
        string? hostId = null,
        OverlayDialogOptions? options = null,
        CancellationToken? token = null)
    {
        var host = OverlayDialogHostManager.GetHost(hostId, options?.TopLevelHashCode);
        if (host is null) return Task.FromResult(MessageBoxResult.None);

        var messageBox = new OverlayMessageBox
        {
            Content = content,
            Title = title,
            Buttons = buttons,
            Severity = severity,
            [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle
        };

        ApplyOptions(messageBox, options);
        host.AddModalDialog(messageBox);
        return messageBox.ShowAsync<MessageBoxResult>(token);
    }

    private static void ApplyOptions(OverlayMessageBox messageBox, OverlayDialogOptions? options)
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
