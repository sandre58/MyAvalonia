// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHost.Shared.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.UI.Controls.Primitives;
using MyNet.Avalonia.UI.Dialogs;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public partial class OverlayDialogHost : Canvas
{
    private static readonly Animation MaskAppearAnimation = CreateOpacityAnimation(true);
    private static readonly Animation MaskDisappearAnimation = CreateOpacityAnimation(false);

    private readonly List<DialogPair> _layers = new(10);

    static OverlayDialogHost() => ClipToBoundsProperty.OverrideDefaultValue<OverlayDialogHost>(true);

    private int _modalCount;

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

    public bool IsTopLevel { get; set; }

    private static Animation CreateOpacityAnimation(bool appear)
    {
        var animation = new Animation
        {
            FillMode = FillMode.Forward
        };
        var keyFrame1 = new KeyFrame { Cue = new Cue(0.0) };
        keyFrame1.Setters.Add(new Setter { Property = OpacityProperty, Value = appear ? 0.0 : 1.0 });
        var keyFrame2 = new KeyFrame { Cue = new Cue(1.0) };
        keyFrame2.Setters.Add(new Setter { Property = OpacityProperty, Value = appear ? 1.0 : 0.0 });
        animation.Children.Add(keyFrame1);
        animation.Children.Add(keyFrame2);
        animation.Duration = TimeSpan.FromSeconds(0.2);
        return animation;
    }

    public string? HostId { get; set; }

    public static readonly StyledProperty<IBrush?> OverlayMaskBrushProperty =
        AvaloniaProperty.Register<OverlayDialogHost, IBrush?>(
            nameof(OverlayMaskBrush));

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
            IsVisible = true
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
    private int? _toplevelHash;

    protected sealed override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _toplevelHash = TopLevel.GetTopLevel(this)?.GetHashCode();
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

        OverlayDialogHostManager.RegisterHost(this, HostId, _toplevelHash);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        while (_layers.Count > 0)
        {
            _layers[0].Element.Close();
        }

        _modalStatusSubscription?.Dispose();
        OverlayDialogHostManager.UnregisterHost(HostId, _toplevelHash);
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
                case OverlayDialogBase d:
                    ResetDialogPosition(d, e.NewSize);
                    break;
                case DrawerBase drawer:
                    ResetDrawerPosition(drawer, e.NewSize);
                    break;
                default:
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

    internal T? Recall<T>()
    {
        var element = _layers.LastOrDefault(a => a.Element.Content?.GetType() == typeof(T));
        return element?.Element.Content is T t ? t : default;
    }

    private sealed class DialogPair(PureRectangle? mask, OverlayFeedbackElement element, bool modal = true)
    {
        internal PureRectangle? Mask { get; } = mask;

        internal OverlayFeedbackElement Element { get; } = element;

        internal bool Modal { get; } = modal;
    }
}
