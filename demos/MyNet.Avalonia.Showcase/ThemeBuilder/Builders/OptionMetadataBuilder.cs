// -----------------------------------------------------------------------
// <copyright file="OptionMetadataBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;
using MyNet.Avalonia.Showcase.ThemeBuilder.Registry;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders;

/// <summary>
/// Represents a builder for option metadata, allowing to configure the display name and editor metadata for options in the theme builder. This builder provides methods to set the display name using either a value provider or a resource key, as well as a method to specify the editor metadata by configuring an editor builder. The Build method creates an immutable OptionMetadata instance based on the configured properties in the builder, which can then be used in the theme builder to define control options.
/// </summary>
internal sealed class OptionMetadataBuilder
{
    private readonly ChoiceMetadataRegistry _choiceMetadataRegistry = new();
    private IProvideValue<string>? _displayNameFunc;
    private IEditorMetadata? _metadata;
    private MaterialIconKind? _icon;
    private string? _group;

    /// <summary>
    /// Sets the display name for the option using a value provider that can dynamically provide the display name based on the current context or state. This allows for more flexible and dynamic display names that can change based on user interactions or other factors in the theme builder. The provided value provider will be used to retrieve the display name when needed, allowing for dynamic updates to the display name as the context changes.
    /// </summary>
    /// <param name="displayNameFunc">The value provider that supplies the display name.</param>
    /// <returns>The current instance of <see cref="OptionMetadataBuilder"/> for method chaining.</returns>
    public OptionMetadataBuilder DisplayName(IProvideValue<string> displayNameFunc)
    {
        _displayNameFunc = displayNameFunc;
        return this;
    }

    /// <summary>
    /// Sets the display name for the option using the specified resource key.
    /// </summary>
    /// <param name="resourceKey">The resource key used to retrieve the display name for localization purposes.</param>
    /// <returns>Returns the current instance of the OptionMetadataBuilder to allow for method chaining.</returns>
    public OptionMetadataBuilder DisplayName(string resourceKey)
    {
        _displayNameFunc = new LocalizedString(resourceKey);
        return this;
    }

    /// <summary>
    /// Sets the icon for the choice using the specified object. The icon can be any object that represents a visual element, such as an image or a vector graphic, depending on the implementation of the theme builder and how it handles icons.
    /// </summary>
    /// <param name="icon">The object representing the icon for the choice.</param>
    /// <returns>The current instance of the ChoiceMetadataBuilder, allowing for method chaining.</returns>
    public OptionMetadataBuilder WithIcon(MaterialIconKind icon)
    {
        _icon = icon;
        return this;
    }

    /// <summary>
    /// Specifies the group to which the option belongs. This allows for organizing options into different groups within the theme builder, making it easier for users to navigate and find specific options based on their grouping. By assigning a group to an option, you can create a more structured and user-friendly interface in the theme builder, allowing users to easily locate and configure options based on their categorization.
    /// </summary>
    /// <param name="group">The name of the group to which the option belongs.</param>
    /// <returns>Returns the current instance of the OptionMetadataBuilder to allow for method chaining.</returns>
    public OptionMetadataBuilder Group(string group)
    {
        _group = group;
        return this;
    }

    /// <summary>
    /// Specifies the editor metadata for the option by configuring an editor builder of the specified type. This method allows you to define the editor metadata for the option by providing a configuration action that can be used to customize the editor builder. The editor builder will be used to create the editor metadata, which defines how the option will be edited in the theme builder. By using this method, you can easily configure the editor metadata for the option based on your specific requirements and preferences.
    /// </summary>
    /// <typeparam name="TEditor">The type of the editor builder to be used for configuring the editor metadata.</typeparam>
    /// <param name="configure">An optional action to configure the editor builder.</param>
    /// <returns>Returns the current instance of the OptionMetadataBuilder to allow for method chaining.</returns>
    public OptionMetadataBuilder Of<TEditor>(Action<TEditor>? configure = null)
        where TEditor : IEditorBuilder, new()
    {
        var builder = new TEditor();
        configure?.Invoke(builder);

        _metadata = builder.Build();

        if (builder is IEditorWithChoicesBuilder choicesEditor)
        {
            _choiceMetadataRegistry.Merge(choicesEditor.BuildChoiceMetadata());
        }

        return this;
    }

    /// <summary>
    /// Creates a new instance of the OptionMetadata class using the configured display name function and metadata.
    /// </summary>
    /// <returns>An OptionMetadata instance that encapsulates the display name logic and associated metadata.</returns>
    public OptionMetadata Build() => new(_displayNameFunc, _icon, _metadata, _group);

    /// <summary>
    /// Gets the registry that contains metadata for available choices and their configuration options.
    /// </summary>
    /// <returns>The current instance of the <see cref="ChoiceMetadataRegistry"/> that holds metadata for choices.</returns>
    public ChoiceMetadataRegistry BuildChoiceMetadata() => _choiceMetadataRegistry;
}
