// -----------------------------------------------------------------------
// <copyright file="ToastHoverLifetimeAttachment.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using MyNet.UI.Toasting;
using MyNet.UI.Toasting.Models;
using MyNet.UI.Toasting.Settings;

namespace MyNet.Avalonia.Extended.Toasting;

/// <summary>
/// Host-side auto-close with hover pause for toasts configured with <see cref="ToastSettings.FreezeOnMouseEnter"/>.
/// </summary>
internal sealed class ToastHoverLifetimeAttachment : IDisposable
{
    private readonly IToast _toast;
    private readonly IToastManager _toastManager;
    private readonly Control _control;
    private readonly DispatcherTimer _timer = new();
    private DateTimeOffset _expiresAt;
    private TimeSpan _remaining;
    private bool _isPaused;
    private bool _isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToastHoverLifetimeAttachment"/> class.
    /// </summary>
    /// <param name="toast">The toast being displayed.</param>
    /// <param name="toastManager">The toast manager that owns the toast collection.</param>
    /// <param name="control">The visual root receiving pointer events.</param>
    /// <param name="duration">The auto-close duration.</param>
    public ToastHoverLifetimeAttachment(
        IToast toast,
        IToastManager toastManager,
        Control control,
        TimeSpan duration)
    {
        _toast = toast ?? throw new ArgumentNullException(nameof(toast));
        _toastManager = toastManager ?? throw new ArgumentNullException(nameof(toastManager));
        _control = control ?? throw new ArgumentNullException(nameof(control));

        _remaining = duration;
        _expiresAt = DateTimeOffset.UtcNow + duration;
        _timer.Tick += OnTimerTick;

        _control.PointerEntered += OnPointerEntered;
        _control.PointerExited += OnPointerExited;

        StartTimer(duration);
    }

    /// <summary>
    /// Gets whether hover-managed auto-close should be attached for the given toast.
    /// </summary>
    /// <param name="toast">The toast to inspect.</param>
    /// <returns><see langword="true"/> when the host should manage auto-close with hover pause.</returns>
    internal static bool ShouldAttach(IToast toast)
    {
        ArgumentNullException.ThrowIfNull(toast);

        return toast.Settings is { FreezeOnMouseEnter: true, ClosingStrategy: ToastClosingStrategy.AutoClose or ToastClosingStrategy.Both };
    }

    /// <summary>
    /// Ensures toast content is hosted by a control that can receive pointer events.
    /// </summary>
    /// <param name="content">The toast visual content.</param>
    /// <param name="toast">The toast being displayed.</param>
    /// <param name="toastManager">The toast manager that owns the toast collection.</param>
    /// <param name="defaultDuration">The fallback auto-close duration.</param>
    /// <returns>The display content and optional hover attachment.</returns>
    internal static (object DisplayContent, ToastHoverLifetimeAttachment? Attachment) Prepare(
        object content,
        IToast toast,
        IToastManager toastManager,
        TimeSpan defaultDuration)
    {
        ArgumentNullException.ThrowIfNull(content);
        ArgumentNullException.ThrowIfNull(toast);
        ArgumentNullException.ThrowIfNull(toastManager);

        if (!ShouldAttach(toast))
            return (content, null);

        var duration = toast.Settings.Duration ?? defaultDuration;

        if (content is Control control)
            return (content, new(toast, toastManager, control, duration));

        var wrapper = new Border
        {
            Child = content as Control ?? new ContentControl { Content = content }
        };

        return (wrapper, new(toast, toastManager, wrapper, duration));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_isDisposed)
            return;

        _isDisposed = true;
        _timer.Tick -= OnTimerTick;
        _timer.Stop();
        _control.PointerEntered -= OnPointerEntered;
        _control.PointerExited -= OnPointerExited;
    }

    private void OnPointerEntered(object? sender, PointerEventArgs e)
    {
        if (_isPaused || _isDisposed)
            return;

        _isPaused = true;
        _timer.Stop();
        _remaining = _expiresAt - DateTimeOffset.UtcNow;

        if (_remaining < TimeSpan.Zero)
            _remaining = TimeSpan.Zero;
    }

    private void OnPointerExited(object? sender, PointerEventArgs e)
    {
        if (!_isPaused || _isDisposed)
            return;

        _isPaused = false;

        if (_remaining <= TimeSpan.Zero)
        {
            Expire();
            return;
        }

        _expiresAt = DateTimeOffset.UtcNow + _remaining;
        StartTimer(_remaining);
    }

    private void OnTimerTick(object? sender, EventArgs e) => Expire();

    private void StartTimer(TimeSpan interval)
    {
        _timer.Stop();
        _timer.Interval = interval;
        _timer.Start();
    }

    private void Expire()
    {
        if (_isDisposed)
            return;

        _timer.Stop();
        _toastManager.Remove(_toast);
    }
}
