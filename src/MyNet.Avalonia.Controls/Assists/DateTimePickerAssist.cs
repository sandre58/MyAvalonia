// -----------------------------------------------------------------------
// <copyright file="DateTimePickerAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Controls.Assists;

public static class DateTimePickerAssist
{
    #region OverrideWatermark

    /// <summary>
    /// Provides OverrideWatermark Property for attached TextFieldAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> OverrideWatermarkProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("OverrideWatermark", typeof(DateTimePickerAssist), true);

    /// <summary>
    /// Accessor for Attached  <see cref="OverrideWatermarkProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="OverrideWatermarkProperty"/>.</param>
    public static void SetOverrideWatermark(StyledElement element, bool value) => element.SetValue(OverrideWatermarkProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="OverrideWatermarkProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetOverrideWatermark(StyledElement element) => element.GetValue(OverrideWatermarkProperty);

    #endregion

    #region WatermarkDay

    /// <summary>
    /// Provides WatermarkDay Property for attached DatePickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> WatermarkDayProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("WatermarkDay", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkDayProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WatermarkDayProperty"/>.</param>
    public static void SetWatermarkDay(StyledElement element, string value) => element.SetValue(WatermarkDayProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkDayProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetWatermarkDay(StyledElement element) => element.GetValue(WatermarkDayProperty);

    #endregion

    #region WatermarkMonth

    /// <summary>
    /// Provides WatermarkMonth Property for attached DatePickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> WatermarkMonthProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("WatermarkMonth", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkMonthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WatermarkMonthProperty"/>.</param>
    public static void SetWatermarkMonth(StyledElement element, string value) => element.SetValue(WatermarkMonthProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkMonthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetWatermarkMonth(StyledElement element) => element.GetValue(WatermarkMonthProperty);

    #endregion

    #region WatermarkYear

    /// <summary>
    /// Provides WatermarkYear Property for attached DatePickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> WatermarkYearProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("WatermarkYear", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkYearProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WatermarkYearProperty"/>.</param>
    public static void SetWatermarkYear(StyledElement element, string value) => element.SetValue(WatermarkYearProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkYearProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetWatermarkYear(StyledElement element) => element.GetValue(WatermarkYearProperty);

    #endregion

    #region WatermarkHour

    /// <summary>
    /// Provides WatermarkHour Property for attached DateTimePickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> WatermarkHourProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("WatermarkHour", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkHourProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WatermarkHourProperty"/>.</param>
    public static void SetWatermarkHour(StyledElement element, string value) => element.SetValue(WatermarkHourProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkHourProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetWatermarkHour(StyledElement element) => element.GetValue(WatermarkHourProperty);

    #endregion

    #region WatermarkMinute

    /// <summary>
    /// Provides WatermarkMinute Property for attached DateTimePickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> WatermarkMinuteProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("WatermarkMinute", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkMinuteProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WatermarkMinuteProperty"/>.</param>
    public static void SetWatermarkMinute(StyledElement element, string value) => element.SetValue(WatermarkMinuteProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkMinuteProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetWatermarkMinute(StyledElement element) => element.GetValue(WatermarkMinuteProperty);

    #endregion

    #region WatermarkSecond

    /// <summary>
    /// Provides WatermarkSecond Property for attached DateTimePickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> WatermarkSecondProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("WatermarkSecond", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkSecondProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WatermarkSecondProperty"/>.</param>
    public static void SetWatermarkSecond(StyledElement element, string value) => element.SetValue(WatermarkSecondProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WatermarkSecondProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetWatermarkSecond(StyledElement element) => element.GetValue(WatermarkSecondProperty);

    #endregion
}
