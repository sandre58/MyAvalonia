// -----------------------------------------------------------------------
// <copyright file="ChoiceViewModelFactory.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Registry;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Choices;
using MyNet.Avalonia.Theme.Classes;
using MyNet.Avalonia.Theme.Theming.Core;
using MyNet.Humanizer.Facade;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.Factories;

/// <summary>
/// Represents a factory for creating choice view models based on the provided metadata registry. This factory is responsible for generating instances of choice view models, such as <see cref="ClassChoiceViewModel"/> and <see cref="RoleChoiceViewModel"/>, by retrieving the relevant metadata from the <see cref="ChoiceMetadataRegistry"/>. The factory uses the metadata to determine the display name and other properties of the choice view models, ensuring that they are properly configured based on the underlying theme classes and roles defined in the theming system.
/// </summary>
/// <param name="metadataRegistry">The metadata registry used to retrieve choice metadata.</param>
internal sealed class ChoiceViewModelFactory(ChoiceMetadataRegistry metadataRegistry)
{
    /// <summary>
    /// Creates a new instance of the ClassChoiceViewModel using the specified CSS class.
    /// </summary>
    /// <remarks>If display name metadata for the specified class is not found, a default display name is
    /// generated from the class name.</remarks>
    /// <param name="class">The CSS class to use when creating the ClassChoiceViewModel. This parameter cannot be null.</param>
    /// <returns>A ClassChoiceViewModel initialized with the provided CSS class and its associated display name metadata.</returns>
    public ClassChoiceViewModel Create(CssClass @class)
    {
        var metadata = metadataRegistry.Get(@class);

        return new(@class, metadata?.DisplayName ?? new LocalizedString(@class), metadata?.Icon);
    }

    /// <summary>
    /// Creates a new instance of the RoleChoiceViewModel for the specified theme role.
    /// </summary>
    /// <remarks>If the display name metadata for the specified role is not found, the display name defaults
    /// to a humanized version of the role.</remarks>
    /// <param name="role">The theme role for which to create the RoleChoiceViewModel. This parameter cannot be null.</param>
    /// <returns>A RoleChoiceViewModel that represents the specified role and its associated display name.</returns>
    public RoleChoiceViewModel Create(ThemeRole role)
    {
        var metadata = metadataRegistry.Get(role);

        return new(role, metadata?.DisplayName ?? new CultureBoundValue<string>(() => role.Humanize()));
    }

    /// <summary>
    /// Creates a new ChoiceViewModel instance that wraps the specified choice and associates it with its display name
    /// metadata.
    /// </summary>
    /// <remarks>If display name metadata for the specified choice is not found, the display name defaults to
    /// the string representation of the choice.</remarks>
    /// <typeparam name="T">The type of the choice to be wrapped by the view model.</typeparam>
    /// <param name="choice">The choice to be represented by the view model. This parameter cannot be null.</param>
    /// <returns>A ChoiceViewModel instance containing the provided choice and its associated display name metadata.</returns>
    public ChoiceViewModel<T> Create<T>(T choice)
    {
        var metadata = metadataRegistry.Get(choice);

        return new(choice, metadata?.DisplayName ?? new CultureBoundValue<string>(choice.ToString), metadata?.Icon);
    }
}
