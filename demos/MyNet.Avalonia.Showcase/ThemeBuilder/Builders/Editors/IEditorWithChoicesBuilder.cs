// -----------------------------------------------------------------------
// <copyright file="IEditorWithChoicesBuilder.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Avalonia.Showcase.ThemeBuilder.Registry;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Defines a builder for creating a registry that provides metadata about selectable choices for an editor component.
/// </summary>
/// <remarks>Implement this interface to supply or update metadata describing the set of available choices in an
/// editor context. The resulting registry enables consumers to query or enumerate choice information, which may be used
/// for populating UI elements or validating input.</remarks>
internal interface IEditorWithChoicesBuilder : IEditorBuilder
{
    /// <summary>
    /// Adds a single choice to the specified list and registers its metadata if provided. This method ensures that duplicate choices are not added to the list.
    /// </summary>
    /// <typeparam name="T">The type of the choice.</typeparam>
    /// <param name="choice">The choice to add.</param>
    /// <param name="configure">An optional action to configure the metadata for the choice.</param>
    /// <returns>The current instance of the ListBoxEditor, enabling method chaining.</returns>
    IEditorWithChoicesBuilder AddChoice<T>(T choice, Action<ChoiceMetadataBuilder>? configure = null);

    /// <summary>
    /// Adds the specified choices to the provided collection, optionally registering associated metadata for each new
    /// choice.
    /// </summary>
    /// <remarks>If a choice already exists in the list, it is not added again and no metadata is registered
    /// for it. Use the configure parameter to customize metadata for each choice before registration.</remarks>
    /// <typeparam name="T">The type of elements contained in the collections.</typeparam>
    /// <param name="choices">The collection of choices to add to the list.</param>
    /// <param name="configure">An optional action that configures metadata for each choice as it is added. If provided, this action is invoked
    /// for each new choice.</param>
    /// <returns>The current instance of the ListBoxEditor, enabling method chaining.</returns>
    IEditorWithChoicesBuilder AddChoices<T>(ICollection<T> choices, Action<T, ChoiceMetadataBuilder>? configure = null);

    /// <summary>
    /// Builds and returns a registry containing metadata for available choices.
    /// </summary>
    /// <remarks>Use this method to initialize or refresh the choice metadata registry, ensuring that the
    /// latest choice information is accessible.</remarks>
    /// <returns>A <see cref="ChoiceMetadataRegistry"/> instance that provides access to metadata describing the available
    /// choices.</returns>
    ChoiceMetadataRegistry BuildChoiceMetadata();
}
