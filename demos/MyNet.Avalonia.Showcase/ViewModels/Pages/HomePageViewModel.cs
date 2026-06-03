// -----------------------------------------------------------------------
// <copyright file="HomePageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Material.Icons;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Observable;
using MyNet.UI.Commands;
using MyNet.UI.Navigation;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class HomePageViewModel : PageViewModel
{
    private readonly INavigationClient _navigationClient;

    public HomePageViewModel(INavigationClient navigationClient, ICommandFactory commandFactory)
    {
        _navigationClient = navigationClient;
        OpenLinkCommand = commandFactory.CreateRequired<HomeLinkViewModel>(link => NavigateAsync(link.NavigationKey));
    }

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.Home;

    public IReadOnlyList<HomeLinkViewModel> FeaturedLinks { get; } =
    [
        new(MaterialIconKind.Palette, nameof(MenuResources.Theme), "FeaturedThemeDescription", "Theme"),
        new(MaterialIconKind.TagFaces, nameof(MenuResources.Icons), "FeaturedIconsDescription", "Icons"),
        new(MaterialIconKind.FormatLineStyle, "FeaturedFormTitle", "FeaturedFormDescription", "Form"),
        new(MaterialIconKind.MessageAlertOutline, nameof(MenuResources.Feedback), "FeaturedDialogsDescription", "Dialogs")
    ];

    public IReadOnlyList<HomeLinkViewModel> CategoryLinks { get; } =
    [
        new(MaterialIconKind.FormatText, nameof(MenuResources.Typography), "CategoryTextsDescription", "Typography"),
        new(MaterialIconKind.GestureTapButton, nameof(MenuResources.ButtonsAndActions), "CategoryButtonsDescription", "ButtonsAndActions"),
        new(MaterialIconKind.CheckboxMarkedOutline, nameof(MenuResources.Selection), "CategorySelectionDescription", "Selection"),
        new(MaterialIconKind.FormTextbox, nameof(MenuResources.Inputs), "CategoryInputsDescription", "Inputs"),
        new(MaterialIconKind.ViewDashboardOutline, nameof(MenuResources.Layout), "CategoryContainersDescription", "Layout"),
        new(MaterialIconKind.Table, nameof(MenuResources.DataAndLists), "CategoryDataDescription", "DataAndLists"),
        new(MaterialIconKind.BookOpenPageVariantOutline, nameof(MenuResources.ShellNavigation), "CategoryNavigationDescription", "ShellNavigation"),
        new(MaterialIconKind.MessageAlertOutline, nameof(MenuResources.Feedback), "CategoryDialogsDescription", "Feedback"),
        new(MaterialIconKind.Shape, nameof(MenuResources.ShapesAndDrawing), "CategoryShapesDescription", "ShapesAndDrawing")
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

    /// <summary>Gets the localized card title.</summary>
    public LocalizedString Title { get; } = new(titleResourceKey);

    /// <summary>Gets the localized card description.</summary>
    public LocalizedString Description { get; } = new(descriptionResourceKey);

    /// <summary>Gets the navigation target key.</summary>
    internal string NavigationKey { get; } = navigationKey;
}
