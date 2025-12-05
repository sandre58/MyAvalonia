// -----------------------------------------------------------------------
// <copyright file="DateTimePickerBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class DateTimePickerBehavior
{
    #region OverrideWatermark

    /// <summary>
    /// Provides OverrideWatermark Property for attached TextFieldBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> OverrideWatermarkProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("OverrideWatermark", typeof(DateTimePickerBehavior), true);

    /// <summary>
    /// Accessor for Attached  <see cref="OverrideWatermarkProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="OverrideWatermarkProperty"/>.</param>
    public static void SetOverrideWatermark(StyledElement element, bool value) => element.SetValue(OverrideWatermarkProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="OverrideWatermarkProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetOverrideWatermark(StyledElement element) => element.GetValue(OverrideWatermarkProperty);

    #endregion
}
