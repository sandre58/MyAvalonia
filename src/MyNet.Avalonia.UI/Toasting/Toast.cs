// -----------------------------------------------------------------------
// <copyright file="Toast.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.UI.Notifications;
using MyNet.UI.Toasting.Settings;

namespace MyNet.Avalonia.UI.Toasting;

public class Toast(INotification notification, ToastSettings settings, Action<INotification>? onClick = null, Action? onClose = null)
{
    private readonly Guid _id = Guid.NewGuid();

    public INotification Notification { get; } = notification;

    public ToastSettings Settings { get; } = settings;

    public Action<INotification>? OnClick { get; } = onClick;

    public Action? OnClose { get; } = onClose;

    public bool IsLocked { get; set; }

    public event EventHandler? CloseRequest;

    public void Close() => CloseRequest?.Invoke(this, EventArgs.Empty);

    public override bool Equals(object? obj) => obj != null && GetType() == obj.GetType() && _id == ((Toast)obj)._id;

    public override int GetHashCode() => _id.GetHashCode();
}
