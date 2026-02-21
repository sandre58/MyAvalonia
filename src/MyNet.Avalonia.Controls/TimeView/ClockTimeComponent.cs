// -----------------------------------------------------------------------
// <copyright file="ClockTimeComponent.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder Structure

public class ClockTimeComponent : TemplatedControl, IComponentTimeSelector
{
    private readonly Dictionary<int, ClockTimeComponentCell> _cachedAccessors = [];
    private Panel? _cellPanel;

    private bool _isDragging;
    private Control? _pointer;
    private Control? _pointerPin;

    private int? _value;

    public event EventHandler? IsDragged;

    public event EventHandler<ValueChangedEventArgs<int>>? ValueChanged;

    public static readonly RoutedEvent<ValueChangedEventArgs<int>> ValueChangedEvent = RoutedEvent.Register<ClockTimeComponent, ValueChangedEventArgs<int>>(nameof(ValueChanged), RoutingStrategies.Bubble);

    static ClockTimeComponent() { }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var pointer = e.NameScope.Find<Control>("PART_Pointer");
        var canvas = e.NameScope.Find<Canvas>("PART_CellPanel");
        var pointerPin = e.NameScope.Find<Control>("PART_PointerPin");

        _pointer = pointer;
        _pointerPin = pointerPin;
        _cellPanel = canvas;

        UpdateCellPanel();
        AdjustPointer();
        UpdateVisual(_value);
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == MinimumProperty ||
            change.Property == MaximumProperty ||
            change.Property == StepFrequencyProperty ||
            change.Property == RadiusMultiplierProperty)
        {
            OnNext();
        }
        else if (change.Property == ValueProperty)
        {
            var (removedValue, addedValue) = change.GetOldAndNewValue<int?>();
            OnValueChanged(removedValue, addedValue);
        }
        else if (change.Property == BoundsProperty)
        {
            OnCanvasResize();
        }
    }

    private void OnValueChanged(int? oldValue, int? newValue) => ValueChanged?.Invoke(this, new ValueChangedEventArgs<int>(ValueChangedEvent,  oldValue, newValue));

    #region Value

    public static readonly DirectProperty<ClockTimeComponent, int?> ValueProperty = AvaloniaProperty.RegisterDirect<ClockTimeComponent, int?>(nameof(Value), o => o.Value, (o, v) => o.Value = v, unsetValue: 0);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1721:Property names should not match get methods", Justification = "It's different")]
    public int? Value
    {
        get => _value;
        set
        {
            SetAndRaise(ValueProperty, ref _value, value);
            UpdateVisual(value);
        }
    }

    #endregion

    #region Minimum

    public static readonly StyledProperty<int> MinimumProperty = AvaloniaProperty.Register<ClockTimeComponent, int>(nameof(Minimum));

    public int Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    #endregion

    #region Maximum

    public static readonly StyledProperty<int> MaximumProperty = AvaloniaProperty.Register<ClockTimeComponent, int>(nameof(Maximum));

    public int Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    #endregion

    #region StepFrequency

    public static readonly StyledProperty<int> StepFrequencyProperty = AvaloniaProperty.Register<ClockTimeComponent, int>(nameof(StepFrequency), 1);

    public int StepFrequency
    {
        get => GetValue(StepFrequencyProperty);
        set => SetValue(StepFrequencyProperty, value);
    }

    #endregion

    #region RadiusMultiplier

    public static readonly StyledProperty<double> RadiusMultiplierProperty = AvaloniaProperty.Register<ClockTimeComponent, double>(nameof(RadiusMultiplier));

    public double RadiusMultiplier
    {
        get => GetValue(RadiusMultiplierProperty);
        set => SetValue(RadiusMultiplierProperty, value);
    }

    #endregion

    #region CellShiftNumber

    public static readonly StyledProperty<int> CellShiftNumberProperty = AvaloniaProperty.Register<ClockTimeComponent, int>(nameof(CellShiftNumber));

    public int CellShiftNumber
    {
        get => GetValue(CellShiftNumberProperty);
        set => SetValue(CellShiftNumberProperty, value);
    }

    #endregion

    #region HandBrush

    /// <summary>
    /// Provides HandBrush Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> HandBrushProperty = AvaloniaProperty.Register<ClockTimeComponent, IBrush?>(nameof(HandBrush));

    /// <summary>
    /// Gets or sets the HandBrush property.
    /// </summary>
    public IBrush? HandBrush
    {
        get => GetValue(HandBrushProperty);
        set => SetValue(HandBrushProperty, value);
    }

    #endregion

    #region CenterBackground

    /// <summary>
    /// Provides CenterBackground Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> CenterBackgroundProperty = AvaloniaProperty.Register<ClockTimeComponent, IBrush?>(nameof(CenterBackground));

    /// <summary>
    /// Gets or sets the CenterBackground property.
    /// </summary>
    public IBrush? CenterBackground
    {
        get => GetValue(CenterBackgroundProperty);
        set => SetValue(CenterBackgroundProperty, value);
    }

    #endregion

    #region CenterBorderBrush

    /// <summary>
    /// Provides CenterBorderBrush Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> CenterBorderBrushProperty = AvaloniaProperty.Register<ClockTimeComponent, IBrush?>(nameof(CenterBorderBrush));

    /// <summary>
    /// Gets or sets the CenterBorderBrush property.
    /// </summary>
    public IBrush? CenterBorderBrush
    {
        get => GetValue(CenterBorderBrushProperty);
        set => SetValue(CenterBorderBrushProperty, value);
    }

    #endregion

    #region CenterBorderThickness

    /// <summary>
    /// Provides CenterBorderThickness Property.
    /// </summary>
    public static readonly StyledProperty<double> CenterBorderThicknessProperty = AvaloniaProperty.Register<ClockTimeComponent, double>(nameof(CenterBorderThickness));

    /// <summary>
    /// Gets or sets the CenterBorderThickness property.
    /// </summary>
    public double CenterBorderThickness
    {
        get => GetValue(CenterBorderThicknessProperty);
        set => SetValue(CenterBorderThicknessProperty, value);
    }

    #endregion

    #region HandWidth

    public static readonly StyledProperty<double> HandWidthProperty = AvaloniaProperty.Register<ClockTimeComponent, double>(nameof(HandWidth), 2);

    public double HandWidth
    {
        get => GetValue(HandWidthProperty);
        set => SetValue(HandWidthProperty, value);
    }

    #endregion

    #region Mouse handlers

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        _isDragging = true;

        ProcessPointerEvent(e.GetPosition(this));
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!_isDragging)
            return;

        ProcessPointerEvent(e.GetPosition(this));
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        _isDragging = false;
        IsDragged?.Invoke(this, EventArgs.Empty);
    }

    private void ProcessPointerEvent(Point point)
    {
        var halfSize = (float)(Bounds.Width / 2);
        var rad = (float)Math.Atan2(point.Y - halfSize, point.X - halfSize);
        var degrees = (rad * 180 / Math.PI) + 90;

        if (degrees < 0)
            degrees += 360;

        if (degrees > 360)
            degrees -= 360;

        // degree to value
        var value = (int)Math.Round((degrees / 360 * (Maximum + 1 - Minimum)) + Minimum - CellShiftNumber);

        if (value == Maximum + 1)
            value = Minimum;

        if (!_cachedAccessors.TryGetValue(value, out var c))
            return;

        Value = c.Value;
    }

    #endregion

    private void OnCanvasResize()
    {
        UpdateCellPanel();
        AdjustPointer();
    }

    private void OnNext()
    {
        UpdateCellPanel();
        AdjustPointer();
    }

    private void UpdateVisual(int? currentValueNullable)
    {
        if (currentValueNullable is not { } currentValue)
        {
            _pointer?.IsVisible = false;
            return;
        }

        if (!_cachedAccessors.TryGetValue(currentValue, out var cell))
            return;

        foreach (var c in _cachedAccessors.Values)
        {
            c.IsSelected = false;

            if (!ReferenceEquals(c, cell))
                continue;

            c.IsSelected = true;
        }

        if (_pointer == null)
            return;
        _pointer.IsVisible = true;

        var degrees = (currentValue - Minimum + CellShiftNumber) * (360f / (Maximum + 1 - Minimum));

        var transform = (RotateTransform)(_pointer.RenderTransform ??= new RotateTransform());
        transform.Angle = degrees + 180;
    }

    private void UpdateCellPanel()
    {
        if (_cellPanel == null)
            return;

        var step = StepFrequency;
        var min = Minimum;
        var max = Maximum;
        var cellShift = CellShiftNumber;

        var radiusMultiplier = RadiusMultiplier;

        _cachedAccessors.Clear();
        _cellPanel.Children.Clear();

        for (var i = min; i <= max; i++)
        {
            var cell = new ClockTimeComponentCell
            {
                Value = i
            };

            if (step > 0)
            {
                if (i % step == 0)
                    cell.IsDot = false;
            }

            _cellPanel.Children.Add(cell);

            arrangeCell(cell, getAngle(i));

            _cachedAccessors.Add(i, cell);
        }

        UpdateVisual(Value);
        return;

        float getAngle(int value) => (value - min + cellShift) * (360f / (max + 1 - min));

        void arrangeCell(ClockTimeComponentCell cell, double degree)
        {
            var canvasBounds = _cellPanel.Bounds;

            var w = canvasBounds.Width;
            var h = canvasBounds.Height;

            var hW = w / 2;
            var hH = h / 2;

            var rad = (float)((degree - 90) * Math.PI / 180);

            var x = (float)(hW * radiusMultiplier * Math.Cos(rad)) + hW;
            var y = (float)(hH * radiusMultiplier * Math.Sin(rad)) + hH;

            cell.RenderTransform = new TranslateTransform(x, y);
        }
    }

    private void AdjustPointer()
    {
        if (_pointerPin == null)
            return;

        if (_cellPanel == null)
            return;

        var radius = _cellPanel.Bounds.Width / 2;
        _pointerPin.Height = radius * RadiusMultiplier;
    }
}
