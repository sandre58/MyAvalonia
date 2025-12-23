// -----------------------------------------------------------------------
// <copyright file="TranslateTextBlockExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Markup extension that provides automatic translation of resource keys and returns a <see cref="TextBlock"/> control.
/// </summary>
/// <remarks>
/// This extension extends <see cref="TranslateExtension"/> to return a <see cref="TextBlock"/> control instead of a binding.
/// It is useful for content properties that support formatted text, such as buttons and menu items.
/// If you want to support access keys (keyboard mnemonics), use an <see cref="TextBlock"/> control instead.
/// </remarks>
/// <example>
/// <code>
/// &lt;Button Content="{my:TranslateTextBlock Button_Save}" /&gt;
/// &lt;MenuItem Header="{my:TranslateTextBlock Menu_File}" /&gt;
/// </code>
/// </example>
public class TranslateTextBlockExtension : TranslateExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateTextBlockExtension"/> class.
    /// </summary>
    public TranslateTextBlockExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateTextBlockExtension"/> class.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    public TranslateTextBlockExtension(string key)
        : base(key) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TranslateTextBlockExtension"/> class with a specific format string.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    /// <param name="format">The format string to apply to the translated value.</param>
    public TranslateTextBlockExtension(string key, string format)
        : base(key, format) { }

    /// <inheritdoc />
    /// <remarks>
    /// Returns a <see cref="TextBlock"/> control with its <see cref="TextBlock.TextProperty"/> bound to the translated value using a multi-binding.
    /// </remarks>
    public override object ProvideValue(IServiceProvider serviceProvider) => new TextBlock
    {
        [!TextBlock.TextProperty] = CreateMultiBinding()
    };
}
