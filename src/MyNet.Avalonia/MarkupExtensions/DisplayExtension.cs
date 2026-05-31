// -----------------------------------------------------------------------
// <copyright file="DisplayExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Data.Converters;
using MyNet.Avalonia.Converters;
using MyNet.Globalization.Localization.Translation;
using MyNet.Text.TextCasing;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Markup extension that formats a bound value for display with <see cref="StringConverter"/>,
/// and re-evaluates when culture or time zone changes.
/// </summary>
/// <example>
/// <code>
/// &lt;Setter Property="my:ClipboardAssist.Content" Value="{my:Display Content, RelativeSource={RelativeSource Self}}" /&gt;
/// &lt;TextBlock Text="{my:Display Count, Format=ItemsCount, Style=Abbreviation, Quantity=True}" /&gt;
/// &lt;TextBlock Text="{my:Display Date, Format=LongDatePattern, Casing=Title}" /&gt;
/// </code>
/// </example>
public class DisplayExtension() : GlobalizationExtensionBase(updateOnCultureChanged: true, updateOnTimeZoneChanged: true)
{
    public DisplayExtension(string path)
        : this() => Path = path;

    /// <summary>
    /// Gets or sets the property path to bind for the value to display. If empty, binds to the current data context.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the letter casing to apply to the displayed value (e.g., normal, upper, lower, title).
    /// </summary>
    public LetterCasing Casing { get; set; } = LetterCasing.Normal;

    /// <summary>
    /// Gets or sets the format resource key passed to <see cref="StringConverter"/>.
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets the translation display style for resource keys and humanized values.
    /// </summary>
    public DisplayStyle Style { get; set; } = DisplayStyle.Default;

    /// <summary>
    /// Gets or sets a value indicating whether the bound value is used as <see cref="TranslationOptions.Quantity"/>
    /// when translating the <see cref="Format"/> resource key (for pluralization).
    /// </summary>
    public bool Quantity { get; set; }

    /// <summary>
    /// Gets or sets the name of the element to bind to as the source of the value. If set, <see cref="Source"/> and <see cref="RelativeSource"/> are ignored.
    /// </summary>
    public string? ElementName { get; set; }

    /// <summary>
    /// Gets or sets the relative source to bind to as the source of the value. Ignored if <see cref="ElementName"/> is set.
    /// </summary>
    public RelativeSource? RelativeSource { get; set; }

    /// <summary>
    /// Gets or sets the explicit source object to bind to as the source of the value. Ignored if <see cref="ElementName"/> is set.
    /// </summary>
    public object? Source { get; set; }

    /// <inheritdoc/>
    protected override BindingBase CreateBinding()
    {
        var binding = new ReflectionBinding(string.IsNullOrEmpty(Path) ? "." : Path)
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

    /// <inheritdoc/>
    protected override IMultiValueConverter CreateConverter() => LocalizationMarkupConverterFactory.Create(Casing, Style, Quantity);

    /// <inheritdoc/>
    protected override object? CreateConverterParameter() => Format;
}
