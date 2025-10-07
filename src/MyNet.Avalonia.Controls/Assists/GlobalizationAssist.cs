// -----------------------------------------------------------------------
// <copyright file="GlobalizationAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Extensions;
using MyNet.Utilities;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.Controls.Assists;

public static class GlobalizationAssist
{
    static GlobalizationAssist() => UpdateOnCultureChangedProperty.Changed.Subscribe(OnUpdateOnCultureChangedCallback);

    #region UpdateOnCultureChanged

    /// <summary>
    /// Provides UpdateOnCultureChanged Property for attached GlobalizationAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> UpdateOnCultureChangedProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UpdateOnCultureChanged", typeof(GlobalizationAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="UpdateOnCultureChangedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UpdateOnCultureChangedProperty"/>.</param>
    public static void SetUpdateOnCultureChanged(StyledElement element, bool value) => element.SetValue(UpdateOnCultureChangedProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UpdateOnCultureChangedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUpdateOnCultureChanged(StyledElement element) => element.GetValue(UpdateOnCultureChangedProperty);

    private static void OnUpdateOnCultureChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not Control element) return;

        if (((bool?)args.NewValue).IsTrue())
        {
            args.Sender.OnLoading<Control>(x =>
                {
                    UpdateControl(x);
                    GlobalizationService.Current.CultureChanged += onCultureChanged;
                },
                _ => GlobalizationService.Current.CultureChanged -= onCultureChanged);
        }
        else
        {
            GlobalizationService.Current.CultureChanged -= onCultureChanged;
        }

        void onCultureChanged(object? sender, EventArgs e) => UpdateControl(element);
    }

    private static void UpdateControl(Control? element)
    {
        if (element is global::Avalonia.Controls.TimePicker tp)
            UpdateTimeFormat(tp);

        if (element is global::Avalonia.Controls.CalendarDatePicker calendarDatePicker)
            UpdateDateFormat(calendarDatePicker);
    }

    private static void UpdateTimeFormat(global::Avalonia.Controls.TimePicker timePicker) => timePicker.ClockIdentifier = GlobalizationService.Current.Culture.DateTimeFormat.ShortTimePattern.Contains("HH", StringComparison.InvariantCulture) ? "24HourClock" : "12HourClock";

    private static void UpdateDateFormat(global::Avalonia.Controls.CalendarDatePicker calendarDatePicker)
    {
        calendarDatePicker.SelectedDateFormat = CalendarDatePickerFormat.Custom;
        calendarDatePicker.CustomDateFormatString = GlobalizationService.Current.Culture.DateTimeFormat.ShortDatePattern;
    }

    #endregion
}
