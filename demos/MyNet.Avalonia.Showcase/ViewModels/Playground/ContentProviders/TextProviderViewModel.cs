// -----------------------------------------------------------------------
// <copyright file="TextProviderViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.ContentProviders;

/// <summary>
/// Content provider that generates text content.
/// </summary>
public class TextProviderViewModel : ObservableObject, IContentProviderViewModel
{
    /// <summary>
    /// Gets the identifier for this content provider, which is used to identify the type of content being provided. In this case, it returns ContentProviderType.Text to indicate that text content should be displayed in the control preview.
    /// </summary>
    public ContentProviderType Id => ContentProviderType.Text;

    /// <summary>
    /// Gets or sets the text content to provide.
    /// </summary>
    public string Text { get; set; } = "Preview";

    /// <summary>
    /// Provides the text content.
    /// </summary>
    /// <returns>The text content.</returns>
    public object ProvideContent() => Text;
}
