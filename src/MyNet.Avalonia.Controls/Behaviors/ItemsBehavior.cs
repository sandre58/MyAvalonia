// -----------------------------------------------------------------------
// <copyright file="ItemsBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using MyNet.Observable.Translatables;
using MyNet.Utilities;

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

        var excludedValues = GetExcludedValues(sender) ?? [];

        IEnumerable? values;
        if (type.IsEnum)
        {
            values = Enum.GetValues(type).Cast<Enum>().Where(x => !excludedValues.Contains(x)).Select(x => new EnumTranslatable(x));
        }
        else if (type.IsAssignableTo(typeof(IEnumeration)))
        {
            values = EnumClass.GetAll(type).Cast<IEnumeration>().Where(x => !excludedValues.Contains(x)).Select(x => new EnumClassTranslatable(x));
        }
        else
        {
            return;
        }

        sender.ItemsSource = values;
        sender.SelectedValueBinding = CompiledBinding.Create<EnumTranslatable, Enum?>(x => x.Value);

        if (GetUseDisplayMember(sender))
        {
            sender.ItemTemplate = null;
            sender.DisplayMemberBinding = CompiledBinding.Create<EnumTranslatable, string>(x => x.Display);
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
