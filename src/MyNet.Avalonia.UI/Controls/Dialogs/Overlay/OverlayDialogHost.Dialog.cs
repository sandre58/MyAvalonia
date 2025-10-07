// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHost.Dialog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Input;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.UI.Controls.Primitives;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public partial class OverlayDialogHost
{
    public Thickness SnapThickness { get; set; } = new(0);

    private static void ResetDialogPosition(OverlayDialogBase control, Size newSize)
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

    internal void AddDialog(OverlayDialogBase control)
    {
        PureRectangle? mask = null;
        if (control.CanLightDismiss) mask = CreateOverlayMask(false, control.CanLightDismiss);
        if (mask is not null) Children.Add(mask);
        Children.Add(control);
        _layers.Add(new DialogPair(mask, control, false));
        if (control.IsFullScreen)
        {
            control.Width = Bounds.Width;
            control.Height = Bounds.Height;
        }

        control.MaxWidth = Bounds.Width;
        control.MaxHeight = Bounds.Height;
        control.Measure(Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosingAsync);
        control.AddHandler(OverlayDialogBase.LayerChangedEvent, OnDialogLayerChanged);
        ResetZIndices();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Used by AddHandler")]
    private async Task OnDialogControlClosingAsync(object? sender, object? e)
    {
        if (sender is not OverlayDialogBase control) return;
        var layer = _layers.FirstOrDefault(a => a.Element == control);
        if (layer is null) return;
        _ = _layers.Remove(layer);

        control.RemoveHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosingAsync);
        control.RemoveHandler(OverlayDialogBase.LayerChangedEvent, OnDialogLayerChanged);
        layer.Mask?.RemoveHandler(PointerPressedEvent, DragMaskToMoveWindow);

        _ = Children.Remove(control);

        if (layer.Mask is not null)
        {
            _ = Children.Remove(layer.Mask);
            if (layer.Modal)
            {
                _modalCount--;
                IsInModalStatus = _modalCount > 0;
                if (!IsAnimationDisabled) await MaskDisappearAnimation.RunAsync(layer.Mask).ConfigureAwait(false);
            }
        }

        ResetZIndices();
    }

    /// <summary>
    ///     Add a dialog as a modal dialog to the host.
    /// </summary>
    /// <param name="control">.</param>
    internal void AddModalDialog(OverlayDialogBase control)
    {
        var mask = CreateOverlayMask(true, control.CanLightDismiss);
        _layers.Add(new DialogPair(mask, control));
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
        control.Arrange(new Rect(control.DesiredSize));
        SetToPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosingAsync);
        control.AddHandler(OverlayDialogBase.LayerChangedEvent, OnDialogLayerChanged);

        // Notice: mask animation here is not really awaited, because currently dialogs appears immediately.
        if (!IsAnimationDisabled) _ = MaskAppearAnimation.RunAsync(mask);

        var element = control.GetVisualDescendants().OfType<InputElement>()
                             .FirstOrDefault(FocusAssist.GetDialogFocusHint);
        element ??= control.GetVisualDescendants().OfType<InputElement>().FirstOrDefault(a => a.Focusable);
        _ = element?.Focus();
        _modalCount++;
        IsInModalStatus = _modalCount > 0;
        control.IsClosed = false;
    }

    // Handle dialog layer change event
    private void OnDialogLayerChanged(object? sender, OverlayDialogLayerChangeEventArgs e)
    {
        if (sender is not OverlayDialogBase control)
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

    private void SetToPosition(OverlayDialogBase? control)
    {
        if (control is null) return;
        var left = GetLeftPosition(control);
        var top = GetTopPosition(control);
        SetLeft(control, left);
        SetTop(control, top);
        control.AnchorAndUpdatePositionInfo();
    }

    private double GetLeftPosition(OverlayDialogBase control)
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

    private double GetTopPosition(OverlayDialogBase control)
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
