// -----------------------------------------------------------------------
// <copyright file="ItemsBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using MyNet.Avalonia.Controls;
using MyNet.Globalization.Culture;
using MyNet.Globalization.Facade;
using MyNet.Observable;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class ItemsBehavior
{
    [SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "Used for null value representation in enum lists.")]
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty", Justification = "Used for null value representation in enum lists.")]
    private sealed class NullEnumListItem(string display)
    {
        public Enum? Value { get; }

        public string Display { get; } = display;
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local", Justification = "Used for null value representation in enum lists.")]
    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty", Justification = "Used for null value representation in enum lists.")]
    private sealed class NullSmartEnumListItem(string display)
    {
        public ISmartEnum? Value { get; }

        public string Display { get; } = display;
    }

    private sealed class State
    {
        public EventHandler<CultureChangedEventArgs>? CultureChangedHandler { get; set; }
    }

    private static readonly ConditionalWeakTable<SelectingItemsControl, State> States = [];

    static ItemsBehavior()
    {
        EnumSourceTypeProperty.Changed.Subscribe(EnumSourceTypePropertyChangedCallback);
        ExcludedValuesProperty.Changed.Subscribe(RefreshOnOptionsChangedCallback);
        UseDisplayMemberProperty.Changed.Subscribe(RefreshOnOptionsChangedCallback);
        SortByDisplayProperty.Changed.Subscribe(RefreshOnOptionsChangedCallback);
        IncludeNullValueProperty.Changed.Subscribe(RefreshOnOptionsChangedCallback);
        NullDisplayTextProperty.Changed.Subscribe(RefreshOnOptionsChangedCallback);
        NullDisplayResourceKeyProperty.Changed.Subscribe(RefreshOnOptionsChangedCallback);
        NullDisplayResourceFilenameProperty.Changed.Subscribe(RefreshOnOptionsChangedCallback);
    }

    #region EnumSourceType

    /// <summary>
    /// Provides EnumSourceType Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<Type?> EnumSourceTypeProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, Type?>("EnumSourceType", typeof(ItemsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="EnumSourceTypeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="EnumSourceTypeProperty"/>.</param>
    public static void SetEnumSourceType(StyledElement element, Type? value) => element.SetValue(EnumSourceTypeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="EnumSourceTypeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Type? GetEnumSourceType(StyledElement element) => element.GetValue(EnumSourceTypeProperty);

    private static void EnumSourceTypePropertyChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not SelectingItemsControl sender) return;

        if (args.NewValue is not Type type)
        {
            UnsubscribeCulture(States.GetOrCreateValue(sender));
            return;
        }

        RefreshEnumSource(sender, type);
        UpdateCultureSubscription(sender);
    }

    #endregion

    #region ExcludedValues

    /// <summary>
    /// Provides ExcludedValues Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<ICollection<object>?> ExcludedValuesProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, ICollection<object>?>("ExcludedValues", typeof(ItemsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="ExcludedValuesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ExcludedValuesProperty"/>.</param>
    public static void SetExcludedValues(StyledElement element, ICollection<object>? value) => element.SetValue(ExcludedValuesProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ExcludedValuesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ICollection<object>? GetExcludedValues(StyledElement element) => element.GetValue(ExcludedValuesProperty);

    #endregion

    #region UseDisplayMember

    /// <summary>
    /// Provides UseDisplayMember Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> UseDisplayMemberProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>("UseDisplayMember", typeof(ItemsBehavior), true);

    /// <summary>
    /// Accessor for Attached  <see cref="UseDisplayMemberProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UseDisplayMemberProperty"/>.</param>
    public static void SetUseDisplayMember(StyledElement element, bool value) => element.SetValue(UseDisplayMemberProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UseDisplayMemberProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetUseDisplayMember(StyledElement element) => element.GetValue(UseDisplayMemberProperty);

    #endregion

    #region SortByDisplay

    /// <summary>
    /// Provides SortByDisplay Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> SortByDisplayProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>("SortByDisplay", typeof(ItemsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="SortByDisplayProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="SortByDisplayProperty"/>.</param>
    public static void SetSortByDisplay(StyledElement element, bool value) => element.SetValue(SortByDisplayProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="SortByDisplayProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetSortByDisplay(StyledElement element) => element.GetValue(SortByDisplayProperty);

    #endregion

    #region IncludeNullValue

    /// <summary>
    /// Provides IncludeNullValue Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> IncludeNullValueProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, bool>("IncludeNullValue", typeof(ItemsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="IncludeNullValueProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IncludeNullValueProperty"/>.</param>
    public static void SetIncludeNullValue(StyledElement element, bool value) => element.SetValue(IncludeNullValueProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IncludeNullValueProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIncludeNullValue(StyledElement element) => element.GetValue(IncludeNullValueProperty);

    #endregion

    #region NullDisplayText

    /// <summary>
    /// Provides NullDisplayText Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string?> NullDisplayTextProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, string?>("NullDisplayText", typeof(ItemsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="NullDisplayTextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="NullDisplayTextProperty"/>.</param>
    public static void SetNullDisplayText(StyledElement element, string? value) => element.SetValue(NullDisplayTextProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="NullDisplayTextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string? GetNullDisplayText(StyledElement element) => element.GetValue(NullDisplayTextProperty);

    #endregion

    #region NullDisplayResourceKey

    /// <summary>
    /// Provides NullDisplayResourceKey Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string?> NullDisplayResourceKeyProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, string?>("NullDisplayResourceKey", typeof(ItemsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="NullDisplayResourceKeyProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="NullDisplayResourceKeyProperty"/>.</param>
    public static void SetNullDisplayResourceKey(StyledElement element, string? value) => element.SetValue(NullDisplayResourceKeyProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="NullDisplayResourceKeyProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string? GetNullDisplayResourceKey(StyledElement element) => element.GetValue(NullDisplayResourceKeyProperty);

    #endregion

    #region NullDisplayResourceFilename

    /// <summary>
    /// Provides NullDisplayResourceFilename Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<string?> NullDisplayResourceFilenameProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, string?>("NullDisplayResourceFilename", typeof(ItemsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="NullDisplayResourceFilenameProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="NullDisplayResourceFilenameProperty"/>.</param>
    public static void SetNullDisplayResourceFilename(StyledElement element, string? value) => element.SetValue(NullDisplayResourceFilenameProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="NullDisplayResourceFilenameProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static string? GetNullDisplayResourceFilename(StyledElement element) => element.GetValue(NullDisplayResourceFilenameProperty);

    #endregion

    private static void RefreshOnOptionsChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not SelectingItemsControl sender) return;
        if (GetEnumSourceType(sender) is not { } type) return;

        RefreshEnumSource(sender, type);
        UpdateCultureSubscription(sender);
    }

    private static void RefreshEnumSource(SelectingItemsControl sender, Type type)
    {
        var currentSelectedValue = sender.SelectedValue;
        var excludedValues = GetExcludedValues(sender) ?? [];

        IList items;
        if (type.IsEnum)
        {
            var values = type.GetLocalizedEnums(excludedValues).Cast<object>().ToList();
            if (GetSortByDisplay(sender))
                SortByDisplay(values, static item => ((LocalizedEnum)item).Display);

            if (GetIncludeNullValue(sender))
                values.Insert(0, new NullEnumListItem(ResolveNullDisplay(sender)));

            items = values;
            sender.SelectedValueBinding = CompiledBinding.Create<LocalizedEnum, Enum?>(x => x.Value);
        }
        else if (type.IsAssignableTo(typeof(ISmartEnum)))
        {
            var values = type.GetLocalizedSmartEnums(excludedValues).Cast<object>().ToList();
            if (GetSortByDisplay(sender))
                SortByDisplay(values, static item => ((LocalizedSmartEnum)item).Display);

            if (GetIncludeNullValue(sender))
                values.Insert(0, new NullSmartEnumListItem(ResolveNullDisplay(sender)));

            items = values;
            sender.SelectedValueBinding = CompiledBinding.Create<LocalizedSmartEnum, ISmartEnum?>(x => x.Value);
        }
        else
        {
            return;
        }

        sender.ItemsSource = items;
        sender.SelectedValue = currentSelectedValue;

        if (GetUseDisplayMember(sender))
        {
            sender.ItemTemplate = null;
            sender.DisplayMemberBinding = type.IsEnum
                ? CompiledBinding.Create<LocalizedEnum, string>(x => x.Display)
                : CompiledBinding.Create<LocalizedSmartEnum, string>(x => x.Display);
        }
    }

    private static void SortByDisplay<T>(List<T> items, Func<T, string> displaySelector)
    {
        var comparer = GlobalizationServices.Current.CurrentCulture.CompareInfo;
        items.Sort((left, right) => comparer.Compare(displaySelector(left), displaySelector(right), CompareOptions.IgnoreCase));
    }

    private static string ResolveNullDisplay(SelectingItemsControl sender)
    {
        var resourceKey = GetNullDisplayResourceKey(sender);
        if (!string.IsNullOrEmpty(resourceKey))
        {
            var filename = GetNullDisplayResourceFilename(sender);
            return string.IsNullOrEmpty(filename)
                ? resourceKey.Translate()
                : resourceKey.Translate(filename);
        }

        return GetNullDisplayText(sender) ?? string.Empty;
    }

    private static bool RequiresCultureRefresh(SelectingItemsControl control)
        => GetSortByDisplay(control)
           || (GetIncludeNullValue(control) && !string.IsNullOrEmpty(GetNullDisplayResourceKey(control)));

    private static void UpdateCultureSubscription(SelectingItemsControl control)
    {
        var state = States.GetOrCreateValue(control);

        if (GetEnumSourceType(control) is not null && RequiresCultureRefresh(control))
        {
            control.OnLoading<SelectingItemsControl>(
                c =>
                {
                    SubscribeCulture(c, state);
                    if (GetEnumSourceType(c) is { } type)
                        RefreshEnumSource(c, type);
                },
                _ => UnsubscribeCulture(state));
        }
        else
        {
            UnsubscribeCulture(state);
        }
    }

    private static void SubscribeCulture(SelectingItemsControl control, State state)
    {
        if (state.CultureChangedHandler is not null) return;

        state.CultureChangedHandler = (_, _) =>
        {
            if (GetEnumSourceType(control) is not { } type) return;
            RefreshEnumSource(control, type);
        };

        GlobalizationServices.Current.CultureChanged += state.CultureChangedHandler;
    }

    private static void UnsubscribeCulture(State state)
    {
        if (state.CultureChangedHandler is null) return;

        GlobalizationServices.Current.CultureChanged -= state.CultureChangedHandler;
        state.CultureChangedHandler = null;
    }
}
