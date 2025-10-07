// -----------------------------------------------------------------------
// <copyright file="DialogResizer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.UI.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class DialogResizer : TemplatedControl
{
    public const string PartTop = "PART_Top";
    public const string PartBottom = "PART_Bottom";
    public const string PartLeft = "PART_Left";
    public const string PartRight = "PART_Right";
    public const string PartTopLeft = "PART_TopLeft";
    public const string PartTopRight = "PART_TopRight";
    public const string PartBottomLeft = "PART_BottomLeft";
    public const string PartBottomRight = "PART_BottomRight";

    private Thumb? _top;
    private Thumb? _bottom;
    private Thumb? _left;
    private Thumb? _right;
    private Thumb? _topLeft;
    private Thumb? _topRight;
    private Thumb? _bottomLeft;
    private Thumb? _bottomRight;

    public static readonly StyledProperty<ResizeDirection> ResizeDirectionProperty = AvaloniaProperty.Register<DialogResizer, ResizeDirection>(
        nameof(ResizeDirection), ResizeDirection.All);

    /// <summary>
    /// Gets or sets defines what direction the dialog is allowed to be resized.
    /// </summary>
    public ResizeDirection ResizeDirection
    {
        get => GetValue(ResizeDirectionProperty);
        set => SetValue(ResizeDirectionProperty, value);
    }

    static DialogResizer() => ResizeDirectionProperty.Changed.AddClassHandler<DialogResizer, ResizeDirection>((resizer, e) => resizer.OnResizeDirectionChanged(e));

    private void OnResizeDirectionChanged(AvaloniaPropertyChangedEventArgs<ResizeDirection> args) => UpdateThumbVisibility(args.NewValue.Value);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _top = e.NameScope.Find<Thumb>(PartTop);
        _bottom = e.NameScope.Find<Thumb>(PartBottom);
        _left = e.NameScope.Find<Thumb>(PartLeft);
        _right = e.NameScope.Find<Thumb>(PartRight);
        _topLeft = e.NameScope.Find<Thumb>(PartTopLeft);
        _topRight = e.NameScope.Find<Thumb>(PartTopRight);
        _bottomLeft = e.NameScope.Find<Thumb>(PartBottomLeft);
        _bottomRight = e.NameScope.Find<Thumb>(PartBottomRight);
        UpdateThumbVisibility(ResizeDirection);
    }

    private void UpdateThumbVisibility(ResizeDirection direction)
    {
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.Top), _top);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.Bottom), _bottom);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.Left), _left);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.Right), _right);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.TopLeft), _topLeft);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.TopRight), _topRight);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.BottomLeft), _bottomLeft);
        IsVisibleProperty.SetValue(direction.HasFlag(ResizeDirection.BottomRight), _bottomRight);
    }
}
