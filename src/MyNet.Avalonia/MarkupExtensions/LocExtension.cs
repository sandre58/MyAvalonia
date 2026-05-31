// -----------------------------------------------------------------------
// <copyright file="LocExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Metadata;
using MyNet.Avalonia.Converters;
using MyNet.Globalization.Localization.Translation;
using MyNet.Text.TextCasing;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Markup extension that provides automatic translation of resource keys for Avalonia UI elements.
/// </summary>
/// <example>
/// <code>
/// &lt;TextBlock Text="{my:Loc MyResourceKey}" /&gt;
/// &lt;TextBlock Text="{my:Loc MyResourceKey, Filename=MyResourceFile}" /&gt;
/// &lt;TextBlock Text="{my:Loc MyResourceKey, Style=Abbreviation, Casing=Title}" /&gt;
/// </code>
/// </example>
public class LocExtension : GlobalizationExtensionBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LocExtension"/> class for the given resource key.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    public LocExtension(string key)
        : base(true, false) => Key = key;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocExtension"/> class for the given resource key and format.
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
    /// Gets or sets the resource key to translate. This is the key used to look up the localized string in the resource files.
    /// </summary>
    [ConstructorArgument("key")]
    public string Key { get; set; }

    /// <summary>
    /// Gets or sets the name of the resource file to use for translation. If not set, the default resource file is used.
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Gets or sets the format string to apply to the translated value.
    /// </summary>
    [ConstructorArgument("format")]
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets the letter casing to apply to the translated value.
    /// </summary>
    public LetterCasing Casing { get; set; } = LetterCasing.Normal;

    /// <summary>
    /// Gets or sets the translation display style (default, abbreviation, symbol, …).
    /// </summary>
    public DisplayStyle Style { get; set; } = DisplayStyle.Default;

    /// <inheritdoc/>
    protected override BindingBase CreateBinding() => new ReflectionBinding
    {
        Source = new Localizable(Key, Filename),
        Mode = BindingMode.OneTime
    };

    /// <inheritdoc/>
    protected override IMultiValueConverter CreateConverter() =>
        LocalizationMarkupConverterFactory.Create(Casing, Style, quantityFromValue: false);

    /// <inheritdoc/>
    protected override object? CreateConverterParameter() => Format;
}
