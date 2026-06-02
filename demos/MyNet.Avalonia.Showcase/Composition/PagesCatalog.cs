// -----------------------------------------------------------------------
// <copyright file="PagesCatalog.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Material.Icons;
using MyNet.Avalonia.Showcase.Pages;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ViewModels.Base;
using MyNet.Avalonia.Showcase.ViewModels.Navigation;
using MyNet.Avalonia.Showcase.ViewModels.Pages;

namespace MyNet.Avalonia.Showcase.Composition;

/// <summary>
/// Explicit registry of showcase demo pages and menu groups.
/// </summary>
internal static class PagesCatalog
{
    /// <summary>Returns all page providers for the showcase navigation menu.</summary>
    public static IReadOnlyList<IPagesProvider> GetProviders()
        =>
        [
            new PageAssociation(typeof(HomePageViewModel), typeof(HomePage)),
            new PageAssociation(typeof(ThemePageViewModel), typeof(ThemePage)),
            new PageAssociation(typeof(IconsPageViewModel), typeof(IconsPage)),

            new PagesGroup(nameof(MenuResources.Texts), MaterialIconKind.FormatText, [
                new(typeof(LabelPageViewModel), typeof(LabelPage)),
                new(typeof(SelectableTextBlockPageViewModel), typeof(SelectableTextBlockPage)),
                new(typeof(TextBlockPageViewModel), typeof(TextBlockPage))
            ]),

            new PagesGroup(nameof(MenuResources.Buttons), MaterialIconKind.GestureTapButton, [
                new(typeof(ButtonPageViewModel), typeof(ButtonPage)),
                new(typeof(ButtonSpinnerPageViewModel), typeof(ButtonSpinnerPage)),
                new(typeof(CheckBoxPageViewModel), typeof(CheckBoxPage)),
                new(typeof(DropDownButtonPageViewModel), typeof(DropDownButtonPage)),
                new(typeof(HyperLinkButtonPageViewModel), typeof(HyperLinkButtonPage)),
                new(typeof(RadioButtonPageViewModel), typeof(RadioButtonPage)),
                new(typeof(SplitButtonPageViewModel), typeof(SplitButtonPage)),
                new(typeof(ToggleButtonPageViewModel), typeof(ToggleButtonPage)),
                new(typeof(ToggleSplitButtonPageViewModel), typeof(ToggleSplitButtonPage)),
                new(typeof(ToggleSwitchPageViewModel), typeof(ToggleSwitchPage))
            ]),

            new PagesGroup(nameof(MenuResources.Inputs), MaterialIconKind.FormTextbox, [
                new(typeof(ColorViewPageViewModel), typeof(ColorViewPage)),
                new(typeof(CalendarPageViewModel), typeof(CalendarPage)),
                new(typeof(ClockPageViewModel), typeof(ClockPage)),
                new(typeof(ClockSelectorPageViewModel), typeof(ClockSelectorPage)),
                new(typeof(FieldsPageViewModel), typeof(FieldsPage)),
                new(typeof(SliderPageViewModel), typeof(SliderPage)),
                new(typeof(TimeViewPageViewModel), typeof(TimeViewPage))
            ]),

            new PagesGroup(nameof(MenuResources.Containers), MaterialIconKind.ViewCarousel, [
                new(typeof(AvatarPageViewModel), typeof(AvatarPage)),
                new(typeof(BadgePageViewModel), typeof(BadgePage)),
                new(typeof(BannerPageViewModel), typeof(BannerPage)),
                new(typeof(CarouselPageViewModel), typeof(CarouselPage)),
                new(typeof(ExpanderPageViewModel), typeof(ExpanderPage)),
                new(typeof(FormPageViewModel), typeof(FormPage)),
                new(typeof(GridSplitterPageViewModel), typeof(GridSplitterPage)),
                new(typeof(HeaderedContentControlPageViewModel), typeof(HeaderedContentControlPage)),
                new(typeof(SplitViewPageViewModel), typeof(SplitViewPage)),
                new(typeof(TabControlPageViewModel), typeof(TabControlPage))
            ]),

            new PagesGroup(nameof(MenuResources.DataAndLists), MaterialIconKind.Table, [
                new(typeof(DataGridPageViewModel), typeof(DataGridPage)),
                new(typeof(ListBoxPageViewModel), typeof(ListBoxPage)),
                new(typeof(TreeViewPageViewModel), typeof(TreeViewPage))
            ]),

            new PagesGroup(nameof(MenuResources.Navigation), MaterialIconKind.BookOpenPageVariantOutline, [
                new(typeof(ContentPagePageViewModel), typeof(ContentPagePage)),
                new(typeof(CarouselPagePageViewModel), typeof(CarouselPagePage)),
                new(typeof(DrawerPagePageViewModel), typeof(DrawerPagePage)),
                new(typeof(MenuPageViewModel), typeof(MenuPage)),
                new(typeof(NavigationMenuPageViewModel), typeof(NavigationMenuPage)),
                new(typeof(PaginationPageViewModel), typeof(PaginationPage)),
                new(typeof(TabbedPagePageViewModel), typeof(TabbedPagePage))
            ]),

            new PagesGroup(nameof(MenuResources.DialogsAndFeeback), MaterialIconKind.MessageAlertOutline, [
                new(typeof(DialogPageViewModel), typeof(DialogPage)),
                new(typeof(NotificationPageViewModel), typeof(NotificationPage)),
                new(typeof(ProgressBarPageViewModel), typeof(ProgressBarPage))
            ]),

            new PagesGroup(nameof(MenuResources.ShapesAndVisuals), MaterialIconKind.Shape, [
                new(typeof(BorderPageViewModel), typeof(BorderPage)),
                new(typeof(EllipsePageViewModel), typeof(EllipsePage)),
                new(typeof(ExtendedIconPageViewModel), typeof(ExtendedIconPage))
            ])
        ];

    public static IMenuItemViewModel CreateMenuItem(IPagesProvider pagesProvider, IServiceProvider services)
    {
        switch (pagesProvider)
        {
            case PageAssociation pageAssociation:
                return new LazyPageMenuItem(pageAssociation.ViewModelType, services);

            case PagesGroup pagesGroup:
                var group = new PagesGroupViewModel(pagesGroup.ResourceKey, pagesGroup.Icon);
                group.AddPages(pagesGroup.Associations.Select(x => x.ViewModelType), services);
                return group;

            default:
                throw new ArgumentOutOfRangeException(nameof(pagesProvider), pagesProvider, null);
        }
    }

    internal interface IPagesProvider
    {
        IEnumerable<PageAssociation> GetPageAssociations();
    }

    internal sealed record PagesGroup(string? ResourceKey, MaterialIconKind Icon, IList<PageAssociation> Associations)
        : IPagesProvider
    {
        public IEnumerable<PageAssociation> GetPageAssociations() => Associations;
    }

    internal sealed record PageAssociation(Type ViewModelType, Type ViewType) : IPagesProvider
    {
        public IEnumerable<PageAssociation> GetPageAssociations() => [this];
    }
}
