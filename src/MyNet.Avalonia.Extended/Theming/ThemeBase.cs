// -----------------------------------------------------------------------
// <copyright file="ThemeBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Styling;
using MyNet.UI.Theming;

namespace MyNet.Avalonia.Extended.Theming;

/// <summary>
/// Represents the base theme configuration for a specific Avalonia theme variant.
/// </summary>
/// <param name="themeVariant">The theme variant associated with this theme base.</param>
/// <param name="isDark">A value indicating whether this theme base represents a dark theme variant.</param>
/// <param name="isHighContrast">A value indicating whether this theme base represents a high contrast theme variant.</param>
public sealed class ThemeBase(ThemeVariant themeVariant, bool isDark, bool isHighContrast) : IThemeBase
{
    /// <summary>
    /// Gets the Avalonia theme variant associated with this theme base.
    /// </summary>
    public ThemeVariant ThemeVariant { get; } = themeVariant;

    /// <inheritdoc />
    public string Name => ThemeVariant.Key.ToString() ?? string.Empty;

    /// <inheritdoc />
    public bool IsDark { get; } = isDark;

    /// <inheritdoc />
    public bool IsHighContrast { get; } = isHighContrast;

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is IThemeBase other && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override int GetHashCode() => Name.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
