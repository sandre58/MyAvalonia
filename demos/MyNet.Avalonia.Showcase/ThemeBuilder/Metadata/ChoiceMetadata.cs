// -----------------------------------------------------------------------
// <copyright file="ChoiceMetadata.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Observable;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Metadata;

/// <summary>
/// Represents the metadata of a choice, which is used to define the display name and icon of a choice in a theme builder. The display name is provided by an implementation of the <see cref="IProvideValue{T}"/> interface, which allows for dynamic values to be used. The icon is an optional object that can be used to represent the choice visually.
/// </summary>
/// <param name="DisplayName">The display name of the choice, provided as an <see cref="IProvideValue{T}"/> to support dynamic and localized values.</param>
/// <param name="Icon">The icon representing the choice, which is optional.</param>
internal sealed record ChoiceMetadata(IProvideValue<string> DisplayName, MaterialIconKind? Icon = null);
