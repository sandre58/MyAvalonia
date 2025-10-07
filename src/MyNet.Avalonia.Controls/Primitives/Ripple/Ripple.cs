// -----------------------------------------------------------------------
// <copyright file="Ripple.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Rendering.Composition;
using Avalonia.Threading;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class Ripple : ContentControl
{
    public static Easing Easing { get; set; } = new CircularEaseOut();

    public static TimeSpan Duration { get; set; } = new(0, 0, 0, 1, 200);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1450:Private fields only used as local variables in methods should become local variables", Justification = "False positive")]
    private bool _isCancelled;
    private CompositionContainerVisual? _container;
    private CompositionCustomVisual? _last;
    private byte _pointers;

    static Ripple() => BackgroundProperty.OverrideDefaultValue<Ripple>(Brushes.Transparent);

    public Ripple()
    {
        AddHandler(LostFocusEvent, LostFocusHandler);
        AddHandler(PointerReleasedEvent, PointerReleasedHandler);
        AddHandler(PointerPressedEvent, PointerPressedHandler);
        AddHandler(PointerCaptureLostEvent, PointerCaptureLostHandler);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        var thisVisual = ElementComposition.GetElementVisual(this)!;
        _container = thisVisual.Compositor.CreateContainerVisual();
        (_container.Size, _container.Offset) = ComputeContainerLayout(Bounds.Size);
        ElementComposition.SetElementChildVisual(this, _container);
    }

    private (Vector Size, Vector3D Offset) ComputeContainerLayout(Size size)
    {
        var newSize = new Vector(size.Width * SizeMultiplier, size.Height * SizeMultiplier);
        var newOffset = new Vector3D(-(size.Width * (SizeMultiplier - 1) / 2), -(size.Height * (SizeMultiplier - 1) / 2), 0);

        return (newSize, newOffset);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        _container = null;
        ElementComposition.SetElementChildVisual(this, null);
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);

        if (_container is not { } container)
            return;
        var layout = ComputeContainerLayout(e.NewSize);
        if (layout == default)
            return;
        container.Size = layout.Size;
        container.Offset = layout.Offset;
        foreach (var child in container.Children)
        {
            child.Size = layout.Size;
            child.Offset = layout.Offset;
        }
    }

    private void PointerPressedHandler(object? sender, PointerPressedEventArgs e)
    {
        var (x, y) = e.GetPosition(this);
        if (_container is null || x < 0 || x > Bounds.Width || y < 0 || y > Bounds.Height)
        {
            return;
        }

        _isCancelled = false;

        if (!IsActive)
            return;

        if (_pointers != 0)
            return;

        // Only first pointer can arrive a ripple
        _pointers++;
        var r = CreateRipple(x, y, IsCentered);
        _last = r;

        // Attach ripple instance to canvas
        _container.Children.Add(r);
        r.SendHandlerMessage(RippleHandler.FirstStepMessage);

        if (_isCancelled)
        {
            RemoveLastRipple();
        }
    }

    private void LostFocusHandler(object? sender, RoutedEventArgs e)
    {
        _isCancelled = true;
        RemoveLastRipple();
    }

    private void PointerReleasedHandler(object? sender, PointerReleasedEventArgs e)
    {
        _isCancelled = true;
        RemoveLastRipple();
    }

    private void PointerCaptureLostHandler(object? sender, PointerCaptureLostEventArgs e)
    {
        _isCancelled = true;
        RemoveLastRipple();
    }

    private void RemoveLastRipple()
    {
        if (_last == null)
            return;

        _pointers--;

        // This way to handle pointer released is pretty tricky
        // could have more better way to improve
        OnReleaseHandler(_last);
        _last = null;
    }

    private void OnReleaseHandler(CompositionCustomVisual r)
    {
        // Fade out ripple
        r.SendHandlerMessage(RippleHandler.SecondStepMessage);

        // Remove ripple from canvas to finalize ripple instance
        var container = _container;
        _ = DispatcherTimer.RunOnce(() => container?.Children.Remove(r), Duration, DispatcherPriority.Render);
    }

    private CompositionCustomVisual CreateRipple(double x, double y, bool isCentered)
    {
        var width = Bounds.Width * SizeMultiplier;
        var height = Bounds.Height * SizeMultiplier;
        Point center;
        double radius;

        if (isCentered)
        {
            radius = Math.Max(width / 2, height / 2);
            center = new Point(width / 2, height / 2);
        }
        else
        {
            radius = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            center = new Point(x, y);
        }

        var handler = new RippleHandler(
            RippleFill.ToImmutable(),
            Easing,
            Duration,
            RippleOpacity,
            center,
            radius,
            UseTransitions);

        var visual = ElementComposition.GetElementVisual(this)!.Compositor.CreateCustomVisual(handler);
        visual.Size = _container?.Size ?? default;
        return visual;
    }

    #region Styled properties

    public static readonly StyledProperty<IBrush> RippleFillProperty =
        AvaloniaProperty.Register<Ripple, IBrush>(nameof(RippleFill), defaultValue: Brushes.White, inherits: true);

    public IBrush RippleFill
    {
        get => GetValue(RippleFillProperty);
        set => SetValue(RippleFillProperty, value);
    }

    public static readonly StyledProperty<double> RippleOpacityProperty =
        AvaloniaProperty.Register<Ripple, double>(nameof(RippleOpacity), defaultValue: 0.6, inherits: true);

    public double RippleOpacity
    {
        get => GetValue(RippleOpacityProperty);
        set => SetValue(RippleOpacityProperty, value);
    }

    public static readonly StyledProperty<bool> IsCenteredProperty =
        AvaloniaProperty.Register<Ripple, bool>(nameof(IsCentered));

    public bool IsCentered
    {
        get => GetValue(IsCenteredProperty);
        set => SetValue(IsCenteredProperty, value);
    }

    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<Ripple, bool>(nameof(IsActive), defaultValue: true);

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public static readonly StyledProperty<bool> UseTransitionsProperty =
        AvaloniaProperty.Register<Ripple, bool>(nameof(UseTransitions), defaultValue: true);

    public bool UseTransitions
    {
        get => GetValue(UseTransitionsProperty);
        set => SetValue(UseTransitionsProperty, value);
    }

    public static readonly StyledProperty<double> SizeMultiplierProperty =
        AvaloniaProperty.Register<Ripple, double>(nameof(SizeMultiplier), defaultValue: 1.0);

    public double SizeMultiplier
    {
        get => GetValue(SizeMultiplierProperty);
        set => SetValue(SizeMultiplierProperty, value);
    }

    #endregion Styled properties
}
