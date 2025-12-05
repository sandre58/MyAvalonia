// -----------------------------------------------------------------------
// <copyright file="FocusBehavior.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Input;

namespace MyNet.Avalonia.Controls.Behaviors;

public static class FocusBehavior
{
    #region DialogFocusHint

    /// <summary>
    /// Provides DialogFocusHint Property for attached FocusBehavior element.
    /// </summary>
    public static readonly AttachedProperty<bool> DialogFocusHintProperty = AvaloniaProperty.RegisterAttached<InputElement, bool>("DialogFocusHint", typeof(FocusBehavior));

    /// <summary>
    /// Accessor for Attached  <see cref="DialogFocusHintProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="DialogFocusHintProperty"/>.</param>
    public static void SetDialogFocusHint(InputElement element, bool value) => element.SetValue(DialogFocusHintProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="DialogFocusHintProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetDialogFocusHint(InputElement element) => element.GetValue(DialogFocusHintProperty);

    #endregion
}
