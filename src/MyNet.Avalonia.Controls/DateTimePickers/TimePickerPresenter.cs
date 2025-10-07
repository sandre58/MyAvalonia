// -----------------------------------------------------------------------
// <copyright file="TimePickerPresenter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartPickerContainer, typeof(Grid))]
[TemplatePart(PartHourSelector, typeof(DateTimePickerPanel))]
[TemplatePart(PartMinuteSelector, typeof(DateTimePickerPanel))]
[TemplatePart(PartSecondSelector, typeof(DateTimePickerPanel))]
[TemplatePart(PartPeriodSelector, typeof(DateTimePickerPanel))]
[TemplatePart(PartHourScrollPanel, typeof(Control))]
[TemplatePart(PartMinuteScrollPanel, typeof(Control))]
[TemplatePart(PartSecondScrollPanel, typeof(Control))]
[TemplatePart(PartPeriodScrollPanel, typeof(Control))]
[TemplatePart(PartFirstSeparator, typeof(Control))]
[TemplatePart(PartSecondSeparator, typeof(Control))]
[TemplatePart(PartThirdSeparator, typeof(Control))]
public class TimePickerPresenter : TemplatedControl
{
    public const string PartHourSelector = "PART_HourSelector";
    public const string PartMinuteSelector = "PART_MinuteSelector";
    public const string PartSecondSelector = "PART_SecondSelector";
    public const string PartPeriodSelector = "PART_PeriodSelector";
    public const string PartPickerContainer = "PART_PickerContainer";

    public const string PartHourScrollPanel = "PART_HourScrollPanel";
    public const string PartMinuteScrollPanel = "PART_MinuteScrollPanel";
    public const string PartSecondScrollPanel = "PART_SecondScrollPanel";
    public const string PartPeriodScrollPanel = "PART_PeriodScrollPanel";

    public const string PartFirstSeparator = "PART_FirstSeparator";
    public const string PartSecondSeparator = "PART_SecondSeparator";
    public const string PartThirdSeparator = "PART_ThirdSeparator";

    public static readonly StyledProperty<bool> NeedsConfirmationProperty =
        AvaloniaProperty.Register<TimePickerPresenter, bool>(
            nameof(NeedsConfirmation));

    public static readonly StyledProperty<int> MinuteIncrementProperty =
        AvaloniaProperty.Register<TimePickerPresenter, int>(
            nameof(MinuteIncrement));

    public static readonly StyledProperty<int> SecondIncrementProperty =
        AvaloniaProperty.Register<TimePickerPresenter, int>(
            nameof(SecondIncrement));

    public static readonly StyledProperty<string> PanelFormatProperty =
        AvaloniaProperty.Register<TimePickerPresenter, string>(
            nameof(PanelFormat), "HH mm ss t");

    public static readonly RoutedEvent<TimeChangedEventArgs> SelectedTimeChangedEvent =
    RoutedEvent.Register<TimePickerPresenter, TimeChangedEventArgs>(
        nameof(SelectedTimeChanged), RoutingStrategies.Bubble);

    private Grid? _pickerContainer;
    private Control? _ampmScrollPanel;
    private DateTimePickerPanel? _periodSelector;
    private Control? _firstSeparator;
    private Control? _hourScrollPanel;

    private DateTimePickerPanel? _hourSelector;
    private Control? _minuteScrollPanel;
    private DateTimePickerPanel? _minuteSelector;
    private Control? _secondScrollPanel;
    private DateTimePickerPanel? _secondSelector;
    private Control? _secondSeparator;
    private Control? _thirdSeparator;
    private bool _use12Clock;
    private bool _surpressTimeEvent = true;
    private TimeSpan? _timeHolder;

    static TimePickerPresenter() => _ = PanelFormatProperty.Changed.AddClassHandler<TimePickerPresenter, string>((presenter, args) =>
                                             presenter.OnPanelFormatChanged(args));

    public bool NeedsConfirmation
    {
        get => GetValue(NeedsConfirmationProperty);
        set => SetValue(NeedsConfirmationProperty, value);
    }

    public int MinuteIncrement
    {
        get => GetValue(MinuteIncrementProperty);
        set => SetValue(MinuteIncrementProperty, value);
    }

    public int SecondIncrement
    {
        get => GetValue(SecondIncrementProperty);
        set => SetValue(SecondIncrementProperty, value);
    }

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    public event EventHandler<TimeChangedEventArgs> SelectedTimeChanged
    {
        add => AddHandler(SelectedTimeChangedEvent, value);
        remove => RemoveHandler(SelectedTimeChangedEvent, value);
    }

    private void OnPanelFormatChanged(AvaloniaPropertyChangedEventArgs<string> args)
    {
        var format = args.NewValue.Value;
        UpdatePanelLayout(format);
    }

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
                if ((part.Contains('h', StringComparison.OrdinalIgnoreCase) || part.Contains('H', StringComparison.OrdinalIgnoreCase)) && !panels.Contains(_hourScrollPanel))
                {
                    panels.Add(_hourScrollPanel);
                    _use12Clock = !part.Equals("hh", StringComparison.OrdinalIgnoreCase);
                    var itemFormat = part.Equals("h", StringComparison.OrdinalIgnoreCase) ? "%h" : part.ToLower(CultureInfo.CurrentCulture);
                    _ = _hourSelector?.SetValue(DateTimePickerPanel.ItemFormatProperty, itemFormat);
                    if (_hourSelector is not null)
                    {
                        _hourSelector.MaximumValue = _use12Clock ? 12 : 23;
                        _hourSelector.MinimumValue = _use12Clock ? 1 : 0;
                    }
                }
                else if (part[0] == 'm' && !panels.Contains(_minuteSelector))
                {
                    panels.Add(_minuteScrollPanel);
                    _ = _minuteSelector?.SetValue(DateTimePickerPanel.ItemFormatProperty, part);
                }
                else if (part[0] == 's' && !panels.Contains(_secondScrollPanel))
                {
                    panels.Add(_secondScrollPanel);
                    _ = _secondSelector?.SetValue(DateTimePickerPanel.ItemFormatProperty, part.Replace('s', 'm'));
                }
                else if (part[0] == 't' && !panels.Contains(_ampmScrollPanel))
                {
                    panels.Add(_ampmScrollPanel);
                    _ = _periodSelector?.SetValue(DateTimePickerPanel.ItemFormatProperty, part);
                }
            }
            catch
            {
                // ignored
            }
        }

        if (panels.Count < 1) return;
        IsVisibleProperty.SetValue(false, _hourScrollPanel, _minuteScrollPanel, _secondScrollPanel, _ampmScrollPanel, _firstSeparator, _secondSeparator, _thirdSeparator);
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _hourSelector = e.NameScope.Find<DateTimePickerPanel>(PartHourSelector);
        _minuteSelector = e.NameScope.Find<DateTimePickerPanel>(PartMinuteSelector);
        _secondSelector = e.NameScope.Find<DateTimePickerPanel>(PartSecondSelector);
        _periodSelector = e.NameScope.Find<DateTimePickerPanel>(PartPeriodSelector);

        _pickerContainer = e.NameScope.Find<Grid>(PartPickerContainer);
        _hourScrollPanel = e.NameScope.Find<Control>(PartHourScrollPanel);
        _minuteScrollPanel = e.NameScope.Find<Control>(PartMinuteScrollPanel);
        _secondScrollPanel = e.NameScope.Find<Control>(PartSecondScrollPanel);
        _ampmScrollPanel = e.NameScope.Find<Control>(PartPeriodScrollPanel);

        _firstSeparator = e.NameScope.Find<Control>(PartFirstSeparator);
        _secondSeparator = e.NameScope.Find<Control>(PartSecondSeparator);
        _thirdSeparator = e.NameScope.Find<Control>(PartThirdSeparator);
        Initialize();
        UpdatePanelLayout(PanelFormat);
        _surpressTimeEvent = false;
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        UpdatePanelsFromSelectedTime(_timeHolder);
        if (_hourSelector is not null) _hourSelector.SelectionChanged += OnPanelSelectionChanged;
        if (_minuteSelector is not null) _minuteSelector.SelectionChanged += OnPanelSelectionChanged;
        if (_secondSelector is not null) _secondSelector.SelectionChanged += OnPanelSelectionChanged;
        if (_periodSelector is not null) _periodSelector.SelectionChanged += OnPanelSelectionChanged;
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        if (_hourSelector is not null) _hourSelector.SelectionChanged -= OnPanelSelectionChanged;
        if (_minuteSelector is not null) _minuteSelector.SelectionChanged -= OnPanelSelectionChanged;
        if (_secondSelector is not null) _secondSelector.SelectionChanged -= OnPanelSelectionChanged;
        if (_periodSelector is not null) _periodSelector.SelectionChanged -= OnPanelSelectionChanged;
    }

    private void OnPanelSelectionChanged(object? sender, System.EventArgs e)
    {
        if (_surpressTimeEvent) return;
        if (!_use12Clock && Equals(sender, _periodSelector)) return;
        var time = _timeHolder ?? DateTime.Now.TimeOfDay;
        var hour = _hourSelector?.SelectedValue ?? time.Hours;
        var minute = _minuteSelector?.SelectedValue ?? time.Minutes;
        var second = _secondSelector?.SelectedValue ?? time.Seconds;
        var ampm = _periodSelector?.SelectedValue ?? (time.Hours >= 12 ? 1 : 0);
        if (_use12Clock)
        {
            hour = ampm switch
            {
                0 when hour == 12 => 0,
                1 when hour < 12 => hour + 12,
                _ => hour
            };
        }
        else
        {
            ampm = hour switch
            {
                >= 12 => 1,
                _ => 0
            };
            SetIfChanged(_periodSelector, ampm);
        }

        var newTime = new TimeSpan(hour, minute, second);
        if (NeedsConfirmation)
        {
            _timeHolder = newTime;
        }
        else
        {
            if (_surpressTimeEvent) return;
            RaiseEvent(new TimeChangedEventArgs(null, newTime) { RoutedEvent = SelectedTimeChangedEvent });
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        OnPanelSelectionChanged(null, EventArgs.Empty);
    }

    private void UpdatePanelsFromSelectedTime(TimeSpan? time)
    {
        if (time is null) return;
        if (_hourSelector is not null)
        {
            var index = _use12Clock ? time.Value.Hours % 12 : time.Value.Hours;
            if (_use12Clock && index == 0) index = 12;
            SetIfChanged(_hourSelector, index);
        }

        SetIfChanged(_minuteSelector, time.Value.Minutes);
        SetIfChanged(_secondSelector, time.Value.Seconds);
        var ampm = time.Value.Hours switch
        {
            >= 12 => 1,
            _ => 0
        };

        SetIfChanged(_periodSelector, ampm);
        _periodSelector?.IsEnabled = _use12Clock;
    }

    private void Initialize()
    {
        if (_pickerContainer is null) return;
        if (_hourSelector is not null)
        {
            _hourSelector.ItemFormat = "hh";
            _hourSelector.MaximumValue = _use12Clock ? 12 : 23;
            _hourSelector.MinimumValue = _use12Clock ? 1 : 0;
        }

        if (_minuteSelector is not null)
        {
            _minuteSelector.ItemFormat = "mm";
            _minuteSelector.MaximumValue = 59;
            _minuteSelector.MinimumValue = 0;
        }

        if (_secondSelector is not null)
        {
            _secondSelector.ItemFormat = "mm";
            _secondSelector.MaximumValue = 59;
            _secondSelector.MinimumValue = 0;
        }

        if (_periodSelector is not null)
        {
            _periodSelector.ItemFormat = "t";
            _periodSelector.MaximumValue = 1;
            _periodSelector.MinimumValue = 0;
        }
    }

    public void Confirm()
    {
        if (NeedsConfirmation)
            RaiseEvent(new TimeChangedEventArgs(null, _timeHolder) { RoutedEvent = SelectedTimeChangedEvent });
    }

    private void SetIfChanged(DateTimePickerPanel? panel, int index)
    {
        if (panel is null) return;
        panel.SelectionChanged -= OnPanelSelectionChanged;
        if (panel.SelectedValue != index) panel.SelectedValue = index;
        panel.SelectionChanged += OnPanelSelectionChanged;
    }

    internal void SyncTime(TimeSpan? time)
    {
        _surpressTimeEvent = true;
        _timeHolder = time;
        UpdatePanelsFromSelectedTime(time);
        _surpressTimeEvent = false;
    }
}
