// -----------------------------------------------------------------------
// <copyright file="ContentDialogServiceBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.UI.Dialogs;

public abstract class ContentDialogServiceBase : IContentDialogService
{
    public ObservableCollection<IDialogViewModel> OpenedDialogs { get; } = [];

    public event EventHandler<ContentDialogEventArgs>? DialogOpened;

    public event EventHandler<ContentDialogEventArgs>? DialogClosed;

    /// <inheritdoc />
    public abstract Task ShowAsync(object view, IDialogViewModel viewModel);

    /// <inheritdoc />
    public virtual async Task<bool?> ShowModalAsync(object view, IDialogViewModel viewModel)
    {
        OpenedDialogs.Add(viewModel);

        DialogOpened?.Invoke(this, new ContentDialogEventArgs(viewModel));

        var result = await ShowDialogCoreAsync(view, viewModel).ConfigureAwait(false);

        _ = OpenedDialogs.Remove(viewModel);

        DialogClosed?.Invoke(this, new ContentDialogEventArgs(viewModel));

        return result;
    }

    public Task<bool?> CloseAsync(IDialogViewModel dialog)
    {
        dialog.Close();

        return Task.FromResult(dialog.DialogResult);
    }

    protected abstract Task<bool?> ShowDialogCoreAsync(object view, IDialogViewModel viewModel);
}
