// -----------------------------------------------------------------------
// <copyright file="OverlayDialogPresenter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extended.Dialogs.Internal;
using MyNet.Avalonia.Threading;
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Locators.Factories;

namespace MyNet.Avalonia.Extended.Dialogs.Presentation;

/// <summary>
/// Presents dialogs inside an <see cref="OverlayDialogHost"/>.
/// </summary>
internal sealed class OverlayDialogPresenter(DialogHostOptions hostOptions, IViewFactory viewFactory, DialogSessionRegistry sessions, IUiThreadDispatcher uiThread) : IDialogPresenter
{
    /// <inheritdoc />
    public int Priority => 100;

    /// <inheritdoc />
    public bool CanPresent(IDialog dialog, DialogOptions? options)
    {
        var request = DialogOptionsFactory.Resolve(options);
        if (request.Mode != DialogPresentationMode.Overlay)
            return false;

        var topLevelKey = request.OverlayOptions?.TopLevelKey
                          ?? OverlayDialogHostManager.GetTopLevelKey(hostOptions.TopLevelProvider());
        return OverlayDialogHostManager.GetHost(request.OverlayHostId, topLevelKey) is not null;
    }

    /// <inheritdoc />
    public async Task<DialogResult<bool>> PresentAsync(
        IDialog dialog,
        DialogOptions options,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dialog);
        ArgumentNullException.ThrowIfNull(options);

        var request = DialogOptionsFactory.Resolve(options);
        var topLevelKey = request.OverlayOptions?.TopLevelKey
                          ?? OverlayDialogHostManager.GetTopLevelKey(hostOptions.TopLevelProvider());
        var host = OverlayDialogHostManager.GetHost(request.OverlayHostId, topLevelKey)
                   ?? throw new InvalidOperationException("No overlay dialog host is registered.");

        var view = dialog is MessageBoxViewModel
            ? dialog
            : viewFactory.CreateView(dialog.GetType());

        var overlay = OverlayDialogBuilder.Create(dialog, view, options, request);
        var session = sessions.Register(dialog, new(() => sessions.Remove(dialog), uiThread) { Overlay = overlay });

        try
        {
            if (options.IsModal)
            {
                host.AddModalDialog(overlay);
                var result = await overlay.ShowAsync<object?>(cancellationToken).ConfigureAwait(true);

                if (dialog is MessageBoxViewModel messageBox)
                    DialogResultMapper.ApplyMessageBoxResult(messageBox, result);

                return DialogResultMapper.Map(result);
            }

            host.AddDialog(overlay);
            var completion = new TaskCompletionSource<DialogResult<bool>>(TaskCreationOptions.RunContinuationsAsynchronously);
            await using var registration = cancellationToken.Register(() => uiThread.Post(() =>
                {
                    if (completion.Task.IsCompleted)
                        return;

                    session.CloseVisual();
                    completion.TrySetResult(DialogResult.Dismiss());
                })).ConfigureAwait(false);

            overlay.Closed += onClosed;
            return await completion.Task.ConfigureAwait(true);

            void onClosed(object? sender, ResultEventArgs args)
            {
                overlay.Closed -= onClosed;
                if (dialog is MessageBoxViewModel messageBox)
                    DialogResultMapper.ApplyMessageBoxResult(messageBox, args.Result);

                completion.TrySetResult(DialogResultMapper.Map(args.Result));
            }
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
}
