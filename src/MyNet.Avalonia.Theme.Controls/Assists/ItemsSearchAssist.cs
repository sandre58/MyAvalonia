// -----------------------------------------------------------------------
// <copyright file="ItemsSearchAssist.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;
using Avalonia.Styling;

namespace MyNet.Avalonia.Theme.Controls.Assists;

/// <summary>
/// Theme presentation attached properties for popup item search fields.
/// </summary>
public static class ItemsSearchAssist
{
    #region PlaceholderText

    /// <summary>
    /// Provides PlaceholderText Property for attached ItemsSearchAssist element.
    /// </summary>
    public static readonly AttachedProperty<string?> PlaceholderTextProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, string?>("PlaceholderText", typeof(ItemsSearchAssist));

    /// <summary>
    /// Accessor for Attached <see cref="PlaceholderTextProperty"/>.
    /// </summary>
    public static void SetPlaceholderText(StyledElement element, string? value) => element.SetValue(PlaceholderTextProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="PlaceholderTextProperty"/>.
    /// </summary>
    public static string? GetPlaceholderText(StyledElement element) => element.GetValue(PlaceholderTextProperty);

    #endregion

    #region TextBoxTheme

    /// <summary>
    /// Provides TextBoxTheme Property for attached ItemsSearchAssist element.
    /// </summary>
    public static readonly AttachedProperty<ControlTheme?> TextBoxThemeProperty =
        AvaloniaProperty.RegisterAttached<StyledElement, ControlTheme?>("TextBoxTheme", typeof(ItemsSearchAssist));

    /// <summary>
    /// Accessor for Attached <see cref="TextBoxThemeProperty"/>.
    /// </summary>
    public static void SetTextBoxTheme(StyledElement element, ControlTheme? value) => element.SetValue(TextBoxThemeProperty, value);

    /// <summary>
    /// Accessor for Attached <see cref="TextBoxThemeProperty"/>.
    /// </summary>
    public static ControlTheme? GetTextBoxTheme(StyledElement element) => element.GetValue(TextBoxThemeProperty);

    #endregion
}
