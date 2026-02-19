// -----------------------------------------------------------------------
// <copyright file="NoContentProviderViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Observable;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;

/// <summary>
/// Content provider that generates random icon content.
/// </summary>
public class NoContentProviderViewModel : ObservableObject, IContentProviderViewModel
{
    /// <summary>
    /// Gets the identifier for this content provider, which is used to identify the type of content being provided. In this case, it returns ContentProviderType.None to indicate that no content should be displayed in the control preview.
    /// </summary>
    public ContentProviderType Id => ContentProviderType.None;

    /// <summary>
    /// Provides a null content, indicating that no content should be displayed in the control preview.
    /// </summary>
    /// <returns>A random icon object.</returns>
    public object? ProvideContent() => null;
}
