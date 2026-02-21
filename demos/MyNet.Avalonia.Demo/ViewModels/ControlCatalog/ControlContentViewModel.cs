// -----------------------------------------------------------------------
// <copyright file="ControlContentViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData.Binding;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog.ContentProviders;
using MyNet.Observable;
using MyNet.Utilities;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

/// <summary>
/// View model for managing control content settings.
/// </summary>
internal sealed class ControlContentViewModel : ObservableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ControlContentViewModel"/> class.
    /// </summary>
    public ControlContentViewModel() => Disposables.AddRange(AvailableProviders.Select(x => x.WhenAnyPropertyChanged().Subscribe(_ => OnPropertyChanged(nameof(Content)))));

    /// <summary>
    /// Gets the collection of available content providers for the control.
    /// </summary>
    public ReadOnlyCollection<IContentProviderViewModel> AvailableProviders { get; } = new List<IContentProviderViewModel>
    {
        new NoContentProviderViewModel(),
        new IconProviderViewModel(),
        new TextProviderViewModel()
    }.AsReadOnly();

    /// <summary>
    /// Gets or sets the currently selected content provider.
    /// </summary>
    [AlsoNotifyFor(nameof(Content))]
    public ContentProviderType SelectedProviderType { get; set; }

    /// <summary>
    /// Gets the content provided by the currently selected content provider. This property retrieves the content by invoking the ProvideContent method of the selected provider, allowing dynamic content generation based on the user's selection.
    /// </summary>
    public object? Content => AvailableProviders.GetById(SelectedProviderType).ProvideContent();
}
