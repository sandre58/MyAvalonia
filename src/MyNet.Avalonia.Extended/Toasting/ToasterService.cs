// -----------------------------------------------------------------------
// <copyright file="ToasterService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using MyNet.Avalonia.Extended.Controls;
using MyNet.Avalonia.Extended.Toasting.Lifetime.Clear;
using MyNet.Avalonia.Templates;
using MyNet.UI.Notifications;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Settings;
using WindowNotificationManager = Avalonia.Controls.Notifications.WindowNotificationManager;

namespace MyNet.Avalonia.Extended.Toasting;

public class ToasterService : IToasterService, IDisposable
{
    private WindowNotificationManager? _windowNotificationManager;
    private readonly TimeSpan _defaultDuration;
    private readonly ConcurrentDictionary<int, Toast> _activeToasts = new();

    public event EventHandler<ToastEventArgs>? ToastShown;

    public event EventHandler<ToastEventArgs>? ToastClosed;

    public event EventHandler<ToastEventArgs>? ToastClicked;

    public ToasterService(Func<TopLevel?> topLevel)
        : this(topLevel, ToasterSettings.Default)
    { }

    public ToasterService(Func<TopLevel?> topLevel, ToasterSettings settings)
    {
        _defaultDuration = settings.Duration;

        Dispatcher.UIThread.Post(() => _windowNotificationManager = new(topLevel())
        {
            Position = ConvertPosition(settings.Position),
            MaxItems = settings.MaxItems,
            Margin = new(settings.OffsetX, settings.OffsetY)
        });

        RegisteredDataTemplate.Register<MessageNotification>(x => new MessageNotificationControl
        {
            Header = x.Title,
            Severity = x.Severity,
            Content = x.Message,
            Width = settings.Width
        },
        nameof(INotification));
    }

    protected virtual Toast CreateToast(INotification notification, ToastSettings settings, Action<INotification>? onClick = null, Action? onClose = null)
        => new(notification, settings, onClick, onClose);

    #region IToasterService

    /// <summary>
    /// Shows a toast notification using the native <see cref="WindowNotificationManager"/>.
    /// </summary>
    public void Show(INotification notification, ToastSettings settings, bool isUnique = false, Action<INotification>? onClick = null, Action? onClose = null)
    {
        if (isUnique)
            ClearToasts(new ClearBySimilarNotification(notification));

        var toast = CreateToast(notification, settings, onClick, onClose);
        ShowToast(toast);
    }

    /// <summary>
    /// Hide all messages.
    /// </summary>
    public void Clear() => ClearToasts(new ClearAll());

    /// <summary>
    /// Hide a message if is displayed.
    /// </summary>
    /// <param name="notification">.</param>
    public void Hide(INotification notification) => ClearToasts(new ClearByNotification(notification));

    public IEnumerable<INotification> GetActiveToasts() => [.. _activeToasts.Values.Select(x => x.Notification)];

    private void ClearToasts(IClearStrategy clearStrategy)
    {
        var toastsToRemove = clearStrategy.GetToastsToRemove(_activeToasts.Values).ToList();
        foreach (var toast in toastsToRemove)
            CloseToast(toast);
    }

    #endregion

    #region Display Notification

    private void ShowToast(Toast toast)
    {
        var classes = new List<string>();

        if (toast.Settings.ClosingStrategy is ToastClosingStrategy.CloseButton or ToastClosingStrategy.Both)
            classes.Add("is-closable");

        var type = toast.Notification.Severity switch
        {
            NotificationSeverity.Information => global::Avalonia.Controls.Notifications.NotificationType.Information,
            NotificationSeverity.Success => global::Avalonia.Controls.Notifications.NotificationType.Success,
            NotificationSeverity.Warning => global::Avalonia.Controls.Notifications.NotificationType.Warning,
            NotificationSeverity.Error => global::Avalonia.Controls.Notifications.NotificationType.Error,
            NotificationSeverity.None => global::Avalonia.Controls.Notifications.NotificationType.Information,
            _ => throw new InvalidOperationException()
        };

        var expiration = toast.Settings.ClosingStrategy is ToastClosingStrategy.AutoClose or ToastClosingStrategy.Both
            ? _defaultDuration
            : TimeSpan.Zero;

        _activeToasts[toast.GetHashCode()] = toast;

        var onClick = new Action(() =>
        {
            toast.OnClick?.Invoke(toast.Notification);
            ToastClicked?.Invoke(this, new(toast.Notification));
        });

        var onClose = new Action(() =>
        {
            if (_activeToasts.TryRemove(toast.GetHashCode(), out _))
                ToastClosed?.Invoke(this, new(toast.Notification));
            toast.OnClose?.Invoke();
        });

        // Use Background priority to ensure the manager's OnApplyTemplate has run
        // (layout pass executes at higher priority than Background).
        Dispatcher.UIThread.Post(() =>
        {
            _windowNotificationManager?.Show(toast.Notification, type, expiration, onClick, onClose, [.. classes]);
            ToastShown?.Invoke(this, new(toast.Notification));
        },
        DispatcherPriority.Background);
    }

    private void CloseToast(Toast toast)
    {
        if (_activeToasts.TryRemove(toast.GetHashCode(), out _))
        {
            Dispatcher.UIThread.Post(() => _windowNotificationManager?.Close(toast.Notification));
            ToastClosed?.Invoke(this, new(toast.Notification));
        }
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;
        _activeToasts.Clear();
    }

    #endregion IDisposable

    private static global::Avalonia.Controls.Notifications.NotificationPosition ConvertPosition(ToasterPosition position)
        => position switch
        {
            ToasterPosition.TopLeft => global::Avalonia.Controls.Notifications.NotificationPosition.TopLeft,
            ToasterPosition.TopRight => global::Avalonia.Controls.Notifications.NotificationPosition.TopRight,
            ToasterPosition.BottomLeft => global::Avalonia.Controls.Notifications.NotificationPosition.BottomLeft,
            ToasterPosition.BottomRight => global::Avalonia.Controls.Notifications.NotificationPosition.BottomRight,
            ToasterPosition.TopCenter => global::Avalonia.Controls.Notifications.NotificationPosition.TopCenter,
            ToasterPosition.BottomCenter => global::Avalonia.Controls.Notifications.NotificationPosition.BottomCenter,
            _ => throw new ArgumentOutOfRangeException(nameof(position), position, null)
        };
}
