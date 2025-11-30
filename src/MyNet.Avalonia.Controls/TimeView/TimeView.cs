// -----------------------------------------------------------------------
// <copyright file="TimeView.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using Avalonia;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Utilities;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class TimeView : TimeSelectorBase
{
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

    protected override void AddComponentHandlers(IComponentTimeSelector component)
    {
        base.AddComponentHandlers(component);
        component.GotFocus += Component_GotFocus;
    }

    protected override void RemoveComponentHandlers(IComponentTimeSelector component)
    {
        base.RemoveComponentHandlers(component);
        component.GotFocus -= Component_GotFocus;
    }

    private void Component_GotFocus(object? sender, global::Avalonia.Input.GotFocusEventArgs e) => SelectedComponent = Components.FirstOrDefault(x => x.Value?.Equals(sender) == true).Key;

    protected override void ShowComponent(IComponentTimeSelector component)
    {
        Components.Values.OfType<NumericUpDownTimeComponent>().ForEach(x => x.IsActive = false);
        component.IfIs<NumericUpDownTimeComponent>(x => x.IsActive = true);
    }
}
