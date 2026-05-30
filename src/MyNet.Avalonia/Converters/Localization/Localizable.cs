// -----------------------------------------------------------------------
// <copyright file="Localizable.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace MyNet.Avalonia.Converters;
#pragma warning restore IDE0130

/// <summary>
/// Represents a localizable resource key and optional filename for <see cref="StringConverter"/>.
/// </summary>
/// <param name="Key">The resource key to translate.</param>
/// <param name="Filename">The optional .resx filename (without extension).</param>
/// <example>
/// Bind a <see cref="Localizable"/> value through <see cref="StringConverter"/> to translate a keyed resource.
/// </example>
public record Localizable(string Key, string? Filename);
