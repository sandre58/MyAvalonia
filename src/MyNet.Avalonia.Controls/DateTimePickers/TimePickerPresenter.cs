// -----------------------------------------------------------------------
// <copyright file="TimePickerPresenter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities.Suspending;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartHourSelector, typeof(NumericUpDown))]
[TemplatePart(PartMinuteSelector, typeof(NumericUpDown))]
[TemplatePart(PartSecondSelector, typeof(NumericUpDown))]
[TemplatePart(PartPeriodSelector, typeof(ListBox))]
[TemplatePart(PartClockSelector, typeof(ClockSelector))]
[TemplatePart(PartFirstSeparator, typeof(Control))]
[TemplatePart(PartSecondSeparator, typeof(Control))]
[TemplatePart(PartThirdSeparator, typeof(Control))]
public class TimePickerPresenter : TemplatedControl
{
    public const string PartHourSelector = "PART_HourSelector";
    public const string PartMinuteSelector = "PART_MinuteSelector";
    public const string PartSecondSelector = "PART_SecondSelector";
    public const string PartPeriodSelector = "PART_PeriodSelector";
    public const string PartPmSelector = "PART_PmSelector";
    public const string PartClockSelector = "PART_ClockSelector";
    public const string PartFirstSeparator = "PART_FirstSeparator";
    public const string PartSecondSeparator = "PART_SecondSeparator";
    public const string PartThirdSeparator = "PART_ThirdSeparator";

    private readonly Suspender _valueChangingSuspender = new();

    private NumericUpDown? _hourNumericUpDown;
    private NumericUpDown? _minuteNumericUpDown;
    private NumericUpDown? _secondNumericUpDown;
    private Control? _firstSeparator;
    private Control? _secondSeparator;
    private Control? _thirdSeparator;
    private ListBox? _periodSelector;
    private ClockSelector? _clockSelector;
    private bool _use12Clock;
    private TimeSpan? _timeHolder;

    static TimePickerPresenter() => _ = PanelFormatProperty.Changed.AddClassHandler<TimePickerPresenter, string>((presenter, args) => presenter.OnPanelFormatChanged(args));

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        //if (_hourNumericUpDown is not null) _hourNumericUpDown.ValueChanged -= OnNumericUpDownValueChanged;
        //if (_minuteNumericUpDown is not null) _minuteNumericUpDown.ValueChanged -= OnNumericUpDownValueChanged;
        //if (_secondNumericUpDown is not null) _secondNumericUpDown.ValueChanged -= OnNumericUpDownValueChanged;
        //if (_periodSelector is not null) _periodSelector.SelectionChanged -= OnNumericUpDownValueChanged;

        _hourNumericUpDown = e.NameScope.Find<NumericUpDown>(PartHourSelector);
        _minuteNumericUpDown = e.NameScope.Find<NumericUpDown>(PartMinuteSelector);
        _secondNumericUpDown = e.NameScope.Find<NumericUpDown>(PartSecondSelector);
        _periodSelector = e.NameScope.Find<ListBox>(PartPeriodSelector);
        _clockSelector = e.NameScope.Find<ClockSelector>(PartClockSelector);
        _firstSeparator = e.NameScope.Find<Control>(PartFirstSeparator);
        _secondSeparator = e.NameScope.Find<Control>(PartSecondSeparator);
        _thirdSeparator = e.NameScope.Find<Control>(PartThirdSeparator);

        //if (_hourNumericUpDown is not null) _hourNumericUpDown.ValueChanged += OnNumericUpDownValueChanged;
        //if (_minuteNumericUpDown is not null) _minuteNumericUpDown.ValueChanged += OnNumericUpDownValueChanged;
        //if (_secondNumericUpDown is not null) _secondNumericUpDown.ValueChanged += OnNumericUpDownValueChanged;
        //if (_periodSelector is not null) _periodSelector.SelectionChanged += OnNumericUpDownValueChanged;

        UpdatePanelLayout(PanelFormat);
        //UpdateNumericUpDownsFromSelectedTime(_timeHolder);
    }

    #region SelectedTime

    public static readonly RoutedEvent<TimeChangedEventArgs> SelectedTimeChangedEvent = RoutedEvent.Register<TimePickerPresenter, TimeChangedEventArgs>(nameof(SelectedTimeChanged), RoutingStrategies.Bubble);

    public event EventHandler<TimeChangedEventArgs> SelectedTimeChanged
    {
        add => AddHandler(SelectedTimeChangedEvent, value);
        remove => RemoveHandler(SelectedTimeChangedEvent, value);
    }

    #endregion

    #region PanellFormat

    public static readonly StyledProperty<string> PanelFormatProperty = AvaloniaProperty.Register<TimePickerPresenter, string>(nameof(PanelFormat), "HH mm");

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    private void OnPanelFormatChanged(AvaloniaPropertyChangedEventArgs<string> args)
    {
        var format = args.NewValue.Value;
        UpdatePanelLayout(format);
    }

    #endregion

    #region NeedsConfirmation

    public static readonly StyledProperty<bool> NeedsConfirmationProperty = AvaloniaProperty.Register<TimePickerPresenter, bool>(nameof(NeedsConfirmation));

    public bool NeedsConfirmation
    {
        get => GetValue(NeedsConfirmationProperty);
        set => SetValue(NeedsConfirmationProperty, value);
    }

    #endregion

    private void UpdatePanelLayout(string? panelFormat)
    {
        if (panelFormat is null) return;
        var parts = panelFormat.Split([' ', '-', ':'], StringSplitOptions.RemoveEmptyEntries);
        var panels = new List<Control?>();
        foreach (var part in parts)
        {
            if (part.Length < 1) continue;
            try
            {
                if ((part.Contains('h', StringComparison.OrdinalIgnoreCase) || part.Contains('H', StringComparison.OrdinalIgnoreCase)) && !panels.Contains(_hourNumericUpDown))
                {
                    panels.Add(_hourNumericUpDown);
                    _use12Clock = !part.Equals("hh", StringComparison.OrdinalIgnoreCase);
                    if (_hourNumericUpDown is not null)
                    {
                        _hourNumericUpDown.Maximum = _use12Clock ? 12 : 23;
                        _hourNumericUpDown.Minimum = _use12Clock ? 1 : 0;
                    }
                }
                else if (part[0] == 'm' && !panels.Contains(_minuteNumericUpDown))
                {
                    panels.Add(_minuteNumericUpDown);
                }
                else if (part[0] == 's' && !panels.Contains(_secondNumericUpDown))
                {
                    panels.Add(_secondNumericUpDown);
                }
                else if (part[0] == 't' && !panels.Contains(_periodSelector))
                {
                    panels.Add(_periodSelector);
                }
            }
            catch
            {
                // ignored
            }
        }

        if (panels.Count < 1) return;
        IsVisibleProperty.SetValue(false, _hourNumericUpDown, _minuteNumericUpDown, _secondNumericUpDown, _periodSelector, _firstSeparator, _secondSeparator, _thirdSeparator);
        for (var i = 0; i < panels.Count; i++)
        {
            var panel = panels[i];
            if (panel is null) continue;
            panel.IsVisible = true;
            Grid.SetColumn(panel, 2 * i);
            var separator = i switch
            {
                0 => _firstSeparator,
                1 => _secondSeparator,
                2 => _thirdSeparator,
                _ => null
            };
            if (i != panels.Count - 1) IsVisibleProperty.SetValue(true, separator);
        }
    }

    //private void OnHourNumericUpDownValueChanged(object? sender, EventArgs e)
    //{
    //    if (_valueChangingSuspender.IsSuspended) return;
    //    if (!_use12Clock && Equals(sender, _periodSelector)) return;

    //    var time = _timeHolder ?? DateTime.Now.TimeOfDay;
    //    var hour = (int?)_hourNumericUpDown?.Value ?? time.Hours;
    //    var minute = (int?)_minuteNumericUpDown?.Value ?? time.Minutes;
    //    var second = (int?)_secondNumericUpDown?.Value ?? time.Seconds;
    //    var ampm = _periodSelector?.SelectedIndex ?? (time.Hours >= 12 ? 1 : 0);
    //    if (_use12Clock)
    //    {
    //        hour = ampm switch
    //        {
    //            0 when hour == 12 => 0,
    //            1 when hour < 12 => hour + 12,
    //            _ => hour
    //        };
    //    }
    //    else
    //    {
    //        ampm = hour switch
    //        {
    //            >= 12 => 1,
    //            _ => 0
    //        };
    //        using (_valueChangingSuspender.Suspend())
    //            _periodSelector?.SelectedIndex = ampm;
    //    }

    //    var newTime = new TimeSpan(hour, minute, second);
    //    if (NeedsConfirmation)
    //    {
    //        _timeHolder = newTime;
    //    }
    //    else
    //    {
    //        RaiseEvent(new TimeChangedEventArgs(null, newTime) { RoutedEvent = SelectedTimeChangedEvent });
    //    }
    //}

    //private void OnNumericUpDownValueChanged(object? sender, EventArgs e)
    //{
    //    if (_valueChangingSuspender.IsSuspended) return;
    //    if (!_use12Clock && Equals(sender, _periodSelector)) return;

    //    var time = _timeHolder ?? DateTime.Now.TimeOfDay;
    //    var hour = (int?)_hourNumericUpDown?.Value ?? time.Hours;
    //    var minute = (int?)_minuteNumericUpDown?.Value ?? time.Minutes;
    //    var second = (int?)_secondNumericUpDown?.Value ?? time.Seconds;
    //    var ampm = _periodSelector?.SelectedIndex ?? (time.Hours >= 12 ? 1 : 0);
    //    if (_use12Clock)
    //    {
    //        hour = ampm switch
    //        {
    //            0 when hour == 12 => 0,
    //            1 when hour < 12 => hour + 12,
    //            _ => hour
    //        };
    //    }
    //    else
    //    {
    //        ampm = hour switch
    //        {
    //            >= 12 => 1,
    //            _ => 0
    //        };
    //        using (_valueChangingSuspender.Suspend())
    //            _periodSelector?.SelectedIndex = ampm;
    //    }

    //    var newTime = new TimeSpan(hour, minute, second);
    //    if (NeedsConfirmation)
    //    {
    //        _timeHolder = newTime;
    //    }
    //    else
    //    {
    //        RaiseEvent(new TimeChangedEventArgs(null, newTime) { RoutedEvent = SelectedTimeChangedEvent });
    //    }
    //}

    //private void UpdateNumericUpDownsFromSelectedTime(TimeSpan? time)
    //{
    //    using (_valueChangingSuspender.Suspend())
    //    {
    //        if (time is null)
    //        {
    //            _hourNumericUpDown?.Value = null;
    //            _minuteNumericUpDown?.Value = null;
    //            _secondNumericUpDown?.Value = null;
    //            _periodSelector?.SelectedIndex = 0;
    //            return;
    //        }

    //        if (_hourNumericUpDown is not null)
    //        {
    //            var index = _use12Clock ? time.Value.Hours % 12 : time.Value.Hours;
    //            if (_use12Clock && index == 0) index = 12;
    //            _hourNumericUpDown.Value = index;
    //        }

    //        _minuteNumericUpDown?.Value = time.Value.Minutes;
    //        _secondNumericUpDown?.Value = time.Value.Seconds;
    //        var ampm = time.Value.Hours switch
    //        {
    //            >= 12 => 1,
    //            _ => 0
    //        };

    //        _periodSelector?.SelectedIndex = ampm;
    //        _periodSelector?.IsEnabled = _use12Clock;
    //    }
    //}

    //public void Confirm()
    //{
    //    if (NeedsConfirmation)
    //        RaiseEvent(new TimeChangedEventArgs(null, _timeHolder) { RoutedEvent = SelectedTimeChangedEvent });
    //}

    internal void MoveToTime(TimeSpan? time)
    {
        _timeHolder = time;
        //UpdateNumericUpDownsFromSelectedTime(time);
    }
}
