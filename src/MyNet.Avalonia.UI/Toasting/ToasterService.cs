// -----------------------------------------------------------------------
// <copyright file="ToasterService.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using MyNet.Avalonia.Templates;
using MyNet.Avalonia.UI.Controls;
using MyNet.Avalonia.UI.Toasting.Lifetime;
using MyNet.Avalonia.UI.Toasting.Lifetime.Clear;
using MyNet.UI.Notifications;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Settings;
using MyNet.Utilities;

namespace MyNet.Avalonia.UI.Toasting;

public class ToasterService : IToasterService, IDisposable
{
    private readonly Lazy<WindowNotificationManager> _windowNotificationManager;
    private readonly TimeAndCountBasedLifetimeSupervisor _lifetimeSupervisor;
    private readonly CompositeDisposable _cleanup = [];

    public event EventHandler<ToastEventArgs>? ToastShown;

    public event EventHandler<ToastEventArgs>? ToastClosed;

    public event EventHandler<ToastEventArgs>? ToastClicked;

    public ToasterService(Func<TopLevel?> topLevel)
        : this(topLevel, ToasterSettings.Default)
    { }

    public ToasterService(Func<TopLevel?> topLevel, ToasterSettings settings)
    {
        _lifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(settings.Duration, MaximumToastsCount.FromCount(settings.MaxItems));

        _windowNotificationManager = new Lazy<WindowNotificationManager>(() => Dispatcher.UIThread.Invoke(() => new WindowNotificationManager(topLevel())
        {
            Position = ConvertPosition(settings.Position),
            Margin = new Thickness(settings.OffsetX, settings.OffsetY)
        }));

        _cleanup.AddRange([

            System.Reactive.Linq.Observable.FromEventPattern<ShowToastEventArgs>(x => _lifetimeSupervisor.ShowToastRequested += x, x => _lifetimeSupervisor.ShowToastRequested -= x)
                      .Subscribe(x => ShowToast(x.EventArgs.Toast)),

            System.Reactive.Linq.Observable.FromEventPattern<CloseToastEventArgs>(x => _lifetimeSupervisor.CloseToastRequested += x, x => _lifetimeSupervisor.CloseToastRequested -= x)
                      .Subscribe(x => CloseToast(x.EventArgs.Toast))
        ]);

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
    /// Displays a modal dialog of a type that is determined by the dialog type locator.
    /// </summary>
    public void Show(INotification notification, ToastSettings settings, bool isUnique = false, Action<INotification>? onClick = null, Action? onClose = null)
    {
        if (isUnique)
            ClearToasts(new ClearBySimilarNotification(notification));

        _lifetimeSupervisor.PushToast(CreateToast(notification, settings, onClick, onClose));
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

    private void ClearToasts(IClearStrategy clearStrategy) => _lifetimeSupervisor.ClearToasts(clearStrategy);

    #endregion

    #region Display Notification

    private void ShowToast(Toast toast)
    {
        var classes = new List<string>();

        if (toast.Settings.ClosingStrategy is ToastClosingStrategy.CloseButton or ToastClosingStrategy.Both)
            classes.Add("Closable");

        var type = toast.Notification.Severity switch
        {
            NotificationSeverity.Information => global::Avalonia.Controls.Notifications.NotificationType.Information,
            NotificationSeverity.Success => global::Avalonia.Controls.Notifications.NotificationType.Success,
            NotificationSeverity.Warning => global::Avalonia.Controls.Notifications.NotificationType.Warning,
            NotificationSeverity.Error => global::Avalonia.Controls.Notifications.NotificationType.Error,
            NotificationSeverity.None => global::Avalonia.Controls.Notifications.NotificationType.Information,
            _ => throw new InvalidOperationException()
        };

        var onEnter = new Action(() =>
        {
            if (toast.Settings.FreezeOnMouseEnter)
                toast.IsLocked = true;
        });

        var onLeave = new Action(() =>
        {
            if (toast.Settings.FreezeOnMouseEnter)
                toast.IsLocked = false;
        });

        var onClick = new Action(() =>
        {
            toast.OnClick?.Invoke(toast.Notification);
            ToastClicked?.Invoke(this, new ToastEventArgs(toast.Notification));
        });

        _windowNotificationManager.Value.Show(toast.Notification, type, TimeSpan.FromHours(1), onClick, toast.OnClose, onEnter, onLeave, [.. classes]);

        ToastShown?.Invoke(this, new ToastEventArgs(toast.Notification));
    }

    private void CloseToast(Toast toast)
    {
        _windowNotificationManager.Value.Close(toast.Notification);

        ToastClosed?.Invoke(this, new ToastEventArgs(toast.Notification));
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
        _cleanup.Dispose();
        _lifetimeSupervisor.Dispose();
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

    public IEnumerable<INotification> GetActiveToasts() => throw new NotImplementedException();
}
