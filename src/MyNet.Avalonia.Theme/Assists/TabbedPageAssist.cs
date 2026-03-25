// -----------------------------------------------------------------------
// <copyright file="TabbedPageAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Styling;

namespace MyNet.Avalonia.Theme.Assists;

public static class TabbedPageAssist
{
    #region TabControlTheme

    /// <summary>
    /// Provides TabControlTheme Property for attached TabbedPageAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme> TabControlThemeProperty = AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme>("TabControlTheme", typeof(TabbedPageAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="TabControlThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="TabControlThemeProperty"/>.</param>
    public static void SetTabControlTheme(StyledElement element, ControlTheme value) => element.SetValue(TabControlThemeProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="TabControlThemeProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static ControlTheme GetTabControlTheme(StyledElement element) => element.GetValue(TabControlThemeProperty);

    #endregion
}
