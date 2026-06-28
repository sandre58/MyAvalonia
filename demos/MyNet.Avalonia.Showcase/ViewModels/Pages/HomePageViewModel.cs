// -----------------------------------------------------------------------
// <copyright file="HomePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Showcase.Composition;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Observable;
using MyNet.UI.Commands;
using MyNet.UI.Navigation;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class HomePageViewModel : PageViewModel
{
    private const int NuGetPackageCount = 6;

    private readonly INavigationClient _navigationClient;

    public HomePageViewModel(INavigationClient navigationClient, ICommandFactory commandFactory)
    {
        _navigationClient = navigationClient;
        OpenLinkCommand = commandFactory.CreateRequired<HomeLinkViewModel>(link => NavigateAsync(link.NavigationKey));
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Home;

    public IReadOnlyList<HomeStatViewModel> Stats { get; } =
    [
        new(MaterialIconKind.ViewDashboardOutline, PagesCatalog.DemoPageCount.ToString(System.Globalization.CultureInfo.InvariantCulture), "StatDemoPagesLabel"),
        new(MaterialIconKind.FolderOutline, PagesCatalog.CategoryGroupCount.ToString(System.Globalization.CultureInfo.InvariantCulture), "StatCategoriesLabel"),
        new(MaterialIconKind.PackageVariantClosed, NuGetPackageCount.ToString(System.Globalization.CultureInfo.InvariantCulture), "StatPackagesLabel"),
        new(MaterialIconKind.TagFaces, "7000+", "StatMaterialIconsLabel")
    ];

    public IReadOnlyList<HomeLinkViewModel> FeaturedLinks { get; } =
    [
        new(MaterialIconKind.Palette, nameof(MenuResources.Theme), "FeaturedThemeDescription", "Theme"),
        new(MaterialIconKind.TagFaces, nameof(MenuResources.Icons), "FeaturedIconsDescription", "Icons"),
        new(MaterialIconKind.FormatLineStyle, "FeaturedFormTitle", "FeaturedFormDescription", "Form"),
        new(MaterialIconKind.MessageAlertOutline, nameof(MenuResources.Feedback), "FeaturedDialogsDescription", "Dialogs")
    ];

    public IReadOnlyList<HomeCapabilityViewModel> Capabilities { get; } =
    [
        new(MaterialIconKind.PaletteOutline, "CapabilityThemeTitle", "CapabilityThemeDescription"),
        new(MaterialIconKind.NavigationVariantOutline, "CapabilityNavigationTitle", "CapabilityNavigationDescription"),
        new(MaterialIconKind.CheckDecagramOutline, "CapabilityValidationTitle", "CapabilityValidationDescription"),
        new(MaterialIconKind.Translate, "CapabilityGlobalizationTitle", "CapabilityGlobalizationDescription"),
        new(MaterialIconKind.BellOutline, "CapabilityFeedbackTitle", "CapabilityFeedbackDescription"),
        new(MaterialIconKind.TuneVariant, "CapabilityPlaygroundTitle", "CapabilityPlaygroundDescription")
    ];

    public IReadOnlyList<HomeLinkViewModel> CategoryLinks { get; } =
    [
        new(MaterialIconKind.FormatText, nameof(MenuResources.Typography), "CategoryTextsDescription", "Typography", 3),
        new(MaterialIconKind.GestureTapButton, nameof(MenuResources.ButtonsAndActions), "CategoryButtonsDescription", "ButtonsAndActions", 6),
        new(MaterialIconKind.CheckboxMarkedOutline, nameof(MenuResources.Selection), "CategorySelectionDescription", "Selection", 4),
        new(MaterialIconKind.FormTextbox, nameof(MenuResources.Inputs), "CategoryInputsDescription", "Inputs", 8),
        new(MaterialIconKind.ViewDashboardOutline, nameof(MenuResources.Layout), "CategoryContainersDescription", "Layout", 10),
        new(MaterialIconKind.Table, nameof(MenuResources.DataAndLists), "CategoryDataDescription", "DataAndLists", 3),
        new(MaterialIconKind.BookOpenPageVariantOutline, nameof(MenuResources.ShellNavigation), "CategoryNavigationDescription", "ShellNavigation", 7),
        new(MaterialIconKind.MessageAlertOutline, nameof(MenuResources.Feedback), "CategoryDialogsDescription", "Feedback", 3),
        new(MaterialIconKind.Shape, nameof(MenuResources.ShapesAndDrawing), "CategoryShapesDescription", "ShapesAndDrawing", 3)
    ];

    public IReadOnlyList<HomeStepViewModel> GettingStartedSteps { get; } =
    [
        new(1, MaterialIconKind.PackageVariant, "GettingStartedStep1"),
        new(2, MaterialIconKind.Palette, "GettingStartedStep2"),
        new(3, MaterialIconKind.CogOutline, "GettingStartedStep3"),
        new(4, MaterialIconKind.NavigationVariant, "GettingStartedStep4"),
        new(5, MaterialIconKind.CheckCircleOutline, "GettingStartedStep5")
    ];

    public ICommand OpenLinkCommand { get; }

    private Task NavigateAsync(string key)
        => key switch
        {
            "Theme" => _navigationClient.NavigateToAsync<ThemePageViewModel>(),
            "Icons" => _navigationClient.NavigateToAsync<IconsPageViewModel>(),
            "Form" => _navigationClient.NavigateToAsync<FormPageViewModel>(),
            "Dialogs" => _navigationClient.NavigateToAsync<DialogPageViewModel>(),
            "Typography" => _navigationClient.NavigateToAsync<LabelPageViewModel>(),
            "ButtonsAndActions" => _navigationClient.NavigateToAsync<ButtonPageViewModel>(),
            "Selection" => _navigationClient.NavigateToAsync<CheckBoxPageViewModel>(),
            "Inputs" => _navigationClient.NavigateToAsync<FieldsPageViewModel>(),
            "Layout" => _navigationClient.NavigateToAsync<ExpanderPageViewModel>(),
            "DataAndLists" => _navigationClient.NavigateToAsync<DataGridPageViewModel>(),
            "ShellNavigation" => _navigationClient.NavigateToAsync<NavigationMenuPageViewModel>(),
            "Feedback" => _navigationClient.NavigateToAsync<DialogPageViewModel>(),
            "ShapesAndDrawing" => _navigationClient.NavigateToAsync<BorderPageViewModel>(),
            _ => Task.CompletedTask
        };
}

/// <summary>Highlight metric displayed on the home page hero.</summary>
internal sealed class HomeStatViewModel(MaterialIconKind icon, string value, string labelResourceKey)
{
    public MaterialIconKind Icon { get; } = icon;

    public string Value { get; } = value;

    public LocalizedString Label { get; } = new(labelResourceKey);
}

/// <summary>Framework capability highlighted on the home page.</summary>
internal sealed class HomeCapabilityViewModel(MaterialIconKind icon, string titleResourceKey, string descriptionResourceKey)
{
    public MaterialIconKind Icon { get; } = icon;

    public LocalizedString Title { get; } = new(titleResourceKey);

    public LocalizedString Description { get; } = new(descriptionResourceKey);
}

/// <summary>Numbered onboarding step on the home page.</summary>
internal sealed class HomeStepViewModel(int step, MaterialIconKind icon, string textResourceKey)
{
    public int Step { get; } = step;

    public MaterialIconKind Icon { get; } = icon;

    public LocalizedString Text { get; } = new(textResourceKey);
}

/// <summary>
/// Describes a navigable card on the home page.
/// </summary>
/// <param name="icon">Material icon for the card.</param>
/// <param name="titleResourceKey">Resource key for the card title.</param>
/// <param name="descriptionResourceKey">Resource key for the card description.</param>
/// <param name="navigationKey">Internal navigation target key.</param>
/// <param name="demoPageCount">Optional number of demos in the category.</param>
internal sealed class HomeLinkViewModel(
    MaterialIconKind icon,
    string titleResourceKey,
    string descriptionResourceKey,
    string navigationKey,
    int demoPageCount = 0)
{
    /// <summary>Gets the card icon.</summary>
    public MaterialIconKind Icon { get; } = icon;

    /// <summary>Gets the localized card title.</summary>
    public LocalizedString Title { get; } = new(titleResourceKey);

    /// <summary>Gets the localized card description.</summary>
    public LocalizedString Description { get; } = new(descriptionResourceKey);

    /// <summary>Gets the navigation target key.</summary>
    internal string NavigationKey { get; } = navigationKey;

    /// <summary>Gets the number of demo pages in the category, when applicable.</summary>
    public int DemoPageCount { get; } = demoPageCount;
}
