// -----------------------------------------------------------------------
// <copyright file="WindowDialogPresenter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs.Internal;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Locators.Factories;

namespace MyNet.Avalonia.Extended.Dialogs.Presentation;

/// <summary>
/// Presents dialogs inside a <see cref="WindowDialog"/>.
/// </summary>
public sealed class WindowDialogPresenter(
    DialogHostOptions hostOptions,
    IViewFactory viewFactory,
    DialogSessionRegistry sessions) : IDialogPresenter
{
    /// <inheritdoc />
    public int Priority => 110;

    /// <inheritdoc />
    public bool CanPresent(IDialog dialog, UI.Dialogs.ContentDialogs.DialogOptions? options) => DialogOptions.Resolve(options).Mode == DialogPresentationMode.Window;

    /// <inheritdoc />
    public async Task<DialogResult<bool>> PresentAsync(
        IDialog dialog,
        UI.Dialogs.ContentDialogs.DialogOptions options,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dialog);
        ArgumentNullException.ThrowIfNull(options);

        var request = DialogOptions.Resolve(options);
        var owner = request.WindowOwner ?? ResolveOwnerWindow();

        var view = dialog is MessageBoxViewModel
            ? dialog
            : viewFactory.CreateView(dialog.GetType());

        Window window = dialog is MessageBoxViewModel messageBoxVm
            ? WindowDialogBuilder.CreateMessageBox(messageBoxVm, options)
            : WindowDialogBuilder.Create(dialog, view, options);

        var windowDialog = window as WindowDialog;
        var session = sessions.Register(
            dialog,
            new(() => sessions.Remove(dialog)) { Window = windowDialog });

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (owner is null || !options.IsModal)
            {
                return await PresentWindowAwaitingCloseAsync(
                        window,
                        windowDialog,
                        dialog,
                        owner,
                        session,
                        cancellationToken)
                    .ConfigureAwait(true);
            }

            if (owner.Icon is not null)
                window.Icon = owner.Icon;

            if (window is WindowMessageBox)
            {
                var messageBoxResult = await window.ShowDialog<MessageBoxResult>(owner).ConfigureAwait(true);
                if (dialog is MessageBoxViewModel messageBox)
                    DialogResultMapper.ApplyMessageBoxResult(messageBox, messageBoxResult);

                return DialogResultMapper.Map(messageBoxResult);
            }

            var result = await window.ShowDialog<bool?>(owner).ConfigureAwait(true);
            return DialogResultMapper.Map(result);
        }
        finally
        {
            session.Dispose();
        }
    }

    /// <inheritdoc />
    public Task CloseAsync(IDialog dialog)
    {
        if (sessions.TryGet(dialog, out var session))
            session.CloseVisual();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Shows a window without <see cref="Window.ShowDialog"/> and completes when it closes.
    /// Used for non-modal presentation and when no owner window is available.
    /// </summary>
    private static async Task<DialogResult<bool>> PresentWindowAwaitingCloseAsync(
        Window window,
        WindowDialog? windowDialog,
        IDialog dialog,
        Window? owner,
        DialogSession session,
        CancellationToken cancellationToken)
    {
        var completion = new TaskCompletionSource<DialogResult<bool>>(TaskCreationOptions.RunContinuationsAsynchronously);
        await using var registration = cancellationToken.Register(() => Dispatcher.UIThread.Post(() =>
        {
            if (completion.Task.IsCompleted)
                return;

            session.CloseVisual();
            completion.TrySetResult(DialogResult.Dismiss());
        })).ConfigureAwait(false);

        window.Closed += onClosed;

        if (owner is not null)
        {
            if (owner.Icon is not null)
                window.Icon = owner.Icon;

            window.Show(owner);
        }
        else
        {
            window.Show();
        }

        return await completion.Task.ConfigureAwait(true);

        void onClosed(object? sender, EventArgs e)
        {
            window.Closed -= onClosed;
            var result = windowDialog?.LastCloseResult;
            if (dialog is MessageBoxViewModel messageBox)
                DialogResultMapper.ApplyMessageBoxResult(messageBox, result);

            completion.TrySetResult(DialogResultMapper.Map(result));
        }
    }

    private Window? ResolveOwnerWindow()
    {
        var topLevel = hostOptions.TopLevelProvider();
        if (topLevel is Window window)
            return window;

        var lifetime = Application.Current?.ApplicationLifetime;
        return lifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } mainWindow }
            ? mainWindow
            : null;
    }
}
