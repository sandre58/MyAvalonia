// -----------------------------------------------------------------------
// <copyright file="ListBoxBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class ListBoxBehavior
{
    private sealed class State
    {
        public bool IsSynchronizing { get; set; }

        public bool IsRouting { get; set; }

        public INotifyCollectionChanged? ObservedCollection { get; set; }

        public NotifyCollectionChangedEventHandler? CollectionChangedHandler { get; set; }
    }

    private static readonly ConditionalWeakTable<ListBox, State> States = [];

    static ListBoxBehavior()
    {
        SelectedValuesProperty.Changed.Subscribe(SelectedValuesChangedCallback);
        SelectedValueOrValuesProperty.Changed.Subscribe(SelectedValueOrValuesChangedCallback);
        ListBox.SelectionModeProperty.Changed.Subscribe(SelectionModeChangedCallback);
        SelectingItemsControl.SelectedValueProperty.Changed.Subscribe(SelectedValueChangedCallback);
    }

    #region SelectedValues

    /// <summary>
    /// Provides SelectedValues Property for attached ListBoxBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IList?> SelectedValuesProperty = AvaloniaProperty.RegisterAttached<StyledElement, IList?>("SelectedValues", typeof(ListBoxBehavior), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Accessor for Attached  <see cref="SelectedValuesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="SelectedValuesProperty"/>.</param>
    public static void SetSelectedValues(StyledElement element, IList? value) => element.SetValue(SelectedValuesProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="SelectedValuesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IList? GetSelectedValues(StyledElement element) => element.GetValue(SelectedValuesProperty);

    private static void SelectedValuesChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not ListBox listBox)
            return;

        var state = States.GetValue(listBox, CreateState);

        if (state.ObservedCollection is not null && state.CollectionChangedHandler is not null)
            state.ObservedCollection.CollectionChanged -= state.CollectionChangedHandler;

        state.ObservedCollection = args.NewValue as INotifyCollectionChanged;

        if (state.ObservedCollection is not null)
        {
            state.CollectionChangedHandler ??= (_, _) => SynchronizeSelectionFromValues(listBox, state);
            state.ObservedCollection.CollectionChanged += state.CollectionChangedHandler;
        }

        SynchronizeSelectionFromValues(listBox, state);

        if (listBox.IsSet(SelectedValueOrValuesProperty) && IsMultipleSelection(listBox))
            SynchronizeSelectedValueOrValuesFromControl(listBox, state);
    }

    #endregion

    #region SelectedValueOrValues

    /// <summary>
    /// Provides SelectedValueOrValues Property for attached ListBoxBehavior element.
    /// </summary>
    public static readonly AttachedProperty<object?> SelectedValueOrValuesProperty = AvaloniaProperty.RegisterAttached<StyledElement, object?>("SelectedValueOrValues", typeof(ListBoxBehavior), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Accessor for Attached  <see cref="SelectedValueOrValuesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="SelectedValueOrValuesProperty"/>.</param>
    public static void SetSelectedValueOrValues(StyledElement element, object? value) => element.SetValue(SelectedValueOrValuesProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="SelectedValueOrValuesProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object? GetSelectedValueOrValues(StyledElement element) => element.GetValue(SelectedValueOrValuesProperty);

    private static void SelectedValueOrValuesChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not ListBox listBox)
            return;

        var state = States.GetValue(listBox, CreateState);
        if (state.IsRouting)
            return;

        ApplySelectedValueOrValuesToControl(listBox, state, args.NewValue);
    }

    #endregion

    private static State CreateState(ListBox listBox)
    {
        var state = new State();
        listBox.SelectionChanged += (_, _) => SynchronizeValuesFromSelection(listBox, state);
        return state;
    }

    private static void SelectionModeChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not ListBox listBox || !listBox.IsSet(SelectedValueOrValuesProperty))
            return;

        var state = States.GetValue(listBox, CreateState);
        ApplySelectedValueOrValuesToControl(listBox, state, GetSelectedValueOrValues(listBox));
    }

    private static void SelectedValueChangedCallback(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not ListBox listBox || !listBox.IsSet(SelectedValueOrValuesProperty) || IsMultipleSelection(listBox))
            return;

        var state = States.GetValue(listBox, CreateState);
        SynchronizeSelectedValueOrValuesFromControl(listBox, state);
    }

    private static void ApplySelectedValueOrValuesToControl(ListBox listBox, State state, object? value)
    {
        if (state.IsRouting)
            return;

        state.IsRouting = true;
        try
        {
            if (IsMultipleSelection(listBox))
            {
                switch (value)
                {
                    case IList values:
                        SetSelectedValues(listBox, values);
                        break;
                    case null:
                        SetSelectedValues(listBox, null);
                        break;
                    default:
                        SetSelectedValues(listBox, new List<object?> { value });
                        break;
                }
            }
            else
            {
                listBox.SetCurrentValue(SelectingItemsControl.SelectedValueProperty, value);
            }
        }
        finally
        {
            state.IsRouting = false;
        }
    }

    private static void SynchronizeSelectedValueOrValuesFromControl(ListBox listBox, State state)
    {
        if (state.IsRouting || !listBox.IsSet(SelectedValueOrValuesProperty))
            return;

        state.IsRouting = true;
        try
        {
            SetSelectedValueOrValues(listBox, IsMultipleSelection(listBox) ? GetSelectedValues(listBox) : listBox.SelectedValue);
        }
        finally
        {
            state.IsRouting = false;
        }
    }

    private static void SynchronizeSelectionFromValues(ListBox listBox, State state)
    {
        if (state.IsSynchronizing || !IsMultipleSelection(listBox))
            return;

        var selectedValues = GetSelectedValues(listBox);

        state.IsSynchronizing = true;
        try
        {
            listBox.Selection.Clear();

            if (selectedValues is null)
                return;

            var index = 0;
            foreach (var item in EnumerateItems(listBox))
            {
                var itemValue = GetItemSelectedValue(listBox, item);
                if (ContainsValue(selectedValues, itemValue))
                    listBox.Selection.Select(index);

                index++;
            }
        }
        finally
        {
            state.IsSynchronizing = false;
        }
    }

    private static void SynchronizeValuesFromSelection(ListBox listBox, State state)
    {
        if (state.IsSynchronizing || !IsMultipleSelection(listBox) || listBox.SelectedItems is null)
            return;

        var values = listBox.SelectedItems.Cast<object?>().Select(x => GetItemSelectedValue(listBox, x)).ToList();
        var selectedValues = GetSelectedValues(listBox);

        state.IsSynchronizing = true;
        try
        {
            switch (selectedValues)
            {
                case null:
                    SetSelectedValues(listBox, values);
                    break;
                case { IsReadOnly: false, IsFixedSize: false }:
                    {
                        selectedValues.Clear();
                        foreach (var value in values)
                            selectedValues.Add(value);
                        break;
                    }
            }
        }
        finally
        {
            state.IsSynchronizing = false;
        }

        SynchronizeSelectedValueOrValuesFromControl(listBox, state);
    }

    private static IEnumerable<object?> EnumerateItems(ListBox listBox) => listBox.ItemsSource switch
    {
        { } source => source.Cast<object?>(),
        _ => listBox.Items
    };

    private static bool IsMultipleSelection(ListBox listBox) => (listBox.SelectionMode & SelectionMode.Multiple) == SelectionMode.Multiple;

    private static object? GetItemSelectedValue(ListBox listBox, object? item)
    {
        var binding = listBox.SelectedValueBinding;
        if (binding is null)
            return item;

        var valueHost = new ContentControl
        {
            DataContext = item
        };

        using var bindingExpression = valueHost.Bind(ContentControl.ContentProperty, binding);
        return valueHost.Content;
    }

    private static bool ContainsValue(IList values, object? value) => values.Cast<object?>().Contains(value);
}
