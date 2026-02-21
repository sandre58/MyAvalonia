// -----------------------------------------------------------------------
// <copyright file="PopupAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Palettes;

namespace MyNet.Avalonia.Theme.Assists;

public static class PopupAssist
{
    #region Background

    /// <summary>
    /// Provides Background Property for attached PopupBehavior element.
    /// </summary>
    public static readonly AttachedProperty<IBrush> BackgroundProperty = AvaloniaProperty.RegisterAttached<StyledElement, IBrush>("Background", typeof(PopupAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="BackgroundProperty"/>.</param>
    public static void SetBackground(StyledElement element, IBrush value) => element.SetValue(BackgroundProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="BackgroundProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static IBrush GetBackground(StyledElement element) => element.GetValue(BackgroundProperty);

    #endregion

    #region Role

    /// <summary>
    /// Defines the Role attached property for assigning a semantic color role to a control.
    /// </summary>
    public static readonly AttachedProperty<ThemeRole> RoleProperty = AvaloniaProperty.RegisterAttached<AvaloniaObject, ThemeRole>("Role", typeof(PopupAssist));

    /// <summary>
    /// Gets the theme role for the specified control.
    /// </summary>
    /// <param name="element">The control to query.</param>
    /// <returns>The assigned theme role.</returns>
    public static ThemeRole GetRole(AvaloniaObject element) => element.GetValue(RoleProperty);

    /// <summary>
    /// Sets the theme role for the specified control.
    /// </summary>
    /// <param name="element">The control to update.</param>
    /// <param name="value">The theme role to assign.</param>
    public static void SetRole(AvaloniaObject element, ThemeRole value) => element.SetValue(RoleProperty, value);

    #endregion

    #region Height

    /// <summary>
    /// Provides Height Property for attached PopupBehavior element.
    /// </summary>
    public static readonly AttachedProperty<double> HeightProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Height", typeof(PopupAssist), double.NaN);

    /// <summary>
    /// Accessor for Attached  <see cref="HeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="HeightProperty"/>.</param>
    public static void SetHeight(StyledElement element, double value) => element.SetValue(HeightProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="HeightProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetHeight(StyledElement element) => element.GetValue(HeightProperty);

    #endregion

    #region Width

    /// <summary>
    /// Provides Width Property for attached PopupBehavior element.
    /// </summary>
    public static readonly AttachedProperty<double> WidthProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("Width", typeof(PopupAssist), double.NaN);

    /// <summary>
    /// Accessor for Attached  <see cref="WidthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="WidthProperty"/>.</param>
    public static void SetWidth(StyledElement element, double value) => element.SetValue(WidthProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="WidthProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetWidth(StyledElement element) => element.GetValue(WidthProperty);

    #endregion
}
