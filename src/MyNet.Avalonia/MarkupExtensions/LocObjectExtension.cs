// -----------------------------------------------------------------------
// <copyright file="LocObjectExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using MyNet.Observable;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Markup extension that provides automatic translation of resource keys and returns a <see cref="LocalizedString"/> object.
/// </summary>
/// <remarks>
/// This extension is designed for properties that expect an object value rather than a binding,
/// such as attached properties. The returned <see cref="LocalizedString"/> object automatically
/// updates when the culture changes.
/// </remarks>
/// <example>
/// <code>
/// &lt;Button my:FormItem.Label="{my:LocObject Button_Save}" /&gt;
/// &lt;Control ToolTip.Tip="{my:LocObject Tooltip_Help}" /&gt;
/// </code>
/// </example>
public class LocObjectExtension : LocExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LocObjectExtension"/> class.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    public LocObjectExtension(string key)
        : base(key) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocObjectExtension"/> class with a specific format string.
    /// </summary>
    /// <param name="key">The resource key to translate.</param>
    /// <param name="format">The format string to apply to the translated value.</param>
    public LocObjectExtension(string key, string? format)
        : base(key, format) { }

    /// <summary>
    /// Provides the value for the markup extension.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A <see cref="LocalizedString"/> instance that updates automatically.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
        => new LocalizedString(Key, Casing, Filename);
}
