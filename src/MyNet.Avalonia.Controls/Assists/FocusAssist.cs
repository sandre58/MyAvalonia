// -----------------------------------------------------------------------
// <copyright file="FocusAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Input;

namespace MyNet.Avalonia.Controls.Assists;

public static class FocusAssist
{
    #region DialogFocusHint

    /// <summary>
    /// Provides DialogFocusHint Property for attached FocusAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> DialogFocusHintProperty = AvaloniaProperty.RegisterAttached<InputElement, bool>("DialogFocusHint", typeof(FocusAssist));

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
