// -----------------------------------------------------------------------
// <copyright file="ChoiceMetadataBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders;

/// <summary>
/// Provides a builder for choice metadata, allowing to configure the display name and other properties of choices in the theme builder.
/// </summary>
internal sealed class ChoiceMetadataBuilder
{
    private IObservableValue<string>? _displayNameFunc;
    private MaterialIconKind? _icon;

    /// <summary>
    /// Sets the display name for the choice using the specified value provider.
    /// </summary>
    /// <remarks>Use this method to assign a dynamic display name that can change based on runtime context or
    /// localization requirements.</remarks>
    /// <param name="displayNameFunc">An object that provides the display name as a string. This provider is invoked to retrieve the display name when
    /// needed.</param>
    /// <returns>The current instance of the ChoiceMetadataBuilder to enable method chaining.</returns>
    public ChoiceMetadataBuilder DisplayName(IObservableValue<string> displayNameFunc)
    {
        _displayNameFunc = displayNameFunc;
        return this;
    }

    /// <summary>
    /// Sets the display name for the choice using the specified value provider.
    /// </summary>
    /// <remarks>Use this method to assign a dynamic display name that can change based on runtime context or
    /// localization requirements.</remarks>
    /// <param name="displayNameFunc">An object that provides the display name as a string. This provider is invoked to retrieve the display name when
    /// needed.</param>
    /// <returns>The current instance of the ChoiceMetadataBuilder to enable method chaining.</returns>
    public ChoiceMetadataBuilder DisplayName(Func<string?> displayNameFunc)
    {
        _displayNameFunc = new CultureBoundValue<string>(displayNameFunc);
        return this;
    }

    /// <summary>
    /// Sets the display name for the choice using the specified resource key.
    /// </summary>
    /// <remarks>This method is typically used to define a user-friendly name for the choice that can be
    /// translated based on the application's localization settings.</remarks>
    /// <param name="resourceKey">The resource key used to retrieve the display name from a localization source.</param>
    /// <returns>The current instance of the ChoiceMetadataBuilder, allowing for method chaining.</returns>
    public ChoiceMetadataBuilder DisplayName(string resourceKey)
    {
        _displayNameFunc = new LocalizedString(resourceKey);
        return this;
    }

    /// <summary>
    /// Sets the icon for the choice using the specified object. The icon can be any object that represents a visual element, such as an image or a vector graphic, depending on the implementation of the theme builder and how it handles icons.
    /// </summary>
    /// <param name="icon">The object representing the icon for the choice.</param>
    /// <returns>The current instance of the ChoiceMetadataBuilder, allowing for method chaining.</returns>
    public ChoiceMetadataBuilder WithIcon(MaterialIconKind icon)
    {
        _icon = icon;
        return this;
    }

    /// <summary>
    /// Builds and returns an instance of ChoiceMetadata based on the configured properties in the builder. This method should be called after setting the desired properties to create an immutable ChoiceMetadata object that can be used in the theme builder.
    /// </summary>
    /// <returns>An instance of <see cref="ChoiceMetadata"/> containing the configured properties.</returns>
    public ChoiceMetadata Build() => new(_displayNameFunc, _icon);
}
