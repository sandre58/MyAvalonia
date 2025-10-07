// -----------------------------------------------------------------------
// <copyright file="ToggleButtonAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;

namespace MyNet.Avalonia.Controls.Assists;

public static class ToggleButtonAssist
{
    #region IndeterminatePath

    /// <summary>
    /// Provides IndeterminatePath Property for attached ToggleButtonAssist element.
    /// </summary>
    public static readonly AttachedProperty<Geometry> IndeterminatePathProperty = AvaloniaProperty.RegisterAttached<StyledElement, Geometry>("IndeterminatePath", typeof(ToggleButtonAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IndeterminatePathProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IndeterminatePathProperty"/>.</param>
    public static void SetIndeterminatePath(StyledElement element, Geometry value) => element.SetValue(IndeterminatePathProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IndeterminatePathProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Geometry GetIndeterminatePath(StyledElement element) => element.GetValue(IndeterminatePathProperty);

    #endregion

    #region UncheckedPath

    /// <summary>
    /// Provides UncheckedPath Property for attached ToggleButtonAssist element.
    /// </summary>
    public static readonly AttachedProperty<Geometry> UncheckedPathProperty = AvaloniaProperty.RegisterAttached<StyledElement, Geometry>("UncheckedPath", typeof(ToggleButtonAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="UncheckedPathProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="UncheckedPathProperty"/>.</param>
    public static void SetUncheckedPath(StyledElement element, Geometry value) => element.SetValue(UncheckedPathProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="UncheckedPathProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Geometry GetUncheckedPath(StyledElement element) => element.GetValue(UncheckedPathProperty);

    #endregion

    #region CheckedPath

    /// <summary>
    /// Provides CheckedPath Property for attached ToggleButtonAssist element.
    /// </summary>
    public static readonly AttachedProperty<Geometry> CheckedPathProperty = AvaloniaProperty.RegisterAttached<StyledElement, Geometry>("CheckedPath", typeof(ToggleButtonAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="CheckedPathProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="CheckedPathProperty"/>.</param>
    public static void SetCheckedPath(StyledElement element, Geometry value) => element.SetValue(CheckedPathProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="CheckedPathProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static Geometry GetCheckedPath(StyledElement element) => element.GetValue(CheckedPathProperty);

    #endregion
}
