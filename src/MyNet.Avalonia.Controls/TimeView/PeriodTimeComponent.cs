// -----------------------------------------------------------------------
// <copyright file="PeriodTimeComponent.cs" company="Stéphane ANDRE">
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
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

[TemplatePart(PartAmButton, typeof(RadioButton))]
[TemplatePart(PartPmButton, typeof(RadioButton))]
public sealed class PeriodTimeComponent : TemplatedControl, IComponentTimeSelector
{
    public const string PartAmButton = "PART_AmButton";
    public const string PartPmButton = "PART_PmButton";

    private RadioButton? _amButton;
    private RadioButton? _pmButton;

    public static readonly RoutedEvent<ValueChangedEventArgs<int>> ValueChangedEvent =
        RoutedEvent.Register<PeriodTimeComponent, ValueChangedEventArgs<int>>("ValueChanged", RoutingStrategies.Bubble);

    private event EventHandler<ValueChangedEventArgs<int>>? InternalValueChanged;

    static PeriodTimeComponent()
    {
        FocusableProperty.OverrideDefaultValue<PeriodTimeComponent>(true);
        IsAmProperty.Changed.AddClassHandler<PeriodTimeComponent>((component, e) =>
        {
            var (oldValue, newValue) = e.GetOldAndNewValue<bool>();
            component.InternalValueChanged?.Invoke(component, new(ValueChangedEvent, oldValue ? 0 : 1, newValue ? 0 : 1));
        });
    }

    int IComponentTimeSelector.Minimum => 0;

    int IComponentTimeSelector.Maximum => 1;

    int IComponentTimeSelector.StepFrequency => 1;

    int? IComponentTimeSelector.Value
    {
        get => IsAm ? 0 : 1;
        set
        {
            if (value.HasValue)
                IsAm = value == 0;
        }
    }

    event EventHandler<ValueChangedEventArgs<int>>? IComponentTimeSelector.ValueChanged
    {
        add => InternalValueChanged += value;
        remove => InternalValueChanged -= value;
    }

    #region IsAm

    public static readonly StyledProperty<bool> IsAmProperty = AvaloniaProperty.Register<PeriodTimeComponent, bool>(nameof(IsAm), true);

    public bool IsAm
    {
        get => GetValue(IsAmProperty);
        set => SetValue(IsAmProperty, value);
    }

    #endregion

    #region IsActive

    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<PeriodTimeComponent, bool>(nameof(IsActive));

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _amButton = e.NameScope.Find<RadioButton>(PartAmButton);
        _pmButton = e.NameScope.Find<RadioButton>(PartPmButton);
    }

    protected override void OnGotFocus(FocusChangedEventArgs e)
    {
        base.OnGotFocus(e);

        if (ReferenceEquals(e.Source, this))
            _ = FocusCurrentPeriod(NavigationMethod.Directional);
    }

    private bool FocusCurrentPeriod(NavigationMethod method) => IsAm ? _amButton?.Focus(method) == true : _pmButton?.Focus(method) == true || _amButton?.Focus(method) == true;
}
