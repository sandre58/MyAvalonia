// -----------------------------------------------------------------------
// <copyright file="TimeSelectorBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using MyNet.Utilities;
using MyNet.Utilities.DateTimes;
using MyNet.Utilities.Suspending;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public abstract class TimeSelectorBase : TemplatedControl, IValueSelector<TimeSpan?>
{
    private const string PartHour = "PART_Hour";
    private const string PartMinute = "PART_Minute";
    private const string PartSecond = "PART_Second";

    private readonly Suspender _componentValueChangedSuspender = new();

    static TimeSelectorBase()
    {
        SelectedValueProperty.Changed.AddClassHandler<TimeSelectorBase>((o, _) => o.UpdateTimeValues());
        TimeFormatProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.UpdateTimeValues());
        HourProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.OnComponentChanged(true));
        MinuteProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.OnComponentChanged());
        SecondProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.OnComponentChanged());
        IsAmProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.OnComponentChanged());
        SelectedComponentProperty.Changed.AddClassHandler<TimeSelectorBase>((x, args) => x.OnSelectedComponentChanged(args));
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == SelectedValueProperty)
        {
            var (oldValue, newValue) = change.GetOldAndNewValue<TimeSpan?>();

            OnValueSelected(oldValue, newValue);
        }

        base.OnPropertyChanged(change);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        foreach (var item in Components)
        {
            if (item.Value is { } component)
                RemoveComponentHandlers(component);
        }

        Components[TimeComponent.Hour] = e.NameScope.Find<IComponentTimeSelector>(PartHour);
        Components[TimeComponent.Minute] = e.NameScope.Find<IComponentTimeSelector>(PartMinute);
        Components[TimeComponent.Second] = e.NameScope.Find<IComponentTimeSelector>(PartSecond);

        foreach (var item in Components)
        {
            if (item.Value is { } component)
                AddComponentHandlers(component);
        }

        UpdateTimeValues();
    }

    protected virtual void AddComponentHandlers(IComponentTimeSelector component) { }

    protected virtual void RemoveComponentHandlers(IComponentTimeSelector component) { }

    protected Dictionary<TimeComponent, IComponentTimeSelector?> Components { get; } = [];

    public IComponentTimeSelector? CurrentComponent => Components.GetValueOrDefault(SelectedComponent);

    #region SelectedValue

    public event EventHandler<SelectionChangedEventArgs>? SelectedValueChanged;

    /// <summary>
    /// Defines the <see cref="SelectedValue"/> property.
    /// </summary>
    public static readonly StyledProperty<TimeSpan?> SelectedValueProperty = AvaloniaProperty.Register<TimeSelectorBase, TimeSpan?>(nameof(SelectedValue), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Gets or sets the selected time. Can be null.
    /// </summary>
    public TimeSpan? SelectedValue
    {
        get => GetValue(SelectedValueProperty); set => SetValue(SelectedValueProperty, value);
    }

    private void OnValueSelected(TimeSpan? addedValue, TimeSpan? removedValue)
    {
        var handler = SelectedValueChanged;
        if (handler != null)
        {
            var addedItems = new Collection<TimeSpan?>();
            var removedItems = new Collection<TimeSpan?>();

            if (addedValue is not null)
                addedItems.Add(addedValue);

            if (removedValue is not null)
                removedItems.Add(removedValue);

            handler(this, new SelectionChangedEventArgs(SelectingItemsControl.SelectionChangedEvent, removedItems, addedItems));
        }
    }

    #endregion

    #region Hour

    /// <summary>
    /// Provides Hour Property.
    /// </summary>
    public static readonly StyledProperty<int?> HourProperty = AvaloniaProperty.Register<TimeSelectorBase, int?>(nameof(Hour));

    /// <summary>
    /// Gets or sets the Hour property.
    /// </summary>
    public int? Hour
    {
        get => GetValue(HourProperty);
        set => SetValue(HourProperty, value);
    }

    #endregion

    #region Minute

    /// <summary>
    /// Provides Minute Property.
    /// </summary>
    public static readonly StyledProperty<int?> MinuteProperty = AvaloniaProperty.Register<TimeSelectorBase, int?>(nameof(Minute));

    /// <summary>
    /// Gets or sets the Minute property.
    /// </summary>
    public int? Minute
    {
        get => GetValue(MinuteProperty);
        set => SetValue(MinuteProperty, value);
    }

    #endregion

    #region Second

    /// <summary>
    /// Provides Second Property.
    /// </summary>
    public static readonly StyledProperty<int?> SecondProperty = AvaloniaProperty.Register<TimeSelectorBase, int?>(nameof(Second));

    /// <summary>
    /// Gets or sets the Second property.
    /// </summary>
    public int? Second
    {
        get => GetValue(SecondProperty);
        set => SetValue(SecondProperty, value);
    }

    #endregion

    #region SelectedComponent

    /// <summary>
    /// Provides SelectedComponent Property.
    /// </summary>
    public static readonly StyledProperty<TimeComponent> SelectedComponentProperty = AvaloniaProperty.Register<TimeSelectorBase, TimeComponent>(nameof(SelectedComponent));

    /// <summary>
    /// Gets or sets the SelectedComponent property.
    /// </summary>
    public TimeComponent SelectedComponent
    {
        get => GetValue(SelectedComponentProperty);
        set => SetValue(SelectedComponentProperty, value);
    }

    private void OnSelectedComponentChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var value = e.GetNewValue<TimeComponent>();

        if (Components.GetValueOrDefault(value) is { } componentTimeSelector)
            ShowComponent(componentTimeSelector);
    }

    #endregion

    #region IsAm

    /// <summary>
    /// Provides IsAm Property.
    /// </summary>
    public static readonly StyledProperty<bool> IsAmProperty = AvaloniaProperty.Register<TimeSelectorBase, bool>(nameof(IsAm));

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets the IsAm property.
    /// </summary>
    public bool IsAm
    {
        get => GetValue(IsAmProperty);
        set => SetValue(IsAmProperty, value);
    }

    #endregion

    #region ShowSeconds

    /// <summary>
    /// Defines the <see cref="ShowSeconds"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowSecondsProperty = AvaloniaProperty.Register<TimeSelectorBase, bool>(nameof(ShowSeconds));

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets is seconds selector is displayed.
    /// </summary>
    public bool ShowSeconds
    {
        get => GetValue(ShowSecondsProperty);
        set => SetValue(ShowSecondsProperty, value);
    }

    #endregion

    #region TimeFormat

    /// <summary>
    /// Defines the <see cref="TimeFormat"/> property.
    /// </summary>
    public static readonly StyledProperty<TimeFormat> TimeFormatProperty = AvaloniaProperty.Register<TimeSelectorBase, TimeFormat>(nameof(TimeFormat));

    /// <summary>
    /// Gets or sets the time format.
    /// </summary>
    public TimeFormat TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    #endregion

    #region Keyboard handlers

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case Key.Space:
            case Key.Enter:
                MoveToNextComponent();
                e.Handled = true;
                break;

            case Key.Left:
                MoveToPreviousComponent();
                e.Handled = true;
                break;

            case Key.Up:
                Previous();
                e.Handled = true;
                break;

            case Key.Right:
                MoveToNextComponent();
                e.Handled = true;
                break;

            case Key.Down:
                Next();
                e.Handled = true;
                break;

            case Key.PageDown:
                NextLarge();
                e.Handled = true;
                break;

            case Key.PageUp:
                PreviousLarge();
                e.Handled = true;
                break;

            case Key.Home:
                First();
                e.Handled = true;
                break;

            case Key.End:
                Last();
                e.Handled = true;
                break;
        }
    }

    #endregion

    private void UpdateTimeValues()
    {
        using (_componentValueChangedSuspender.Suspend())
        {
            var timeContext = TimeContext.FromTimeSpan(SelectedValue, TimeFormat);
            SetCurrentValue(HourProperty, timeContext.Hours);
            SetCurrentValue(MinuteProperty, timeContext.Minutes);
            SetCurrentValue(SecondProperty, timeContext.Seconds);
            SetCurrentValue(IsAmProperty, timeContext.IsAm);
        }
    }

    private void OnComponentChanged(bool computeIsAm = false)
    {
        if (_componentValueChangedSuspender.IsSuspended) return;

        var timeContext = new TimeContext(Hour, Minute, Second, computeIsAm && Hour.HasValue ? TimeContext.ComputeIsAm(Hour.Value) : IsAm);
        SetCurrentValue(SelectedValueProperty, timeContext.ToTimeSpan());
    }

    public void Previous() => CurrentComponent?.IfIs<IComponentTimeSelector>(Previous);

    public void Next() => CurrentComponent?.IfIs<IComponentTimeSelector>(Next);

    public void PreviousLarge() => CurrentComponent?.IfIs<IComponentTimeSelector>(PreviousLarge);

    public void NextLarge() => CurrentComponent?.IfIs<IComponentTimeSelector>(NextLarge);

    public void Last() => CurrentComponent?.IfIs<IComponentTimeSelector>(Last);

    public void First() => CurrentComponent?.IfIs<IComponentTimeSelector>(First);

    public bool IsEmpty() => Components.All(c => c.Value is null);

    public void Clear() => SetCurrentValue(SelectedValueProperty, null);

    private static void Previous(IComponentTimeSelector component)
    {
        if (component.Value is not { } current)
        {
            component.Value = component.Maximum;
            return;
        }

        var newValue = current - 1;
        component.Value = newValue < component.Minimum ? component.Maximum : newValue;
    }

    private static void Next(IComponentTimeSelector component)
    {
        if (component.Value is not { } current)
        {
            component.Value = component.Minimum;
            return;
        }

        var newValue = current + 1;
        component.Value = newValue > component.Maximum ? component.Minimum : newValue;
    }

    private static void PreviousLarge(IComponentTimeSelector component)
    {
        if (component.Value is not { } current)
        {
            component.Value = component.Maximum - ((component.Maximum - component.Minimum) % component.StepFrequency);
            return;
        }

        var newValue = current - component.StepFrequency;
        while (newValue < component.Minimum)
            newValue += component.Maximum - component.Minimum + 1;

        var offset = (newValue - component.Minimum) % component.StepFrequency;
        if (offset != 0)
            newValue -= offset;

        component.Value = newValue;
    }

    private static void NextLarge(IComponentTimeSelector component)
    {
        if (component.Value is not { } current)
        {
            component.Value = component.Minimum;
            return;
        }

        var newValue = current + component.StepFrequency;
        while (newValue > component.Maximum)
            newValue -= component.Maximum - component.Minimum + 1;

        var offset = (newValue - component.Minimum) % component.StepFrequency;
        if (offset != 0)
            newValue += component.StepFrequency - offset;

        component.Value = newValue;
    }

    private static void First(IComponentTimeSelector component) => component.Value = component.Minimum;

    private static void Last(IComponentTimeSelector component) => component.Value = component.Maximum;

    public void MoveToPreviousComponent()
    {
        var components = Components.Where(x => x.Key < SelectedComponent && x.Value?.IsEnabled == true).Select(x => x.Key).OrderDescending().ToList();

        if (components.Count > 0)
            SetCurrentValue(SelectedComponentProperty, components[0]);
    }

    public void MoveToNextComponent()
    {
        var components = Components.Where(x => x.Key > SelectedComponent && x.Value?.IsEnabled == true).Select(x => x.Key).Order().ToList();

        if (components.Count > 0)
            SetCurrentValue(SelectedComponentProperty, components[0]);
    }

    protected virtual void ShowComponent(IComponentTimeSelector component) { }
}

internal sealed record TimeContext(int? Hours, int? Minutes, int? Seconds, bool IsAm)
{
    public static TimeContext FromTimeSpan(TimeSpan? time, TimeFormat format)
    {
        if (!time.HasValue) return new TimeContext(null, null, null, true);

        var isAm = ComputeIsAm(time.Value.Hours);
        var hours = format switch
        {
            TimeFormat.TwelveHour => ConvertTo12FormattedHours(time.Value.Hours),
            _ => time.Value.Hours
        };
        return new TimeContext(hours, time.Value.Minutes, time.Value.Seconds, isAm);
    }

    public TimeSpan? ToTimeSpan() => !Hours.HasValue ? null : new TimeSpan(ConvertTo24FormattedHours(Hours.Value, IsAm), Minutes ?? 0, Seconds ?? 0);

    public static bool ComputeIsAm(int hours) => hours < 12;

    private static int ConvertTo12FormattedHours(int hours) => hours > 12 ? hours - 12 : hours == 0 ? 12 : hours;

    private static int ConvertTo24FormattedHours(int hours, bool isAm)
        => isAm && hours < 12 ? hours
            : isAm && hours > 12 ? hours - 12
            : !isAm && hours < 12 ? hours + 12
            : !isAm && hours > 12 ? hours : 0;
}
