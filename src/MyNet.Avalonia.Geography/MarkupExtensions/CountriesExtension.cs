// -----------------------------------------------------------------------
// <copyright file="CountriesExtension.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Markup.Xaml;

namespace MyNet.Avalonia.Geography.MarkupExtensions;

/// <summary>
/// Markup extension that provides a collection of translatable <see cref="MyNet.Geography.Country"/> values for data binding.
/// </summary>
/// <example>
/// <code>
/// &lt;ComboBox ItemsSource="{geo:Countries}" /&gt;
/// </code>
/// </example>
public sealed class CountriesExtension : MarkupExtension
{
    /// <inheritdoc/>
    public override object ProvideValue(IServiceProvider serviceProvider) => CountrySource.GetAllOrderedByDisplay();
}
