// -----------------------------------------------------------------------
// <copyright file="TimeRangeView.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives.Intervals;
using MyNet.Primitives.Temporal;
using MyNet.Utilities.Suspending;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartStartTimeView, typeof(TimeView))]
[TemplatePart(PartEndTimeView, typeof(TimeView))]
[TemplatePart(PartBoundarySelector, typeof(TabControl))]
public class TimeRangeView : TemplatedControl, IValueSelector<Period?>
{
    public const string PartStartTimeView = "PART_StartTimeView";
    public const string PartEndTimeView = "PART_EndTimeView";
    public const string PartBoundarySelector = "PART_BoundarySelector";

    private readonly Suspender _syncSuspender = new();

    private TimeView? _startTimeView;
    private TimeView? _endTimeView;
    private TabControl? _boundaryTabControl;
    private TimeSpan? _startTime;
    private TimeSpan? _endTime;
    private bool _isSyncingBoundaryTabs;
    private bool _endTimeViewTemplateReady;
    private TimeRangeBoundary _lastEditedBoundary = TimeRangeBoundary.Start;

    static TimeRangeView()
    {
        SelectedValueProperty.Changed.AddClassHandler<TimeRangeView>((view, e) =>
            view.OnSelectedValueChanged(e.GetOldValue<Period?>(), e.GetNewValue<Period?>()));

        ActiveBoundaryProperty.Changed.AddClassHandler<TimeRangeView>((view, e) =>
        {
            var oldValue = e.GetOldValue<TimeRangeBoundary>();
            var newValue = e.GetNewValue<TimeRangeBoundary>();
            if (oldValue != newValue)
                view.PersistSlotFromTimeView(oldValue);

            view.SyncBoundaryTabs();

            if (!view._isSyncingBoundaryTabs)
                view.FocusActiveHourComponent();
        });
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        DetachHandlers();

        base.OnApplyTemplate(e);

        _startTimeView = e.NameScope.Find<TimeView>(PartStartTimeView);
        _endTimeView = e.NameScope.Find<TimeView>(PartEndTimeView);
        _boundaryTabControl = e.NameScope.Find<TabControl>(PartBoundarySelector);
        _endTimeViewTemplateReady = false;

        AttachHandlers();
        SyncBoundaryTabs();
        PushSlotsToTimeViews();
    }

    #region SelectedValue

    public event EventHandler<SelectionChangedEventArgs>? SelectedValueChanged;

    public static readonly StyledProperty<Period?> SelectedValueProperty =
        AvaloniaProperty.Register<TimeRangeView, Period?>(nameof(SelectedValue), defaultBindingMode: BindingMode.TwoWay);

    public Period? SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    private void OnSelectedValueChanged(Period? oldValue, Period? newValue)
    {
        if (!_syncSuspender.IsSuspended)
        {
            if (newValue is null)
            {
                _startTime = null;
                _endTime = null;
            }
            else
            {
                _startTime = TimeRangeHelper.GetPeriodStartTime(newValue);
                _endTime = TimeRangeHelper.GetPeriodEndTime(newValue);
            }

            PushSlotsToTimeViews();
        }

        RaiseSelectedValueChanged(oldValue, newValue);
    }

    private void RaiseSelectedValueChanged(Period? oldValue, Period? newValue)
    {
        var handler = SelectedValueChanged;
        if (handler is null)
            return;

        var addedItems = new Collection<Period?>();
        var removedItems = new Collection<Period?>();

        if (newValue is not null)
            addedItems.Add(newValue);

        if (oldValue is not null)
            removedItems.Add(oldValue);

        handler(this, new(SelectingItemsControl.SelectionChangedEvent, removedItems, addedItems));
    }

    #endregion

    #region ActiveBoundary

    public static readonly StyledProperty<TimeRangeBoundary> ActiveBoundaryProperty =
        AvaloniaProperty.Register<TimeRangeView, TimeRangeBoundary>(nameof(ActiveBoundary), TimeRangeBoundary.Start);

    public TimeRangeBoundary ActiveBoundary
    {
        get => GetValue(ActiveBoundaryProperty);
        set => SetValue(ActiveBoundaryProperty, value);
    }

    #endregion

    #region ActiveTime

    public static readonly StyledProperty<TimeSpan?> ActiveTimeProperty =
        AvaloniaProperty.Register<TimeRangeView, TimeSpan?>(nameof(ActiveTime), defaultBindingMode: BindingMode.TwoWay);

    public TimeSpan? ActiveTime
    {
        get => GetActiveSlot();
        set => SetActiveSlot(value);
    }

    #endregion

    #region StartTime / EndTime

    public TimeSpan? StartTime
    {
        get => _startTime;
        set
        {
            _startTime = value;
            _lastEditedBoundary = TimeRangeBoundary.Start;
            PushStartSlotToTimeView();
            SyncSlotsToSelectedValue();
        }
    }

    public TimeSpan? EndTime
    {
        get => _endTime;
        set
        {
            _endTime = value;
            _lastEditedBoundary = TimeRangeBoundary.End;
            PushEndSlotToTimeView();
            SyncSlotsToSelectedValue();
        }
    }

    #endregion

    #region Relayed time properties

    public static readonly StyledProperty<string> NumberFormatProperty = TimeView.NumberFormatProperty.AddOwner<TimeRangeView>();

    public string NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    public static readonly StyledProperty<bool> ShowSecondsProperty = TimeSelectorBase.ShowSecondsProperty.AddOwner<TimeRangeView>();

    public bool ShowSeconds
    {
        get => GetValue(ShowSecondsProperty);
        set => SetValue(ShowSecondsProperty, value);
    }

    public static readonly StyledProperty<TimeFormat> TimeFormatProperty = TimeSelectorBase.TimeFormatProperty.AddOwner<TimeRangeView>();

    public TimeFormat TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    public static readonly StyledProperty<bool> AllowOvernightProperty =
        AvaloniaProperty.Register<TimeRangeView, bool>(nameof(AllowOvernight));

    public bool AllowOvernight
    {
        get => GetValue(AllowOvernightProperty);
        set => SetValue(AllowOvernightProperty, value);
    }

    public static readonly StyledProperty<DateTime> ReferenceDateProperty =
        AvaloniaProperty.Register<TimeRangeView, DateTime>(nameof(ReferenceDate), DateTime.Today);

    public DateTime ReferenceDate
    {
        get => GetValue(ReferenceDateProperty);
        set => SetValue(ReferenceDateProperty, value);
    }

    #endregion

    public bool IsEmpty() => !_startTime.HasValue && !_endTime.HasValue && SelectedValue is null;

    public void Clear()
    {
        _startTime = null;
        _endTime = null;
        SetCurrentValue(SelectedValueProperty, null);
        SetCurrentValue(ActiveBoundaryProperty, TimeRangeBoundary.Start);
        PushSlotsToTimeViews();
    }

    public void ResetForPopupOpen()
    {
        SetCurrentValue(ActiveBoundaryProperty, TimeRangeBoundary.Start);
        PushSlotsToTimeViews();
    }

    /// <summary>Applies boundary selection and hour focus after the popup layout is ready.</summary>
    internal void FinalizePopupOpen()
    {
        SetCurrentValue(ActiveBoundaryProperty, TimeRangeBoundary.Start);
        EnsureEndTimeViewTemplateReady();
        PushSlotsToTimeViews();
        FocusActiveHourComponent();

        Dispatcher.UIThread.Post(FocusActiveHourComponent, DispatcherPriority.Render);
    }

    internal TimeView? StartTimeViewPart => _startTimeView;

    internal TimeView? EndTimeViewPart => _endTimeView;

    internal void FocusStartHour() => FinalizePopupOpen();

    internal bool IsSourceInStartSection(object? source)
    {
        if (source is not Visual visual || _startTimeView is null)
            return false;

        return visual.FindAncestorOfType<TimeView>(includeSelf: true) == _startTimeView;
    }

    internal bool IsSourceInEndSection(object? source)
    {
        if (source is not Visual visual || _endTimeView is null)
            return false;

        return visual.FindAncestorOfType<TimeView>(includeSelf: true) == _endTimeView;
    }

    internal TimeRangeBuildResult TryBuildSelectedValue()
    {
        if (_startTime is not { } start || _endTime is not { } end)
            return new(null, IsValid: false);

        return TimeRangeHelper.BuildPeriod(start, end, ReferenceDate, AllowOvernight);
    }

    private void SyncSlotsToSelectedValue()
    {
        if (_syncSuspender.IsSuspended)
            return;

        if (_startTime is not { } start || _endTime is not { } end)
            return;

        if (!AllowOvernight && end < start)
        {
            using (_syncSuspender.Suspend())
            {
                (start, end) = TimeRangeHelper.CoerceSameDayRange(start, end, _lastEditedBoundary);
                _startTime = start;
                _endTime = end;
            }

            PushSlotsToTimeViews();
        }

        var result = TimeRangeHelper.BuildPeriod(start, end, ReferenceDate, AllowOvernight);
        SetCurrentValue(SelectedValueProperty, result.Period);
    }

    internal void LoadFromPeriod(Period value)
    {
        using (_syncSuspender.Suspend())
        {
            _startTime = TimeRangeHelper.GetPeriodStartTime(value);
            _endTime = TimeRangeHelper.GetPeriodEndTime(value);
            SetCurrentValue(SelectedValueProperty, value);
        }

        PushSlotsToTimeViews();
    }

    public void SwitchToEnd(bool autoAdvance)
    {
        PersistSlotFromTimeView(TimeRangeBoundary.Start);

        if (autoAdvance && !_endTime.HasValue && _startTime is { } start)
            _endTime = SeedEndTime(start);

        PushEndSlotToTimeView();
        SwitchBoundary(TimeRangeBoundary.End);
        FocusActiveHourComponent();
    }

    public void SwitchBoundary(TimeRangeBoundary boundary)
    {
        if (ActiveBoundary == boundary)
            return;

        SetCurrentValue(ActiveBoundaryProperty, boundary);
    }

    private static TimeSpan SeedEndTime(TimeSpan start)
    {
        var seeded = start.Add(TimeSpan.FromHours(1));
        return seeded.TotalDays >= 1 ? new(23, 59, 59) : seeded;
    }

    private TimeSpan? GetActiveSlot() =>
        ActiveBoundary == TimeRangeBoundary.Start ? _startTime : _endTime;

    private void SetActiveSlot(TimeSpan? value)
    {
        if (ActiveBoundary == TimeRangeBoundary.Start)
        {
            _startTime = value;
            _lastEditedBoundary = TimeRangeBoundary.Start;
            PushStartSlotToTimeView();
        }
        else
        {
            _endTime = value;
            _lastEditedBoundary = TimeRangeBoundary.End;
            PushEndSlotToTimeView();
        }

        SyncSlotsToSelectedValue();
    }

    private void PersistSlotFromTimeView(TimeRangeBoundary boundary)
    {
        _lastEditedBoundary = boundary;

        if (boundary == TimeRangeBoundary.Start)
        {
            if (_startTimeView is not null)
                _startTime = _startTimeView.SelectedValue;
        }
        else if (_endTimeView is not null)
        {
            _endTime = _endTimeView.SelectedValue;
        }

        SyncSlotsToSelectedValue();
    }

    private void PushSlotsToTimeViews()
    {
        PushStartSlotToTimeView();
        PushEndSlotToTimeView();
    }

    private void PushStartSlotToTimeView()
    {
        if (_startTimeView is null)
            return;

        using (_syncSuspender.Suspend())
            _startTimeView.SelectedValue = _startTime;
    }

    private void PushEndSlotToTimeView()
    {
        if (_endTimeView is null)
            return;

        using (_syncSuspender.Suspend())
            _endTimeView.SelectedValue = _endTime;
    }

    private void EnsureEndTimeViewTemplateReady()
    {
        if (_endTimeViewTemplateReady || _boundaryTabControl is null || _endTimeView is null)
            return;

        SetBoundaryTabIndex(1);
        PushEndSlotToTimeView();
        _endTimeView.UpdateLayout();
        _endTimeViewTemplateReady = true;
        SetBoundaryTabIndex(0);
        SetCurrentValue(ActiveBoundaryProperty, TimeRangeBoundary.Start);
    }

    private void SetBoundaryTabIndex(int index)
    {
        if (_boundaryTabControl is null)
            return;

        _isSyncingBoundaryTabs = true;
        try
        {
            _boundaryTabControl.SetCurrentValue(TabControl.SelectedIndexProperty, index);
        }
        finally
        {
            _isSyncingBoundaryTabs = false;
        }
    }

    private void SyncBoundaryTabs() => SetBoundaryTabIndex((int)ActiveBoundary);

    private void FocusActiveHourComponent()
    {
        var timeView = ActiveBoundary == TimeRangeBoundary.Start ? _startTimeView : _endTimeView;
        timeView?.SetCurrentValue(TimeSelectorBase.SelectedComponentProperty, TimeComponent.Hour);
        timeView?.FocusComponent(TimeComponent.Hour);
    }

    private void AttachHandlers()
    {
        if (_startTimeView is not null)
        {
            _startTimeView.SelectedValueChanged += OnStartTimeViewSelectedValueChanged;
            _startTimeView.AddHandler(TimeSelectorBase.InputCompletedEvent, OnTimeViewInputCompleted);
        }

        if (_endTimeView is not null)
        {
            _endTimeView.SelectedValueChanged += OnEndTimeViewSelectedValueChanged;
            _endTimeView.AddHandler(TimeSelectorBase.InputCompletedEvent, OnTimeViewInputCompleted);
        }

        if (_boundaryTabControl is not null)
            _boundaryTabControl.SelectionChanged += OnBoundaryTabSelectionChanged;
    }

    private void DetachHandlers()
    {
        if (_startTimeView is not null)
        {
            _startTimeView.SelectedValueChanged -= OnStartTimeViewSelectedValueChanged;
            _startTimeView.RemoveHandler(TimeSelectorBase.InputCompletedEvent, OnTimeViewInputCompleted);
        }

        if (_endTimeView is not null)
        {
            _endTimeView.SelectedValueChanged -= OnEndTimeViewSelectedValueChanged;
            _endTimeView.RemoveHandler(TimeSelectorBase.InputCompletedEvent, OnTimeViewInputCompleted);
        }

        if (_boundaryTabControl is not null)
            _boundaryTabControl.SelectionChanged -= OnBoundaryTabSelectionChanged;
    }

    private void OnBoundaryTabSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_isSyncingBoundaryTabs || _boundaryTabControl is null)
            return;

        var index = _boundaryTabControl.SelectedIndex;
        if (index is not (0 or 1))
            return;

        var boundary = (TimeRangeBoundary)index;
        if (boundary == ActiveBoundary)
            return;

        SetCurrentValue(ActiveBoundaryProperty, boundary);
    }

    private void OnStartTimeViewSelectedValueChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_syncSuspender.IsSuspended || _startTimeView is null)
            return;

        _startTime = _startTimeView.SelectedValue;
        _lastEditedBoundary = TimeRangeBoundary.Start;
        SyncSlotsToSelectedValue();
    }

    private void OnEndTimeViewSelectedValueChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_syncSuspender.IsSuspended || _endTimeView is null)
            return;

        _endTime = _endTimeView.SelectedValue;
        _lastEditedBoundary = TimeRangeBoundary.End;
        SyncSlotsToSelectedValue();
    }

    private void OnTimeViewInputCompleted(object? sender, TimeInputCompletedEventArgs e)
    {
        var boundary = _startTimeView is not null && ReferenceEquals(sender, _startTimeView) ? TimeRangeBoundary.Start
            : _endTimeView is not null && ReferenceEquals(sender, _endTimeView) ? TimeRangeBoundary.End
            : ActiveBoundary;

        if (boundary == TimeRangeBoundary.Start)
        {
            PersistSlotFromTimeView(TimeRangeBoundary.Start);
            SwitchToEnd(autoAdvance: true);
            return;
        }

        PersistSlotFromTimeView(TimeRangeBoundary.End);

        if (!_startTime.HasValue)
            SwitchBoundary(TimeRangeBoundary.Start);
    }
}
