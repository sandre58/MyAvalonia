// -----------------------------------------------------------------------
// <copyright file="TimePickerBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using MyNet.Utilities.Localization;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public abstract class TimePickerBase : TemplatedControl
{
    protected TimePickerBase() => GlobalizationService.Current.CultureChanged += (_, _) =>
    {
        DisplayFormat = GlobalizationService.Current.Culture.DateTimeFormat.ShortTimePattern;
        PanelFormat = GlobalizationService.Current.Culture.DateTimeFormat.ShortTimePattern.Replace(":", " ", System.StringComparison.OrdinalIgnoreCase);
    };

    public static readonly StyledProperty<string?> DisplayFormatProperty =
        AvaloniaProperty.Register<TimePickerBase, string?>(
            nameof(DisplayFormat), GlobalizationService.Current.Culture.DateTimeFormat.ShortTimePattern);

    public static readonly StyledProperty<string> PanelFormatProperty = AvaloniaProperty.Register<TimePickerBase, string>(
        nameof(PanelFormat), GlobalizationService.Current.Culture.DateTimeFormat.ShortTimePattern.Replace(":", " ", System.StringComparison.OrdinalIgnoreCase));

    public static readonly StyledProperty<bool> NeedConfirmationProperty = AvaloniaProperty.Register<TimePickerBase, bool>(
        nameof(NeedConfirmation));

    public static readonly StyledProperty<bool> IsDropDownOpenProperty = AvaloniaProperty.Register<TimePickerBase, bool>(
        nameof(IsDropDownOpen), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<bool> IsReadonlyProperty = AvaloniaProperty.Register<TimePickerBase, bool>(
        nameof(IsReadonly));

    public static readonly StyledProperty<string?> WatermarkProperty = AvaloniaProperty.Register<TimePickerBase, string?>(
nameof(Watermark));

    public string? Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }

    public bool IsReadonly
    {
        get => GetValue(IsReadonlyProperty);
        set => SetValue(IsReadonlyProperty, value);
    }

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    public string? DisplayFormat
    {
        get => GetValue(DisplayFormatProperty);
        set => SetValue(DisplayFormatProperty, value);
    }

    public string PanelFormat
    {
        get => GetValue(PanelFormatProperty);
        set => SetValue(PanelFormatProperty, value);
    }

    public bool NeedConfirmation
    {
        get => GetValue(NeedConfirmationProperty);
        set => SetValue(NeedConfirmationProperty, value);
    }
}
