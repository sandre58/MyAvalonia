// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHost.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Behaviors;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Canvas overlay that stacks modal and non-modal <see cref="Primitives.OverlayDialog"/> instances above its parent surface.
/// </summary>
/// <remarks>
/// Hosts register with <see cref="OverlayDialogHostManager"/> when attached to the visual tree.
/// Prefer declaring a host in XAML with <see cref="HostId"/> rather than relying on automatic window injection.
/// See <c>Dialogs/Overlay/README.md</c>.
/// </remarks>
public class OverlayDialogHost : Canvas
{
    private const int DialogDisappearDurationMs = 150;

    // Dialog animations are driven by XAML Transitions on [IsClosed=True]/[IsClosed=False] styles.
    // Mask animations use programmatic Transitions so the continuation always runs on the UI thread.
    private static readonly TimeSpan MaskAppearDuration = TimeSpan.FromSeconds(0.2);
    private static readonly TimeSpan MaskDisappearDuration = TimeSpan.FromSeconds(0.15);

    private readonly List<DialogPair> _layers = new(10);

    static OverlayDialogHost() => ClipToBoundsProperty.OverrideDefaultValue<OverlayDialogHost>(true);

    private int _modalCount;

    public Thickness SnapThickness { get; set; } = new(0);

    public static readonly AttachedProperty<bool> IsModalStatusScopeProperty = AvaloniaProperty.RegisterAttached<OverlayDialogHost, Control, bool>("IsModalStatusScope");

    public static void SetIsModalStatusScope(Control obj, bool value) => obj.SetValue(IsModalStatusScopeProperty, value);

    internal static bool GetIsModalStatusScope(Control obj) => obj.GetValue(IsModalStatusScopeProperty);

    public static readonly AttachedProperty<bool> IsInModalStatusProperty = AvaloniaProperty.RegisterAttached<OverlayDialogHost, Control, bool>(nameof(IsInModalStatus));

    internal static void SetIsInModalStatus(Control obj, bool value) => obj.SetValue(IsInModalStatusProperty, value);

    public static bool GetIsInModalStatus(Control obj) => obj.GetValue(IsInModalStatusProperty);

    public static readonly StyledProperty<bool> IsModalStatusReporterProperty = AvaloniaProperty.Register<OverlayDialogHost, bool>(nameof(IsModalStatusReporter));

    public bool IsModalStatusReporter
    {
        get => GetValue(IsModalStatusReporterProperty);
        set => SetValue(IsModalStatusReporterProperty, value);
    }

    public bool IsInModalStatus
    {
        get => GetValue(IsInModalStatusProperty);
        set => SetValue(IsInModalStatusProperty, value);
    }

    public bool IsAnimationDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this host covers the entire window client area and may drag the window from its mask when no modal mask is shown.
    /// </summary>
    public bool IsTopLevel { get; set; }

    /// <summary>
    /// Gets or sets the logical host identifier used with <see cref="OverlayDialogHostManager.GetHost"/>.
    /// </summary>
    public string? HostId { get; set; }

    public static readonly StyledProperty<IBrush?> OverlayMaskBrushProperty =
        AvaloniaProperty.Register<OverlayDialogHost, IBrush?>(nameof(OverlayMaskBrush));

    public IBrush? OverlayMaskBrush
    {
        get => GetValue(OverlayMaskBrushProperty);
        set => SetValue(OverlayMaskBrushProperty, value);
    }

    private PureRectangle CreateOverlayMask(bool modal, bool canCloseOnClick)
    {
        PureRectangle rec = new()
        {
            Width = Bounds.Width,
            Height = Bounds.Height,
            IsVisible = true,

            // Start transparent so the appear transition animates from 0 → 1.
            Opacity = 0
        };

        if (modal)
        {
            rec[!PureRectangle.BackgroundProperty] = this[!OverlayMaskBrushProperty];
        }
        else if (canCloseOnClick)
        {
            rec.SetCurrentValue(PureRectangle.BackgroundProperty, Brushes.Transparent);
        }

        if (canCloseOnClick)
        {
            rec.AddHandler(PointerReleasedEvent, ClickMaskToCloseDialog);
        }
        else if (IsTopLevel)
        {
            rec.AddHandler(PointerPressedEvent, DragMaskToMoveWindow);
        }

        return rec;
    }

    private static void TriggerMaskAppear(PureRectangle mask, TimeSpan duration)
    {
        mask.Transitions = [new DoubleTransition { Property = OpacityProperty, Duration = duration }];
        mask.Opacity = 1.0;
    }

    private static void TriggerMaskDisappear(PureRectangle mask, TimeSpan duration)
    {
        mask.Transitions = [new DoubleTransition { Property = OpacityProperty, Duration = duration }];
        mask.Opacity = 0.0;
    }

    private void DragMaskToMoveWindow(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            return;
        }

        if (sender is not PureRectangle mask) return;
        if (TopLevel.GetTopLevel(mask) is Window window)
        {
            window.BeginMoveDrag(e);
        }
    }

    private void ClickMaskToCloseDialog(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is not PureRectangle border) return;
        var layer = _layers.FirstOrDefault(a => a.Mask == border);
        if (layer is null) return;
        border.RemoveHandler(PointerReleasedEvent, ClickMaskToCloseDialog);
        border.RemoveHandler(PointerPressedEvent, DragMaskToMoveWindow);
        layer.Element.Close();
    }

    private IDisposable? _modalStatusSubscription;
    private int? _topLevelKey;

    protected sealed override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _topLevelKey = OverlayDialogHostManager.GetTopLevelKey(TopLevel.GetTopLevel(this));
        var modalHost = this.GetVisualAncestors().OfType<Control>().FirstOrDefault(GetIsModalStatusScope);
        if (modalHost is not null)
        {
            _modalStatusSubscription = this.GetObservable(IsInModalStatusProperty)
                .Subscribe(a =>
                {
                    if (IsModalStatusReporter)
                    {
                        SetIsInModalStatus(modalHost, a);
                    }
                });
        }

        OverlayDialogHostManager.Register(this, HostId, _topLevelKey);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs? e)
    {
        while (_layers.Count > 0)
        {
            _layers[0].Element.Close();
        }

        _modalStatusSubscription?.Dispose();
        OverlayDialogHostManager.Unregister(HostId, _topLevelKey);
        base.OnDetachedFromVisualTree(e);
    }

    protected sealed override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        foreach (var t in _layers)
        {
            if (t.Mask is { } rect)
            {
                rect.Width = Bounds.Width;
                rect.Height = Bounds.Height;
            }

            switch (t.Element)
            {
                case OverlayDialog d:
                    ResetDialogPosition(d, e.NewSize);
                    break;
            }
        }
    }

    private void ResetZIndices()
    {
        var index = 0;
        foreach (var t in _layers)
        {
            if (t.Mask is { } mask)
            {
                mask.ZIndex = index;
                index++;
            }

            if (t.Element is not { } dialog)
                continue;
            dialog.ZIndex = index;
            index++;
        }
    }

    /// <summary>
    /// Returns the content of the top-most open dialog whose <see cref="IContentControl.Content"/> is assignable to <typeparamref name="T"/>.
    /// </summary>
    public T? Recall<T>()
    {
        for (var i = _layers.Count - 1; i >= 0; i--)
        {
            if (_layers[i].Element.Content is T content)
                return content;
        }

        return default;
    }

    private sealed class DialogPair(PureRectangle? mask, OverlayFeedbackElement element, bool modal = true)
    {
        internal PureRectangle? Mask { get; } = mask;

        internal OverlayFeedbackElement Element { get; } = element;

        internal bool Modal { get; } = modal;
    }

    private static void ResetDialogPosition(OverlayDialog control, Size newSize)
    {
        control.MaxWidth = newSize.Width;
        control.MaxHeight = newSize.Height;
        if (control.IsFullScreen)
        {
            control.Width = newSize.Width;
            control.Height = newSize.Height;
            SetLeft(control, 0);
            SetTop(control, 0);
            return;
        }

        var width = newSize.Width - control.Bounds.Width;
        var height = newSize.Height - control.Bounds.Height;
        var newLeft = width * control.HorizontalOffsetRatio ?? 0;
        var newTop = height * control.VerticalOffsetRatio ?? 0;
        newLeft = control.ActualHorizontalAnchor switch
        {
            HorizontalPosition.Left => 0,
            HorizontalPosition.Right => newSize.Width - control.Bounds.Width,
            HorizontalPosition.Center => newLeft,
            _ => throw new InvalidOperationException()
        };
        newTop = control.ActualVerticalAnchor switch
        {
            VerticalPosition.Top => 0,
            VerticalPosition.Bottom => newSize.Height - control.Bounds.Height,
            VerticalPosition.Center => newTop,
            _ => throw new InvalidOperationException()
        };
        SetLeft(control, Math.Max(0.0, newLeft));
        SetTop(control, Math.Max(0.0, newTop));
    }

    public void AddDialog(OverlayDialog control)
    {
        PureRectangle? mask = null;
        if (control.CanLightDismiss) mask = CreateOverlayMask(false, control.CanLightDismiss);
        if (mask is not null) Children.Add(mask);
        Children.Add(control);
        _layers.Add(new(mask, control, false));
        if (control.IsFullScreen)
        {
            control.Width = Bounds.Width;
            control.Height = Bounds.Height;
        }

        control.MaxWidth = Bounds.Width;
        control.MaxHeight = Bounds.Height;
        control.Measure(Bounds.Size);
        control.Arrange(new(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosingAsync);
        control.AddHandler(OverlayDialog.LayerChangedEvent, OnDialogLayerChanged);
        ResetZIndices();

        // Trigger mask appear transition and dialog XAML appear transition (IsClosed true → false).
        if (mask is not null && !IsAnimationDisabled)
        {
            TriggerMaskAppear(mask, MaskAppearDuration);
        }
        else
        {
            mask?.Opacity = 1.0;
        }

        control.IsClosed = false;
    }

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Used by AddHandler")]
    private async Task OnDialogControlClosingAsync(object? sender, object? e)
    {
        if (sender is not OverlayDialog control) return;
        var layer = _layers.FirstOrDefault(a => a.Element == control);
        if (layer is null) return;
        _ = _layers.Remove(layer);

        control.RemoveHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosingAsync);
        control.RemoveHandler(OverlayDialog.LayerChangedEvent, OnDialogLayerChanged);
        layer.Mask?.RemoveHandler(PointerPressedEvent, DragMaskToMoveWindow);
        layer.Mask?.RemoveHandler(PointerReleasedEvent, ClickMaskToCloseDialog);

        // Trigger disappear animations while elements are still in the visual tree, then wait.
        // The dialog XAML transition fires automatically (IsClosed → true via OnClosed class handler).
        // No ConfigureAwait(false): the continuation must run on the UI thread to safely remove Children.
        if (!IsAnimationDisabled)
        {
            if (layer.Mask is not null) TriggerMaskDisappear(layer.Mask, MaskDisappearDuration);
            await Task.Delay(DialogDisappearDurationMs).ConfigureAwait(true);
        }

        _ = Children.Remove(control);

        if (layer.Mask is not null)
        {
            _ = Children.Remove(layer.Mask);
            if (layer.Modal)
            {
                _modalCount--;
                IsInModalStatus = _modalCount > 0;
            }
        }

        ResetZIndices();
    }

    /// <summary>
    ///     Add a dialog as a modal dialog to the host.
    /// </summary>
    /// <param name="control">.</param>
    public void AddModalDialog(OverlayDialog control)
    {
        var mask = CreateOverlayMask(true, control.CanLightDismiss);
        _layers.Add(new(mask, control));
        control.SetAsModal(true);
        ResetZIndices();
        Children.Add(mask);
        Children.Add(control);
        if (control.IsFullScreen)
        {
            control.Width = Bounds.Width;
            control.Height = Bounds.Height;
        }

        control.MaxWidth = Bounds.Width;
        control.MaxHeight = Bounds.Height;
        control.Measure(Bounds.Size);
        control.Arrange(new(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosingAsync);
        control.AddHandler(OverlayDialog.LayerChangedEvent, OnDialogLayerChanged);

        // Trigger mask appear transition (opacity 0 → 1 over 200ms).
        if (!IsAnimationDisabled) TriggerMaskAppear(mask, MaskAppearDuration);
        else mask.Opacity = 1.0;

        var element = control.GetVisualDescendants().OfType<InputElement>()
                             .FirstOrDefault(FocusBehavior.GetDialogFocusHint);
        element ??= control.GetVisualDescendants().OfType<InputElement>().FirstOrDefault(a => a.Focusable);
        _ = element?.Focus();
        _modalCount++;
        IsInModalStatus = _modalCount > 0;

        // IsClosed = false triggers the XAML appear transition (opacity 0 → 1, scale 0.95 → 1 over 200ms).
        control.IsClosed = false;
    }

    // Handle dialog layer change event
    private void OnDialogLayerChanged(object? sender, OverlayDialogLayerChangeEventArgs e)
    {
        if (sender is not OverlayDialog control)
            return;
        var layer = _layers.FirstOrDefault(a => a.Element == control);
        if (layer is null) return;
        var index = _layers.IndexOf(layer);
        _ = _layers.Remove(layer);
        var newIndex = e.ChangeType switch
        {
            OverlayDialogLayerChangeType.BringForward => (index + 1).SafeClamp(0, _layers.Count),
            OverlayDialogLayerChangeType.SendBackward => (index - 1).SafeClamp(0, _layers.Count),
            OverlayDialogLayerChangeType.BringToFront => _layers.Count,
            OverlayDialogLayerChangeType.SendToBack => 0,
            _ => index
        };

        _layers.Insert(newIndex, layer);
        ResetZIndices();
    }

    private void SetToPosition(OverlayDialog? control)
    {
        if (control is null) return;
        var left = GetLeftPosition(control);
        var top = GetTopPosition(control);
        SetLeft(control, left);
        SetTop(control, top);
        control.AnchorAndUpdatePositionInfo();
    }

    private double GetLeftPosition(OverlayDialog control)
    {
        var offset = Math.Max(0, control.HorizontalOffset ?? 0);
        var left = Bounds.Width - control.Bounds.Width;
        switch (control.HorizontalAnchor)
        {
            case HorizontalPosition.Center:
                left *= 0.5;
                left = left.SafeClamp(0, Bounds.Width * 0.5);
                break;
            case HorizontalPosition.Left:
                left = left.SafeClamp(0, offset);
                break;

            case HorizontalPosition.Right:
                {
                    var leftOffset = Bounds.Width - control.Bounds.Width - offset;
                    leftOffset = Math.Max(0, leftOffset);
                    if (control.HorizontalOffset.HasValue) left = left.SafeClamp(0, leftOffset);
                    break;
                }

            default:
                throw new ArgumentOutOfRangeException(nameof(control));
        }

        return left;
    }

    private double GetTopPosition(OverlayDialog control)
    {
        var offset = Math.Max(0, control.VerticalOffset ?? 0);
        var top = Bounds.Height - control.Bounds.Height;
        switch (control.VerticalAnchor)
        {
            case VerticalPosition.Center:
                top *= 0.5;
                return top.SafeClamp(0, Bounds.Height * 0.5);
            case VerticalPosition.Top:
                return top.SafeClamp(0, offset);

            case VerticalPosition.Bottom:
                {
                    var topOffset = Math.Max(0, Bounds.Height - control.Bounds.Height - offset);
                    return top.SafeClamp(0, topOffset);
                }

            default:
                throw new ArgumentOutOfRangeException(nameof(control));
        }
    }
}
