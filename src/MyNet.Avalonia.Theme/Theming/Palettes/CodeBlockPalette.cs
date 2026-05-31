// -----------------------------------------------------------------------
// <copyright file="CodeBlockPalette.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Avalonia.Media;
using MyNet.Primitives;

namespace MyNet.Avalonia.Theme.Theming.Palettes;

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
    /// Gets the color used for language keywords in code.
    /// </summary>
    public required Color Keyword { get; init; }

    /// <summary>
    /// Gets the color used for numeric literals in code.
    /// </summary>
    public required Color Number { get; init; }

    /// <summary>
    /// Converts the code block palette to a read-only dictionary suitable for use as resource dictionary keys and values.
    /// </summary>
    /// <returns>A dictionary containing all code block colors with their corresponding resource keys.</returns>
    public IReadOnlyDictionary<string, object> ToResourceDictionary(string prefix = nameof(ThemeVariantPalette.CodeBlock)) => new Dictionary<string, object>
        {
            { nameof(Unknown).WithPrefix(prefix, "."), Unknown },
            { nameof(Space).WithPrefix(prefix, "."), Space },
            { nameof(Comment).WithPrefix(prefix, "."), Comment },
            { nameof(Tag).WithPrefix(prefix, "."), Tag },
            { nameof(Quote).WithPrefix(prefix, "."), Quote },
            { nameof(AttributeValue).WithPrefix(prefix, "."), AttributeValue },
            { nameof(AttributeKey).WithPrefix(prefix, "."), AttributeKey },
            { nameof(Brace).WithPrefix(prefix, "."), Brace },
            { nameof(Entity).WithPrefix(prefix, "."), Entity },
            { nameof(Keyword).WithPrefix(prefix, "."), Keyword },
            { nameof(Number).WithPrefix(prefix, "."), Number }
        };

    /// <summary>
    /// Creates a CodeBlockPalette instance from a resource dictionary.
    /// </summary>
    /// <param name="dictionary">The resource dictionary containing color definitions.</param>
    /// <param name="prefix">The prefix used for resource keys (default: "CodeBlock").</param>
    /// <returns>A new CodeBlockPalette instance.</returns>
    public static CodeBlockPalette FromResourceDictionary(IReadOnlyDictionary<string, object> dictionary, string prefix = nameof(ThemeVariantPalette.CodeBlock))
    {
        var defaultColor = global::Avalonia.Media.Colors.Gray;
        return new()
        {
            Unknown = (Color)dictionary.GetValueOrDefault(nameof(Unknown).WithPrefix(prefix, "."), defaultColor),
            Space = (Color)dictionary.GetValueOrDefault(nameof(Space).WithPrefix(prefix, "."), defaultColor),
            Comment = (Color)dictionary.GetValueOrDefault(nameof(Comment).WithPrefix(prefix, "."), defaultColor),
            Tag = (Color)dictionary.GetValueOrDefault(nameof(Tag).WithPrefix(prefix, "."), defaultColor),
            Quote = (Color)dictionary.GetValueOrDefault(nameof(Quote).WithPrefix(prefix, "."), defaultColor),
            AttributeValue = (Color)dictionary.GetValueOrDefault(nameof(AttributeValue).WithPrefix(prefix, "."), defaultColor),
            AttributeKey = (Color)dictionary.GetValueOrDefault(nameof(AttributeKey).WithPrefix(prefix, "."), defaultColor),
            Brace = (Color)dictionary.GetValueOrDefault(nameof(Brace).WithPrefix(prefix, "."), defaultColor),
            Entity = (Color)dictionary.GetValueOrDefault(nameof(Entity).WithPrefix(prefix, "."), defaultColor),
            Keyword = (Color)dictionary.GetValueOrDefault(nameof(Keyword).WithPrefix(prefix, "."), defaultColor),
            Number = (Color)dictionary.GetValueOrDefault(nameof(Number).WithPrefix(prefix, "."), defaultColor)
        };
    }
}
