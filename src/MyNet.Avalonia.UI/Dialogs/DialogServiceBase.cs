// -----------------------------------------------------------------------
// <copyright file="DialogServiceBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MyNet.UI.Dialogs.CustomDialogs;

namespace MyNet.Avalonia.UI.Dialogs;

public abstract class DialogServiceBase : ICustomDialogService
{
    public ObservableCollection<IDialogViewModel> OpenedDialogs { get; } = [];

    public event EventHandler<DialogEventArgs>? DialogOpened;

    public event EventHandler<DialogEventArgs>? DialogClosed;

    /// <inheritdoc />
    public abstract Task ShowAsync(object view, IDialogViewModel viewModel);

    /// <inheritdoc />
    public virtual async Task<bool?> ShowDialogAsync(object view, IDialogViewModel viewModel)
    {
        OpenedDialogs.Add(viewModel);

        DialogOpened?.Invoke(this, new DialogEventArgs(viewModel));

        var result = await ShowDialogCoreAsync(view, viewModel).ConfigureAwait(false);

        _ = OpenedDialogs.Remove(viewModel);

        DialogClosed?.Invoke(this, new DialogEventArgs(viewModel));

        return result;
    }

    public void CloseDialog(IDialogViewModel dialog) => dialog.Close();

    protected abstract Task<bool?> ShowDialogCoreAsync(object view, IDialogViewModel viewModel);
}
