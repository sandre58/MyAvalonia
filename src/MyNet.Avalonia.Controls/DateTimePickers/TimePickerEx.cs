// -----------------------------------------------------------------------
// <copyright file="TimePickerEx.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.DateTimes;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartButton, typeof(Button))]
[TemplatePart(PartPopup, typeof(Popup))]
[TemplatePart(PartTextBox, typeof(TextBox))]
[TemplatePart(PartPreviewer, typeof(Control))]
[PseudoClasses(PseudoClassName.FlyoutOpen, PseudoClassName.Pressed)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Improve Avalonia control")]
public class TimePickerEx : TextPicker<TimeSpan?, TimeView>
{
    static TimePickerEx()
    {
        CloseOnCommitProperty.OverrideDefaultValue<TimePickerEx>(false);
        DisplayFormatProperty.OverrideDefaultValue<TimePickerEx>("hh\\:mm");
        ShowSecondsProperty.Changed.AddClassHandler<TimePickerEx>((o, _) => o.DisplayFormat = o.ComputeDisplayFormat());
        TimeFormatProperty.Changed.AddClassHandler<TimePickerEx>((o, _) => o.DisplayFormat = o.ComputeDisplayFormat());
    }

    #region NumberFormat

    /// <summary>
    /// Provides NumberFormat Property.
    /// </summary>
    public static readonly StyledProperty<string> NumberFormatProperty = TimeView.NumberFormatProperty.AddOwner<TimePickerEx>();

    /// <summary>
    /// Gets or sets the NumberFormat property.
    /// </summary>
    public string NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    #endregion

    #region Hour

    /// <summary>
    /// Provides Hour Property.
    /// </summary>
    public static readonly StyledProperty<int?> HourProperty = TimeSelectorBase.HourProperty.AddOwner<TimePickerEx>();

    /// <summary>
    /// Gets or sets the Hour property.
    /// </summary>
    public int? Hour
    {
        get => GetValue(HourProperty);
        set => SetValue(HourProperty, value);
    }

    #endregion

    #region Minute

    /// <summary>
    /// Provides Minute Property.
    /// </summary>
    public static readonly StyledProperty<int?> MinuteProperty = TimeSelectorBase.MinuteProperty.AddOwner<TimePickerEx>();

    /// <summary>
    /// Gets or sets the Minute property.
    /// </summary>
    public int? Minute
    {
        get => GetValue(MinuteProperty);
        set => SetValue(MinuteProperty, value);
    }

    #endregion

    #region Second

    /// <summary>
    /// Provides Second Property.
    /// </summary>
    public static readonly StyledProperty<int?> SecondProperty = TimeSelectorBase.SecondProperty.AddOwner<TimePickerEx>();

    /// <summary>
    /// Gets or sets the Second property.
    /// </summary>
    public int? Second
    {
        get => GetValue(SecondProperty);
        set => SetValue(SecondProperty, value);
    }

    #endregion

    #region IsAm

    /// <summary>
    /// Provides IsAm Property.
    /// </summary>
    public static readonly StyledProperty<bool> IsAmProperty = TimeSelectorBase.IsAmProperty.AddOwner<TimePickerEx>();

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the IsAm property.
    /// </summary>
    public bool IsAm
    {
        get => GetValue(IsAmProperty);
        set => SetValue(IsAmProperty, value);
    }

    #endregion

    #region ShowSeconds

    /// <summary>
    /// Defines the <see cref="ShowSeconds"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowSecondsProperty = TimeSelectorBase.ShowSecondsProperty.AddOwner<TimePickerEx>();

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets is seconds selector is displayed.
    /// </summary>
    public bool ShowSeconds
    {
        get => GetValue(ShowSecondsProperty);
        set => SetValue(ShowSecondsProperty, value);
    }

    #endregion

    #region TimeFormat

    /// <summary>
    /// Defines the <see cref="TimeFormat"/> property.
    /// </summary>
    public static readonly StyledProperty<TimeFormat> TimeFormatProperty = TimeSelectorBase.TimeFormatProperty.AddOwner<TimePickerEx>();

    /// <summary>
    /// Gets or sets the time format.
    /// </summary>
    public TimeFormat TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    #endregion

    #region Selector

    protected override void AddPreviewerHandlers() => Previewer?.OnLoading<TimeView>(x => x.SelectedValueChanged += OnTimeChanged, x => x.SelectedValueChanged -= OnTimeChanged);

    private void OnTimeChanged(object? sender, SelectionChangedEventArgs e) => OnPreviewValueChanged();

    #endregion

    protected override TimeSpan? IncrementValue(int offset) => SelectedValue?.Add(offset.Minutes());

    protected override TimeSpan? IncrementLargeValue(int offset) => SelectedValue?.Add(offset.Hours());

    protected override string? ConvertValueToString(TimeSpan? value)
    {
        if (value == null)
            return null;

        if (string.IsNullOrWhiteSpace(DisplayFormat))
            return value.ToString();

        var dateTime = DateTime.Today.Add(value.Value);
        return dateTime.ToString(DisplayFormat, CultureInfo.CurrentCulture);
    }

    protected override TimeSpan? ConvertValueFromString(string text) => string.IsNullOrWhiteSpace(text)
            ? null
            : DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out var dateTime)
            ? dateTime.TimeOfDay
            : TimeSpan.TryParse(text, CultureInfo.CurrentCulture, out var timeSpan) ? timeSpan : null;

    protected override void SetPreviewValue(TimeSpan? value) => Previewer?.SelectedValue = value;

    protected override TimeSpan? GetPreviewValue() => Previewer?.SelectedValue;

    private string ComputeDisplayFormat()
    {
        var baseFormat = TimeFormat == TimeFormat.TwelveHour ? "h:mm" : "HH:mm";
        var format = ShowSeconds ? $"{baseFormat}:ss" : baseFormat;

        if (TimeFormat == TimeFormat.TwelveHour)
            format += " tt";

        return format;
    }
}
