// -----------------------------------------------------------------------
// <copyright file="TranslateExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Data.Converters;
using MyNet.Avalonia.Converters;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Markup extension that provides dynamic translation with formatting, pluralization, and casing options for Avalonia UI elements.
/// </summary>
/// <remarks>
/// This extension allows you to bind to a property and automatically translate its value using a resource key, with support for formatting, pluralization, abbreviation, and casing.
/// It extends <see cref="LocExtension"/> and is suitable for dynamic scenarios where the value to translate is not static.
/// </remarks>
/// <example>
/// <code>
/// &lt;TextBlock Text="{my:Translate Path=MyProperty, Format='N2', Pluralize=True, Casing=Title}" /&gt;
/// </code>
/// </example>
public class TranslateExtension : LocExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
    /// </summary>
    public TranslateExtension()
        : base(string.Empty) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateExtension"/> class with a resource key.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    public TranslateExtension(string key)
        : base(key) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateExtension"/> class with a resource key and format string.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    /// <param name="format">The format string to apply to the translated value.</param>
    public TranslateExtension(string key, string? format)
        : base(key, format) { }

    /// <summary>
    /// Gets or sets a value indicating whether to apply pluralization rules based on the numeric value.
    /// </summary>
    /// <remarks>
    /// When true, the format string is treated as a translation key that supports pluralization
    /// (e.g., "Item_Plural" might resolve to "item" or "items" based on the count).
    /// </remarks>
    public bool Pluralize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use abbreviated translations for enums and enumerations.
    /// </summary>
    public bool Abbreviate { get; set; }

    /// <summary>
    /// Gets or sets the name of the element to use as the binding source.
    /// </summary>
    public string? ElementName { get; set; }

    /// <summary>
    /// Gets or sets the relative source for the binding.
    /// </summary>
    public RelativeSource? RelativeSource { get; set; }

    /// <summary>
    /// Gets or sets the binding source object.
    /// </summary>
    public object? Source { get; set; }

    /// <summary>
    /// Creates the main binding for the translation, supporting dynamic binding scenarios.
    /// </summary>
    /// <returns>A binding to the property to translate.</returns>
    protected override BindingBase? CreateBinding()
    {
        var binding = new Binding(Key)
        {
            Mode = BindingMode.OneWay
        };

        if (ElementName is not null)
            binding.ElementName = ElementName;

        if (Source is not null)
            binding.Source = Source;

        if (RelativeSource is not null)
            binding.RelativeSource = RelativeSource;

        return binding;
    }

    /// <summary>
    /// Creates the multi-value converter for the translation, with support for pluralization and abbreviation.
    /// </summary>
    /// <returns>The <see cref="IMultiValueConverter"/> for string localization.</returns>
    protected override IMultiValueConverter CreateConverter() => new StringConverter(Casing, Pluralize, Abbreviate);
}
