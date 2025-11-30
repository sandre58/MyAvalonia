// -----------------------------------------------------------------------
// <copyright file="ControlExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Utilities;

namespace MyNet.Avalonia.Controls.Extensions;

public static class ControlExtensions
{
    private const BindingFlags PropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
    private static readonly Dictionary<Control, Popup?> PopupCache = [];

    public static Control? GetFirstFocusableControl(this Control ctrl)
        => ctrl.GetVisualDescendants().OfType<Control>().FirstOrDefault(x => x.Focusable && x.IsEffectivelyEnabled && x.IsVisible);

    public static Popup? GetPopup(this Control ctrl)
    {
        if (PopupCache.TryGetValue(ctrl, out var cachedPopup))
            return cachedPopup;

        var popup = ctrl.GetVisualDescendants().OfType<Popup>().FirstOrDefault();

        if (popup is not null || ctrl.IsLoaded)
            PopupCache[ctrl] = popup;

        return popup;
    }

    public static bool IsPopupOpen(this Control ctrl) => TryGetBooleanPropertyValue(ctrl, "IsDropDownOpen", out var isDropDownOpen)
            ? isDropDownOpen
            : TryGetFlyout(ctrl, out var flyout) && TryGetBooleanPropertyValue(flyout, "IsOpen", out var isFlyoutOpen)
            ? isFlyoutOpen
            : TryGetBooleanPropertyValue(ctrl, "IsOpen", out var isOpen)
            ? isOpen
            : ctrl.GetPopup()?.IsOpen ?? false;

    public static void OpenPopup(this Control ctrl)
    {
        if (TrySetBooleanProperty(ctrl, "IsDropDownOpen", true))
        {
        }
        else if (TryGetFlyout(ctrl, out var flyout))
        {
            flyout?.ShowAt(ctrl);
        }
        else if (TryGetProperty<Popup>(ctrl, "Popup", out var popup))
        {
            popup?.IsOpen = true;
        }
        else
        {
            ctrl.GetPopup()?.Open();
        }
    }

    public static void ClosePopup(this Control ctrl)
    {
        if (TrySetBooleanProperty(ctrl, "IsDropDownOpen", false))
        {
        }
        else if (TryGetFlyout(ctrl, out var flyout))
        {
            flyout?.Hide();
        }
        else if (TryGetProperty<Popup>(ctrl, "Popup", out var popup))
        {
            popup?.IsOpen = false;
        }
        else
        {
            ctrl.GetPopup()?.Close();
        }
    }

    public static bool Increment(this TemplatedControl tc, int value) => tc switch
    {
        DatePicker datePicker => datePicker.IncrementDay(value),
        global::Avalonia.Controls.TimePicker timePicker => timePicker.IncrementMinute(value),
        NumericUpDown numericUpDown => numericUpDown.IncrementNumericUpDown(value),
        ComboBox comboBox => comboBox.IncrementComboBox(value),
        IIncrementableControl incrementableControl => incrementableControl.Increment(value),
        _ => false
    };

    public static bool IncrementLarge(this TemplatedControl tc, int value) => tc switch
    {
        DatePicker datePicker => datePicker.IncrementMonth(value),
        global::Avalonia.Controls.TimePicker timePicker => timePicker.IncrementHour(value),
        NumericUpDown numericUpDown => numericUpDown.IncrementLargeNumericUpDown(value),
        ComboBox comboBox => comboBox.IncrementLargeComboBox(value),
        IIncrementableControl incrementableControl => incrementableControl.IncrementLarge(value),
        _ => false
    };

    private static bool IncrementLargeComboBox(this ComboBox comboBox, int value)
        => IncrementComboBoxCore(comboBox, value * 5);

    private static bool IncrementComboBox(this ComboBox comboBox, int value)
        => IncrementComboBoxCore(comboBox, value);

    private static bool IncrementComboBoxCore(this ComboBox comboBox, int value)
    {
        if (comboBox.SelectedIndex <= -1)
            return false;

        var newIndex = comboBox.SelectedIndex + value;
        var itemCount = comboBox.Items.Count;
        comboBox.SelectedIndex = newIndex switch
        {
            -1 => itemCount - 1,
            _ when newIndex >= itemCount => 0,
            _ => newIndex
        };
        return true;
    }

    private static bool IncrementNumericUpDown(this NumericUpDown numericUpDown, int value)
        => IncrementNumericUpDownCore(numericUpDown, value * numericUpDown.Increment);

    private static bool IncrementLargeNumericUpDown(this NumericUpDown numericUpDown, int value)
        => IncrementNumericUpDownCore(numericUpDown, value * numericUpDown.Increment * 10);

    private static bool IncrementNumericUpDownCore(this NumericUpDown numericUpDown, decimal incrementValue)
    {
        if (numericUpDown.Value is not { } currentValue)
            return false;

        var newValue = currentValue + incrementValue;

        if (newValue >= numericUpDown.Minimum && newValue <= numericUpDown.Maximum)
        {
            numericUpDown.Value = newValue;
            return true;
        }

        return false;
    }

    private static bool IncrementMinute(this global::Avalonia.Controls.TimePicker timePicker, int value)
    {
        if (timePicker.SelectedTime is not { } time)
            return false;

        timePicker.SelectedTime = time.Add(value.Minutes());
        return true;
    }

    private static bool IncrementHour(this global::Avalonia.Controls.TimePicker timePicker, int value)
    {
        if (timePicker.SelectedTime is not { } time)
            return false;

        timePicker.SelectedTime = time.Add(value.Hours());
        return true;
    }

    private static bool IncrementDay(this DatePicker datePicker, int value)
    {
        if (datePicker.SelectedDate is not { } date)
            return false;

        datePicker.SelectedDate = date.AddDays(value);
        return true;
    }

    private static bool IncrementMonth(this DatePicker datePicker, int value)
    {
        if (datePicker.SelectedDate is not { } date)
            return false;

        datePicker.SelectedDate = date.AddMonths(value);
        return true;
    }

    private static bool TryGetBooleanPropertyValue(object? obj, string propertyName, out bool value)
    {
        value = false;
        var property = obj?.GetType().GetProperty(propertyName, PropertyFlags);

        if (property?.PropertyType != typeof(bool))
            return false;

        value = (bool)property.GetValue(obj)!;
        return true;
    }

    private static bool TrySetBooleanProperty(object obj, string propertyName, bool value)
    {
        var property = obj.GetType().GetProperty(propertyName, PropertyFlags);

        if (property?.PropertyType != typeof(bool) || !property.CanWrite)
            return false;

        property.SetValue(obj, value);
        return true;
    }

    private static bool TryGetFlyout(Control ctrl, out FlyoutBase? flyout)
    {
        flyout = (FlyoutBase?)ctrl.GetType().GetProperty("Flyout", PropertyFlags)?.GetValue(ctrl);
        return flyout is not null;
    }

    private static bool TryGetProperty<T>(object obj, string propertyName, out T? value)
        where T : class
    {
        value = obj.GetType().GetProperty(propertyName, PropertyFlags)?.GetValue(obj) as T;
        return value is not null;
    }
}
