// -----------------------------------------------------------------------
// <copyright file="TimeView.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Automation;
using Avalonia.Automation.Peers;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Collections;
using MyNet.Primitives;
using MyNet.Primitives.Temporal;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class TimeView : TimeSelectorBase
{
    private const string PartPeriod = "PART_Period";

    private string _digitBuffer = string.Empty;

    static TimeView() =>
        AutomationProperties.ControlTypeOverrideProperty.OverrideDefaultValue<TimeView>(AutomationControlType.Spinner);

    #region NumberFormat

    /// <summary>
    /// Provides NumberFormat Property.
    /// </summary>
    public static readonly StyledProperty<string> NumberFormatProperty = AvaloniaProperty.Register<TimeView, string>(nameof(NumberFormat), "00");

    /// <summary>
    /// Gets or sets the NumberFormat property.
    /// </summary>
    public string NumberFormat
    {
        get => GetValue(NumberFormatProperty);
        set => SetValue(NumberFormatProperty, value);
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (Components.TryGetValue(TimeComponent.Period, out var existingPeriod) && existingPeriod is { } period)
            RemoveComponentHandlers(period);

        base.OnApplyTemplate(e);

        var periodComponent = e.NameScope.Find(PartPeriod) as IComponentTimeSelector;
        Components[TimeComponent.Period] = periodComponent;

        if (periodComponent is { })
            AddComponentHandlers(periodComponent);

        UpdatePeriodAvailability();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == TimeFormatProperty)
            UpdatePeriodAvailability();

        base.OnPropertyChanged(change);
    }

    protected override void OnActiveComponentChanged(TimeComponent component) => _digitBuffer = string.Empty;

    // Never grab keyboard focus implicitly when the active component changes: focus is moved
    // explicitly during digit entry, and otherwise should follow Tab/click only. This prevents
    // focus from bouncing back to a header field when the clock advances the active component.
    protected override bool ShouldFocusActiveComponent(TimeComponent component) => false;

    protected override bool TryProcessDigitKey(KeyEventArgs e)
    {
        if (e.KeyModifiers != KeyModifiers.None)
            return false;

        if (SelectedComponent is not (TimeComponent.Hour or TimeComponent.Minute or TimeComponent.Second))
            return false;

        if (CurrentComponent is not { } component)
            return false;

        if (e.Key == Key.Back)
        {
            if (_digitBuffer.Length == 0)
                return false;

            _digitBuffer = _digitBuffer[..^1];
            if (_digitBuffer.Length > 0 && int.TryParse(_digitBuffer, NumberStyles.None, CultureInfo.InvariantCulture, out var partial))
            {
                using (SuppressComponentValueChanged())
                    component.Value = partial;
            }

            return true;
        }

        if (KeyToDigit(e.Key) is not { } digit)
            return false;

        _digitBuffer += digit.ToString(CultureInfo.InvariantCulture);
        if (!int.TryParse(_digitBuffer, NumberStyles.None, CultureInfo.InvariantCulture, out var parsed))
        {
            _digitBuffer = digit.ToString(CultureInfo.InvariantCulture);
            parsed = digit;
        }

        var max = component.Maximum;
        var shouldAdvance = _digitBuffer.Length >= 2 || parsed > max / 10 || parsed * 10 > max;

        if (shouldAdvance)
        {
            component.Value = Math.Clamp(parsed, component.Minimum, max);
            _digitBuffer = string.Empty;
            if (!MoveToNextComponent())
                RaiseInputCompleted(TimeInputCompletionMode.FieldAdvance);
            FocusSelectedComponent();
        }
        else
        {
            using (SuppressComponentValueChanged())
                component.Value = parsed;
        }

        return true;
    }

    protected override void ShowComponent(IComponentTimeSelector component)
    {
        Components.Values.OfType<NumericUpDownTimeComponent>().ForEach(x => x.IsActive = false);
        Components.Values.OfType<PeriodTimeComponent>().ForEach(x => x.IsActive = false);

        component.IfIs<NumericUpDownTimeComponent>(x => x.IsActive = true);
        component.IfIs<PeriodTimeComponent>(x => x.IsActive = true);
    }

    private void UpdatePeriodAvailability()
    {
        if (!Components.TryGetValue(TimeComponent.Period, out var component) || component is not PeriodTimeComponent period)
            return;

        var isTwelveHour = TimeFormat == TimeFormat.TwelveHour;
        period.IsVisible = isTwelveHour;
        period.IsEnabled = isTwelveHour;

        if (!isTwelveHour && SelectedComponent == TimeComponent.Period)
            SetCurrentValue(SelectedComponentProperty, TimeComponent.Hour);
    }

    private static int? KeyToDigit(Key key) => key switch
    {
        Key.D0 or Key.NumPad0 => 0,
        Key.D1 or Key.NumPad1 => 1,
        Key.D2 or Key.NumPad2 => 2,
        Key.D3 or Key.NumPad3 => 3,
        Key.D4 or Key.NumPad4 => 4,
        Key.D5 or Key.NumPad5 => 5,
        Key.D6 or Key.NumPad6 => 6,
        Key.D7 or Key.NumPad7 => 7,
        Key.D8 or Key.NumPad8 => 8,
        Key.D9 or Key.NumPad9 => 9,
        _ => null
    };
}
