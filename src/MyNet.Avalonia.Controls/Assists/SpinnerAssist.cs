// -----------------------------------------------------------------------
// <copyright file="SpinnerAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Styling;
using MyNet.Avalonia.Controls.Enums;

namespace MyNet.Avalonia.Controls.Assists;

public static class SpinnerAssist
{
    #region SwitchButtons

    /// <summary>
    /// Provides SwitchButtons Property for attached SpinnerAssist element.
    /// </summary>
    public static readonly AttachedProperty<bool> SwitchButtonsProperty = AvaloniaProperty.RegisterAttached<StyledElement, bool>("SwitchButtons", typeof(SpinnerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="SwitchButtonsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="SwitchButtonsProperty"/>.</param>
    public static void SetSwitchButtons(StyledElement element, bool value) => element.SetValue(SwitchButtonsProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="SwitchButtonsProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static bool GetSwitchButtons(StyledElement element) => element.GetValue(SwitchButtonsProperty);

    #endregion

    #region Layout

    /// <summary>
    /// Provides Layout Property for attached SpinnerAssist element.
    /// </summary>
    public static readonly AttachedProperty<SpinnerLayout> LayoutProperty = AvaloniaProperty.RegisterAttached<StyledElement, SpinnerLayout>("Layout", typeof(SpinnerAssist), SpinnerLayout.Vertical);

    /// <summary>
    /// Accessor for Attached  <see cref="LayoutProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="LayoutProperty"/>.</param>
    public static void SetLayout(StyledElement element, SpinnerLayout value) => element.SetValue(LayoutProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="LayoutProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static SpinnerLayout GetLayout(StyledElement element) => element.GetValue(LayoutProperty);

    #endregion

    #region ButtonTheme

    /// <summary>
    /// Provides ButtonTheme Property for attached SpinnerAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme> ButtonThemeProperty = AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme>("ButtonTheme", typeof(SpinnerAssist));

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

    #region DecreaseContent

    /// <summary>
    /// Provides DecreaseContent Property for attached SpinnerAssist element.
    /// </summary>
    public static readonly AttachedProperty<object> DecreaseContentProperty = AvaloniaProperty.RegisterAttached<StyledElement, object>("DecreaseContent", typeof(SpinnerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="DecreaseContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="DecreaseContentProperty"/>.</param>
    public static void SetDecreaseContent(StyledElement element, object value) => element.SetValue(DecreaseContentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="DecreaseContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object GetDecreaseContent(StyledElement element) => element.GetValue(DecreaseContentProperty);

    #endregion

    #region IncreaseContent

    /// <summary>
    /// Provides IncreaseContent Property for attached SpinnerAssist element.
    /// </summary>
    public static readonly AttachedProperty<object> IncreaseContentProperty = AvaloniaProperty.RegisterAttached<StyledElement, object>("IncreaseContent", typeof(SpinnerAssist));

    /// <summary>
    /// Accessor for Attached  <see cref="IncreaseContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    /// <param name="value">The value to set  <see cref="IncreaseContentProperty"/>.</param>
    public static void SetIncreaseContent(StyledElement element, object value) => element.SetValue(IncreaseContentProperty, value);

    /// <summary>
    /// Accessor for Attached  <see cref="IncreaseContentProperty"/>.
    /// </summary>
    /// <param name="element">Target element.</param>
    public static object GetIncreaseContent(StyledElement element) => element.GetValue(IncreaseContentProperty);

    #endregion
}
