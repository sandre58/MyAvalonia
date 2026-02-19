// -----------------------------------------------------------------------
// <copyright file="ExpanderAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Styling;

namespace MyNet.Avalonia.Theme.Assists;

public static class ExpanderAssist
{
    #region ButtonTheme

    /// <summary>
    /// Provides ButtonTheme Property for attached ExpanderBehavior element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme> ButtonThemeProperty = AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme>("ButtonTheme", typeof(ExpanderAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ButtonThemeProperty"/>.</param>
    public static void SetButtonTheme(StyledElement element, ControlTheme value) => element.SetValue(ButtonThemeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlTheme GetButtonTheme(StyledElement element) => element.GetValue(ButtonThemeProperty);

    #endregion

    #region IsExpandable

    /// <summary>
    /// Provides IsExpandable Property for attached Expander element to control whether the expander can be collapsed/expanded.
    /// </summary>
    public static readonly AttachedProperty<bool> IsExpandableProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("IsExpandable", typeof(ExpanderAssist), defaultValue: true);

    /// <summary>
    /// Accessor for Attached  <see cref="IsExpandableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IsExpandableProperty"/>.</param>
    public static void SetIsExpandable(StyledElement element, bool value) => element.SetValue(IsExpandableProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IsExpandableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIsExpandable(StyledElement element) => element.GetValue(IsExpandableProperty);

    #endregion
}
