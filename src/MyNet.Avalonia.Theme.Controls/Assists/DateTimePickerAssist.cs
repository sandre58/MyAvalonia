// -----------------------------------------------------------------------
// <copyright file="DateTimePickerAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Theme.Controls.Assists;

public static class DateTimePickerAssist
{
    #region PlaceholderDay

    /// <summary>
    /// Provides PlaceholderDay Property for attached DatePickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> PlaceholderDayProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("PlaceholderDay", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderDayProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderDayProperty"/>.</param>
    public static void SetPlaceholderDay(StyledElement element, string value) => element.SetValue(PlaceholderDayProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderDayProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetPlaceholderDay(StyledElement element) => element.GetValue(PlaceholderDayProperty);

    #endregion

    #region PlaceholderMonth

    /// <summary>
    /// Provides PlaceholderMonth Property for attached DatePickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> PlaceholderMonthProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("PlaceholderMonth", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderMonthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderMonthProperty"/>.</param>
    public static void SetPlaceholderMonth(StyledElement element, string value) => element.SetValue(PlaceholderMonthProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderMonthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetPlaceholderMonth(StyledElement element) => element.GetValue(PlaceholderMonthProperty);

    #endregion

    #region PlaceholderYear

    /// <summary>
    /// Provides PlaceholderYear Property for attached DatePickerAssist element.
    /// </summary>
    public static readonly AttachedProperty<string> PlaceholderYearProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("PlaceholderYear", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderYearProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderYearProperty"/>.</param>
    public static void SetPlaceholderYear(StyledElement element, string value) => element.SetValue(PlaceholderYearProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderYearProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetPlaceholderYear(StyledElement element) => element.GetValue(PlaceholderYearProperty);

    #endregion

    #region PlaceholderHour

    /// <summary>
    /// Provides PlaceholderHour Property for attached DateTimePickerBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string> PlaceholderHourProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("PlaceholderHour", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderHourProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderHourProperty"/>.</param>
    public static void SetPlaceholderHour(StyledElement element, string value) => element.SetValue(PlaceholderHourProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderHourProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetPlaceholderHour(StyledElement element) => element.GetValue(PlaceholderHourProperty);

    #endregion

    #region PlaceholderMinute

    /// <summary>
    /// Provides PlaceholderMinute Property for attached DateTimePickerBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string> PlaceholderMinuteProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("PlaceholderMinute", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderMinuteProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderMinuteProperty"/>.</param>
    public static void SetPlaceholderMinute(StyledElement element, string value) => element.SetValue(PlaceholderMinuteProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderMinuteProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetPlaceholderMinute(StyledElement element) => element.GetValue(PlaceholderMinuteProperty);

    #endregion

    #region PlaceholderSecond

    /// <summary>
    /// Provides PlaceholderSecond Property for attached DateTimePickerBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string> PlaceholderSecondProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("PlaceholderSecond", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderSecondProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderSecondProperty"/>.</param>
    public static void SetPlaceholderSecond(StyledElement element, string value) => element.SetValue(PlaceholderSecondProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderSecondProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetPlaceholderSecond(StyledElement element) => element.GetValue(PlaceholderSecondProperty);

    #endregion

    #region PlaceholderPeriod

    /// <summary>
    /// Provides PlaceholderPeriod Property for attached DateTimePickerBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string> PlaceholderPeriodProperty = AvaloniaProperty.RegisterAttached<StyledElement, string>("PlaceholderPeriod", typeof(DateTimePickerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderPeriodProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="PlaceholderPeriodProperty"/>.</param>
    public static void SetPlaceholderPeriod(StyledElement element, string value) => element.SetValue(PlaceholderPeriodProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="PlaceholderPeriodProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string GetPlaceholderPeriod(StyledElement element) => element.GetValue(PlaceholderPeriodProperty);

    #endregion
}
