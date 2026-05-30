// -----------------------------------------------------------------------
// <copyright file="DisplayExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Data;
using Avalonia.Data.Converters;
using MyNet.Avalonia.Converters;
using MyNet.Text.TextCasing;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Markup extension that formats a bound value for display with <see cref="StringConverter"/>,
/// and re-evaluates when culture or time zone changes.
/// </summary>
/// <example>
/// <code>
/// &lt;Setter Property="my:ClipboardAssist.Content" Value="{my:Display Content, RelativeSource={RelativeSource Self}}" /&gt;
/// &lt;TextBlock Text="{my:Display Date, Format=LongDatePattern, Casing=Title, RelativeSource={RelativeSource AncestorType=Page}}" /&gt;
/// </code>
/// </example>
public class DisplayExtension : GlobalizationExtensionBase
{
    /// <summary>
    /// Initializes a new instance with culture and time zone change tracking enabled.
    /// </summary>
    public DisplayExtension()
        : base(updateOnCultureChanged: true, updateOnTimeZoneChanged: true) { }

    /// <summary>
    /// Initializes a new instance for the given binding path.
    /// </summary>
    /// <param name="path">The property path to bind.</param>
    public DisplayExtension(string path)
        : this() => Path = path;

    /// <summary>
    /// Gets or sets the binding path.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the letter casing to apply.
    /// </summary>
    public LetterCasing Casing { get; set; } = LetterCasing.Normal;

    /// <summary>
    /// Gets or sets the format string (converter parameter).
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets whether pluralization rules apply to the format string.
    /// </summary>
    public bool Pluralize { get; set; }

    /// <summary>
    /// Gets or sets whether abbreviated translations are used for enums.
    /// </summary>
    public bool Abbreviate { get; set; }

    /// <summary>
    /// Gets or sets the element name for the binding source.
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    protected override IMultiValueConverter CreateConverter() => new StringConverter(Casing, Pluralize, Abbreviate);

    /// <inheritdoc />
    protected override object? CreateConverterParameter() => Format;
}
