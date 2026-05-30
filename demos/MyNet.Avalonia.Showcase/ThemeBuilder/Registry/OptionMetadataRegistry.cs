// -----------------------------------------------------------------------
// <copyright file="OptionMetadataRegistry.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Registry;

/// <summary>
/// Represents a registry for option metadata, which allows to associate metadata information with specific control option definitions in the context of a theme builder. This registry enables the retrieval of metadata for control options, which can include display names and other relevant information, to enhance the user experience when configuring themes. The registry is designed to be flexible and can be extended to support various types of control option definitions and their associated metadata as needed in the theme builder application.
/// </summary>
internal sealed class OptionMetadataRegistry : MetadataRegistry<IControlOptionDefinition, OptionMetadata>;
