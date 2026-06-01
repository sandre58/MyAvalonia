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
using MyNet.UI.Dialogs.ContentDialogs;
using MyNet.UI.Dialogs.MessageBox;
using MyNet.UI.Locators.Factories;

namespace MyNet.Avalonia.Extended.Dialogs.Presentation;

/// <summary>
/// Presents dialogs inside an <see cref="OverlayDialogHost"/>.
/// </summary>
public sealed class OverlayDialogPresenter(
    DialogHostOptions hostOptions,
    IViewFactory viewFactory,
    AvaloniaDialogSessionRegistry sessions) : IDialogPresenter
{
    /// <inheritdoc />
    public int Priority => 100;

    /// <inheritdoc />
    public bool CanPresent(IDialog dialog, UI.Dialogs.ContentDialogs.DialogOptions? options)
    {
        var request = DialogOptions.Resolve(options);
        if (request.Mode != DialogPresentationMode.Overlay)
            return false;

        var hash = request.OverlayOptions?.TopLevelHashCode ?? hostOptions.TopLevelProvider()?.GetHashCode();
        return OverlayDialogHostManager.GetHost(request.OverlayHostId, hash) is not null;
    }

    /// <inheritdoc />
    public async Task<DialogResult<bool>> PresentAsync(
        IDialog dialog,
        UI.Dialogs.ContentDialogs.DialogOptions options,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(dialog);
        ArgumentNullException.ThrowIfNull(options);

        var request = DialogOptions.Resolve(options);
        var hash = request.OverlayOptions?.TopLevelHashCode ?? hostOptions.TopLevelProvider()?.GetHashCode();
        var host = OverlayDialogHostManager.GetHost(request.OverlayHostId, hash)
                   ?? throw new InvalidOperationException("No overlay dialog host is registered.");

        var view = dialog is MessageBoxViewModel
            ? dialog
            : viewFactory.CreateView(dialog.GetType());

        var overlay = AvaloniaOverlayDialogBuilder.Create(dialog, view, options, request);
        var session = sessions.Register(
            dialog,
            new(() => sessions.Remove(dialog)) { Overlay = overlay });

        try
        {
            if (options.IsModal)
            {
                host.AddModalDialog(overlay);
                var result = await overlay.ShowAsync<object?>(cancellationToken).ConfigureAwait(true);

                if (dialog is MessageBoxViewModel messageBox)
                    AvaloniaDialogResultMapper.ApplyMessageBoxResult(messageBox, result);

                return AvaloniaDialogResultMapper.MapBool(result);
            }

            host.AddDialog(overlay);
            var completion = new TaskCompletionSource<DialogResult<bool>>(TaskCreationOptions.RunContinuationsAsynchronously);
            cancellationToken.Register(() => completion.TrySetResult(DialogResult.Dismiss()));

            overlay.Closed += onClosed;
            return await completion.Task.ConfigureAwait(true);

            void onClosed(object? sender, ResultEventArgs args)
            {
                overlay.Closed -= onClosed;
                if (dialog is MessageBoxViewModel messageBox)
                    AvaloniaDialogResultMapper.ApplyMessageBoxResult(messageBox, args.Result);

                completion.TrySetResult(AvaloniaDialogResultMapper.MapBool(args.Result));
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
