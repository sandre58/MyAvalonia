// -----------------------------------------------------------------------
// <copyright file="ControlEditorRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Options;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Registry;

/// <summary>
/// Represents a registry for control editors, allowing the registration and resolution of editor descriptors based on control option definitions or editor metadata types.
/// </summary>
internal sealed class ControlEditorRegistry
{
    private readonly Dictionary<Type, ControlEditorDescriptor> _typeMappings = [];

    /// <summary>
    /// Registers a control editor descriptor for a specific type. This allows the registry to resolve the appropriate editor descriptor when requested based on the type of control option definition or editor metadata.
    /// </summary>
    /// <typeparam name="T">The type for which the editor descriptor is being registered.</typeparam>
    /// <param name="createOptionViewModel">The control editor descriptor to register.</param>
    public void Register<T>(Func<IControlOptionDefinition, OptionMetadata, OptionViewModel> createOptionViewModel) => _typeMappings[typeof(T)] = new(createOptionViewModel);

    /// <summary>
    /// Registers a factory method for creating an option view model for a specific control option definition type.
    /// </summary>
    /// <remarks>Use this method to associate a custom view model creation function with a particular control
    /// option definition type. This enables dynamic and extensible configuration of how option view models are
    /// constructed for different control types.</remarks>
    /// <typeparam name="TDefinition">The type of control option definition to register. Must implement the IControlOptionDefinition interface.</typeparam>
    /// <param name="createOptionViewModel">A function that creates an OptionViewModel instance for the specified definition and associated metadata. Cannot
    /// be null.</param>
    public void RegisterDefinition<TDefinition>(Func<TDefinition, OptionMetadata, OptionViewModel> createOptionViewModel)
        where TDefinition : IControlOptionDefinition => _typeMappings[typeof(TDefinition)] = new((definition, metadata) => createOptionViewModel((TDefinition)definition, metadata));

    /// <summary>
    /// Registers an editor type for a control option, enabling the creation of a custom setting view model for that
    /// editor.
    /// </summary>
    /// <remarks>Use this method to map a specific editor metadata type to a custom view model factory. This
    /// allows dynamic generation of setting view models tailored to the editor's metadata when configuring control
    /// options.</remarks>
    /// <typeparam name="TEditor">The type of editor metadata to associate with the control option. Must derive from EditorMetadata.</typeparam>
    /// <param name="createOptionViewModel">A function that creates an OptionViewModel instance using the provided control option definition and editor
    /// metadata.</param>
    public void RegisterEditor<TEditor>(Func<IControlOptionDefinition, OptionMetadata<TEditor>, OptionViewModel> createOptionViewModel)
        where TEditor : IEditorMetadata => _typeMappings[typeof(TEditor)] = new((definition, metadata) => createOptionViewModel(definition, new(metadata.DisplayName, metadata.Icon, (TEditor)metadata.Metadata!)));

    /// <summary>
    /// Resolves and returns the editor descriptor associated with the specified type.
    /// </summary>
    /// <remarks>This method relies on a mapping between types and their corresponding editor descriptors.
    /// Ensure that the type has been registered before calling this method to avoid exceptions.</remarks>
    /// <param name="type">The type for which to retrieve the associated editor descriptor. Cannot be null.</param>
    /// <returns>The editor descriptor registered for the specified type.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no editor descriptor is registered for the specified type.</exception>
    public ControlEditorDescriptor Resolve(Type type) => _typeMappings.TryGetValue(type, out var descriptor)
            ? descriptor
            : throw new InvalidOperationException($"No editor registered for {type.Name}");

    /// <summary>
    /// Resolves and returns the editor descriptor associated with the specified type.
    /// </summary>
    /// <remarks>This method relies on a mapping between types and their corresponding editor descriptors.
    /// Ensure that the type has been registered before calling this method to avoid exceptions.</remarks>
    /// <param name="type">The type for which to retrieve the associated editor descriptor. Cannot be null.</param>
    /// <returns>The editor descriptor registered for the specified type.</returns>
    /// <exception cref="InvalidOperationException">Thrown if no editor descriptor is registered for the specified type.</exception>
    public ControlEditorDescriptor? TryResolve(Type type) => _typeMappings.GetValueOrDefault(type);
}

/// <summary>
/// Encapsulates a factory method for creating instances of option view models used by control editors.
/// </summary>
/// <param name="Factory">The factory method that creates an OptionViewModel instance based on the specified control option definition and
/// option metadata. This parameter cannot be null.</param>
internal sealed record ControlEditorDescriptor(Func<IControlOptionDefinition, OptionMetadata, OptionViewModel> Factory);
