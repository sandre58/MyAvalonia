// -----------------------------------------------------------------------
// <copyright file="ShowcaseViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Linq;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Factories;
using MyNet.Utilities;

namespace MyNet.Avalonia.Showcase.ViewModels.Playground;

/// <summary>
/// Represents a view model for showcasing a control in the catalog, providing properties and functionality to manage the interactive playground and options catalog for the control. This class serves as a base for specific control showcase view models, encapsulating common logic for initializing the playground and catalog based on provided theme builders. It allows users to explore different themes and configurations for the showcased control through an interactive interface, enhancing the user experience when browsing the control catalog.
/// </summary>
internal abstract class ShowcaseViewModel : PageViewModel
{
    /// <summary>
    /// Gets the playground view model that manages the interactive area where users can experiment with different themes and configurations for the showcased control. This property is initialized in the constructor using the control name and the theme definitions built from the provided ControlThemeBuilder instances. The Playground property allows users to interact with the control's appearance and behavior based on the defined themes, providing a hands-on experience for exploring the various options available for customization.
    /// </summary>
    public PlaygroundViewModel Playground { get; }

    /// <summary>
    /// Gets the options catalog view model that provides a collection of theme definitions for the showcased control. This property is initialized in the constructor using the theme definitions built from the provided ControlThemeBuilder instances. The Catalog property allows access to the available options and configurations for the control being showcased, enabling users to explore and customize its appearance and behavior based on the defined themes.
    /// </summary>
    public ThemesCatalogViewModel Catalog { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShowcaseViewModel"/> class with the specified control name and theme builders. This constructor takes a control name and an array of ControlThemeBuilder instances, which are used to build the definitions for the playground and catalog. The definitions are created by invoking the Build method of each ControlThemeBuilder with the provided control name, resulting in a collection of theme definitions that are then used to initialize the PlaygroundViewModel and OptionsCatalogViewModel properties.
    /// </summary>
    /// <param name="controlName">The name of the control being showcased.</param>
    /// <param name="builders">An array of ControlThemeBuilder instances used to build the theme definitions.</param>
    protected ShowcaseViewModel(string controlName, ControlThemeBuilder[] builders)
    {
        var themes = builders.Select(x => new ControlThemeViewModelFactory(x).Create(controlName)).ToList().ToObservableCollection();
        Playground = new(controlName, themes);
        Catalog = new(themes);
    }

    /// <summary>
    /// Performs cleanup operations when the view model is disposed. This method overrides the base class's Cleanup method to include additional cleanup logic specific to the ShowcaseViewModel. It ensures that any resources associated with the Playground are properly released by calling its Dispose method, in addition to performing any necessary cleanup defined in the base class.
    /// </summary>
    protected override void Cleanup()
    {
        base.Cleanup();

        Playground.Dispose();
        Catalog.Dispose();
    }
}
