// -----------------------------------------------------------------------
// <copyright file="CodeBlockPalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Media;
using MyNet.Avalonia.Theme.Extensions;

namespace MyNet.Avalonia.Theme.Palettes;

/// <summary>
/// Represents a palette of colors used for different code block elements in syntax highlighting.
/// </summary>
public class CodeBlockPalette
{
    /// <summary>
    /// Gets the color used for unknown code elements.
    /// </summary>
    public required Color Unknown { get; init; }

    /// <summary>
    /// Gets the color used for spaces in code.
    /// </summary>
    public required Color Space { get; init; }

    /// <summary>
    /// Gets the color used for comments in code.
    /// </summary>
    public required Color Comment { get; init; }

    /// <summary>
    /// Gets the color used for tags in code.
    /// </summary>
    public required Color Tag { get; init; }

    /// <summary>
    /// Gets the color used for quoted text in code.
    /// </summary>
    public required Color Quote { get; init; }

    /// <summary>
    /// Gets the color used for attribute values in code.
    /// </summary>
    public required Color AttributeValue { get; init; }

    /// <summary>
    /// Gets the color used for attribute keys in code.
    /// </summary>
    public required Color AttributeKey { get; init; }

    /// <summary>
    /// Gets the color used for braces in code.
    /// </summary>
    public required Color Brace { get; init; }

    /// <summary>
    /// Gets the color used for entities in code.
    /// </summary>
    public required Color Entity { get; init; }

    /// <summary>
    /// Converts the code block palette to a read-only dictionary suitable for use as resource dictionary keys and values.
    /// </summary>
    /// <returns>A dictionary containing all code block colors with their corresponding resource keys.</returns>
    public IReadOnlyDictionary<string, Color> ToResourceDictionary(string prefix = nameof(ThemePalette.CodeBlock)) => new Dictionary<string, Color>
        {
            { nameof(Unknown).WithPrefix(prefix), Unknown },
            { nameof(Space).WithPrefix(prefix), Space },
            { nameof(Comment).WithPrefix(prefix), Comment },
            { nameof(Tag).WithPrefix(prefix), Tag },
            { nameof(Quote).WithPrefix(prefix), Quote },
            { nameof(AttributeValue).WithPrefix(prefix), AttributeValue },
            { nameof(AttributeKey).WithPrefix(prefix), AttributeKey },
            { nameof(Brace).WithPrefix(prefix), Brace },
            { nameof(Entity).WithPrefix(prefix), Entity }
        };
}
