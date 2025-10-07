// -----------------------------------------------------------------------
// <copyright file="Clock.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Threading;
using System.Timers;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartClockTicks, typeof(ClockTicks))]
public sealed class Clock : TemplatedControl, IDisposable
{
    public const string PartClockTicks = "PART_ClockTicks";

    private readonly System.Timers.Timer _timer = new(1.Seconds());
    private CancellationTokenSource _cts = new();

    #region LiveUpdate

    /// <summary>
    /// Provides LiveUpdate Property.
    /// </summary>
    public static readonly StyledProperty<bool> LiveUpdateProperty = AvaloniaProperty.Register<Clock, bool>(nameof(LiveUpdate));

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the LiveUpdate property.
    /// </summary>
    public bool LiveUpdate
    {
        get => GetValue(LiveUpdateProperty);
        set => SetValue(LiveUpdateProperty, value);
    }

    #endregion

    #region TickBrush

    /// <summary>
    /// Provides TickBrush Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> TickBrushProperty = AvaloniaProperty.Register<Clock, IBrush?>(nameof(TickBrush));

    /// <summary>
    /// Gets or sets the TickBrush property.
    /// </summary>
    public IBrush? TickBrush
    {
        get => GetValue(TickBrushProperty);
        set => SetValue(TickBrushProperty, value);
    }

    #endregion

    #region CenterBackground

    /// <summary>
    /// Provides CenterBackground Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> CenterBackgroundProperty = AvaloniaProperty.Register<Clock, IBrush?>(nameof(CenterBackground));

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
    public static readonly StyledProperty<IBrush?> CenterBorderBrushProperty = AvaloniaProperty.Register<Clock, IBrush?>(nameof(CenterBorderBrush));

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
    public static readonly StyledProperty<double> CenterBorderThicknessProperty = AvaloniaProperty.Register<Clock, double>(nameof(CenterBorderThickness));

    /// <summary>
    /// Gets or sets the CenterBorderThickness property.
    /// </summary>
    public double CenterBorderThickness
    {
        get => GetValue(CenterBorderThicknessProperty);
        set => SetValue(CenterBorderThicknessProperty, value);
    }

    #endregion

    #region HourHandBrush

    /// <summary>
    /// Provides HourHandBrush Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> HourHandBrushProperty = AvaloniaProperty.Register<Clock, IBrush?>(nameof(HourHandBrush));

    /// <summary>
    /// Gets or sets the HourHandBrush property.
    /// </summary>
    public IBrush? HourHandBrush
    {
        get => GetValue(HourHandBrushProperty);
        set => SetValue(HourHandBrushProperty, value);
    }

    #endregion

    #region MinuteHandBrush

    /// <summary>
    /// Provides MinuteHandBrush Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> MinuteHandBrushProperty = AvaloniaProperty.Register<Clock, IBrush?>(nameof(MinuteHandBrush));

    /// <summary>
    /// Gets or sets the MinuteHandBrush property.
    /// </summary>
    public IBrush? MinuteHandBrush
    {
        get => GetValue(MinuteHandBrushProperty);
        set => SetValue(MinuteHandBrushProperty, value);
    }

    #endregion

    #region SecondHandBrush

    /// <summary>
    /// Provides SecondHandBrush Property.
    /// </summary>
    public static readonly StyledProperty<IBrush?> SecondHandBrushProperty = AvaloniaProperty.Register<Clock, IBrush?>(nameof(SecondHandBrush));

    /// <summary>
    /// Gets or sets the SecondHandBrush property.
    /// </summary>
    public IBrush? SecondHandBrush
    {
        get => GetValue(SecondHandBrushProperty);
        set => SetValue(SecondHandBrushProperty, value);
    }

    #endregion

    public static readonly StyledProperty<DateTime> TimeProperty = AvaloniaProperty.Register<Clock, DateTime>(
        nameof(Time), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> ShowHourTicksProperty =
        ClockTicks.ShowHourTicksProperty.AddOwner<Clock>();

    public static readonly StyledProperty<bool> ShowMinuteTicksProperty =
        ClockTicks.ShowMinuteTicksProperty.AddOwner<Clock>();

    public static readonly StyledProperty<bool> ShowHourHandProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(ShowHourHand), true);

    public static readonly StyledProperty<bool> ShowMinuteHandProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(ShowMinuteHand), true);

    public static readonly StyledProperty<bool> ShowSecondHandProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(ShowSecondHand), true);

    public static readonly StyledProperty<bool> IsSmoothProperty = AvaloniaProperty.Register<Clock, bool>(
        nameof(IsSmooth));

    public static readonly DirectProperty<Clock, double> HourAngleProperty =
        AvaloniaProperty.RegisterDirect<Clock, double>(
            nameof(HourAngle), o => o.HourAngle);

    public static readonly DirectProperty<Clock, double> MinuteAngleProperty =
        AvaloniaProperty.RegisterDirect<Clock, double>(
            nameof(MinuteAngle), o => o.MinuteAngle);

    public static readonly DirectProperty<Clock, double> SecondAngleProperty =
        AvaloniaProperty.RegisterDirect<Clock, double>(
            nameof(SecondAngle), o => o.SecondAngle, (o, v) => o.SecondAngle = v);

    static Clock()
    {
        _ = TimeProperty.Changed.AddClassHandler<Clock, DateTime>((clock, args) => clock.OnTimeChanged(args));
        _ = IsSmoothProperty.Changed.AddClassHandler<Clock, bool>((clock, args) => clock.OnIsSmoothChanged(args));
        _ = LiveUpdateProperty.Changed.AddClassHandler<Clock, bool>((clock, args) => clock.OnLiveUpdateChanged(args));
    }

    public Clock()
    {
        Time = DateTime.Now;
        _timer.Elapsed += TimerOnElapsed;
    }

    public DateTime Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public bool ShowHourTicks
    {
        get => GetValue(ShowHourTicksProperty);
        set => SetValue(ShowHourTicksProperty, value);
    }

    public bool ShowMinuteTicks
    {
        get => GetValue(ShowMinuteTicksProperty);
        set => SetValue(ShowMinuteTicksProperty, value);
    }

    public bool ShowHourHand
    {
        get => GetValue(ShowHourHandProperty);
        set => SetValue(ShowHourHandProperty, value);
    }

    public bool ShowMinuteHand
    {
        get => GetValue(ShowMinuteHandProperty);
        set => SetValue(ShowMinuteHandProperty, value);
    }

    public bool ShowSecondHand
    {
        get => GetValue(ShowSecondHandProperty);
        set => SetValue(ShowSecondHandProperty, value);
    }

    public bool IsSmooth
    {
        get => GetValue(IsSmoothProperty);
        set => SetValue(IsSmoothProperty, value);
    }

    public double HourAngle
    {
        get;
        private set => SetAndRaise(HourAngleProperty, ref field, value);
    }

    public double MinuteAngle
    {
        get;
        private set => SetAndRaise(MinuteAngleProperty, ref field, value);
    }

    public double SecondAngle
    {
        get;
        private set => SetAndRaise(SecondAngleProperty, ref field, value);
    }

    private readonly Animation _secondsAnimation = new()
    {
        FillMode = FillMode.Forward,
        Duration = TimeSpan.FromSeconds(1),
        Children =
        {
            new KeyFrame
            {
                Cue = new Cue(0.0),
                Setters = { new Setter { Property = SecondAngleProperty } }
            },
            new KeyFrame
            {
                Cue = new Cue(1.0),
                Setters = { new Setter { Property = SecondAngleProperty } }
            }
        }
    };

    private void OnIsSmoothChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.NewValue.Value && !_cts.IsCancellationRequested)
            _cts.Cancel();
    }

    private void OnLiveUpdateChanged(AvaloniaPropertyChangedEventArgs<bool> args)
    {
        if (args.NewValue.Value)
        {
            Time = DateTime.Now;
            _timer.Start();
        }
        else
        {
            _timer.Stop();
        }
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e) => Dispatcher.UIThread.Invoke(() => IsEnabled.IfTrue(() => Time = DateTime.Now));

    private void OnTimeChanged(AvaloniaPropertyChangedEventArgs<DateTime> args)
    {
        var oldSeconds = args.OldValue.Value.Second;
        var time = args.NewValue.Value;
        var hour = time.Hour;
        var minute = time.Minute;
        var second = time.Second;
        var hourAngle = (360.0 / 12 * hour) + (360.0 / 12 / 60 * minute);
        var minuteAngle = (360.0 / 60 * minute) + (360.0 / 60 / 60 * second);
        if (second == 0) second = 60;
        var oldSecondAngle = 360.0 / 60 * oldSeconds;
        var secondAngle = 360.0 / 60 * second;
        HourAngle = hourAngle;
        MinuteAngle = minuteAngle;
        if (!IsLoaded || !IsSmooth)
        {
            SecondAngle = secondAngle;
        }
        else
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
            if (_secondsAnimation.Children[0].Setters[0] is Setter start)
                start.Value = oldSecondAngle;
            if (_secondsAnimation.Children[1].Setters[0] is Setter end)
                end.Value = secondAngle;
            _ = _secondsAnimation.RunAsync(this, _cts.Token);
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var min = Math.Min(availableSize.Height, availableSize.Width);
        var newSize = new Size(min, min);
        var size = base.MeasureOverride(newSize);
        return size;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var min = Math.Min(finalSize.Height, finalSize.Width);
        var newSize = new Size(min, min);
        var size = base.ArrangeOverride(newSize);
        return size;
    }

    public void Dispose()
    {
        _timer.Dispose();
        _cts.Dispose();
    }
}
