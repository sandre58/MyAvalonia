// -----------------------------------------------------------------------
// <copyright file="IconProviderViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Material.Icons;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.ContentProviders;

/// <summary>
/// Content provider that generates random icon content.
/// </summary>
public class IconProviderViewModel : ObservableObject, IContentProviderViewModel
{
    /// <summary>
    /// Gets the identifier for this content provider, which is used to identify the type of content being provided. In this case, it returns ContentProviderType.Icon to indicate that icon content should be displayed in the control preview.
    /// </summary>
    public ContentProviderType Id => ContentProviderType.Icon;

    /// <summary>
    /// Gets or sets the icon data to provide. This property can be used to specify a specific icon, but it is not required for the random icon generation.
    /// </summary>
    public MaterialIconKind? Kind
    {
        get;
        set => SetProperty(ref field, value);
    }

    /// <summary>
    /// Provides a randomly generated icon.
    /// </summary>
    /// <returns>A random icon object.</returns>
    public object ProvideContent() => Kind ?? RandomGenerator.Current.Enum<MaterialIconKind>();
}
