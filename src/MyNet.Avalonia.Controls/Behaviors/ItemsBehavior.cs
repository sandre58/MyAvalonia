// -----------------------------------------------------------------------
// <copyright file="ItemsBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using MyNet.Observable;
using MyNet.Primitives;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class ItemsBehavior
{
    static ItemsBehavior() => EnumSourceTypeProperty.Changed.Subscribe(EnumSourceTypePropertyChangedCallback);

    #region EnumSourceType

    /// <summary>
    /// Provides EnumSourceType Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<Type> EnumSourceTypeProperty = AvaloniaProperty.RegisterAttached<StyledElement, Type>("EnumSourceType", typeof(ItemsBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="EnumSourceTypeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="EnumSourceTypeProperty"/>.</param>
    public static void SetEnumSourceType(StyledElement element, Type value) => element.SetValue(EnumSourceTypeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="EnumSourceTypeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Type GetEnumSourceType(StyledElement element) => element.GetValue(EnumSourceTypeProperty);

    private static void EnumSourceTypePropertyChangedCallback(AvaloniaPropertyChangedEventArgs obj)
    {
        if (obj.Sender is not SelectingItemsControl sender) return;
        if (obj.NewValue is not Type type) return;

        // Keep current selected value so we can re-apply it once item value mapping is ready.
        var currentSelectedValue = sender.SelectedValue;

        var excludedValues = GetExcludedValues(sender) ?? [];

        IEnumerable? values;
        if (type.IsEnum)
        {
            values = LocalizedEnumSource.CreateSystemEnumList(type, excludedValues);
            sender.SelectedValueBinding = CompiledBinding.Create<LocalizedEnum, Enum?>(x => x.Value);
        }
        else if (type.IsAssignableTo(typeof(ISmartEnum)))
        {
            values = LocalizedEnumSource.CreateSmartEnumList(type, excludedValues);
            sender.SelectedValueBinding = CompiledBinding.Create<LocalizedSmartEnum, ISmartEnum?>(x => x.Value);
        }
        else
        {
            return;
        }

        sender.ItemsSource = values;
        sender.SelectedValue = currentSelectedValue;

        if (GetUseDisplayMember(sender))
        {
            sender.ItemTemplate = null;
            sender.DisplayMemberBinding = type.IsEnum
                ? CompiledBinding.Create<LocalizedEnum, string>(x => x.Display)
                : CompiledBinding.Create<LocalizedSmartEnum, string>(x => x.Display);
        }
    }

    #endregion

    #region ExcludedValues

    /// <summary>
    /// Provides ExcludedValues Property for attached ItemsBehavior element.
    /// </summary>
    public static readonly AttachedProperty<ICollection<object>?> ExcludedValuesProperty = AvaloniaProperty.RegisterAttached<StyledElement, ICollection<object>?>("ExcludedValues", typeof(ItemsBehavior));

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
    public static readonly AttachedProperty<bool> UseDisplayMemberProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("UseDisplayMember", typeof(ItemsBehavior), true);

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
}
