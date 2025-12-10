// -----------------------------------------------------------------------
// <copyright file="ThemeRole.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Defines semantic roles for theme colors in the application.
/// Each role represents a functional category of UI elements with associated color palettes.
/// </summary>
public enum ThemeRole
{
    /// <summary>
    /// Default surface role using theme surface colors.
    /// </summary>
    Default,

    /// <summary>
    /// Primary brand color role for main actions and emphasis.
    /// </summary>
    Primary,

    /// <summary>
    /// Accent/secondary brand color role for highlights and secondary actions.
    /// </summary>
    Accent,

    /// <summary>
    /// Success role for positive feedback and confirmation actions.
    /// </summary>
    Success,

    /// <summary>
    /// Warning role for cautionary messages and actions.
    /// </summary>
    Warning,

    /// <summary>
    /// Error role for error states and destructive actions.
    /// </summary>
    Error,

    /// <summary>
    /// Information role for informational messages and neutral actions.
    /// </summary>
    Information,

    /// <summary>
    /// Neutral role for neutral or disabled states.
    /// </summary>
    Neutral,

    /// <summary>
    /// Represents a dark color scheme or theme option.
    /// </summary>
    Dark,

    /// <summary>
    /// Represents an inverse color scheme, typically used for contrast against the parent element.
    /// </summary>
    Inverse,

    /// <summary>
    /// Custom role for user-defined colors via attached properties.
    /// </summary>
    Custom
}
