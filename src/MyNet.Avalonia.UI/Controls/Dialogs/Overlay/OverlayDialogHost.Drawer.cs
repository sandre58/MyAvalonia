// -----------------------------------------------------------------------
// <copyright file="OverlayDialogHost.Drawer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.Threading;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Assists;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.UI.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public partial class OverlayDialogHost
{
    private static void ResetDrawerPosition(DrawerBase control, Size newSize)
    {
        switch (control.Position)
        {
            case Position.Right:
                control.Height = newSize.Height;
                SetLeft(control, newSize.Width - control.Bounds.Width);
                break;
            case Position.Left:
                control.Height = newSize.Height;
                SetLeft(control, 0);
                break;
            case Position.Top:
                control.Width = newSize.Width;
                SetTop(control, 0);
                break;
            case Position.Bottom:
                break;
            default:
                control.Width = newSize.Width;
                SetTop(control, newSize.Height - control.Bounds.Height);
                break;
        }
    }

    internal async Task AddDrawerAsync(DrawerBase control)
    {
        PureRectangle? mask = null;
        if (control.CanLightDismiss)
        {
            mask = CreateOverlayMask(false, true);
        }

        _layers.Add(new DialogPair(mask, control));
        ResetZIndices();
        if (mask is not null) Children.Add(mask);
        Children.Add(control);
        control.Measure(Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetDrawerPosition(control);
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDrawerControlClosingAsync);
        var animation = CreateAnimation(control.Bounds.Size, control.Position);
        if (IsAnimationDisabled)
        {
            ResetDrawerPosition(control, Bounds.Size);
        }
        else
        {
            if (mask is null)
            {
                await animation.RunAsync(control).ConfigureAwait(false);
            }
            else
            {
                await Task.WhenAll(animation.RunAsync(control), MaskAppearAnimation.RunAsync(mask)).ConfigureAwait(false);
            }
        }
    }

    internal async Task AddModalDrawerAsync(DrawerBase control)
    {
        var mask = CreateOverlayMask(true, control.CanLightDismiss);
        _layers.Add(new DialogPair(mask, control));
        Children.Add(mask);
        Children.Add(control);
        ResetZIndices();
        control.Measure(Bounds.Size);
        control.Arrange(new Rect(control.DesiredSize));
        SetDrawerPosition(control);
        _modalCount++;
        IsInModalStatus = _modalCount > 0;
        control.AddHandler(OverlayFeedbackElement.ClosedEvent, OnDrawerControlClosingAsync);
        var animation = CreateAnimation(control.Bounds.Size, control.Position);
        if (IsAnimationDisabled)
        {
            ResetDrawerPosition(control, Bounds.Size);
        }
        else
        {
            await Task.WhenAll(animation.RunAsync(control), MaskAppearAnimation.RunAsync(mask)).ConfigureAwait(false);
        }

        var element = control.GetVisualDescendants().OfType<InputElement>()
                             .FirstOrDefault(FocusAssist.GetDialogFocusHint);
        element ??= control.GetVisualDescendants().OfType<InputElement>().FirstOrDefault(a => a.Focusable);
        _ = element?.Focus();
    }

    private void SetDrawerPosition(DrawerBase control)
    {
        switch (control.Position)
        {
            case Position.Left or Position.Right:
                control.Height = Bounds.Height;
                break;
            case Position.Top or Position.Bottom:
                control.Width = Bounds.Width;
                break;
            default:
                break;
        }
    }

    private Animation CreateAnimation(Size elementBounds, Position position, bool appear = true)
    {
        // left or top.
        double source;
        double target;
        switch (position)
        {
            case Position.Left:
                source = appear ? -elementBounds.Width : 0;
                target = appear ? 0 : -elementBounds.Width;
                break;
            case Position.Right:
                source = appear ? Bounds.Width : Bounds.Width - elementBounds.Width;
                target = appear ? Bounds.Width - elementBounds.Width : Bounds.Width;
                break;
            case Position.Top:
                source = appear ? -elementBounds.Height : 0;
                target = appear ? 0 : -elementBounds.Height;
                break;
            case Position.Bottom:
                source = appear ? Bounds.Height : Bounds.Height - elementBounds.Height;
                target = appear ? Bounds.Height - elementBounds.Height : Bounds.Height;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(position), position, null);
        }

        var targetProperty = position is Position.Left or Position.Right ? LeftProperty : TopProperty;
        var animation = new Animation
        {
            Easing = new CubicEaseOut(),
            FillMode = FillMode.Forward
        };
        var keyFrame1 = new KeyFrame { Cue = new Cue(0.0) };
        keyFrame1.Setters.Add(new Setter { Property = targetProperty, Value = source });
        var keyFrame2 = new KeyFrame { Cue = new Cue(1.0) };
        keyFrame2.Setters.Add(new Setter { Property = targetProperty, Value = target });
        animation.Children.Add(keyFrame1);
        animation.Children.Add(keyFrame2);
        animation.Duration = TimeSpan.FromSeconds(0.3);
        return animation;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "Used by AddHandler")]
    private async Task OnDrawerControlClosingAsync(object? sender, ResultEventArgs args)
    {
        if (sender is DrawerBase control)
        {
            var layer = _layers.FirstOrDefault(a => a.Element == control);
            if (layer is null) return;
            _ = _layers.Remove(layer);
            control.RemoveHandler(OverlayFeedbackElement.ClosedEvent, OnDialogControlClosingAsync);
            control.RemoveHandler(OverlayDialogBase.LayerChangedEvent, OnDialogLayerChanged);
            if (layer.Mask is not null)
            {
                _modalCount--;
                IsInModalStatus = _modalCount > 0;
                layer.Mask.RemoveHandler(PointerPressedEvent, ClickMaskToCloseDialog);
                layer.Mask.RemoveHandler(PointerReleasedEvent, DragMaskToMoveWindow);
                if (!IsAnimationDisabled)
                {
                    var disappearAnimation = CreateAnimation(control.Bounds.Size, control.Position, false);
                    await Task.WhenAll(disappearAnimation.RunAsync(control), MaskDisappearAnimation.RunAsync(layer.Mask)).ConfigureAwait(false);
                }

                _ = Dispatcher.UIThread.Invoke(() => Children.Remove(layer.Mask));
            }
            else
            {
                if (!IsAnimationDisabled)
                {
                    var disappearAnimation = CreateAnimation(control.Bounds.Size, control.Position, false);
                    await disappearAnimation.RunAsync(control).ConfigureAwait(false);
                }
            }

            _ = Dispatcher.UIThread.Invoke(() => Children.Remove(control));
            ResetZIndices();
        }
    }
}
