// -----------------------------------------------------------------------
// <copyright file="GlobalizationExtensionBase.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using MyNet.Avalonia.Bindings;

namespace MyNet.Avalonia.MarkupExtensions;

/// <summary>
/// Abstract base class for Avalonia markup extensions that provide globalization-aware bindings.
/// This class enables automatic updates of bindings when the application's culture or time zone changes.
/// </summary>
/// <remarks>
/// Derive from this class to create custom markup extensions that support localization and time zone awareness.
/// The extension creates a <see cref="MultiBinding"/> that can react to culture and time zone changes by including
/// <see cref="GlobalizationBindingSource"/> as additional binding sources for culture and time zone.
/// </remarks>
/// <param name="updateOnCultureChanged">Whether to update the binding when the culture changes.</param>
/// <param name="updateOnTimeZoneChanged">Whether to update the binding when the time zone changes.</param>
public abstract class GlobalizationExtensionBase(bool updateOnCultureChanged, bool updateOnTimeZoneChanged) : MarkupExtension
{
    /// <summary>
    /// Gets or sets the value to use when the binding target is null.
    /// </summary>
    public object? TargetNullValue { get; set; }

    /// <summary>
    /// Gets or sets the value to use when the binding cannot be resolved.
    /// </summary>
    public object? FallbackValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to update the binding when the culture changes.
    /// </summary>
    public bool UpdateOnCultureChanged { get; set; } = updateOnCultureChanged;

    /// <summary>
    /// Gets or sets a value indicating whether to update the binding when the time zone changes.
    /// </summary>
    public bool UpdateOnTimeZoneChanged { get; set; } = updateOnTimeZoneChanged;

    /// <summary>
    /// Provides the value for the markup extension by creating a <see cref="MultiBinding"/> that reacts to culture and/or time zone changes.
    /// </summary>
    /// <param name="serviceProvider">The service provider for the markup extension.</param>
    /// <returns>A <see cref="MultiBinding"/> instance configured for globalization support.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => CreateMultiBinding();

    /// <summary>
    /// Creates the <see cref="MultiBinding"/> instance with the appropriate bindings and converter.
    /// </summary>
    /// <returns>A configured <see cref="MultiBinding"/>.</returns>
    protected virtual MultiBinding CreateMultiBinding()
    {
        var multiBinding = new MultiBinding
        {
            Converter = CreateConverter(),
            ConverterParameter = CreateConverterParameter(),
            Mode = BindingMode.OneWay
        };

        if (FallbackValue is not null)
        {
            multiBinding.FallbackValue = FallbackValue;
        }

        if (TargetNullValue is not null)
        {
            multiBinding.TargetNullValue = TargetNullValue;
        }

        if (CreateBinding() is { } binding)
        {
            multiBinding.Bindings.Add(binding);
        }

        if (UpdateOnCultureChanged)
            multiBinding.Bindings.Add(GlobalizationBinding.CreateCultureBinding());

        if (UpdateOnTimeZoneChanged)
            multiBinding.Bindings.Add(GlobalizationBinding.CreateTimeZoneBinding());

        return multiBinding;
    }

    /// <summary>
    /// Creates the <see cref="IMultiValueConverter"/> to use for the multi-binding.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <returns>The multi-value converter.</returns>
    protected abstract IMultiValueConverter CreateConverter();

    /// <summary>
    /// Creates the converter parameter to pass to the converter.
    /// Can be overridden by derived classes to provide custom parameters.
    /// </summary>
    /// <returns>The converter parameter, or null if not needed.</returns>
    protected virtual object? CreateConverterParameter() => null;

    /// <summary>
    /// Creates the main binding to use as the primary value in the multi-binding.
    /// Must be implemented by derived classes.
    /// </summary>
    /// <returns>The main binding, or null if not needed.</returns>
    protected abstract BindingBase? CreateBinding();
}
