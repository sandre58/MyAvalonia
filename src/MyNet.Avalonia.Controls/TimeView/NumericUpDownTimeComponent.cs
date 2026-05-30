// -----------------------------------------------------------------------
// <copyright file="NumericUpDownTimeComponent.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder Structure

public sealed class NumericUpDownTimeComponent : NumericUpDown, IComponentTimeSelector
{
    private event EventHandler<ValueChangedEventArgs<int>>? InternalValueChanged;

    int IComponentTimeSelector.Minimum => (int)Minimum;

    int IComponentTimeSelector.Maximum => (int)Maximum;

    int IComponentTimeSelector.StepFrequency => (int)Increment;

    int? IComponentTimeSelector.Value { get => (int?)Value; set => Value = value; }

    event EventHandler<ValueChangedEventArgs<int>>? IComponentTimeSelector.ValueChanged
    {
        add
        {
            InternalValueChanged += value;
            if (InternalValueChanged != null)
                ValueChanged += NumericUpDownValueChanged;
        }

        remove
        {
            InternalValueChanged -= value;
            if (InternalValueChanged == null)
                ValueChanged -= NumericUpDownValueChanged;
        }
    }

    private void NumericUpDownValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        var newValue = (int?)e.NewValue;
        var oldValue = (int?)e.OldValue;
        InternalValueChanged?.Invoke(this, new(ValueChangedEvent, oldValue, newValue));
    }

    #region IsActive

    /// <summary>
    /// Provides IsActive Property.
    /// </summary>
    public static readonly StyledProperty<bool> IsActiveProperty = AvaloniaProperty.Register<NumericUpDownTimeComponent, bool>(nameof(IsActive));

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the IsActive property.
    /// </summary>
    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    #endregion
}
