// -----------------------------------------------------------------------
// <copyright file="WindowNotificationManager.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Notifications;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Avalonia.VisualTree;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// An <see cref="INotificationManager"/> that displays notifications in a <see cref="Window"/>.
/// </summary>
[TemplatePart("PART_Items", typeof(Panel))]
[PseudoClasses(":topleft", ":topright", ":bottomleft", ":bottomright", ":topcenter", ":bottomcenter")]
public class WindowNotificationManager : TemplatedControl
{
    private readonly Dictionary<object, NotificationCard> _notificationCards = [];
    private IList? _items;

    /// <summary>
    /// Defines the <see cref="Position"/> property.
    /// </summary>
    public static readonly StyledProperty<NotificationPosition> PositionProperty =
      AvaloniaProperty.Register<WindowNotificationManager, NotificationPosition>(nameof(Position), NotificationPosition.TopRight);

    /// <summary>
    /// Gets or sets defines which corner of the screen notifications can be displayed in.
    /// </summary>
    /// <seealso cref="NotificationPosition"/>
    public NotificationPosition Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    static WindowNotificationManager()
    {
        HorizontalAlignmentProperty.OverrideDefaultValue<WindowNotificationManager>(global::Avalonia.Layout.HorizontalAlignment.Stretch);
        VerticalAlignmentProperty.OverrideDefaultValue<WindowNotificationManager>(global::Avalonia.Layout.VerticalAlignment.Stretch);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowNotificationManager"/> class.
    /// </summary>
    /// <param name="host">The TopLevel that will host the control.</param>
    public WindowNotificationManager(TopLevel? host)
        : this()
    {
        if (host is not null)
        {
            InstallFromTopLevel(host);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowNotificationManager"/> class.
    /// </summary>
    public WindowNotificationManager() => UpdatePseudoClasses(Position);

    // <inheritdoc/>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var itemsControl = e.NameScope.Find<Panel>("PART_Items");
        _items = itemsControl?.Children;
    }

    // <inheritdoc/>
    public void Show(INotification notification) => Show(notification, notification.Type, notification.Expiration, notification.OnClick, notification.OnClose);

    // <inheritdoc/>
    public void Show(object content)
    {
        if (content is INotification notification)
        {
            Show(notification, notification.Type, notification.Expiration, notification.OnClick, notification.OnClose);
        }
        else
        {
            Show(content, NotificationType.Information);
        }
    }

    /// <summary>
    /// Shows a Notification.
    /// </summary>
    /// <param name="content">the content of the notification.</param>
    /// <param name="type">the type of the notification.</param>
    /// <param name="expiration">the expiration time of the notification after which it will automatically close. If the value is Zero then the notification will remain open until the user closes it.</param>
    /// <param name="onClick">an Action to be run when the notification is clicked.</param>
    /// <param name="onClose">an Action to be run when the notification is closed.</param>
    /// <param name="onEnter">an Action to be run when mouse enter on notification.</param>
    /// <param name="onLeave">an Action to be run when mouse leave on notification.</param>
    /// <param name="classes">style classes to apply.</param>
    public void Show(object content,
                     NotificationType type,
                     TimeSpan? expiration = null,
                     Action? onClick = null,
                     Action? onClose = null,
                     Action? onEnter = null,
                     Action? onLeave = null,
                     string[]? classes = null)
    => Dispatcher.UIThread.Post(async () =>
        {
            var notificationControl = new NotificationCard
            {
                Content = content,
                NotificationType = type
            };

            // Add style classes if any
            if (classes != null)
            {
                notificationControl.Classes.AddRange(classes);
            }

            notificationControl.NotificationClosed += (sender, args) =>
            {
                onClose?.Invoke();

                _items?.Remove(sender);
                _ = _notificationCards.Remove(content);
            };

            notificationControl.PointerPressed += (_, _) => onClick?.Invoke();
            notificationControl.PointerEntered += (_, _) => onEnter?.Invoke();
            notificationControl.PointerExited += (_, _) => onLeave?.Invoke();

            _ = _items?.Add(notificationControl);
            _notificationCards.Add(content, notificationControl);

            if (expiration == TimeSpan.Zero)
            {
                return;
            }

            await Task.Delay(expiration ?? TimeSpan.FromSeconds(5)).ConfigureAwait(false);

            notificationControl.Close();

            _ = _notificationCards.Remove(content);
        });

    // <inheritdoc/>
    public void Close(INotification notification)
    {
        Dispatcher.UIThread.VerifyAccess();

        if (_notificationCards.Remove(notification, out var notificationCard))
        {
            notificationCard.Close();
        }
    }

    // <inheritdoc/>
    public void Close(object content)
    {
        Dispatcher.UIThread.VerifyAccess();

        if (_notificationCards.Remove(content, out var notificationCard))
        {
            notificationCard.Close();
        }
    }

    // <inheritdoc/>
    public void CloseAll()
    {
        Dispatcher.UIThread.VerifyAccess();

        foreach (var kvp in _notificationCards)
        {
            kvp.Value.Close();
        }

        _notificationCards.Clear();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PositionProperty)
        {
            UpdatePseudoClasses(change.GetNewValue<NotificationPosition>());
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _notificationCards.Clear();
    }

    /// <summary>
    /// Installs the <see cref="WindowNotificationManager"/> within the <see cref="AdornerLayer"/>.
    /// </summary>
    private void InstallFromTopLevel(TopLevel topLevel)
    {
        topLevel.TemplateApplied += TopLevelOnTemplateApplied;
        var adorner = topLevel.FindDescendantOfType<VisualLayerManager>()?.AdornerLayer;
        if (adorner is null)
            return;
        adorner.Children.Add(this);
        AdornerLayer.SetAdornedElement(this, adorner);
    }

    private void TopLevelOnTemplateApplied(object? sender, TemplateAppliedEventArgs e)
    {
        if (Parent is AdornerLayer adornerLayer)
        {
            _ = adornerLayer.Children.Remove(this);
            AdornerLayer.SetAdornedElement(this, null);
        }

        // Reinstall notification manager on template reapplied.
        var topLevel = (TopLevel)sender!;
        topLevel.TemplateApplied -= TopLevelOnTemplateApplied;
        InstallFromTopLevel(topLevel);
    }

    private void UpdatePseudoClasses(NotificationPosition position)
    {
        PseudoClasses.Set(":topleft", position == NotificationPosition.TopLeft);
        PseudoClasses.Set(":topright", position == NotificationPosition.TopRight);
        PseudoClasses.Set(":bottomleft", position == NotificationPosition.BottomLeft);
        PseudoClasses.Set(":bottomright", position == NotificationPosition.BottomRight);
        PseudoClasses.Set(":topcenter", position == NotificationPosition.TopCenter);
        PseudoClasses.Set(":bottomcenter", position == NotificationPosition.BottomCenter);
    }
}
