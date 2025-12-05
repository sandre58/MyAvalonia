// -----------------------------------------------------------------------
// <copyright file="ScrollViewerAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Theme.Assists;

public static class ScrollViewerAssist
{
    #region ButtonsIsVisible

    /// <summary>
    /// Provides ButtonsIsVisible Property for attached ScrollBarAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> ButtonsIsVisibleProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("ButtonsIsVisible", typeof(ScrollViewerAssist), true);

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonsIsVisibleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ButtonsIsVisibleProperty"/>.</param>
    public static void SetButtonsIsVisible(StyledElement element, bool value) => element.SetValue(ButtonsIsVisibleProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ButtonsIsVisibleProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetButtonsIsVisible(StyledElement element) => element.GetValue(ButtonsIsVisibleProperty);

    #endregion

    #region ActiveThumbThickness

    /// <summary>
    /// Provides ActiveThumbThickness Property for attached ScrollBarAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> ActiveThumbThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("ActiveThumbThickness", typeof(ScrollViewerAssist), 10.0);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveThumbThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="ActiveThumbThicknessProperty"/>.</param>
    public static void SetActiveThumbThickness(StyledElement element, double value) => element.SetValue(ActiveThumbThicknessProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="ActiveThumbThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetActiveThumbThickness(StyledElement element) => element.GetValue(ActiveThumbThicknessProperty);

    #endregion

    #region InactiveThumbThickness

    /// <summary>
    /// Provides InactiveThumbThickness Property for attached ScrollBarAssist element.
    /// </summary>
    public static readonly AttachedProperty<double> InactiveThumbThicknessProperty = AvaloniaProperty.RegisterAttached<StyledElement, double>("InactiveThumbThickness", typeof(ScrollViewerAssist), 6.0);

    /// <summary>
    /// Accessor for Attached  <see cref="InactiveThumbThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="InactiveThumbThicknessProperty"/>.</param>
    public static void SetInactiveThumbThickness(StyledElement element, double value) => element.SetValue(InactiveThumbThicknessProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="InactiveThumbThicknessProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static double GetInactiveThumbThickness(StyledElement element) => element.GetValue(InactiveThumbThicknessProperty);

    #endregion
}
