// -----------------------------------------------------------------------
// <copyright file="ControlContentDefinition.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents the definition of content for a control, including the associated property and the type of content
/// provider used.
/// </summary>
/// <param name="Property">The Avalonia property associated with the control's content definition.</param>
/// <param name="ContentProviderType">The type of content provider that determines how the content is supplied to the control.</param>
internal sealed record ControlContentDefinition(AvaloniaProperty<object?> Property, ContentProviderType ContentProviderType) : ControlPropertyDefinition<object?>(Property);
