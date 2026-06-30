// -----------------------------------------------------------------------
// <copyright file="TimeRangePickerEx.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using MyNet.Avalonia.Controls.Internals;
using MyNet.Avalonia.Controls.Localization;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Primitives;
using MyNet.Primitives.Intervals;
using MyNet.Primitives.Temporal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartPreviewer, typeof(TimeRangeView))]
[TemplatePart(PartClearButton, typeof(Button))]
[PseudoClasses(PseudoClassName.FlyoutOpen, PseudoClassName.Pressed)]
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Improve Avalonia control")]
public partial class TimeRangePickerEx : TextPicker<Period?, TimeRangeView>
{
    public const string PartClearButton = "PART_ClearButton";

    private Button? _clearButton;
    private TimeSpan? _partialStart;
    private TimeSpan? _partialEnd;

    static TimeRangePickerEx()
    {
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<TimeRangePickerEx>(AutomationControlType.Custom);
        CloseOnCommitProperty.OverrideDefaultValue<TimeRangePickerEx>(false);
        DisplayFormatProperty.OverrideDefaultValue<TimeRangePickerEx>("hh\\:mm");
        ShowSecondsProperty.Changed.AddClassHandler<TimeRangePickerEx>((o, _) => o.DisplayFormat = o.ComputeDisplayFormat());
        TimeFormatProperty.Changed.AddClassHandler<TimeRangePickerEx>((o, _) => o.DisplayFormat = o.ComputeDisplayFormat());
    }

    #region NumberFormat

    public static readonly StyledProperty<string> NumberFormatProperty = TimeView.NumberFormatProperty.AddOwner<TimeRangePickerEx>();

    public string NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    #endregion

    #region ShowSeconds

    public static readonly StyledProperty<bool> ShowSecondsProperty = TimeSelectorBase.ShowSecondsProperty.AddOwner<TimeRangePickerEx>();

    public bool ShowSeconds
    {
        get => GetValue(ShowSecondsProperty);
        set => SetValue(ShowSecondsProperty, value);
    }

    #endregion

    #region TimeFormat

    public static readonly StyledProperty<TimeFormat> TimeFormatProperty = TimeSelectorBase.TimeFormatProperty.AddOwner<TimeRangePickerEx>();

    public TimeFormat TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    #endregion

    #region RangeSeparator

    public static readonly StyledProperty<string> RangeSeparatorProperty =
        AvaloniaProperty.Register<TimeRangePickerEx, string>(nameof(RangeSeparator), " – ");

    public string RangeSeparator
    {
        get => GetValue(RangeSeparatorProperty);
        set => SetValue(RangeSeparatorProperty, value);
    }

    #endregion

    #region AllowOvernight

    public static readonly StyledProperty<bool> AllowOvernightProperty =
        AvaloniaProperty.Register<TimeRangePickerEx, bool>(nameof(AllowOvernight));

    public bool AllowOvernight
    {
        get => GetValue(AllowOvernightProperty);
        set => SetValue(AllowOvernightProperty, value);
    }

    #endregion

    #region InvalidRangeBehavior

    public static readonly StyledProperty<TimeRangeInvalidBehavior> InvalidRangeBehaviorProperty =
        AvaloniaProperty.Register<TimeRangePickerEx, TimeRangeInvalidBehavior>(nameof(InvalidRangeBehavior), TimeRangeInvalidBehavior.Swap);

    public TimeRangeInvalidBehavior InvalidRangeBehavior
    {
        get => GetValue(InvalidRangeBehaviorProperty);
        set => SetValue(InvalidRangeBehaviorProperty, value);
    }

    #endregion

    #region ShowOvernightIndicator

    public static readonly StyledProperty<bool> ShowOvernightIndicatorProperty =
        AvaloniaProperty.Register<TimeRangePickerEx, bool>(nameof(ShowOvernightIndicator));

    public bool ShowOvernightIndicator
    {
        get => GetValue(ShowOvernightIndicatorProperty);
        set => SetValue(ShowOvernightIndicatorProperty, value);
    }

    #endregion

    #region ReferenceDate

    public static readonly StyledProperty<DateTime> ReferenceDateProperty =
        AvaloniaProperty.Register<TimeRangePickerEx, DateTime>(nameof(ReferenceDate), DateTime.Today);

    public DateTime ReferenceDate
    {
        get => GetValue(ReferenceDateProperty);
        set => SetValue(ReferenceDateProperty, value);
    }

    #endregion

    #region StartTime / EndTime

    public TimeSpan? StartTime
    {
        get => SelectedValue is { } period ? TimeRangeHelper.GetPeriodStartTime(period) : _partialStart;
        set => SetBoundaryTime(value, isStart: true);
    }

    public TimeSpan? EndTime
    {
        get => SelectedValue is { } period ? TimeRangeHelper.GetPeriodEndTime(period) : _partialEnd;
        set => SetBoundaryTime(value, isStart: false);
    }

    private void SetBoundaryTime(TimeSpan? value, bool isStart)
    {
        if (!value.HasValue)
        {
            if (isStart)
            {
                _partialStart = null;
                SetCurrentValue(SelectedValueProperty, null);
            }
            else
            {
                _partialEnd = null;
                if (SelectedValue is { } period)
                {
                    _partialStart = TimeRangeHelper.GetPeriodStartTime(period);
                    SetCurrentValue(SelectedValueProperty, null);
                }
            }

            return;
        }

        if (isStart)
            _partialStart = value;
        else
            _partialEnd = value;

        TryCommitPartialRange();
    }

    private void TryCommitPartialRange()
    {
        if (_partialStart is not { } start || _partialEnd is not { } end)
            return;

        var result = TimeRangeHelper.BuildPeriod(start, end, ReferenceDate, AllowOvernight, InvalidRangeBehavior);
        if (!result.IsValid)
        {
            if (result.ShouldReportError)
                ReportInvalidRange();

            return;
        }

        _partialStart = null;
        _partialEnd = null;
        SetCurrentValue(SelectedValueProperty, result.Period);
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_clearButton is not null)
            _clearButton.Click -= OnClearButtonClick;

        _clearButton = e.NameScope.Find<Button>(PartClearButton);
        if (_clearButton is not null)
            _clearButton.Click += OnClearButtonClick;
    }

    private void OnClearButtonClick(object? sender, RoutedEventArgs e) => Clear();

    protected override void AddPreviewerHandlers()
    {
        base.AddPreviewerHandlers();
        Previewer?.OnLoading<TimeRangeView>(
            x =>
            {
                x.SelectedValueChanged += OnPreviewerValueChanged;
                SyncPreviewerSettings(x);
            },
            x => x.SelectedValueChanged -= OnPreviewerValueChanged);
    }

    protected override void OnDropDownClosing()
    {
        if (Previewer?.TryBuildSelectedValue() is { IsValid: true, Period: { } period }
            && !Equals(period, SelectedValue))
        {
            SetCurrentValue(SelectedValueProperty, period);
            DataValidationErrors.ClearErrors(this);
            return;
        }

        base.OnDropDownClosing();
    }

    protected override bool ShouldRollbackOnClose() => HasUncommittedIncompletePreview();

    private void OnPreviewerValueChanged(object? sender, SelectionChangedEventArgs e) => OnPreviewValueChanged();

    protected override void TryFocusPopupContent()
    {
        if (Previewer is { } view)
        {
            view.FinalizePopupOpen();
            return;
        }

        base.TryFocusPopupContent();
    }

    private void SyncPreviewerSettings(TimeRangeView previewer)
    {
        previewer.NumberFormat = NumberFormat;
        previewer.ShowSeconds = ShowSeconds;
        previewer.TimeFormat = TimeFormat;
        previewer.AllowOvernight = AllowOvernight;
        previewer.InvalidRangeBehavior = InvalidRangeBehavior;
        previewer.ReferenceDate = ReferenceDate;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == SelectedValueProperty && IsDropDownOpen)
        {
            var addedValue = change.GetNewValue<Period?>();
            SetCurrentValue(TextProperty, ConvertValueToString(addedValue));
            _partialStart = null;
            _partialEnd = null;
        }
        else
        {
            base.OnPropertyChanged(change);

            if (change.Property == SelectedValueProperty)
            {
                _partialStart = null;
                _partialEnd = null;
            }
        }

        if (change.Property == IsDropDownOpenProperty && change.GetNewValue<bool>())
            Previewer?.ResetForPopupOpen();
        else if (change.Property == RangeSeparatorProperty && SelectedValue is not null)
        {
            SetCurrentValue(TextProperty, ConvertValueToString(SelectedValue));
        }
        else if (Previewer is not null && change.Property == NumberFormatProperty)
        {
            Previewer.NumberFormat = change.GetNewValue<string>();
        }
        else if (Previewer is not null && change.Property == ShowSecondsProperty)
        {
            Previewer.ShowSeconds = change.GetNewValue<bool>();
        }
        else if (Previewer is not null && change.Property == TimeFormatProperty)
        {
            Previewer.TimeFormat = change.GetNewValue<TimeFormat>();
        }
        else if (Previewer is not null && change.Property == AllowOvernightProperty)
        {
            Previewer.AllowOvernight = change.GetNewValue<bool>();
        }
        else if (Previewer is not null && change.Property == InvalidRangeBehaviorProperty)
        {
            Previewer.InvalidRangeBehavior = change.GetNewValue<TimeRangeInvalidBehavior>();
        }
        else if (Previewer is not null && change.Property == ReferenceDateProperty)
        {
            Previewer.ReferenceDate = change.GetNewValue<DateTime>();
        }
        else if (change.Property == ShowOvernightIndicatorProperty && SelectedValue is not null)
        {
            SetCurrentValue(TextProperty, ConvertValueToString(SelectedValue));
        }
    }

    public override void CommitFromPreview()
    {
        var previewValue = GetPreviewValue();
        if (previewValue is null && Previewer?.StartTime is null && Previewer?.EndTime is null)
        {
            SetCurrentValue(SelectedValueProperty, null);
        }
        else if (Previewer?.TryBuildSelectedValue() is { IsValid: false, ShouldReportError: true })
        {
            ReportInvalidRange();
            return;
        }
        else
        {
            SetCurrentValue(SelectedValueProperty, previewValue);
        }

        DataValidationErrors.ClearErrors(this);
    }

    public override void Clear()
    {
        base.Clear();
        Previewer?.Clear();
    }

    protected override Period? IncrementValue(int offset)
    {
        if (SelectedValue is not { } period)
            return null;

        return period.Shift(TimeSpan.FromMinutes(offset));
    }

    protected override Period? IncrementLargeValue(int offset)
    {
        if (SelectedValue is not { } period)
            return null;

        return period.Shift(TimeSpan.FromHours(offset));
    }

    protected override string? ConvertValueToString(Period? value)
    {
        if (value is null)
            return null;

        var format = DisplayFormat ?? "hh\\:mm";
        var culture = CultureInfo.CurrentCulture;
        var start = FormatTime(TimeRangeHelper.GetPeriodStartTime(value), format, culture);
        var end = FormatTime(TimeRangeHelper.GetPeriodEndTime(value), format, culture);
        var text = $"{start}{RangeSeparator}{end}";

        if (ShowOvernightIndicator && TimeRangeHelper.SpansOvernight(value))
            text += TimeRangePickerExResources.OvernightIndicatorSuffix;

        return text;
    }

    protected override Period ConvertValueFromString(string text)
    {
        var separator = RangeSeparator;
        var index = text.IndexOf(separator, StringComparison.Ordinal);
        if (index < 0)
            throw new FormatException($"Expected range separator '{separator}'.");

        var format = DisplayFormat ?? "hh\\:mm";
        var culture = CultureInfo.CurrentCulture;
        var startText = text[..index].Trim();
        var endText = text[(index + separator.Length)..].Trim();

        if (ShowOvernightIndicator)
        {
            var suffix = TimeRangePickerExResources.OvernightIndicatorSuffix;
            if (endText.EndsWith(suffix, StringComparison.Ordinal))
                endText = endText[..^suffix.Length].TrimEnd();
        }

        var start = ParseTime(startText, format, culture);
        var end = ParseTime(endText, format, culture);

        var result = TimeRangeHelper.BuildPeriod(start, end, ReferenceDate, AllowOvernight, InvalidRangeBehavior);
        if (!result.IsValid)
        {
            if (result.ShouldReportError)
                throw new ArgumentOutOfRangeException(nameof(text), TimeRangePickerExResources.EndBeforeStart);

            throw new FormatException($"Invalid time range '{text}'.");
        }

        return result.Period!;
    }

    protected override void SetPreviewValue(Period? value)
    {
        if (Previewer is null)
            return;

        SyncPreviewerSettings(Previewer);

        if (value is null)
        {
            Previewer.Clear();
            return;
        }

        Previewer.LoadFromPeriod(value);
    }

    protected override Period? GetPreviewValue()
    {
        if (Previewer is null)
            return null;

        var result = Previewer.TryBuildSelectedValue();
        return result.IsValid ? result.Period : SelectedValue;
    }

    private bool HasUncommittedIncompletePreview()
    {
        if (Previewer is null)
            return false;

        var preview = Previewer.TryBuildSelectedValue();
        if (preview.IsValid)
            return false;

        return Previewer.StartTime.HasValue || Previewer.EndTime.HasValue;
    }

    private void ReportInvalidRange() =>
        DataValidationErrors.SetError(this, new ArgumentOutOfRangeException(nameof(SelectedValue), TimeRangePickerExResources.EndBeforeStart));

    private static string FormatTime(TimeSpan time, string format, CultureInfo culture) =>
        DateTime.Today.Add(time).ToString(format, culture);

    private static TimeSpan ParseTime(string text, string format, CultureInfo culture)
    {
        if (DateTime.TryParseExact(text, format, culture, DateTimeStyles.None, out var dateTime))
            return dateTime.TimeOfDay;

        if (TimeSpan.TryParse(text, culture, out var timeSpan))
            return timeSpan;

        throw new FormatException($"Could not parse time '{text}'.");
    }

    private string ComputeDisplayFormat()
    {
        var baseFormat = TimeFormat == TimeFormat.TwelveHour ? "h:mm" : "HH:mm";
        var format = ShowSeconds ? $"{baseFormat}:ss" : baseFormat;

        if (TimeFormat == TimeFormat.TwelveHour)
            format += " tt";

        return format;
    }
}
