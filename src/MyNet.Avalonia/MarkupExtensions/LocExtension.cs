// -----------------------------------------------------------------------
// <copyright file="LocExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Converters;
using MyNet.Humanizer;
using MyNet.Observable.Translatables;
using MyNet.Utilities;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Markup extension that provides automatic translation of resource keys for Avalonia UI elements.
/// </summary>
/// <remarks>
/// This extension uses a static cache to share <see cref="StringTranslatable"/> instances across multiple bindings,
/// improving memory efficiency and performance. The translated strings automatically update when the culture changes.
/// </remarks>
/// <example>
/// <code>
/// &lt;TextBlock Text="{my:Loc MyResourceKey}" /&gt;
/// &lt;TextBlock Text="{my:Loc MyResourceKey, MyResourceFile}" /&gt;
/// &lt;TextBlock Text="{my:Loc MyResourceKey, Casing=Title}" /&gt;
/// </code>
/// </example>
public class LocExtension : GlobalizationExtensionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LocExtension"/> class.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    public LocExtension(string key)
        : base(true, false) => Key = key;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocExtension"/> class with a specific format string.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    /// <param name="format">The format string to apply to the translated value.</param>
    public LocExtension(string key, string? format)
        : base(true, false)
    {
        Key = key;
        Format = format;
    }

    /// <summary>
    /// Gets or sets the resource key to translate.
    /// </summary>
    [ConstructorArgument("key")]
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the name of the resource file containing the translation (optional).
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Gets or sets the format string to apply to the translated value (optional).
    /// </summary>
    [ConstructorArgument("format")]
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets the letter casing to apply to the translated string.
    /// </summary>
    public LetterCasing Casing { get; set; } = LetterCasing.Normal;

    /// <summary>
    /// Creates the main binding for the translation.
    /// </summary>
    /// <returns>A binding to a <see cref="Localizable"/> instance.</returns>
    protected override IBinding? CreateBinding() => new Binding
    {
        Source = new Localizable(Key, Filename),
        Mode = BindingMode.OneTime
    };

    /// <summary>
    /// Creates the multi-value converter for the translation.
    /// </summary>
    /// <returns>The <see cref="IMultiValueConverter"/> for string localization.</returns>
    protected override IMultiValueConverter CreateConverter() => StringConverter.Converters[Casing];

    /// <summary>
    /// Creates the converter parameter (format string) for the translation.
    /// </summary>
    /// <returns>The format string, or null if not set.</returns>
    protected override object? CreateConverterParameter() => Format;
}
