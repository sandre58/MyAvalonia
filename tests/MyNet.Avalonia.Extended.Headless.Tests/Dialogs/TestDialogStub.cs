// -----------------------------------------------------------------------
// <copyright file="TestDialogStub.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using MyNet.UI;
using MyNet.UI.Dialogs.ContentDialogs;

namespace MyNet.Avalonia.Extended.Headless.Tests.Dialogs;

internal sealed class TestDialogStub : IDialog
{
    public string Title => "Stub";

    public event EventHandler<CloseRequestedEventArgs>? CloseRequested;

    public Task<bool> CanCloseAsync() => Task.FromResult(true);

    public Task OnOpenedAsync() => Task.CompletedTask;

    public Task OnClosedAsync() => Task.CompletedTask;
}
