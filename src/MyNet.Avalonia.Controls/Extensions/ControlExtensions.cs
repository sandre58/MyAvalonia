// -----------------------------------------------------------------------
// <copyright file="ControlExtensions.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Utilities;

namespace MyNet.Avalonia.Controls.Extensions;

[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Extensions methods must be in a static class, and extension methods cannot be in a nested class.")]
[SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case", Justification = "Extension methods must be in a static class, and extension methods cannot be in a nested class.")]
public static class ControlExtensions
{
    private const BindingFlags PropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
    private static readonly Dictionary<Control, Popup?> PopupCache = [];

    extension(Control ctrl)
    {
        public Control? GetFirstFocusableControl()
            => ctrl.GetVisualDescendants().OfType<Control>().FirstOrDefault(x => x is { Focusable: true, IsEffectivelyEnabled: true, IsVisible: true });

        public Popup? GetPopup()
        {
            if (PopupCache.TryGetValue(ctrl, out var cachedPopup))
                return cachedPopup;

            var popup = ctrl.GetVisualDescendants().OfType<Popup>().FirstOrDefault();

            if (popup is not null || ctrl.IsLoaded)
                PopupCache[ctrl] = popup;

            return popup;
        }

        public bool IsPopupOpen() => TryGetBooleanPropertyValue(ctrl, "IsDropDownOpen", out var isDropDownOpen)
            ? isDropDownOpen
            : TryGetFlyout(ctrl, out var flyout) && TryGetBooleanPropertyValue(flyout, "IsOpen", out var isFlyoutOpen)
                ? isFlyoutOpen
                : TryGetBooleanPropertyValue(ctrl, "IsOpen", out var isOpen)
                    ? isOpen
                    : ctrl.GetPopup()?.IsOpen ?? false;

        public void OpenPopup()
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

        public void ClosePopup()
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
    }

    extension(TemplatedControl tc)
    {
        public bool Increment(int value) => tc switch
        {
            DatePicker datePicker => datePicker.IncrementDay(value),
            TimePicker timePicker => timePicker.IncrementMinute(value),
            NumericUpDown numericUpDown => numericUpDown.IncrementNumericUpDown(value),
            ComboBox comboBox => comboBox.IncrementComboBox(value),
            IIncrementableControl incrementableControl => incrementableControl.Increment(value),
            _ => false
        };

        public bool IncrementLarge(int value) => tc switch
        {
            DatePicker datePicker => datePicker.IncrementMonth(value),
            TimePicker timePicker => timePicker.IncrementHour(value),
            NumericUpDown numericUpDown => numericUpDown.IncrementLargeNumericUpDown(value),
            ComboBox comboBox => comboBox.IncrementLargeComboBox(value),
            IIncrementableControl incrementableControl => incrementableControl.IncrementLarge(value),
            _ => false
        };
    }

    extension(ComboBox comboBox)
    {
        private bool IncrementLargeComboBox(int value)
            => comboBox.IncrementComboBoxCore(value * 5);

        private bool IncrementComboBox(int value)
            => comboBox.IncrementComboBoxCore(value);

        private bool IncrementComboBoxCore(int value)
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
    }

    extension(NumericUpDown numericUpDown)
    {
        private bool IncrementNumericUpDown(int value)
            => numericUpDown.IncrementNumericUpDownCore(value * numericUpDown.Increment);

        private bool IncrementLargeNumericUpDown(int value)
            => numericUpDown.IncrementNumericUpDownCore(value * numericUpDown.Increment * 10);

        private bool IncrementNumericUpDownCore(decimal incrementValue)
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
    }

    extension(TimePicker timePicker)
    {
        private bool IncrementMinute(int value)
        {
            if (timePicker.SelectedTime is not { } time)
                return false;

            timePicker.SelectedTime = time.Add(value.Minutes());
            return true;
        }

        private bool IncrementHour(int value)
        {
            if (timePicker.SelectedTime is not { } time)
                return false;

            timePicker.SelectedTime = time.Add(value.Hours());
            return true;
        }
    }

    extension(DatePicker datePicker)
    {
        private bool IncrementDay(int value)
        {
            if (datePicker.SelectedDate is not { } date)
                return false;

            datePicker.SelectedDate = date.AddDays(value);
            return true;
        }

        private bool IncrementMonth(int value)
        {
            if (datePicker.SelectedDate is not { } date)
                return false;

            datePicker.SelectedDate = date.AddMonths(value);
            return true;
        }
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
