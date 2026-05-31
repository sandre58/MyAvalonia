// -----------------------------------------------------------------------
// <copyright file="ToastClipboardFeedback.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Clipboard;
using MyNet.UI.Notifications;
using MyNet.UI.Notifications.Models;
using MyNet.UI.Resources;

namespace MyNet.Avalonia.Extended.Clipboard;

/// <summary>
/// Publishes clipboard feedback through the notification pipeline.
/// </summary>
/// <param name="notificationPublisher">The notification publisher used to emit feedback messages.</param>
public sealed class ToastClipboardFeedback(INotificationPublisher notificationPublisher) : IClipboardFeedback
{
    /// <inheritdoc />
    public void NotifySuccess()
        => notificationPublisher.Publish(new MessageNotification(
            MessageResources.CopyInClipBoardSuccess,
            severity: NotificationSeverity.Information));

    /// <inheritdoc />
    public void NotifyError()
        => notificationPublisher.Publish(new MessageNotification(
            MessageResources.CopyInClipBoardError,
            severity: NotificationSeverity.Error));
}
