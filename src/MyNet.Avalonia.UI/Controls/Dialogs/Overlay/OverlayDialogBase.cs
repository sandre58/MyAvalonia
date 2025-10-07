// -----------------------------------------------------------------------
// <copyright file="OverlayDialogBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartCloseButton, typeof(Button))]
[TemplatePart(PartTitleArea, typeof(Panel))]
[PseudoClasses(PseudoClassName.Modal, PseudoClassName.FullScreen)]
public abstract class OverlayDialogBase : OverlayFeedbackElement
{
    public const string PartCloseButton = "PART_CloseButton";
    public const string PartTitleArea = "PART_TitleArea";

    public static readonly DirectProperty<OverlayDialogBase, bool> IsFullScreenProperty =
        AvaloniaProperty.RegisterDirect<OverlayDialogBase, bool>(
            nameof(IsFullScreen), o => o.IsFullScreen, (o, v) => o.IsFullScreen = v);

    public static readonly StyledProperty<bool> CanResizeProperty = AvaloniaProperty.Register<OverlayDialogBase, bool>(
        nameof(CanResize));

    private Panel? _titleArea;
    private bool _moveDragging;
    private Point _moveDragStartPoint;

    static OverlayDialogBase()
    {
        _ = CanDragMoveProperty.Changed.AddClassHandler<InputElement, bool>(OnCanDragMoveChanged);
        _ = CanCloseProperty.Changed.AddClassHandler<InputElement, bool>(OnCanCloseChanged);
        IsFullScreenProperty.AffectsPseudoClass<OverlayDialogBase>(PseudoClassName.FullScreen);
    }

    protected internal Button? CloseButton { get; set; }

    public bool CanResize
    {
        get => GetValue(CanResizeProperty);
        set => SetValue(CanResizeProperty, value);
    }

    internal HorizontalPosition HorizontalAnchor { get; set; } = HorizontalPosition.Center;

    internal VerticalPosition VerticalAnchor { get; set; } = VerticalPosition.Center;

    internal HorizontalPosition ActualHorizontalAnchor { get; set; }

    internal VerticalPosition ActualVerticalAnchor { get; set; }

    internal double? HorizontalOffset { get; set; }

    internal double? VerticalOffset { get; set; }

    internal double? HorizontalOffsetRatio { get; set; }

    internal double? VerticalOffsetRatio { get; set; }

    internal bool CanLightDismiss { get; set; }

    internal bool? IsCloseButtonVisible { get; set; }

    public bool IsFullScreen
    {
        get;
        set => SetAndRaise(IsFullScreenProperty, ref field, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _titleArea = e.NameScope.Find<Panel>(PartTitleArea);
        if (GetCanDragMove(this))
        {
            _titleArea?.RemoveHandler(PointerMovedEvent, OnTitlePointerMove);
            _titleArea?.RemoveHandler(PointerPressedEvent, OnTitlePointerPressed);
            _titleArea?.RemoveHandler(PointerReleasedEvent, OnTitlePointerRelease);

            _titleArea?.AddHandler(PointerMovedEvent, OnTitlePointerMove, RoutingStrategies.Bubble);
            _titleArea?.AddHandler(PointerPressedEvent, OnTitlePointerPressed, RoutingStrategies.Bubble);
            _titleArea?.AddHandler(PointerReleasedEvent, OnTitlePointerRelease, RoutingStrategies.Bubble);
        }
        else
        {
            _titleArea?.IsHitTestVisible = false;
        }

        Button.ClickEvent.RemoveHandler(OnCloseButtonClick, CloseButton);
        CloseButton = e.NameScope.Find<Button>(PartCloseButton);
        Button.ClickEvent.AddHandler(OnCloseButtonClick, CloseButton);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Used by AddHandler")]
    private void OnTitlePointerPressed(InputElement sender, PointerPressedEventArgs e)
    {
        if (ContainerPanel is OverlayDialogHost { IsTopLevel: true } && IsFullScreen)
        {
            var top = TopLevel.GetTopLevel(this);
            if (top is Window w)
            {
                w.BeginMoveDrag(e);
                return;
            }
        }

        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        if (IsFullScreen) return;
        _moveDragging = true;
        _moveDragStartPoint = e.GetPosition(this);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Used by AddHandler")]
    private void OnTitlePointerMove(InputElement sender, PointerEventArgs e)
    {
        if (!_moveDragging) return;
        if (ContainerPanel is null) return;
        var p = e.GetPosition(this);
        var left = Canvas.GetLeft(this) + p.X - _moveDragStartPoint.X;
        var top = Canvas.GetTop(this) + p.Y - _moveDragStartPoint.Y;
        left = left.SafeClamp(0, ContainerPanel.Bounds.Width - Bounds.Width);
        top = top.SafeClamp(0, ContainerPanel.Bounds.Height - Bounds.Height);
        Canvas.SetLeft(this, left);
        Canvas.SetTop(this, top);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Used by AddHandler")]
    private void OnTitlePointerRelease(InputElement sender, PointerReleasedEventArgs e)
    {
        _moveDragging = false;
        AnchorAndUpdatePositionInfo();
    }

    private void OnCloseButtonClick(object? sender, RoutedEventArgs args) => Close();

    internal void SetAsModal(bool modal) => PseudoClasses.Set(PseudoClassName.Modal, modal);

    #region Layer Management

    public static readonly RoutedEvent<OverlayDialogLayerChangeEventArgs> LayerChangedEvent =
        RoutedEvent.Register<OverlayDialog, OverlayDialogLayerChangeEventArgs>(
            nameof(LayerChanged), RoutingStrategies.Bubble);

    public event EventHandler<OverlayDialogLayerChangeEventArgs> LayerChanged
    {
        add => AddHandler(LayerChangedEvent, value);
        remove => RemoveHandler(LayerChangedEvent, value);
    }

    public void UpdateLayer(object? o)
    {
        if (o is OverlayDialogLayerChangeType t) RaiseEvent(new OverlayDialogLayerChangeEventArgs(LayerChangedEvent, t));
    }

    #endregion

    #region DragMove AttachedPropert

    public static readonly AttachedProperty<bool> CanDragMoveProperty =
        AvaloniaProperty.RegisterAttached<OverlayDialogBase, InputElement, bool>("CanDragMove");

    public static void SetCanDragMove(InputElement obj, bool value) => obj.SetValue(CanDragMoveProperty, value);

    public static bool GetCanDragMove(InputElement obj) => obj.GetValue(CanDragMoveProperty);

    private static void OnCanDragMoveChanged(InputElement arg1, AvaloniaPropertyChangedEventArgs<bool> arg2)
    {
        if (arg2.NewValue.Value)
        {
            arg1.AddHandler(PointerPressedEvent, onPointerPressed, RoutingStrategies.Bubble);
            arg1.AddHandler(PointerMovedEvent, onPointerMoved, RoutingStrategies.Bubble);
            arg1.AddHandler(PointerReleasedEvent, onPointerReleased, RoutingStrategies.Bubble);
        }
        else
        {
            arg1.RemoveHandler(PointerPressedEvent, onPointerPressed);
            arg1.RemoveHandler(PointerMovedEvent, onPointerMoved);
            arg1.RemoveHandler(PointerReleasedEvent, onPointerReleased);
        }

        void onPointerPressed(InputElement sender, PointerPressedEventArgs e)
        {
            if (sender.FindLogicalAncestorOfType<OverlayDialogBase>() is { } dialog) e.Source = dialog;
        }

        void onPointerMoved(InputElement sender, PointerEventArgs e)
        {
            if (sender.FindLogicalAncestorOfType<OverlayDialogBase>() is { } dialog) e.Source = dialog;
        }

        void onPointerReleased(InputElement sender, PointerReleasedEventArgs e)
        {
            if (sender.FindLogicalAncestorOfType<OverlayDialogBase>() is { } dialog) e.Source = dialog;
        }
    }

    #endregion

    #region Close AttachedProperty

    public static readonly AttachedProperty<bool> CanCloseProperty =
        AvaloniaProperty.RegisterAttached<OverlayDialogBase, InputElement, bool>("CanClose");

    public static void SetCanClose(InputElement obj, bool value) => obj.SetValue(CanCloseProperty, value);

    public static bool GetCanClose(InputElement obj) => obj.GetValue(CanCloseProperty);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Used by AddHandler")]
    private static void OnCanCloseChanged(InputElement arg1, AvaloniaPropertyChangedEventArgs<bool> arg2)
    {
        if (arg2.NewValue.Value) arg1.AddHandler(PointerPressedEvent, onPointerPressed, RoutingStrategies.Bubble);

        static void onPointerPressed(InputElement sender, PointerPressedEventArgs args)
        {
            if (sender.FindLogicalAncestorOfType<OverlayDialogBase>() is { } dialog) dialog.Close();
        }
    }

    #endregion

    protected internal override void AnchorAndUpdatePositionInfo()
    {
        if (ContainerPanel is null) return;
        ActualHorizontalAnchor = HorizontalPosition.Center;
        ActualVerticalAnchor = VerticalPosition.Center;
        var left = Canvas.GetLeft(this);
        var top = Canvas.GetTop(this);
        var right = ContainerPanel.Bounds.Width - left - Bounds.Width;
        var bottom = ContainerPanel.Bounds.Height - top - Bounds.Height;
        if (ContainerPanel is OverlayDialogHost h)
        {
            var snapThickness = h.SnapThickness;
            if (top < snapThickness.Top)
            {
                Canvas.SetTop(this, 0);
                ActualVerticalAnchor = VerticalPosition.Top;
                VerticalOffsetRatio = 0;
            }

            if (bottom < snapThickness.Bottom)
            {
                Canvas.SetTop(this, ContainerPanel.Bounds.Height - Bounds.Height);
                ActualVerticalAnchor = VerticalPosition.Bottom;
                VerticalOffsetRatio = 1;
            }

            if (left < snapThickness.Left)
            {
                Canvas.SetLeft(this, 0);
                ActualHorizontalAnchor = HorizontalPosition.Left;
                HorizontalOffsetRatio = 0;
            }

            if (right < snapThickness.Right)
            {
                Canvas.SetLeft(this, ContainerPanel.Bounds.Width - Bounds.Width);
                ActualHorizontalAnchor = HorizontalPosition.Right;
                HorizontalOffsetRatio = 1;
            }
        }

        left = Canvas.GetLeft(this);
        top = Canvas.GetTop(this);
        right = ContainerPanel.Bounds.Width - left - Bounds.Width;
        bottom = ContainerPanel.Bounds.Height - top - Bounds.Height;

        HorizontalOffsetRatio = (left + right).IsZero() ? 0 : left / (left + right);
        VerticalOffsetRatio = (top + bottom).IsZero() ? 0 : top / (top + bottom);
    }
}
