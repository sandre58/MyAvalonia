// -----------------------------------------------------------------------
// <copyright file="PagesGroupViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using DynamicData;
using Material.Icons;
using MyNet.Observable;
using MyNet.Observable.Attributes;
using MyNet.Observable.Translatables;

namespace MyNet.Avalonia.Showcase.ViewModels.Base;

/// <summary>
/// Represents a view model for a group of pages in the application, providing properties for the group's title and icon. The title is localized based on a resource key provided during instantiation, allowing for dynamic localization support. The icon is specified using the <see cref="MaterialIconKind"/> enumeration, enabling visual representation of the group in user interfaces. This class inherits from <see cref="LocalizableObject"/>, ensuring that it can respond to culture changes and update its properties accordingly.
/// </summary>
internal sealed class PagesGroupViewModel : LocalizableObject, IMenuItemViewModel
{
    [SuppressMessage("Usage", "CA2213:disposable fields should be disposed", Justification = "The field is properly cleaned up in the Cleanup method.")]
    private readonly StringTranslatable _title;
    private readonly ObservableCollection<PageViewModel> _pages = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="PagesGroupViewModel"/> class with the specified resource key for the title and icon. The resource key is used to create a localized title for the group, while the icon is specified using the <see cref="MaterialIconKind"/> enumeration. Ensure that the resource key provided corresponds to a valid entry in the localization resources to avoid null values for the title.
    /// </summary>
    /// <param name="resourceKey">Resource key used for the localized title of the group.</param>
    /// <param name="icon">Icon representing the group, specified as a <see cref="MaterialIconKind"/>.</param>
    public PagesGroupViewModel(string resourceKey, MaterialIconKind icon)
    {
        _title = new(resourceKey);
        Icon = icon;
        Pages = new(_pages);
    }

    /// <summary>
    /// Gets the title of the group, which is a localized string based on the provided resource key. The title is generated using the <see cref="StringTranslatable"/> class, allowing for dynamic localization support. Ensure that the resource key provided in the constructor corresponds to a valid entry in the localization resources to avoid null values.
    /// </summary>
    [UpdateOnCultureChanged]
    public string? Title => _title.Value;

    /// <summary>
    /// Gets the icon data associated with the current instance.
    /// </summary>
    /// <remarks>The icon data can be used to visually represent the instance in user interfaces. Ensure that
    /// the icon is properly initialized before accessing this property.</remarks>
    public MaterialIconKind Icon { get; }

    /// <summary>
    /// Gets a value indicating whether the menu item represents a group of items rather than a single actionable item. This property can be used to differentiate between menu items that serve as containers for other items (groups) and those that represent individual actions or pages. When this property is true, it indicates that the menu item is a group, which may contain child items that can be displayed in a nested manner in user interfaces.
    /// </summary>
    public bool IsGroup { get; } = true;

    /// <summary>
    /// Gets a read-only collection of pages that belong to this group. The collection is initialized as an empty observable collection, allowing for dynamic updates to the list of pages while ensuring that external code cannot modify the collection directly. To add or remove pages, use the underlying <see cref="_pages"/> collection, which is exposed as a read-only collection through this property.
    /// </summary>
    public ReadOnlyObservableCollection<PageViewModel> Pages { get; }

    /// <summary>
    /// Adds one or more pages to the group by adding them to the underlying observable collection. This method allows for dynamic updates to the list of pages associated with the group, and any changes will be reflected in the read-only collection exposed through the <see cref="Pages"/> property. Ensure that the pages being added are properly initialized and valid before calling this method to maintain the integrity of the group's page collection.
    /// </summary>
    /// <param name="pages">New pages.</param>
    public void AddPages(params PageViewModel[] pages) => _pages.AddRange(pages);

    /// <summary>
    /// Performs cleanup of resources used by the view model, including disposing of the title translatable object. This method is called when the view model is being disposed, allowing for proper resource management and preventing memory leaks. Ensure that any additional disposable resources are also cleaned up in this method as needed.
    /// </summary>
    protected override void Cleanup()
    {
        base.Cleanup();

        _title.Dispose();
    }
}
