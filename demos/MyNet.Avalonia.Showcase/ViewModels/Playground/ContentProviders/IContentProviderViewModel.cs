// -----------------------------------------------------------------------
// <copyright file="IContentProviderViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Primitives;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground.ContentProviders;

/// <summary>
/// Interface for content providers that generate content for control previews.
/// </summary>
public interface IContentProviderViewModel : IIdentifiable<ContentProviderType>, INotifyPropertyChanged
{
    /// <summary>
    /// Provides content for the control preview.
    /// </summary>
    /// <returns>The generated content object, or null.</returns>
    object? ProvideContent();
}
