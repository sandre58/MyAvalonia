// -----------------------------------------------------------------------
// <copyright file="AvaloniaToastHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Threading;
using MyNet.Avalonia.Extended.Toasting.Settings;
using MyNet.UI.Notifications.Models;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Models;
using MyNet.UI.Toasting.Settings;
using AvaloniaNotificationPosition = Avalonia.Controls.Notifications.NotificationPosition;
using AvaloniaNotificationType = Avalonia.Controls.Notifications.NotificationType;
using WindowNotificationManager = Avalonia.Controls.Notifications.WindowNotificationManager;

namespace MyNet.Avalonia.Extended.Toasting;

/// <summary>
/// Renders <see cref="IToastManager.Toasts"/> through Avalonia <see cref="WindowNotificationManager"/>.
/// </summary>
public sealed class AvaloniaToastHost : IDisposable
{
    private readonly Func<TopLevel?> _topLevelProvider;
    private readonly IToastManager _toastManager;
    private readonly AvaloniaToastHostOptions _options;
    private readonly Dictionary<Guid, object> _displayContentByNotificationId = [];
    private WindowNotificationManager? _notificationManager;
    private readonly INotifyCollectionChanged _toastsCollection;
    private bool _suppressCloseCallback;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvaloniaToastHost"/> class.
    /// </summary>
    /// <param name="topLevelProvider">Resolves the host top level, typically the main window.</param>
    /// <param name="toastManager">The toast manager whose collection is rendered.</param>
    /// <param name="options">Optional visual layout options.</param>
    public AvaloniaToastHost(
        Func<TopLevel?> topLevelProvider,
        IToastManager toastManager,
        AvaloniaToastHostOptions? options = null)
    {
        _topLevelProvider = topLevelProvider ?? throw new ArgumentNullException(nameof(topLevelProvider));
        _toastManager = toastManager ?? throw new ArgumentNullException(nameof(toastManager));
        _options = options ?? AvaloniaToastHostOptions.Default;

        _toastsCollection = _toastManager.Toasts;
        _toastsCollection.CollectionChanged += OnToastsCollectionChanged;

        foreach (var toast in _toastManager.Toasts.ToList())
            ShowToast(toast);
    }

    /// <summary>
    /// Re-applies layout options and re-renders currently visible toasts.
    /// </summary>
    public void RefreshLayout() => Post(() =>
    {
        foreach (var content in _displayContentByNotificationId.Values.ToList())
            _notificationManager?.Close(content);

        _displayContentByNotificationId.Clear();
        _notificationManager = null;

        foreach (var toast in _toastManager.Toasts.ToList())
            ShowToast(toast);
    });

    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        _toastsCollection.CollectionChanged -= OnToastsCollectionChanged;
        _displayContentByNotificationId.Clear();
        _notificationManager = null;
    }

    private void OnToastsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                foreach (IToast toast in e.NewItems!)
                    ShowToast(toast);
                break;

            case NotifyCollectionChangedAction.Remove:
                foreach (IToast toast in e.OldItems!)
                    HideToast(toast);
                break;

            case NotifyCollectionChangedAction.Reset:
                foreach (var content in _displayContentByNotificationId.Values.ToList())
                    Post(() => _notificationManager?.Close(content));

                _displayContentByNotificationId.Clear();

                foreach (var toast in _toastManager.Toasts)
                    ShowToast(toast);
                break;
        }
    }

    private void ShowToast(IToast toast)
    {
        var content = AvaloniaToastContentFactory.Create(toast.Notification, _options.Width);
        _displayContentByNotificationId[toast.Notification.Id] = content;

        var classes = GetClasses(toast.Settings);
        var type = MapSeverity(toast.Notification.Severity);

        Post(() =>
        {
            EnsureNotificationManager();
            _notificationManager?.Show(
                content,
                type,
                TimeSpan.Zero,
                () => OnToastClicked(toast),
                () => OnToastClosedByUser(toast),
                [.. classes]);
        });
    }

    private void HideToast(IToast toast)
    {
        if (!_displayContentByNotificationId.Remove(toast.Notification.Id, out var content))
            return;

        _suppressCloseCallback = true;
        try
        {
            Post(() => _notificationManager?.Close(content));
        }
        finally
        {
            _suppressCloseCallback = false;
        }
    }

    private static void OnToastClicked(IToast toast)
    {
        if (toast.ClickCommand?.CanExecute(null) == true)
            toast.ClickCommand.Execute(null);
    }

    private void OnToastClosedByUser(IToast toast)
    {
        if (_suppressCloseCallback)
            return;

        if (toast.CloseCommand?.CanExecute(null) == true)
            toast.CloseCommand.Execute(null);
        else
            _toastManager.Remove(toast);
    }

    private void EnsureNotificationManager()
    {
        if (_notificationManager is not null)
            return;

        var topLevel = _topLevelProvider();
        if (topLevel is null)
            return;

        _notificationManager = new(topLevel)
        {
            Position = ConvertPosition(_options.Position),
            MaxItems = _options.MaxItems,
            Margin = new(_options.OffsetX, _options.OffsetY)
        };
    }

    private static IEnumerable<string> GetClasses(ToastSettings settings)
    {
        if (settings.ClosingStrategy is ToastClosingStrategy.CloseButton or ToastClosingStrategy.Both)
            yield return "is-closable";
    }

    private static AvaloniaNotificationType MapSeverity(NotificationSeverity severity)
        => severity switch
        {
            NotificationSeverity.Success => AvaloniaNotificationType.Success,
            NotificationSeverity.Warning => AvaloniaNotificationType.Warning,
            NotificationSeverity.Error => AvaloniaNotificationType.Error,
            _ => AvaloniaNotificationType.Information
        };

    private static AvaloniaNotificationPosition ConvertPosition(AvaloniaToastPosition position)
        => position switch
        {
            AvaloniaToastPosition.TopLeft => AvaloniaNotificationPosition.TopLeft,
            AvaloniaToastPosition.TopCenter => AvaloniaNotificationPosition.TopCenter,
            AvaloniaToastPosition.TopRight => AvaloniaNotificationPosition.TopRight,
            AvaloniaToastPosition.BottomLeft => AvaloniaNotificationPosition.BottomLeft,
            AvaloniaToastPosition.BottomCenter => AvaloniaNotificationPosition.BottomCenter,
            _ => AvaloniaNotificationPosition.BottomRight
        };

    private static void Post(Action action)
        => Dispatcher.UIThread.Post(action, DispatcherPriority.Background);
}
