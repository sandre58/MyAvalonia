// -----------------------------------------------------------------------
// <copyright file="OptionMetadata.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

/// <summary>
/// Represents the metadata of an option, which is used to define the display name and editor metadata of an option in a theme builder. The display name is provided by an implementation of the <see cref="IProvideValue{T}"/> interface, allowing for dynamic values to be used. The editor metadata is an optional object that can be used to provide additional information about the editor associated with the option, such as its type or configuration settings.
/// </summary>
/// <param name="DisplayName">The display name of the option.</param>
/// <param name="Icon">An optional icon associated with the option, which can be used for visual representation in the user interface.</param>
/// <param name="Metadata">The editor metadata associated with the option.</param>
/// <param name="Group">An optional group name to which the option belongs, used for organizing options in the user interface.</param>
internal sealed record OptionMetadata(IProvideValue<string> DisplayName, MaterialIconKind? Icon, IEditorMetadata? Metadata, string? Group = null);

/// <summary>
/// Represents metadata for an option, including a display name and associated editor metadata.
/// </summary>
/// <typeparam name="TEditor">Specifies the type of editor metadata associated with the option. Must implement the IEditorMetadata interface.</typeparam>
/// <param name="DisplayName">The display name of the option, provided as a value provider for localization or dynamic evaluation.</param>
/// <param name="Icon">An optional icon associated with the option, which can be used for visual representation in the user interface.</param>
/// <param name="Metadata">The editor metadata associated with the option.</param>
internal sealed record OptionMetadata<TEditor>(IProvideValue<string> DisplayName, MaterialIconKind? Icon, TEditor Metadata)
    where TEditor : IEditorMetadata;
