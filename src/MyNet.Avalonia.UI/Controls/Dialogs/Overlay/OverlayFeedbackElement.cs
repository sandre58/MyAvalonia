// -----------------------------------------------------------------------
// <copyright file="OverlayFeedbackElement.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using MyNet.UI.Dialogs.CustomDialogs;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public abstract class OverlayFeedbackElement : ContentControl
{
    private bool _resizeDragging;
    private Rect _resizeDragStartBounds;
    private Point _resizeDragStartPoint;

    private WindowEdge? _windowEdge;

    static OverlayFeedbackElement()
    {
        FocusableProperty.OverrideDefaultValue<OverlayFeedbackElement>(false);
        _ = DataContextProperty.Changed.AddClassHandler<OverlayFeedbackElement, object?>((o, e) =>
            o.OnDataContextChange(e));
        _ = ClosedEvent.AddClassHandler<OverlayFeedbackElement>((o, _) => o.OnClosed());
    }

    public static readonly StyledProperty<bool> IsClosedProperty = AvaloniaProperty.Register<OverlayFeedbackElement, bool>(nameof(IsClosed), true);

    public static readonly RoutedEvent<ResultEventArgs> ClosedEvent = RoutedEvent.Register<DrawerBase, ResultEventArgs>(nameof(Closed), RoutingStrategies.Bubble);

    protected Panel? ContainerPanel { get; set; }

    public bool IsClosed
    {
        get => GetValue(IsClosedProperty);
        set => SetValue(IsClosedProperty, value);
    }

    public event EventHandler<ResultEventArgs> Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        Content = null;
    }

    private void OnClosed() => SetCurrentValue(IsClosedProperty, true);

    private void OnDataContextChange(AvaloniaPropertyChangedEventArgs<object?> args)
    {
        if (args.OldValue.Value is IDialogViewModel oldContext) oldContext.CloseRequest -= OnContextRequestClose;
        if (args.NewValue.Value is IDialogViewModel newContext) newContext.CloseRequest += OnContextRequestClose;
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Used by children classes")]
    protected virtual void OnElementClosing(object? sender, object? args) => RaiseEvent(new ResultEventArgs(ClosedEvent, args));

    private void OnContextRequestClose(object? sender, object? args) => RaiseEvent(new ResultEventArgs(ClosedEvent, args));

    public Task<T?> ShowAsync<T>(CancellationToken? token = null)
    {
        var tcs = new TaskCompletionSource<T?>();
        _ = token?.Register(() => Dispatcher.UIThread.Invoke(Close));

        AddHandler(ClosedEvent, onCloseHandler);
        return tcs.Task;

        void onCloseHandler(object? sender, ResultEventArgs? args)
        {
            if (args?.Result is T result)
                tcs.SetResult(result);
            else
                tcs.SetResult(default);
            RemoveHandler(ClosedEvent, onCloseHandler);
        }
    }

    public abstract void Close();

    internal void BeginResizeDrag(WindowEdge windowEdge, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        _resizeDragging = true;
        _resizeDragStartPoint = e.GetPosition(this);
        _resizeDragStartBounds = Bounds;
        _windowEdge = windowEdge;
    }

    internal void BeginMoveDrag(PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        _resizeDragging = true;
        _resizeDragStartPoint = e.GetPosition(this);
        _resizeDragStartBounds = Bounds;
        _windowEdge = null;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        ContainerPanel = this.FindAncestorOfType<Panel>();
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        _resizeDragging = false;
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        _resizeDragging = false;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (!_resizeDragging || _windowEdge is null) return;
        var point = e.GetPosition(this);
        Vector diff = point - _resizeDragStartPoint;
        var left = Canvas.GetLeft(this);
        var top = Canvas.GetTop(this);
        var width = _windowEdge is WindowEdge.West or WindowEdge.NorthWest or WindowEdge.SouthWest
            ? Bounds.Width
            : _resizeDragStartBounds.Width;
        var height = _windowEdge is WindowEdge.North or WindowEdge.NorthEast or WindowEdge.NorthWest
            ? Bounds.Height
            : _resizeDragStartBounds.Height;
        var newBounds = CalculateNewBounds(left, top, width, height, diff, ContainerPanel?.Bounds, _windowEdge.Value);
        Canvas.SetLeft(this, newBounds.Left);
        Canvas.SetTop(this, newBounds.Top);
        SetCurrentValue(WidthProperty, newBounds.Width);
        SetCurrentValue(HeightProperty, newBounds.Height);
        AnchorAndUpdatePositionInfo();
    }

    private Rect CalculateNewBounds(double left, double top, double width, double height, Vector diff, Rect? containerBounds, WindowEdge windowEdge)
    {
        diff = CoerceDelta(left, top, width, height, diff, containerBounds, windowEdge);
        switch (windowEdge)
        {
            case WindowEdge.North:
                top += diff.Y;
                height -= diff.Y;
                break;
            case WindowEdge.NorthEast:
                top += diff.Y;
                width += diff.X;
                height -= diff.Y;
                break;
            case WindowEdge.East:
                width += diff.X;
                break;
            case WindowEdge.SouthEast:
                width += diff.X;
                height += diff.Y;
                break;
            case WindowEdge.South:
                height += diff.Y;
                break;
            case WindowEdge.SouthWest:
                left += diff.X;
                width -= diff.X;
                height += diff.Y;
                break;
            case WindowEdge.West:
                left += diff.X;
                width -= diff.X;
                break;
            case WindowEdge.NorthWest:
                left += diff.X;
                top += diff.Y;
                width -= diff.X;
                height -= diff.Y;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(windowEdge), windowEdge, null);
        }

        return new Rect(left, top, width, height);
    }

    private Vector CoerceDelta(double left, double top, double width, double height, Vector diff, Rect? containerBounds, WindowEdge windowEdge)
    {
        if (containerBounds is null) return diff;
        var minX = windowEdge is WindowEdge.West or WindowEdge.NorthWest or WindowEdge.SouthWest
            ? -left
            : -width;
        var minY = windowEdge is WindowEdge.North or WindowEdge.NorthEast or WindowEdge.NorthWest
            ? -top
            : -height;
        var maxX = windowEdge is WindowEdge.West or WindowEdge.NorthWest or WindowEdge.SouthWest
            ? width - MinWidth
            : containerBounds.Value.Width - left - width;
        var maxY = windowEdge is WindowEdge.North or WindowEdge.NorthEast or WindowEdge.NorthWest
            ? height - MinWidth
            : containerBounds.Value.Height - top - height;
        return new Vector(diff.X.SafeClamp(minX, maxX), diff.Y.SafeClamp(minY, maxY));
    }

    protected internal abstract void AnchorAndUpdatePositionInfo();
}
