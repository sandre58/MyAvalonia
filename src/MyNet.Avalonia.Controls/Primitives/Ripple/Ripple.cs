// -----------------------------------------------------------------------
// <copyright file="Ripple.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using Avalonia;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Rendering.Composition;
using Avalonia.Threading;
using Avalonia.VisualTree;

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
    private IInputElement? _interactiveParent;

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

        AttachInteractiveParent();
    }

    private (Vector Size, Vector3D Offset) ComputeContainerLayout(Size size)
    {
        var newSize = new Vector(size.Width * SizeMultiplier, size.Height * SizeMultiplier);
        var newOffset = new Vector3D(-(size.Width * (SizeMultiplier - 1) / 2), -(size.Height * (SizeMultiplier - 1) / 2), 0);

        return (newSize, newOffset);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        DetachInteractiveParent();

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
        TryStartRipple(x, y, IsCentered);
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

    private void InteractiveParentKeyDownHandler(object? sender, KeyEventArgs e)
    {
        if (e.Key is not Key.Space and not Key.Enter)
            return;

        if (!ShouldRespondToKeyboardActivation())
            return;

        TryStartRipple(Bounds.Width / 2, Bounds.Height / 2, isCentered: true);
    }

    private void InteractiveParentKeyUpHandler(object? sender, KeyEventArgs e)
    {
        if (e.Key is not Key.Space and not Key.Enter)
            return;

        if (!ShouldRespondToKeyboardActivation())
            return;

        _isCancelled = true;
        RemoveLastRipple();
    }

    private bool ShouldRespondToKeyboardActivation()
    {
        if (_interactiveParent is not Visual interactiveParent)
            return false;

        var focused = TopLevel.GetTopLevel(this)?.FocusManager?.GetFocusedElement() as Visual;
        if (focused is null)
            return false;

        if (!ReferenceEquals(focused, interactiveParent)
            && !interactiveParent.IsVisualAncestorOf(focused))
            return false;

        if (ReferenceEquals(focused, interactiveParent))
            return CountRipplesUnder(interactiveParent) == 1;

        if (focused.IsVisualAncestorOf(this))
            return true;

        return this.IsVisualAncestorOf(focused);
    }

    private static int CountRipplesUnder(Visual root) =>
        root.GetVisualDescendants().OfType<Ripple>().Count();

    private void TryStartRipple(double x, double y, bool isCentered)
    {
        if (_container is null || RippleFill is null)
            return;

        if (x < 0 || x > Bounds.Width || y < 0 || y > Bounds.Height)
            return;

        _isCancelled = false;

        if (!IsActive || _pointers != 0)
            return;

        _pointers++;
        var r = CreateRipple(x, y, isCentered);
        _last = r;

        _container.Children.Add(r);
        r.SendHandlerMessage(RippleHandler.FirstStepMessage);

        if (_isCancelled)
            RemoveLastRipple();
    }

    private void RemoveLastRipple()
    {
        if (_last == null)
            return;

        _pointers--;
        OnReleaseHandler(_last);
        _last = null;
    }

    private void OnReleaseHandler(CompositionCustomVisual r)
    {
        r.SendHandlerMessage(RippleHandler.SecondStepMessage);

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
            center = new(width / 2, height / 2);
        }
        else
        {
            radius = Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
            center = new(x, y);
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

    private void AttachInteractiveParent()
    {
        DetachInteractiveParent();

        _interactiveParent = FindInteractiveParent();
        if (_interactiveParent is null)
            return;

        _interactiveParent.AddHandler(KeyDownEvent, InteractiveParentKeyDownHandler, RoutingStrategies.Tunnel);
        _interactiveParent.AddHandler(KeyUpEvent, InteractiveParentKeyUpHandler, RoutingStrategies.Tunnel);
    }

    private void DetachInteractiveParent()
    {
        if (_interactiveParent is null)
            return;

        _interactiveParent.RemoveHandler(KeyDownEvent, InteractiveParentKeyDownHandler);
        _interactiveParent.RemoveHandler(KeyUpEvent, InteractiveParentKeyUpHandler);
        _interactiveParent = null;
    }

    private IInputElement? FindInteractiveParent()
    {
        for (var current = Parent; current is not null; current = current.Parent)
        {
            if (current is IInputElement { Focusable: true } input)
                return input;
        }

        return null;
    }

    #region Styled properties

    public static readonly StyledProperty<IBrush?> RippleFillProperty =
        AvaloniaProperty.Register<Ripple, IBrush?>(nameof(RippleFill), defaultValue: Brushes.White, inherits: true);

    public IBrush? RippleFill
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
