// -----------------------------------------------------------------------
// <copyright file="ListEditor.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;
using MyNet.Avalonia.Showcase.ThemeBuilder.Registry;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;

/// <summary>
/// Provides a builder for configuring and creating a list box editor with customizable choices and selection options.
/// </summary>
/// <remarks>Use this class to fluently specify the available choices and whether multiple selections are allowed
/// when constructing a list box editor. The builder pattern enables chaining configuration methods before generating
/// the final editor metadata.</remarks>
internal abstract class ListEditor<TEditor> : IEditorWithChoicesBuilder
    where TEditor : ListEditor<TEditor>
{
    private readonly List<object?> _choices = [];
    private readonly ChoiceMetadataRegistry _choiceMetadataRegistry = new();

    /// <summary>
    /// Gets the collection of choices that have been added to the list editor. This property returns an array of objects representing the choices available for selection in the list box editor. The choices are stored internally as a list and are exposed as an array to ensure immutability when accessed from outside the builder. This collection is used when building the metadata for the list box editor, providing the necessary information about the options that users can select from in the UI.
    /// </summary>
    protected object?[] Choices => [.. _choices];

    /// <summary>
    /// Adds a single choice to the specified list and registers its metadata if provided. This method ensures that duplicate choices are not added to the list.
    /// </summary>
    /// <typeparam name="T">The type of the choice.</typeparam>
    /// <param name="choice">The choice to add.</param>
    /// <param name="configure">An optional action to configure the metadata for the choice.</param>
    /// <returns>The current instance of the ListBoxEditor, enabling method chaining.</returns>
    public TEditor AddChoice<T>(T choice, Action<ChoiceMetadataBuilder>? configure = null) => AddChoices([choice], configure is not null ? new Action<T, ChoiceMetadataBuilder>((_, builder) => configure(builder)) : null);

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
    public TEditor AddChoices<T>(ICollection<T> choices, Action<T, ChoiceMetadataBuilder>? configure = null)
    {
        foreach (var choice in choices)
        {
            if (!_choices.Contains(choice))
            {
                _choices.Add(choice);
                if (configure is not null)
                {
                    var builder = new ChoiceMetadataBuilder();
                    configure(choice, builder);

                    var metadata = builder.Build();
                    _choiceMetadataRegistry.Register(choice, metadata);
                }
            }
        }

        return (TEditor)this;
    }

    // <inheritdoc />
    IEditorWithChoicesBuilder IEditorWithChoicesBuilder.AddChoice<T>(T choice, Action<ChoiceMetadataBuilder>? configure) => AddChoice(choice, configure);

    // <inheritdoc />
    IEditorWithChoicesBuilder IEditorWithChoicesBuilder.AddChoices<T>(ICollection<T> choices, Action<T, ChoiceMetadataBuilder>? configure) => AddChoices(choices, configure);

    /// <summary>
    /// Builds and returns an instance of <see cref="IEditorMetadata"/> containing the configured choices and selection options for the list box editor.
    /// </summary>
    /// <returns>An instance of <see cref="IEditorMetadata"/> representing the configured list box editor.</returns>
    public abstract IEditorMetadata Build();

    /// <summary>
    /// Gets the registry that contains metadata for available choices and their configuration options.
    /// </summary>
    /// <returns>The current instance of the <see cref="ChoiceMetadataRegistry"/> that holds metadata for choices.</returns>
    public ChoiceMetadataRegistry BuildChoiceMetadata() => _choiceMetadataRegistry;
}
