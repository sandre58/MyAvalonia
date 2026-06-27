// -----------------------------------------------------------------------
// <copyright file="DateTimeScrollPickerPresenter.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Defines the presenter used for selecting a time. Intended for use with
/// <see cref="TimePicker"/> but can be used independently.
/// </summary>
[TemplatePart(TemplateItems.AcceptButtonName, typeof(Button), IsRequired = true)]
[TemplatePart(TemplateItems.DismissButtonName, typeof(Button))]
[TemplatePart(TemplateItems.HourDownButtonName, typeof(Button))]
[TemplatePart(TemplateItems.HourSelectorName, typeof(DateTimePickerPanel), IsRequired = true)]
[TemplatePart(TemplateItems.HourUpButtonName, typeof(Button))]
[TemplatePart(TemplateItems.MinuteDownButtonName, typeof(Button))]
[TemplatePart(TemplateItems.MinuteSelectorName, typeof(DateTimePickerPanel), IsRequired = true)]
[TemplatePart(TemplateItems.MinuteUpButtonName, typeof(Button))]
[TemplatePart(TemplateItems.SecondDownButtonName, typeof(Button))]
[TemplatePart(TemplateItems.SecondHostName, typeof(Panel))]
[TemplatePart(TemplateItems.SecondSelectorName, typeof(DateTimePickerPanel))]
[TemplatePart(TemplateItems.SecondUpButtonName, typeof(Button))]
[TemplatePart(TemplateItems.PeriodDownButtonName, typeof(Button))]
[TemplatePart(TemplateItems.PeriodHostName, typeof(Panel), IsRequired = true)]
[TemplatePart(TemplateItems.PeriodSelectorName, typeof(DateTimePickerPanel), IsRequired = true)]
[TemplatePart(TemplateItems.PeriodUpButtonName, typeof(Button))]
[TemplatePart(TemplateItems.PickerContainerName, typeof(Grid), IsRequired = true)]
[TemplatePart(TemplateItems.SecondSpacerName, typeof(Control), IsRequired = true)]
[TemplatePart(TemplateItems.ThirdSpacerName, typeof(Control))]
public class DateTimeScrollPickerPresenter : DatePickerPresenter
{
    /// <summary>
    /// Defines the <see cref="MinuteIncrement"/> property.
    /// </summary>
    public static readonly StyledProperty<int> MinuteIncrementProperty =
        DateTimeScrollPickerEx.MinuteIncrementProperty.AddOwner<DateTimeScrollPickerPresenter>();

    /// <summary>
    /// Defines the <see cref="SecondIncrement"/> property.
    /// </summary>
    public static readonly StyledProperty<int> SecondIncrementProperty =
        DateTimeScrollPickerEx.SecondIncrementProperty.AddOwner<DateTimeScrollPickerPresenter>();

    /// <summary>
    /// Defines the <see cref="ClockIdentifier"/> property.
    /// </summary>
    public static readonly StyledProperty<string> ClockIdentifierProperty =
        DateTimeScrollPickerEx.ClockIdentifierProperty.AddOwner<DateTimeScrollPickerPresenter>();

    /// <summary>
    /// Defines the <see cref="UseSeconds"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> UseSecondsProperty =
        DateTimeScrollPickerEx.UseSecondsProperty.AddOwner<DateTimeScrollPickerPresenter>();

    /// <summary>
    /// Defines the <see cref="Time"/> property.
    /// </summary>
    public static readonly StyledProperty<TimeSpan> TimeProperty =
        AvaloniaProperty.Register<DateTimeScrollPickerPresenter, TimeSpan>(nameof(Time));

    public DateTimeScrollPickerPresenter() => SetCurrentValue(TimeProperty, DateTime.Now.TimeOfDay);

    static DateTimeScrollPickerPresenter() => KeyboardNavigation.TabNavigationProperty.OverrideDefaultValue<DateTimeScrollPickerPresenter>(KeyboardNavigationMode.Cycle);

    private struct TemplateItems
    {
        public const string PickerContainerName = "PART_PickerContainer";
        public const string AcceptButtonName = "PART_AcceptButton";
        public const string DismissButtonName = "PART_DismissButton";
        public const string SecondSpacerName = "PART_SecondSpacer";
        public const string ThirdSpacerName = "PART_ThirdSpacer";
        public const string SecondHostName = "PART_SecondHost";
        public const string PeriodHostName = "PART_PeriodHost";
        public const string HourSelectorName = "PART_HourSelector";
        public const string MinuteSelectorName = "PART_MinuteSelector";
        public const string SecondSelectorName = "PART_SecondSelector";
        public const string PeriodSelectorName = "PART_PeriodSelector";
        public const string HourUpButtonName = "PART_HourUpButton";
        public const string MinuteUpButtonName = "PART_MinuteUpButton";
        public const string SecondUpButtonName = "PART_SecondUpButton";
        public const string PeriodUpButtonName = "PART_PeriodUpButton";
        public const string HourDownButtonName = "PART_HourDownButton";
        public const string MinuteDownButtonName = "PART_MinuteDownButton";
        public const string SecondDownButtonName = "PART_SecondDownButton";
        public const string PeriodDownButtonName = "PART_PeriodDownButton";

        public Grid PickerContainer { get; init; }

        public Button AcceptButton { get; init; }

        public Button? DismissButton { get; init; }

        public Control SecondSpacer { get; init; } // the 2nd spacer, not seconds of time

        public Control? ThirdSpacer { get; init; }

        public Panel? SecondHost { get; init; }

        public Panel PeriodHost { get; init; }

        public DateTimePickerPanel HourSelector { get; init; }

        public DateTimePickerPanel MinuteSelector { get; init; }

        public DateTimePickerPanel? SecondSelector { get; init; }

        public DateTimePickerPanel PeriodSelector { get; init; }

        public Button? HourUpButton { get; init; }

        public Button? MinuteUpButton { get; init; }

        public Button? SecondUpButton { get; init; }

        public Button? PeriodUpButton { get; init; }

        public Button? HourDownButton { get; init; }

        public Button? MinuteDownButton { get; init; }

        public Button? SecondDownButton { get; init; }

        public Button? PeriodDownButton { get; init; }
    }

    private TemplateItems? _templateItems;

    /// <summary>
    /// Gets or sets the minute increment in the selector.
    /// </summary>
    public int MinuteIncrement
    {
        get => GetValue(MinuteIncrementProperty);
        set => SetValue(MinuteIncrementProperty, value);
    }

    /// <summary>
    /// Gets or sets the second increment in the selector.
    /// </summary>
    public int SecondIncrement
    {
        get => GetValue(SecondIncrementProperty);
        set => SetValue(SecondIncrementProperty, value);
    }

    /// <summary>
    /// Gets or sets the current clock identifier, either 12HourClock or 24HourClock.
    /// </summary>
    public string ClockIdentifier
    {
        get => GetValue(ClockIdentifierProperty);
        set => SetValue(ClockIdentifierProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the selector uses seconds.
    /// </summary>
    public bool UseSeconds
    {
        get => GetValue(UseSecondsProperty);
        set => SetValue(UseSecondsProperty, value);
    }

    /// <summary>
    /// Gets or sets the current time.
    /// </summary>
    public TimeSpan Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _templateItems = new()
        {
            PickerContainer = e.NameScope.Get<Grid>(TemplateItems.PickerContainerName),
            PeriodHost = e.NameScope.Get<Panel>(TemplateItems.PeriodHostName),
            SecondHost = e.NameScope.Find<Panel>(TemplateItems.SecondHostName),
            HourSelector = e.NameScope.Get<DateTimePickerPanel>(TemplateItems.HourSelectorName),
            MinuteSelector = e.NameScope.Get<DateTimePickerPanel>(TemplateItems.MinuteSelectorName),
            SecondSelector = e.NameScope.Find<DateTimePickerPanel>(TemplateItems.SecondSelectorName),
            PeriodSelector = e.NameScope.Get<DateTimePickerPanel>(TemplateItems.PeriodSelectorName),
            SecondSpacer = e.NameScope.Get<Control>(TemplateItems.SecondSpacerName),
            ThirdSpacer = e.NameScope.Find<Control>(TemplateItems.ThirdSpacerName),
            AcceptButton = e.NameScope.Get<Button>(TemplateItems.AcceptButtonName),
            HourUpButton = selectorButton(TemplateItems.HourUpButtonName, DateTimePickerPanelType.Hour, SpinDirection.Decrease),
            HourDownButton = selectorButton(TemplateItems.HourDownButtonName, DateTimePickerPanelType.Hour, SpinDirection.Increase),
            MinuteUpButton = selectorButton(TemplateItems.MinuteUpButtonName, DateTimePickerPanelType.Minute, SpinDirection.Decrease),
            MinuteDownButton = selectorButton(TemplateItems.MinuteDownButtonName, DateTimePickerPanelType.Minute, SpinDirection.Increase),
            SecondUpButton = selectorButton(TemplateItems.SecondUpButtonName, DateTimePickerPanelType.Second, SpinDirection.Decrease),
            SecondDownButton = selectorButton(TemplateItems.SecondDownButtonName, DateTimePickerPanelType.Second, SpinDirection.Increase),
            PeriodUpButton = selectorButton(TemplateItems.PeriodUpButtonName, DateTimePickerPanelType.TimePeriod, SpinDirection.Decrease),
            PeriodDownButton = selectorButton(TemplateItems.PeriodDownButtonName, DateTimePickerPanelType.TimePeriod, SpinDirection.Increase),
            DismissButton = e.NameScope.Find<Button>(TemplateItems.DismissButtonName),
        };

        _templateItems.Value.AcceptButton.Click += OnAcceptButtonClicked;
        if (_templateItems.Value.DismissButton is { } dismissButton)
        {
            dismissButton.Click += OnDismissButtonClicked;
        }

        InitPicker();

        Button? selectorButton(string name, DateTimePickerPanelType type, SpinDirection direction)
        {
            if (e.NameScope.Find<Button>(name) is { } button)
            {
                button.Click += (_, _) => OnSelectorButtonClick(type, direction);
                return button;
            }

            return null;
        }
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == MinuteIncrementProperty ||
            change.Property == SecondIncrementProperty ||
            change.Property == ClockIdentifierProperty ||
            change.Property == UseSecondsProperty ||
            change.Property == TimeProperty)
        {
            InitPicker();
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Escape:
                OnDismiss();
                e.Handled = true;
                break;
            case Key.Tab:
                if (FocusManager.GetFocusManager(this)?.GetFocusedElement() is { } focus)
                {
                    var nextFocus = KeyboardNavigationHandler.GetNext(focus, NavigationDirection.Next);
                    nextFocus?.Focus(NavigationMethod.Tab);
                    e.Handled = true;
                }

                break;
            case Key.Enter:
                OnConfirmed();
                e.Handled = true;
                break;
        }

        base.OnKeyDown(e);
    }

    protected override void OnConfirmed()
    {
        if (_templateItems is { } items)
        {
            var hr = items.HourSelector.SelectedValue;
            var min = items.MinuteSelector.SelectedValue;
            var sec = items.SecondSelector?.SelectedValue ?? 0;
            var per = items.PeriodSelector.SelectedValue;

            if (ClockIdentifier == "12HourClock")
            {
                hr = per == 1 ? (hr == 12) ? 12 : hr + 12 : per == 0 && hr == 12 ? 0 : hr;
            }

            SetCurrentValue(TimeProperty, new(hr, min, UseSeconds ? sec : 0));
        }

        base.OnConfirmed();
    }

    private void InitPicker()
    {
        if (_templateItems is not { } items)
            return;

        var clock12 = ClockIdentifier == "12HourClock";
        items.HourSelector.MaximumValue = clock12 ? 12 : 23;
        items.HourSelector.MinimumValue = clock12 ? 1 : 0;
        items.HourSelector.ItemFormat = "%h";
        var hr = Time.Hours;
        items.HourSelector.SelectedValue = !clock12 ? hr :
            hr > 12 ? hr - 12 :
            hr == 0 ? 12 : hr;

        items.MinuteSelector.MaximumValue = 59;
        items.MinuteSelector.MinimumValue = 0;
        items.MinuteSelector.Increment = MinuteIncrement;
        items.MinuteSelector.ItemFormat = "mm";
        items.MinuteSelector.SelectedValue = Time.Minutes;

        if (items.SecondSelector is { } secondSelector)
        {
            secondSelector.MaximumValue = 59;
            secondSelector.MinimumValue = 0;
            secondSelector.Increment = SecondIncrement;
            secondSelector.ItemFormat = "ss";
            secondSelector.SelectedValue = Time.Seconds;
        }

        items.PeriodSelector.MaximumValue = 1;
        items.PeriodSelector.MinimumValue = 0;
        items.PeriodSelector.SelectedValue = hr >= 12 ? 1 : 0;

        SetGrid(items);
        items.HourSelector.Focus(NavigationMethod.Pointer);
    }

    private void SetGrid(TemplateItems items)
    {
        var use24HourClock = ClockIdentifier == "24HourClock";

        var columnsD = new ColumnDefinitions
        {
            new(GridLength.Star),
            new(GridLength.Auto),
            new(GridLength.Star)
        };

        if (items.SecondHost is not null && items.ThirdSpacer is not null)
        {
            if (UseSeconds)
            {
                columnsD.Add(new(GridLength.Auto));
                columnsD.Add(new(GridLength.Star));
            }

            items.SecondSpacer.IsVisible = UseSeconds;
            items.SecondHost.IsVisible = UseSeconds;
            items.ThirdSpacer.IsVisible = !use24HourClock;
            items.PeriodHost.IsVisible = !use24HourClock;

            var amPmColumn = UseSeconds ? 6 : 4;

            Grid.SetColumn(items.SecondSpacer, UseSeconds ? 3 : 0);
            Grid.SetColumn(items.SecondHost, UseSeconds ? 4 : 0);
            Grid.SetColumn(items.ThirdSpacer, use24HourClock ? 0 : amPmColumn - 1);
            Grid.SetColumn(items.PeriodHost, use24HourClock ? 0 : amPmColumn);
        }
        else
        {
            items.SecondSpacer.IsVisible = !use24HourClock;
            items.PeriodHost.IsVisible = !use24HourClock;
            Grid.SetColumn(items.SecondSpacer, use24HourClock ? 0 : 3);
            Grid.SetColumn(items.PeriodHost, use24HourClock ? 0 : 4);
        }

        if (!use24HourClock)
        {
            columnsD.Add(new(GridLength.Auto));
            columnsD.Add(new(GridLength.Star));
        }

        items.PickerContainer.ColumnDefinitions = columnsD;
    }

    private void OnDismissButtonClicked(object? sender, RoutedEventArgs e) => OnDismiss();

    private void OnAcceptButtonClicked(object? sender, RoutedEventArgs e) => OnConfirmed();

    private void OnSelectorButtonClick(DateTimePickerPanelType type, SpinDirection direction)
    {
        var target = type switch
        {
            DateTimePickerPanelType.Hour => _templateItems?.HourSelector,
            DateTimePickerPanelType.Minute => _templateItems?.MinuteSelector,
            DateTimePickerPanelType.Second => _templateItems?.SecondSelector,
            DateTimePickerPanelType.TimePeriod => _templateItems?.PeriodSelector,
            _ => throw new NotImplementedException(),
        };

        switch (direction)
        {
            case SpinDirection.Increase:
                target?.ScrollDown();
                break;
            case SpinDirection.Decrease:
                target?.ScrollUp();
                break;
            default:
                throw new NotImplementedException();
        }
    }

    internal double GetOffsetForPopup()
    {
        if (_templateItems is not { } items)
            return 0;

        var acceptDismissButtonHeight = items.AcceptButton.Bounds.Height;
        return (-(MaxHeight - acceptDismissButtonHeight) / 2) - (items.HourSelector.ItemHeight / 2);
    }
}
