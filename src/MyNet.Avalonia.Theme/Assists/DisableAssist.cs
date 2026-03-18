// -----------------------------------------------------------------------
// <copyright file="DisableAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Classes.Helpers;

namespace MyNet.Avalonia.Theme.Assists;

public static class DisableAssist
{
    #region IsDisablable

    /// <summary>
    /// Provides IsDisablable Property for attached DisableAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> IsDisablableProperty = AvaloniaPropertyHelper.RegisterBoolProperty("IsDisablable", CssClass.IsDisablable);

    /// <summary>
    /// Accessor for Attached  <see cref="IsDisablableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IsDisablableProperty"/>.</param>
    public static void SetIsDisablable(StyledElement element, bool value) => element.SetValue(IsDisablableProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IsDisablableProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetIsDisablable(StyledElement element) => element.GetValue(IsDisablableProperty);

    #endregion
}
