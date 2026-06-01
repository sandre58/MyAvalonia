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
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Dialogs.Internal;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Locators.Factories;

namespace MyNet.Avalonia.Extended.Dialogs.Presentation;

/// <summary>
/// Presents dialogs inside a modal <see cref="WindowDialog"/>.
/// </summary>
public sealed class WindowDialogPresenter(
    DialogHostOptions hostOptions,
    IViewFactory viewFactory,
    AvaloniaDialogSessionRegistry sessions) : IDialogPresenter
{
    /// <inheritdoc />
    public int Priority => 110;

    /// <inheritdoc />
    public bool CanPresent(IDialog dialog, UI.Dialogs.ContentDialogs.DialogOptions? options)
        => DialogOptions.Resolve(options).Mode == DialogPresentationMode.Window;

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
            ? AvaloniaWindowDialogBuilder.CreateMessageBox(messageBoxVm, options)
            : AvaloniaWindowDialogBuilder.Create(dialog, view, options);

        var session = sessions.Register(
            dialog,
            new AvaloniaDialogSession(() => sessions.Remove(dialog)) { Window = window as WindowDialog });

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (owner is null)
            {
                window.Show();
                return DialogResult.Dismiss();
            }

            if (owner.Icon is not null)
                window.Icon = owner.Icon;

            if (window is WindowMessageBox)
            {
                var messageBoxResult = await window.ShowDialog<MessageBoxResult>(owner).ConfigureAwait(true);
                if (dialog is MessageBoxViewModel messageBox)
                    messageBox.ApplyResult(messageBoxResult);

                return DialogResult.Ok();
            }

            var result = await window.ShowDialog<bool?>(owner).ConfigureAwait(true);
            return AvaloniaDialogResultMapper.MapBool(result);
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
