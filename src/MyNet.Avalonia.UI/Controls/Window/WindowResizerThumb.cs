// -----------------------------------------------------------------------
// <copyright file="WindowResizerThumb.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class WindowResizerThumb : Thumb
{
    private Window? _window;

    public static readonly StyledProperty<ResizeDirection> ResizeDirectionProperty = AvaloniaProperty.Register<WindowResizerThumb, ResizeDirection>(
        nameof(ResizeDirection));

    public ResizeDirection ResizeDirection
    {
        get => GetValue(ResizeDirectionProperty);
        set => SetValue(ResizeDirectionProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _window = TopLevel.GetTopLevel(this) as Window;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (_window?.CanResize != true) return;

        // TODO: Support touch screen resizing but we don't know what it should behave.
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        var windowEdge = ResizeDirection switch
        {
            ResizeDirection.Top => WindowEdge.North,
            ResizeDirection.TopRight => WindowEdge.NorthEast,
            ResizeDirection.Right => WindowEdge.East,
            ResizeDirection.BottomRight => WindowEdge.SouthEast,
            ResizeDirection.Bottom => WindowEdge.South,
            ResizeDirection.BottomLeft => WindowEdge.SouthWest,
            ResizeDirection.Left => WindowEdge.West,
            ResizeDirection.TopLeft => WindowEdge.NorthWest,
            ResizeDirection.Sides => throw new InvalidOperationException(),
            ResizeDirection.Corners => throw new InvalidOperationException(),
            ResizeDirection.All => throw new InvalidOperationException(),
            _ => throw new InvalidOperationException()
        };
        _window.BeginResizeDrag(windowEdge, e);
    }
}
