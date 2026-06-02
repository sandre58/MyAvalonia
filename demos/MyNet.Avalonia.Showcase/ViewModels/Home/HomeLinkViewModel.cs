// -----------------------------------------------------------------------
// <copyright file="HomeLinkViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;

namespace MyNet.Avalonia.Showcase.ViewModels.Home;

/// <summary>
/// Describes a navigable card on the home page.
/// </summary>
/// <param name="icon">Material icon for the card.</param>
/// <param name="titleResourceKey">Resource key for the card title.</param>
/// <param name="descriptionResourceKey">Resource key for the card description.</param>
/// <param name="navigationKey">Internal navigation target key.</param>
internal sealed class HomeLinkViewModel(
    MaterialIconKind icon,
    string titleResourceKey,
    string descriptionResourceKey,
    string navigationKey)
{
    /// <summary>Gets the card icon.</summary>
    public MaterialIconKind Icon { get; } = icon;

    /// <summary>Gets the resource key for the card title (bind with <c>{my:Display TitleResourceKey}</c>).</summary>
    public string TitleResourceKey { get; } = titleResourceKey;

    /// <summary>Gets the resource key for the card description (bind with <c>{my:Display DescriptionResourceKey}</c>).</summary>
    public string DescriptionResourceKey { get; } = descriptionResourceKey;

    /// <summary>Gets the navigation target key.</summary>
    internal string NavigationKey { get; } = navigationKey;
}
