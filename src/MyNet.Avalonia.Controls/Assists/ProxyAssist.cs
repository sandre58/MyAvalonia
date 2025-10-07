// -----------------------------------------------------------------------
// <copyright file="ProxyAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using MyNet.Avalonia.Controls.Proxy;
using MyNet.Utilities;

namespace MyNet.Avalonia.Controls.Assists;

public static class ProxyAssist
{
    private sealed class ProxyBuilder(Func<Control?, bool> canBuild, Func<Control, IControlProxy> build)
    {
        private readonly Func<Control?, bool> _canBuild = canBuild ?? throw new ArgumentNullException(nameof(canBuild));
        private readonly Func<Control, IControlProxy> _build = build ?? throw new ArgumentNullException(nameof(build));

        public bool CanBuild(Control? control) => _canBuild(control);

        public IControlProxy Build(Control control) => _build(control);
    }

    private static readonly List<ProxyBuilder> Builders = [];

    static ProxyAssist()
    {
        _ = EnableProperty.Changed.Subscribe(EnableChangedCallback);

        // Default builders
        Builders.Add(new ProxyBuilder(c => c is TextBox, c => new TextBoxProxy((TextBox)c)));
        Builders.Add(new ProxyBuilder(c => c is ComboBox, c => new ComboBoxProxy((ComboBox)c)));
        Builders.Add(new ProxyBuilder(c => c is AutoCompleteBox, c => new AutoCompleteBoxProxy((AutoCompleteBox)c)));
        Builders.Add(new ProxyBuilder(c => c is NumericUpDown, c => new NumericUpDownProxy((NumericUpDown)c)));
        Builders.Add(new ProxyBuilder(c => c is global::Avalonia.Controls.CalendarDatePicker, c => new CalendarDatePickerDefaultProxy((global::Avalonia.Controls.CalendarDatePicker)c)));
        Builders.Add(new ProxyBuilder(c => c is CalendarDatePicker, c => new CalendarDatePickerProxy((CalendarDatePicker)c)));
        Builders.Add(new ProxyBuilder(c => c is DatePicker, c => new DatePickerProxy((DatePicker)c)));
        Builders.Add(new ProxyBuilder(c => c is global::Avalonia.Controls.TimePicker, c => new TimePickerDefaultProxy((global::Avalonia.Controls.TimePicker)c)));
        Builders.Add(new ProxyBuilder(c => c is TimePicker, c => new TimePickerProxy((TimePicker)c)));
        Builders.Add(new ProxyBuilder(c => c is CodeBlock, c => new CodeBlockProxy((CodeBlock)c)));
        Builders.Add(new ProxyBuilder(c => c is ColorPicker, c => new ColorPickerProxy((ColorPicker)c)));
        Builders.Add(new ProxyBuilder(c => c is DateTimePicker, c => new DateTimePickerProxy((DateTimePicker)c)));
        Builders.Add(new ProxyBuilder(c => c is DateRangePicker, c => new DateRangePickerProxy((DateRangePicker)c)));
        Builders.Add(new ProxyBuilder(c => c is TimeRangePicker, c => new TimeRangePickerProxy((TimeRangePicker)c)));
        Builders.Add(new ProxyBuilder(c => c is MultiComboBox, c => new MultiComboBoxProxy((MultiComboBox)c)));
        Builders.Add(new ProxyBuilder(c => c is TagBox, c => new TagBoxProxy((TagBox)c)));
    }

    public static void RegisterBuilder(Func<Control?, bool> canBuild, Func<Control, IControlProxy> build) => Builders.Add(new ProxyBuilder(canBuild, build));

    private static IControlProxy? GetOrCreateProxy(Control? control)
    {
        if (control is null) return null;
        var builder = Builders.LastOrDefault(v => v.CanBuild(control));
        return builder?.Build(control);
    }

    #region Proxy

    /// <summary>
    /// Provides Proxy Property for attached ProxyAssist element.
    /// </summary>
    public static readonly AttachedProperty<IControlProxy?> ProxyProperty = AvaloniaProperty.RegisterAttached<StyledElement, IControlProxy?>("Proxy", typeof(ProxyAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ProxyProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ProxyProperty"/>.</param>
    private static void SetProxy(StyledElement element, IControlProxy? value) => element.SetValue(ProxyProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ProxyProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IControlProxy? GetProxy(StyledElement element) => element.GetValue(ProxyProperty);

    #endregion

    #region Enable

    /// <summary>
    /// Provides Enable Property for attached ProxyAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> EnableProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("Enable", typeof(ProxyAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="EnableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="EnableProperty"/>.</param>
    public static void SetEnable(StyledElement element, bool value) => element.SetValue(EnableProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="EnableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetEnable(StyledElement element) => element.GetValue(EnableProperty);

    private static void EnableChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not Control control) return;

        var proxy = GetOrCreateProxy(control);
        if (proxy is null) return;

        if (((bool?)args.NewValue).IsTrue())
        {
            proxy.IsActiveChanged += isActiveChanged;
            proxy.IsEmptyChanged += isEmptyChanged;
            proxy.IsFocusedChanged += isFocusedChanged;

            SetProxy(control, proxy);
            SetIsFocused(control, proxy.IsEmpty());
            SetIsEmpty(control, proxy.IsEmpty());
            SetIsActive(control, proxy.IsEmpty());
        }
        else
        {
            proxy.IsActiveChanged -= isActiveChanged;
            proxy.IsEmptyChanged -= isEmptyChanged;
            proxy.IsFocusedChanged -= isFocusedChanged;
        }

        void isFocusedChanged(object? sender, EventArgs e) => SetIsFocused(control, proxy.IsEmpty());

        void isEmptyChanged(object? sender, EventArgs e) => SetIsEmpty(control, proxy.IsEmpty());

        void isActiveChanged(object? sender, EventArgs e) => SetIsActive(control, proxy.IsEmpty());
    }

    #endregion

    #region IsActive

    /// <summary>
    /// Provides IsActive Property for attached ControlAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> IsActiveProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("IsActive", typeof(ProxyAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IsActiveProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IsActiveProperty"/>.</param>
    public static void SetIsActive(StyledElement element, bool value) => element.SetValue(IsActiveProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IsActiveProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIsActive(StyledElement element) => element.GetValue(IsActiveProperty);

    #endregion

    #region IsEmpty

    /// <summary>
    /// Provides IsEmpty Property for attached ControlAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> IsEmptyProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("IsEmpty", typeof(ProxyAssist), true);

    /// <summary>
    /// Accessor for Attached  <see cref="IsEmptyProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IsEmptyProperty"/>.</param>
    public static void SetIsEmpty(StyledElement element, bool value) => element.SetValue(IsEmptyProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IsEmptyProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIsEmpty(StyledElement element) => element.GetValue(IsEmptyProperty);

    #endregion

    #region IsFocused

    /// <summary>
    /// Provides IsFocused Property for attached ControlAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> IsFocusedProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("IsFocused", typeof(ProxyAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IsFocusedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IsFocusedProperty"/>.</param>
    public static void SetIsFocused(StyledElement element, bool value) => element.SetValue(IsFocusedProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IsFocusedProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIsFocused(StyledElement element) => element.GetValue(IsFocusedProperty);

    #endregion
}
