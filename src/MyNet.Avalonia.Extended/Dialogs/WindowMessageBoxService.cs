// -----------------------------------------------------------------------
// <copyright file="WindowMessageBoxService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using MyNet.Avalonia.Extended.Controls;
using MyNet.UI.Dialogs.MessageBox;

namespace MyNet.Avalonia.Extended.Dialogs;

/// <summary>
/// Service for showing message boxes as OS Window dialogs.
/// </summary>
public class WindowMessageBoxService
{
    /// <summary>
    /// Shows a message box as a modal window dialog and returns the user's choice.
    /// </summary>
    public async Task<MessageBoxResult> ShowAsync(
        string message,
        string? title = null,
        MessageBoxResultOption buttons = MessageBoxResultOption.Ok,
        MessageSeverity severity = MessageSeverity.Information)
    {
        var messageBox = new WindowMessageBox(buttons)
        {
            Content = message,
            Title = title ?? string.Empty,
            Severity = severity
        };

        var owner = GetMainWindow();
        if (owner is null)
        {
            messageBox.Show();
            return MessageBoxResult.None;
        }

        messageBox.Icon = owner.Icon;
        var result = await messageBox.ShowDialog<MessageBoxResult>(owner).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Shows a message box with custom content as a modal window dialog.
    /// </summary>
    public async Task<MessageBoxResult> ShowAsync(
        object content,
        string? title = null,
        MessageBoxResultOption buttons = MessageBoxResultOption.Ok,
        MessageSeverity severity = MessageSeverity.Information)
    {
        var messageBox = new WindowMessageBox(buttons)
        {
            Content = content,
            Title = title ?? string.Empty,
            Severity = severity
        };

        var owner = GetMainWindow();
        if (owner is null)
        {
            messageBox.Show();
            return MessageBoxResult.None;
        }

        messageBox.Icon = owner.Icon;
        var result = await messageBox.ShowDialog<MessageBoxResult>(owner).ConfigureAwait(false);
        return result;
    }

    private static Window? GetMainWindow()
    {
        var lifetime = Application.Current?.ApplicationLifetime;
        return lifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } w } ? w : null;
    }
}

