// -----------------------------------------------------------------------
// <copyright file="DateTimeScrollPickerEx.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// A control that allows the user to select a date and a time using scrollable
/// selectors. It merges the behavior of Avalonia's <c>DatePicker</c> and <c>TimePicker</c>.
/// </summary>

[TemplatePart(PartFlyoutButton, typeof(Button))]
[TemplatePart(PartButtonContentGrid, typeof(Grid))]
[TemplatePart(PartDayTextBlock, typeof(TextBlock))]
[TemplatePart(PartMonthTextBlock, typeof(TextBlock))]
[TemplatePart(PartYearTextBlock, typeof(TextBlock))]
[TemplatePart(PartHourTextBlock, typeof(TextBlock))]
[TemplatePart(PartMinuteTextBlock, typeof(TextBlock))]
[TemplatePart(PartSecondTextBlock, typeof(TextBlock))]
[TemplatePart(PartPeriodTextBlock, typeof(TextBlock))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartPickerPresenter, typeof(DateTimeScrollPickerPresenter))]
[PseudoClasses(PseudoHasNoDate, PseudoFlyoutOpen)]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Improve Avalonia control")]
public class DateTimeScrollPickerEx : TemplatedControl, IPopupControl
{
    private const string PartFlyoutButton = "PART_FlyoutButton";
    private const string PartButtonContentGrid = "PART_ButtonContentGrid";
    private const string PartDayTextBlock = "PART_DayTextBlock";
    private const string PartMonthTextBlock = "PART_MonthTextBlock";
    private const string PartYearTextBlock = "PART_YearTextBlock";
    private const string PartHourTextBlock = "PART_HourTextBlock";
    private const string PartMinuteTextBlock = "PART_MinuteTextBlock";
    private const string PartSecondTextBlock = "PART_SecondTextBlock";
    private const string PartPeriodTextBlock = "PART_PeriodTextBlock";
    private const string PartPopup = "PART_Popup";
    private const string PartPickerPresenter = "PART_PickerPresenter";

    private const string PseudoHasNoDate = ":hasnodate";
    private const string PseudoFlyoutOpen = ":flyout-open";

    /// <summary>
    /// Defines the <see cref="SelectedDateTime"/> property.
    /// </summary>
    public static readonly StyledProperty<DateTime?> SelectedDateTimeProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, DateTime?>(nameof(SelectedDateTime), defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

    /// <summary>
    /// Defines the <see cref="IsDropDownOpen"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, bool>(nameof(IsDropDownOpen), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Defines the <see cref="DayVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> DayVisibleProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, bool>(nameof(DayVisible), true);

    /// <summary>
    /// Defines the <see cref="MonthVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> MonthVisibleProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, bool>(nameof(MonthVisible), true);

    /// <summary>
    /// Defines the <see cref="YearVisible"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> YearVisibleProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, bool>(nameof(YearVisible), true);

    /// <summary>
    /// Defines the <see cref="DayFormat"/> property.
    /// </summary>
    public static readonly StyledProperty<string> DayFormatProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, string>(nameof(DayFormat), "%d");

    /// <summary>
    /// Defines the <see cref="MonthFormat"/> property.
    /// </summary>
    public static readonly StyledProperty<string> MonthFormatProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, string>(nameof(MonthFormat), "MMMM");

    /// <summary>
    /// Defines the <see cref="YearFormat"/> property.
    /// </summary>
    public static readonly StyledProperty<string> YearFormatProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, string>(nameof(YearFormat), "yyyy");

    /// <summary>
    /// Defines the <see cref="MinYear"/> property.
    /// </summary>
    public static readonly StyledProperty<DateTimeOffset> MinYearProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, DateTimeOffset>(nameof(MinYear), DateTimeOffset.MinValue, coerce: CoerceMinYear);

    /// <summary>
    /// Defines the <see cref="MaxYear"/> property.
    /// </summary>
    public static readonly StyledProperty<DateTimeOffset> MaxYearProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, DateTimeOffset>(nameof(MaxYear), DateTimeOffset.MaxValue, coerce: CoerceMaxYear);

    /// <summary>
    /// Defines the <see cref="MinuteIncrement"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MinuteIncrementProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, int>(nameof(MinuteIncrement), 1, coerce: CoerceMinuteIncrement);

    /// <summary>
    /// Defines the <see cref="SecondIncrement"/> property.
    /// </summary>
    public static readonly StyledProperty<int> SecondIncrementProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, int>(nameof(SecondIncrement), 1, coerce: CoerceSecondIncrement);

    /// <summary>
    /// Defines the <see cref="ClockIdentifier"/> property.
    /// </summary>
    public static readonly StyledProperty<string> ClockIdentifierProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, string>(nameof(ClockIdentifier), "12HourClock", coerce: CoerceClockIdentifier);

    /// <summary>
    /// Defines the <see cref="UseSeconds"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> UseSecondsProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerEx, bool>(nameof(UseSeconds));

    // Template items
    private Button? _flyoutButton;
    private Grid? _contentGrid;
    private TextBlock? _dayText;
    private TextBlock? _monthText;
    private TextBlock? _yearText;
    private TextBlock? _hourText;
    private TextBlock? _minuteText;
    private TextBlock? _secondText;
    private TextBlock? _periodText;
    private readonly List<Rectangle> _spacers = [];
    private Control? _dateTimeSeparator;
    private Popup? _popup;
    private DateTimeScrollPickerPresenter? _presenter;

    private bool _areControlsAvailable;

    static DateTimeScrollPickerEx() => IsDropDownOpenProperty.AffectsPseudoClass<DateTimeScrollPickerEx>(PseudoFlyoutOpen);

    public DateTimeScrollPickerEx()
    {
        PseudoClasses.Set(PseudoHasNoDate, true);

        var now = DateTimeOffset.Now;
        SetCurrentValue(MinYearProperty, new DateTimeOffset(now.Year - 100, 1, 1, 0, 0, 0, now.Offset));
        SetCurrentValue(MaxYearProperty, new DateTimeOffset(now.Year + 100, 12, 31, 0, 0, 0, now.Offset));

        var timePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
        if (timePattern.Contains('H', StringComparison.Ordinal))
            SetCurrentValue(ClockIdentifierProperty, "24HourClock");
    }

    /// <summary>
    /// Raised when the <see cref="SelectedDateTime"/> property changes.
    /// </summary>
    public event EventHandler<DateTimeScrollPickerSelectedValueChangedEventArgs>? SelectedDateTimeChanged;

    /// <summary>
    /// Raised when the drop-down opens.
    /// </summary>
    public event EventHandler? DropDownOpened;

    /// <summary>
    /// Raised when the drop-down closes.
    /// </summary>
    public event EventHandler? DropDownClosed;

    /// <summary>
    /// Gets or sets the selected date and time. Can be null.
    /// </summary>
    public DateTime? SelectedDateTime
    {
        get => GetValue(SelectedDateTimeProperty);
        set => SetValue(SelectedDateTimeProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the drop down is open.
    /// </summary>
    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the day is visible.
    /// </summary>
    public bool DayVisible
    {
        get => GetValue(DayVisibleProperty);
        set => SetValue(DayVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the month is visible.
    /// </summary>
    public bool MonthVisible
    {
        get => GetValue(MonthVisibleProperty);
        set => SetValue(MonthVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the year is visible.
    /// </summary>
    public bool YearVisible
    {
        get => GetValue(YearVisibleProperty);
        set => SetValue(YearVisibleProperty, value);
    }

    /// <summary>
    /// Gets or sets the day format.
    /// </summary>
    public string DayFormat
    {
        get => GetValue(DayFormatProperty);
        set => SetValue(DayFormatProperty, value);
    }

    /// <summary>
    /// Gets or sets the month format.
    /// </summary>
    public string MonthFormat
    {
        get => GetValue(MonthFormatProperty);
        set => SetValue(MonthFormatProperty, value);
    }

    /// <summary>
    /// Gets or sets the year format.
    /// </summary>
    public string YearFormat
    {
        get => GetValue(YearFormatProperty);
        set => SetValue(YearFormatProperty, value);
    }

    /// <summary>
    /// Gets or sets the minimum pickable year.
    /// </summary>
    public DateTimeOffset MinYear
    {
        get => GetValue(MinYearProperty);
        set => SetValue(MinYearProperty, value);
    }

    /// <summary>
    /// Gets or sets the maximum pickable year.
    /// </summary>
    public DateTimeOffset MaxYear
    {
        get => GetValue(MaxYearProperty);
        set => SetValue(MaxYearProperty, value);
    }

    /// <summary>
    /// Gets or sets the minute increment in the picker.
    /// </summary>
    public int MinuteIncrement
    {
        get => GetValue(MinuteIncrementProperty);
        set => SetValue(MinuteIncrementProperty, value);
    }

    /// <summary>
    /// Gets or sets the second increment in the picker.
    /// </summary>
    public int SecondIncrement
    {
        get => GetValue(SecondIncrementProperty);
        set => SetValue(SecondIncrementProperty, value);
    }

    /// <summary>
    /// Gets or sets the clock identifier, either 12HourClock or 24HourClock.
    /// </summary>
    public string ClockIdentifier
    {
        get => GetValue(ClockIdentifierProperty);
        set => SetValue(ClockIdentifierProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the picker uses seconds.
    /// </summary>
    public bool UseSeconds
    {
        get => GetValue(UseSecondsProperty);
        set => SetValue(UseSecondsProperty, value);
    }

    private static DateTimeOffset CoerceMinYear(AvaloniaObject sender, DateTimeOffset value)
    {
        if (value > sender.GetValue(MaxYearProperty))
            throw new InvalidOperationException($"{MinYearProperty.Name} cannot be greater than {MaxYearProperty.Name}");
        return value;
    }

    private static DateTimeOffset CoerceMaxYear(AvaloniaObject sender, DateTimeOffset value)
    {
        if (value < sender.GetValue(MinYearProperty))
            throw new InvalidOperationException($"{MaxYearProperty.Name} cannot be less than {MinYearProperty.Name}");
        return value;
    }

    private static int CoerceMinuteIncrement(AvaloniaObject sender, int value)
    {
        if (value < 1 || value > 59)
            throw new ArgumentOutOfRangeException(null, "1 >= MinuteIncrement <= 59");
        return value;
    }

    private static int CoerceSecondIncrement(AvaloniaObject sender, int value)
    {
        if (value < 1 || value > 59)
            throw new ArgumentOutOfRangeException(null, "1 >= SecondIncrement <= 59");
        return value;
    }

    private static string CoerceClockIdentifier(AvaloniaObject sender, string value)
    {
        if (!(string.IsNullOrEmpty(value) || value == "12HourClock" || value == "24HourClock"))
            throw new ArgumentException("Invalid ClockIdentifier", default(string));
        return value;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _areControlsAvailable = false;

        if (_flyoutButton != null)
            _flyoutButton.Click -= OnFlyoutButtonClicked;
        if (_popup != null)
            _popup.Closed -= OnPopupClosed;
        if (_presenter != null)
        {
            _presenter.Confirmed -= OnConfirmed;
            _presenter.Dismissed -= OnDismissPicker;
        }

        base.OnApplyTemplate(e);

        _flyoutButton = e.NameScope.Find<Button>(PartFlyoutButton);
        _contentGrid = e.NameScope.Find<Grid>(PartButtonContentGrid);
        _dayText = e.NameScope.Find<TextBlock>(PartDayTextBlock);
        _monthText = e.NameScope.Find<TextBlock>(PartMonthTextBlock);
        _yearText = e.NameScope.Find<TextBlock>(PartYearTextBlock);
        _hourText = e.NameScope.Find<TextBlock>(PartHourTextBlock);
        _minuteText = e.NameScope.Find<TextBlock>(PartMinuteTextBlock);
        _secondText = e.NameScope.Find<TextBlock>(PartSecondTextBlock);
        _periodText = e.NameScope.Find<TextBlock>(PartPeriodTextBlock);
        _popup = e.NameScope.Find<Popup>(PartPopup);
        _presenter = e.NameScope.Find<DateTimeScrollPickerPresenter>(PartPickerPresenter);

        _spacers.Clear();
        foreach (var name in new[] { "PART_FirstSpacer", "PART_SecondSpacer", "PART_ThirdSpacer", "PART_FourthSpacer", "PART_FifthSpacer" })
        {
            if (e.NameScope.Find<Rectangle>(name) is { } spacer)
                _spacers.Add(spacer);
        }

        _dateTimeSeparator = e.NameScope.Find<Control>("PART_DateTimeSeparator");

        _areControlsAvailable = true;

        SetGrid();
        SetSelectedDateTimeText();

        if (_flyoutButton != null)
            _flyoutButton.Click += OnFlyoutButtonClicked;
        if (_popup != null)
            _popup.Closed += OnPopupClosed;

        if (_presenter != null)
        {
            _presenter.Confirmed += OnConfirmed;
            _presenter.Dismissed += OnDismissPicker;

            _presenter[!DateTimeScrollPickerPresenter.DayVisibleProperty] = this[!DayVisibleProperty];
            _presenter[!DateTimeScrollPickerPresenter.MonthVisibleProperty] = this[!MonthVisibleProperty];
            _presenter[!DateTimeScrollPickerPresenter.YearVisibleProperty] = this[!YearVisibleProperty];
            _presenter[!DateTimeScrollPickerPresenter.DayFormatProperty] = this[!DayFormatProperty];
            _presenter[!DateTimeScrollPickerPresenter.MonthFormatProperty] = this[!MonthFormatProperty];
            _presenter[!DateTimeScrollPickerPresenter.YearFormatProperty] = this[!YearFormatProperty];
            _presenter[!DateTimeScrollPickerPresenter.MinYearProperty] = this[!MinYearProperty];
            _presenter[!DateTimeScrollPickerPresenter.MaxYearProperty] = this[!MaxYearProperty];
            _presenter[!DateTimeScrollPickerPresenter.MinuteIncrementProperty] = this[!MinuteIncrementProperty];
            _presenter[!DateTimeScrollPickerPresenter.SecondIncrementProperty] = this[!SecondIncrementProperty];
            _presenter[!DateTimeScrollPickerPresenter.ClockIdentifierProperty] = this[!ClockIdentifierProperty];
            _presenter[!DateTimeScrollPickerPresenter.UseSecondsProperty] = this[!UseSecondsProperty];
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == DayVisibleProperty ||
            change.Property == MonthVisibleProperty ||
            change.Property == YearVisibleProperty ||
            change.Property == ClockIdentifierProperty ||
            change.Property == UseSecondsProperty)
        {
            SetGrid();
            SetSelectedDateTimeText();
        }
        else if (change.Property == DayFormatProperty ||
                 change.Property == MonthFormatProperty ||
                 change.Property == YearFormatProperty)
        {
            SetSelectedDateTimeText();
        }
        else if (change.Property == SelectedDateTimeProperty)
        {
            var (oldValue, newValue) = change.GetOldAndNewValue<DateTime?>();
            SetSelectedDateTimeText();
            SelectedDateTimeChanged?.Invoke(this, new DateTimeScrollPickerSelectedValueChangedEventArgs(oldValue, newValue));
        }
    }

    protected override void UpdateDataValidation(AvaloniaProperty property, BindingValueType state, Exception? error)
    {
        if (property == SelectedDateTimeProperty)
            DataValidationErrors.SetError(this, error);

        base.UpdateDataValidation(property, state, error);
    }

    /// <summary>
    /// Clears the <see cref="SelectedDateTime"/>.
    /// </summary>
    public void Clear() => SetCurrentValue(SelectedDateTimeProperty, null);

    /// <summary>
    /// Gets a value indicating whether the picker has no value.
    /// </summary>
    public bool IsEmpty() => SelectedDateTime is null;

    private void SetGrid()
    {
        if (!_areControlsAvailable || _contentGrid is null)
            return;

        ResetGridChildrenColumns();

        var fmt = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
        List<(TextBlock? Text, int Index)> orderedDateParts =
        [
            (_monthText, MonthVisible ? fmt.IndexOf('m', StringComparison.OrdinalIgnoreCase) : -1),
            (_yearText, YearVisible ? fmt.IndexOf('y', StringComparison.OrdinalIgnoreCase) : -1),
            (_dayText, DayVisible ? fmt.IndexOf('d', StringComparison.OrdinalIgnoreCase) : -1)
        ];
        orderedDateParts.Sort((x, y) => x.Index - y.Index);

        var use24HourClock = ClockIdentifier == "24HourClock";

        var orderedDateSegments = new List<TextBlock>();
        foreach (var (text, index) in orderedDateParts)
        {
            if (text is null)
                continue;
            text.IsVisible = index != -1;
            if (index != -1)
                orderedDateSegments.Add(text);
        }

        var orderedTimeSegments = new List<TextBlock>();
        addSegment(_hourText, true);
        addSegment(_minuteText, true);
        addSegment(_secondText, UseSeconds);
        addSegment(_periodText, !use24HourClock);

        void addSegment(TextBlock? text, bool visible)
        {
            if (text is null)
                return;
            text.IsVisible = visible;
            if (visible)
                orderedTimeSegments.Add(text);
        }

        var orderedSegments = new List<TextBlock>(orderedDateSegments.Count + orderedTimeSegments.Count);
        orderedSegments.AddRange(orderedDateSegments);
        orderedSegments.AddRange(orderedTimeSegments);
        var datePartCount = orderedDateSegments.Count;

        var columns = new ColumnDefinitions();
        var spacerIndex = 0;

        foreach (var spacer in _spacers)
            spacer.IsVisible = false;
        if (_dateTimeSeparator is not null)
            _dateTimeSeparator.IsVisible = false;

        for (var i = 0; i < orderedSegments.Count; i++)
        {
            if (i > 0)
            {
                columns.Add(new ColumnDefinition(0, GridUnitType.Auto));
                var useDateTimeSeparator = datePartCount > 0 && orderedTimeSegments.Count > 0 && i == datePartCount;
                if (useDateTimeSeparator && _dateTimeSeparator is not null)
                {
                    Grid.SetColumn(_dateTimeSeparator, columns.Count - 1);
                    _dateTimeSeparator.IsVisible = true;
                }
                else if (spacerIndex < _spacers.Count)
                {
                    var spacer = _spacers[spacerIndex++];
                    Grid.SetColumn(spacer, columns.Count - 1);
                    spacer.IsVisible = true;
                }
            }

            columns.Add(new ColumnDefinition(GridLength.Star));
            Grid.SetColumn(orderedSegments[i], columns.Count - 1);
        }

        if (columns.Count == 0)
            columns.Add(new ColumnDefinition(GridLength.Star));

        _contentGrid.ColumnDefinitions = columns;
    }

    private void ResetGridChildrenColumns()
    {
        foreach (var text in new[] { _dayText, _monthText, _yearText, _hourText, _minuteText, _secondText, _periodText })
        {
            if (text is not null)
                Grid.SetColumn(text, 0);
        }

        foreach (var spacer in _spacers)
            Grid.SetColumn(spacer, 0);

        if (_dateTimeSeparator is not null)
            Grid.SetColumn(_dateTimeSeparator, 0);
    }

    private void SetSelectedDateTimeText()
    {
        if (!_areControlsAvailable)
            return;

        if (SelectedDateTime.HasValue)
        {
            PseudoClasses.Set(PseudoHasNoDate, false);
            var dt = SelectedDateTime.Value;

            if (_dayText != null)
                _dayText.Text = dt.ToString(DayFormat, CultureInfo.CurrentCulture);
            if (_monthText != null)
                _monthText.Text = dt.ToString(MonthFormat, CultureInfo.CurrentCulture);
            if (_yearText != null)
                _yearText.Text = dt.ToString(YearFormat, CultureInfo.CurrentCulture);

            var displayTime = dt.TimeOfDay;
            if (ClockIdentifier == "12HourClock")
            {
                var hr = dt.Hour;
                hr = hr > 12 ? hr - 12 : hr == 0 ? 12 : hr;
                displayTime = new TimeSpan(hr, dt.Minute, dt.Second);
            }

            if (_hourText != null)
                _hourText.Text = displayTime.ToString("%h", CultureInfo.CurrentCulture);
            if (_minuteText != null)
                _minuteText.Text = displayTime.ToString("mm", CultureInfo.CurrentCulture);
            if (_secondText != null)
                _secondText.Text = displayTime.ToString("ss", CultureInfo.CurrentCulture);
            if (_periodText != null && ClockIdentifier != "24HourClock")
                _periodText.Text = dt.Hour >= 12 ? GetPmDesignator() : GetAmDesignator();
        }
        else
        {
            PseudoClasses.Set(PseudoHasNoDate, true);
            _dayText?.ClearValue(TextBlock.TextProperty);
            _monthText?.ClearValue(TextBlock.TextProperty);
            _yearText?.ClearValue(TextBlock.TextProperty);
            _hourText?.ClearValue(TextBlock.TextProperty);
            _minuteText?.ClearValue(TextBlock.TextProperty);
            _secondText?.ClearValue(TextBlock.TextProperty);
            if (_periodText != null && ClockIdentifier != "24HourClock")
                _periodText.Text = DateTime.Now.Hour >= 12 ? GetPmDesignator() : GetAmDesignator();
        }
    }

    private static string GetAmDesignator() =>
        !string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator)
            ? CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator
            : CultureInfo.InvariantCulture.DateTimeFormat.AMDesignator;

    private static string GetPmDesignator() =>
        !string.IsNullOrEmpty(CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator)
            ? CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator
            : CultureInfo.InvariantCulture.DateTimeFormat.PMDesignator;

    /// <summary>
    /// Toggles the drop-down open state.
    /// </summary>
    public void TogglePopup()
    {
        if (IsDropDownOpen)
            ClosePopup();
        else
            OpenPopup();
    }

    /// <summary>
    /// Opens the drop-down.
    /// </summary>
    public void OpenPopup()
    {
        if (IsDropDownOpen)
            return;

        if (_presenter is null)
            throw new InvalidOperationException("No DateTimeScrollPickerPresenter found.");
        if (_popup is null)
            throw new InvalidOperationException("No Popup found.");

        _presenter.SelectedDateTime = SelectedDateTime ?? DateTime.Now;

        _popup.Placement = PlacementMode.AnchorAndGravity;
        _popup.PlacementAnchor = PopupAnchor.Bottom;
        _popup.PlacementGravity = PopupGravity.Bottom;
        _popup.PlacementConstraintAdjustment = PopupPositionerConstraintAdjustment.SlideY;
        _popup.IsOpen = true;
        SetCurrentValue(IsDropDownOpenProperty, true);

        if (!_presenter.IsMeasureValid)
            this.GetLayoutManager()?.ExecuteInitialLayoutPass();

        var deltaY = _presenter.GetOffsetForPopup();

        // The extra 5 px relates to the default popup placement behavior.
        _popup.VerticalOffset = deltaY + 5;

        DropDownOpened?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Closes the drop-down.
    /// </summary>
    public void ClosePopup() => _popup?.Close();

    private void OnFlyoutButtonClicked(object? sender, RoutedEventArgs e) => OpenPopup();

    private void OnPopupClosed(object? sender, EventArgs e)
    {
        SetCurrentValue(IsDropDownOpenProperty, false);
        DropDownClosed?.Invoke(this, EventArgs.Empty);
    }

    private void OnDismissPicker(object? sender, EventArgs e)
    {
        _popup?.Close();
        Focus();
    }

    private void OnConfirmed(object? sender, EventArgs e)
    {
        _popup?.Close();
        SetCurrentValue(SelectedDateTimeProperty, _presenter!.SelectedDateTime);
    }
}
