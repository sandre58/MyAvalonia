// -----------------------------------------------------------------------
// <copyright file="IProvideStyleProperty.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Rendering;

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

internal interface IProvideStyleProperty
{
    StyleProperty? ProvideStyleProperty(object? value);
}
