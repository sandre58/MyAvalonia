// -----------------------------------------------------------------------
// <copyright file="DisplayTextBlockExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Controls;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Markup extension that binds a <see cref="TextBlock"/> to a value formatted with <see cref="DisplayExtension"/>.
/// </summary>
/// <example>
/// <code>
/// &lt;Button Content="{my:DisplayTextBlock Button_Save}" /&gt;
/// &lt;ContentControl Content="{my:DisplayTextBlock DisplayDateContext, Format='MMM', RelativeSource={RelativeSource TemplatedParent}}" /&gt;
/// </code>
/// </example>
public class DisplayTextBlockExtension : DisplayExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayTextBlockExtension"/> class.
    /// </summary>
    public DisplayTextBlockExtension() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayTextBlockExtension"/> class for the given binding path.
    /// </summary>
    /// <param name="path">The property path to bind.</param>
    public DisplayTextBlockExtension(string path)
        : base(path) { }

    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider) => new TextBlock
    {
        [!TextBlock.TextProperty] = CreateMultiBinding()
    };
}
