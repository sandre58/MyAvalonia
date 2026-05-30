// -----------------------------------------------------------------------
// <copyright file="LocalizableString.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using MyNet.Globalization.Facade;
using MyNet.Globalization.Localization.Translation;
using MyNet.Text;
using MyNet.Text.TextCasing;

namespace MyNet.Avalonia.Localization;

/// <summary>
/// Represents a localizable string that automatically updates when the culture changes.
/// This class can be used as a value in properties that don't support direct bindings.
/// </summary>
public class LocalizableString : INotifyPropertyChanged
{
    private readonly string _key;
    private readonly string? _filename;
    private readonly string? _format;
    private readonly LetterCasing _casing;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizableString"/> class.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    /// <param name="filename">The optional resource filename.</param>
    /// <param name="format">The optional format string.</param>
    /// <param name="casing">The letter casing to apply.</param>
    public LocalizableString(string key, string? filename = null, string? format = null, LetterCasing casing = LetterCasing.Normal)
    {
        _key = key;
        _filename = filename;
        _format = format;
        _casing = casing;

        // Subscribe to culture changes
        UIContext.Globalization.PropertyChanged += OnGlobalizationChanged;
    }

    /// <summary>
    /// Gets the current translated value.
    /// </summary>
    public string Value => GetTranslatedValue();

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets the translated value based on the current culture.
    /// </summary>
    private string GetTranslatedValue()
    {
        var value = !string.IsNullOrEmpty(_filename)
            ? _key.Translate(_filename, UIContext.Globalization.Culture)
            : _key.Translate(DisplayStyle.Default, UIContext.Globalization.Culture);

        if (!string.IsNullOrEmpty(_format))
        {
            value = string.Format(CultureInfo.CurrentCulture, _format, value);
        }

        return _casing != LetterCasing.Normal ? value.ApplyCase(_casing) : value;
    }

    /// <summary>
    /// Handles globalization property changes.
    /// </summary>
    private void OnGlobalizationChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(UIContext.Globalization.Culture))
        {
            OnPropertyChanged(nameof(Value));
        }
    }

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new(propertyName));

    /// <summary>
    /// Returns the translated string value.
    /// </summary>
    public override string ToString() => Value;
}
