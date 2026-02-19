// -----------------------------------------------------------------------
// <copyright file="DateTimePickerBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class DateTimePickerBehavior
{
    #region OverridePlaceholderText

    /// <summary>
    /// Provides OverridePlaceholderText Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> OverridePlaceholderTextProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("OverridePlaceholderText", typeof(DateTimePickerBehavior), true);

    /// <summary>
    /// Accessor for Attached  <see cref="OverridePlaceholderTextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="OverridePlaceholderTextProperty"/>.</param>
    public static void SetOverridePlaceholderText(StyledElement element, bool value) => element.SetValue(OverridePlaceholderTextProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="OverridePlaceholderTextProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetOverridePlaceholderText(StyledElement element) => element.GetValue(OverridePlaceholderTextProperty);

    #endregion
}
