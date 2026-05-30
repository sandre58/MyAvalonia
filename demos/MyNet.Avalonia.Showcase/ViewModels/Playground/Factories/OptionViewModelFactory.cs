// -----------------------------------------------------------------------
// <copyright file="OptionViewModelFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Registry;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Options;
using MyNet.Utilities;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Factories;

/// <summary>
/// Represents a factory responsible for creating instances of <see cref="OptionViewModel"/> based on control option definitions and their associated metadata. The <see cref="OptionViewModelFactory"/> utilizes a <see cref="ControlEditorRegistry"/> to resolve the appropriate editor descriptor for a given control option definition, and an <see cref="OptionMetadataRegistry"/> to retrieve any relevant metadata associated with the definition. The factory's primary method, <see cref="Create(IControlOptionDefinition)"/>, takes an instance of <see cref="IControlOptionDefinition"/> as input and returns a corresponding <see cref="OptionViewModel"/> instance that can be used in the theming system's user interface for editing control options. This design allows for a flexible and extensible way to create view models for various types of control options based on their definitions and metadata.
/// </summary>
/// <param name="registry">The registry used to resolve control editor descriptors.</param>
/// <param name="metadataRegistry">The registry used to retrieve option metadata.</param>
internal sealed class OptionViewModelFactory(ControlEditorRegistry registry, OptionMetadataRegistry metadataRegistry)
{
    /// <summary>
    /// Creates a new instance of <see cref="OptionViewModel"/> based on the provided control option definition.
    /// </summary>
    /// <param name="definition">The control option definition used to create the <see cref="OptionViewModel"/>. This parameter cannot be null.</param>
    /// <returns>An <see cref="OptionViewModel"/> instance initialized with the provided control option definition and its associated metadata.</returns>
    public (OptionViewModel Option, string? Group) Create(IControlOptionDefinition definition)
    {
        var metadata = metadataRegistry.Get(definition);
        var group = metadata?.Group;

        foreach (var type in new List<Type?> { metadata?.Metadata?.GetType(), definition.GetType(), (definition as IControlPropertyDefinition)?.TargetType }.NotNull())
        {
            var descriptor = registry.TryResolve(type);

            if (descriptor is not null)
            {
                return (descriptor.Factory(definition, metadata), group);
            }
        }

        throw new InvalidOperationException("No suitable descriptor found.");
    }
}
