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
using Avalonia.Automation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using MyNet.Primitives;
using MyNet.Primitives.Temporal;
using MyNet.Utilities.Suspending;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace MyNet.Avalonia.Controls.Primitives;
#pragma warning restore IDE0130, IDE0130 // Namespace does not match folder structure

public abstract class TimeSelectorBase : TemplatedControl, IValueSelector<TimeSpan?>
{
    private const string PartHour = "PART_Hour";
    private const string PartMinute = "PART_Minute";
    private const string PartSecond = "PART_Second";

    private readonly Suspender _componentValueChangedSuspender = new();
    private bool _syncingComponentFromFocus;

    static TimeSelectorBase()
    {
        FocusableProperty.OverrideDefaultValue<TimeSelectorBase>(true);
        SelectedValueProperty.Changed.AddClassHandler<TimeSelectorBase>((o, _) =>
        {
            o.UpdateTimeValues();
            o.UpdateAutomationName();
        });
        TimeFormatProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.UpdateTimeValues());
        HourProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.OnComponentChanged());
        MinuteProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.OnComponentChanged());
        SecondProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.OnComponentChanged());
        IsAmProperty.Changed.AddClassHandler<TimeSelectorBase>((x, _) => x.OnComponentChanged());
        SelectedComponentProperty.Changed.AddClassHandler<TimeSelectorBase>((x, args) => x.OnSelectedComponentChanged(args));
    }

    protected TimeSelectorBase() => UpdateAutomationName();

    private void UpdateAutomationName() =>
        AutomationProperties.SetName(this, SelectedValue?.ToString() ?? string.Empty);

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

    protected virtual void AddComponentHandlers(IComponentTimeSelector component)
    {
        if (component is not InputElement input)
            return;

        input.AddHandler(KeyDownEvent, OnComponentKeyDown, RoutingStrategies.Tunnel);
        input.AddHandler(GotFocusEvent, OnComponentGotFocus, RoutingStrategies.Bubble);
    }

    protected virtual void RemoveComponentHandlers(IComponentTimeSelector component)
    {
        if (component is not InputElement input)
            return;

        input.RemoveHandler(KeyDownEvent, OnComponentKeyDown);
        input.RemoveHandler(GotFocusEvent, OnComponentGotFocus);
    }

    protected Dictionary<TimeComponent, IComponentTimeSelector?> Components { get; } = [];

    public IComponentTimeSelector? CurrentComponent => Components.GetValueOrDefault(SelectedComponent);

    #region SelectedValue

    public static readonly RoutedEvent<TimeInputCompletedEventArgs> InputCompletedEvent =
        RoutedEvent.Register<TimeSelectorBase, TimeInputCompletedEventArgs>(nameof(InputCompleted), RoutingStrategies.Bubble);

    public event EventHandler<TimeInputCompletedEventArgs>? InputCompleted;

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

            handler(this, new(SelectingItemsControl.SelectionChangedEvent, removedItems, addedItems));
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

        if (ShouldFocusActiveComponent(value))
            FocusSelectedComponent();

        OnActiveComponentChanged(value);
    }

    protected virtual bool ShouldFocusActiveComponent(TimeComponent component) =>
        !_syncingComponentFromFocus && IsKeyboardFocusWithin && !IsFocusWithinComponent(component);

    private bool IsFocusWithinComponent(TimeComponent component)
    {
        if (TopLevel.GetTopLevel(this)?.FocusManager.GetFocusedElement() is not Visual focused)
            return false;

        if (Components.GetValueOrDefault(component) is not Visual componentVisual)
            return false;

        return ReferenceEquals(componentVisual, focused) || componentVisual.IsVisualAncestorOf(focused);
    }

    protected virtual void OnActiveComponentChanged(TimeComponent component) { }

    #endregion

    #region IsAm

    /// <summary>
    /// Provides IsAm Property.
    /// </summary>
    public static readonly StyledProperty<bool> IsAmProperty = AvaloniaProperty.Register<TimeSelectorBase, bool>(nameof(IsAm));

    /// <summary>
    /// Gets or sets a value indicating whether it gets or sets the IsAm property.
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
    /// Gets or sets a value indicating whether it gets or sets is seconds selector is displayed.
    /// </summary>
    public bool ShowSeconds
    {
        get => GetValue(ShowSecondsProperty);
        set => SetValue(ShowSecondsProperty, value);
    }

    #endregion

    #region ShowClock

    /// <summary>
    /// Defines the <see cref="ShowClock"/> property.
    /// </summary>
    public static readonly StyledProperty<bool> ShowClockProperty = AvaloniaProperty.Register<TimeSelectorBase, bool>(nameof(ShowClock), true);

    /// <summary>
    /// Gets or sets a value indicating whether the analog clock selector is displayed.
    /// </summary>
    public bool ShowClock
    {
        get => GetValue(ShowClockProperty);
        set => SetValue(ShowClockProperty, value);
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

        if (TryProcessDigitKey(e) || ProcessKeyboardKey(e))
            e.Handled = true;
    }

    protected bool ProcessKeyboardKey(KeyEventArgs e)
    {
        if (!IsEnabled || e.Handled)
            return false;

        if (ProcessTabKey(e))
            return true;

        if (e.KeyModifiers != KeyModifiers.None)
            return false;

        return e.Key switch
        {
            Key.Enter when IsOnLastSelectableComponent() => RaiseInputCompleted(TimeInputCompletionMode.EnterKey) || true,
            Key.Space or Key.Enter or Key.Right => MoveToNextComponent(wrap: true) || true,
            Key.Left => MoveToPreviousComponent(wrap: true) || true,
            Key.Up => Previous() || true,
            Key.Down => Next() || true,
            Key.PageDown => NextLarge() || true,
            Key.PageUp => PreviousLarge() || true,
            Key.Home => First() || true,
            Key.End => Last() || true,
            _ => false
        };
    }

    private void OnComponentKeyDown(object? sender, KeyEventArgs e)
    {
        if (ProcessTabKey(e) || TryProcessDigitKey(e) || ProcessKeyboardKey(e))
            e.Handled = true;
    }

    private bool ProcessTabKey(KeyEventArgs e)
    {
        if (e.Key != Key.Tab)
            return false;

        if ((e.KeyModifiers & ~KeyModifiers.Shift) != KeyModifiers.None)
            return false;

        var shift = e.KeyModifiers.HasFlag(KeyModifiers.Shift);

        if (shift)
        {
            if (!MoveToPreviousComponent(wrap: false))
                return false;

            FocusSelectedComponent(NavigationMethod.Tab);
            return true;
        }

        if (!MoveToNextComponent(wrap: false))
            return false;

        FocusSelectedComponent(NavigationMethod.Tab);
        return true;
    }

    protected virtual bool TryProcessDigitKey(KeyEventArgs e) => false;

    private void OnComponentGotFocus(object? sender, RoutedEventArgs e)
    {
        if (ResolveComponentKey(sender ?? e.Source) is not { } key)
            return;

        _syncingComponentFromFocus = true;
        try
        {
            SetCurrentValue(SelectedComponentProperty, key);
        }
        finally
        {
            _syncingComponentFromFocus = false;
        }
    }

    #endregion

    #region Focus

    public void FocusComponent(TimeComponent component)
    {
        EnsureComponentActive(component);
        FocusSelectedComponent(NavigationMethod.Tab);
    }

    public void FocusActiveComponent()
    {
        var component = SelectedComponent;
        if (!GetSelectableComponents().Contains(component) && GetSelectableComponents().FirstOrDefault() is { } first)
            component = first;

        FocusComponent(component);
    }

    private void EnsureComponentActive(TimeComponent component)
    {
        if (Components.GetValueOrDefault(component) is not { } activeComponent)
            return;

        if (SelectedComponent != component)
            SetCurrentValue(SelectedComponentProperty, component);
        else
            ShowComponent(activeComponent);
    }

    protected void FocusSelectedComponent(NavigationMethod method = NavigationMethod.Directional)
    {
        if (Components.GetValueOrDefault(SelectedComponent) is not IInputElement { Focusable: true, IsEffectivelyEnabled: true } input)
            return;

        if (TryFocusInputEditor(input, method))
            return;

        input.Focus(method);
    }

    private static bool TryFocusInputEditor(IInputElement input, NavigationMethod method)
    {
        if (input is not Control control)
            return false;

        var editor = control.GetVisualDescendants()
            .OfType<TextBox>()
            .FirstOrDefault(x => x is { IsEffectivelyEnabled: true, IsVisible: true });

        return editor?.Focus(method) == true;
    }

    protected TimeComponent? ResolveComponentKey(object? source)
    {
        if (source is not Visual visual)
            return null;

        foreach (var (key, component) in Components)
        {
            if (component is not Visual componentVisual)
                continue;

            if (ReferenceEquals(componentVisual, visual) || componentVisual.IsVisualAncestorOf(visual))
                return key;
        }

        return null;
    }

    protected IEnumerable<TimeComponent> GetSelectableComponents() =>
        Components
            .Where(x => x.Value is Control { IsEnabled: true })
            .OrderBy(x => x.Key)
            .Select(x => x.Key);

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

    protected IDisposable SuppressComponentValueChanged() => _componentValueChangedSuspender.Suspend();

    private void OnComponentChanged()
    {
        if (_componentValueChangedSuspender.IsSuspended) return;

        SetCurrentValue(SelectedValueProperty, new TimeContext(Hour, Minute, Second, IsAm).ToTimeSpan(TimeFormat));
    }

    public bool Previous()
    {
        if (CurrentComponent is not { } component)
            return false;

        Previous(component);
        return true;
    }

    public bool Next()
    {
        if (CurrentComponent is not { } component)
            return false;

        Next(component);
        return true;
    }

    public bool PreviousLarge()
    {
        if (CurrentComponent is not { } component)
            return false;

        PreviousLarge(component, GetLargeStepFrequency(SelectedComponent));
        return true;
    }

    public bool NextLarge()
    {
        if (CurrentComponent is not { } component)
            return false;

        NextLarge(component, GetLargeStepFrequency(SelectedComponent));
        return true;
    }

    protected virtual int GetLargeStepFrequency(TimeComponent component) => component switch
    {
        TimeComponent.Hour => 3,
        TimeComponent.Minute => 10,
        TimeComponent.Second => 15,
        _ => 1
    };

    public bool Last()
    {
        if (CurrentComponent is not { } component)
            return false;

        Last(component);
        return true;
    }

    public bool First()
    {
        if (CurrentComponent is not { } component)
            return false;

        First(component);
        return true;
    }

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

    private static void PreviousLarge(IComponentTimeSelector component, int step)
    {
        if (component.Value is not { } current)
        {
            component.Value = component.Maximum;
            return;
        }

        var newValue = current - step;
        while (newValue < component.Minimum)
            newValue += component.Maximum - component.Minimum + 1;

        component.Value = newValue;
    }

    private static void NextLarge(IComponentTimeSelector component, int step)
    {
        if (component.Value is not { } current)
        {
            component.Value = component.Minimum;
            return;
        }

        var newValue = current + step;
        while (newValue > component.Maximum)
            newValue -= component.Maximum - component.Minimum + 1;

        component.Value = newValue;
    }

    private static void First(IComponentTimeSelector component) => component.Value = component.Minimum;

    private static void Last(IComponentTimeSelector component) => component.Value = component.Maximum;

    public bool MoveToPreviousComponent(bool wrap = false)
    {
        var selectable = GetSelectableComponents().ToList();
        if (selectable.Count == 0)
            return false;

        var index = selectable.IndexOf(SelectedComponent);

        if (index > 0)
        {
            SetCurrentValue(SelectedComponentProperty, selectable[index - 1]);
            return true;
        }

        if (!wrap)
            return false;

        SetCurrentValue(SelectedComponentProperty, selectable[^1]);
        return true;
    }

    public bool MoveToNextComponent(bool wrap = false)
    {
        var selectable = GetSelectableComponents().ToList();
        if (selectable.Count == 0)
            return false;

        var index = selectable.IndexOf(SelectedComponent);

        if (index >= 0 && index < selectable.Count - 1)
        {
            SetCurrentValue(SelectedComponentProperty, selectable[index + 1]);
            return true;
        }

        if (!wrap)
            return false;

        SetCurrentValue(SelectedComponentProperty, selectable[0]);
        return true;
    }

    protected bool IsOnLastSelectableComponent()
    {
        var selectable = GetSelectableComponents().ToList();
        return selectable.Count > 0 && selectable[^1] == SelectedComponent;
    }

    protected bool RaiseInputCompleted(TimeInputCompletionMode mode = TimeInputCompletionMode.EnterKey)
    {
        var args = new TimeInputCompletedEventArgs(InputCompletedEvent) { Mode = mode };
        RaiseEvent(args);
        return true;
    }

    protected virtual void ShowComponent(IComponentTimeSelector component) { }
}

internal sealed record TimeContext(int? Hours, int? Minutes, int? Seconds, bool IsAm)
{
    public static TimeContext FromTimeSpan(TimeSpan? time, TimeFormat format)
    {
        if (!time.HasValue) return new(null, null, null, true);

        var isAm = ComputeIsAm(time.Value.Hours);
        var hours = format switch
        {
            TimeFormat.TwelveHour => ConvertTo12FormattedHours(time.Value.Hours),
            _ => time.Value.Hours
        };
        return new(hours, time.Value.Minutes, time.Value.Seconds, isAm);
    }

    public TimeSpan? ToTimeSpan(TimeFormat format) => !Hours.HasValue
        ? null
        : format == TimeFormat.TwentyFourHour
            ? new TimeSpan(Hours.Value, Minutes ?? 0, Seconds ?? 0)
            : new TimeSpan(ConvertTo12HourClockTo24(Hours.Value, IsAm), Minutes ?? 0, Seconds ?? 0);

    public static bool ComputeIsAm(int hours) => hours < 12;

    private static int ConvertTo12FormattedHours(int hours) => hours > 12 ? hours - 12 : hours == 0 ? 12 : hours;

    private static int ConvertTo12HourClockTo24(int hours, bool isAm) => isAm
        ? hours == 12 ? 0 : hours
        : hours == 12 ? 12 : hours + 12;
}
