// -----------------------------------------------------------------------
// <copyright file="CardPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Windows.Input;
using Avalonia.Controls;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Playground.Options;
using MyNet.Avalonia.Theme.Assists;
using MyNet.Avalonia.Theme.Classes;
using MyNet.UI.Commands;
using MyNet.UI.Notifications;
using MyNet.UI.Notifications.Models;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class CardPageViewModel(ICommandFactory commands, INotificationPublisher notificationPublisher) : ShowcaseViewModel(nameof(Card), commands, [
    new ControlThemeBuilder()
        .WithContent(Card.TitleProperty, ContentProviderType.Text)
        .WithIcon(RegionControl.LeadingProperty, true)
        .AddAllVariants()
        .AddVariants(CssClass.ShadowSurface, CssClass.ShadowControl)
        .AddVariant(new("is-hover"))
        .AddAllRoles()
        .AddDefaultSizes()
        .AddEnumProperty<CardLayout, ListBoxEditor>(Card.CardLayoutProperty, configure: x => x.DisplayName(nameof(SettingsResources.Layout)))
        .AddProperty(Card.SubtitleProperty, configure: x => x.DisplayName(nameof(CardPageResources.Subtitle)).Of<TextBoxEditor>(y => y.WithValue("Subtitle with a very long text must be wrapped").WithRandomizeText(RandomizeText.Sentence)))
        .AddProperty(RegionControl.HeaderProperty, configure: x => x.DisplayName(nameof(CardPageResources.Header)).Of<TextBoxEditor>())
        .AddProperty(ContentControl.ContentProperty, configure: x => x.DisplayName(nameof(SettingsResources.Content)).Of<TextBoxEditor>())
        .AddProperty(RegionControl.ActionsProperty, configure: x => x.DisplayName(nameof(CardPageResources.Footer)).Of<TextBoxEditor>())
        .AddProperty(TrailingAssist.IsVisibleProperty, configure: x => x.DisplayName(nameof(CardPageResources.ShowTrailing)).Of<ToggleSwitchEditor>()),

    new ControlThemeBuilder("Interactive")
        .WithContent(Card.TitleProperty, ContentProviderType.Text)
        .WithIcon(RegionControl.LeadingProperty, true)
        .AddAllVariants()
        .AddAllRoles()
        .AddDefaultSizes()
        .AddEnumProperty<CardLayout, ListBoxEditor>(Card.CardLayoutProperty, configure: x => x.DisplayName(nameof(SettingsResources.Layout)))
        .AddProperty(Card.SubtitleProperty, configure: x => x.DisplayName(nameof(CardPageResources.Subtitle)).Of<TextBoxEditor>(y => y.WithValue("Subtitle with a very long text must be wrapped").WithRandomizeText(RandomizeText.Sentence)))
        .AddProperty(RegionControl.HeaderProperty, configure: x => x.DisplayName(nameof(CardPageResources.Header)).Of<TextBoxEditor>())
        .AddProperty(ContentControl.ContentProperty, configure: x => x.DisplayName(nameof(SettingsResources.Content)).Of<TextBoxEditor>())
        .AddProperty(RegionControl.ActionsProperty, configure: x => x.DisplayName(nameof(CardPageResources.Footer)).Of<TextBoxEditor>())
        .AddProperty(TrailingAssist.IsVisibleProperty, defaultValue: true, configure: x => x.DisplayName(nameof(CardPageResources.ShowTrailing)).Of<ToggleSwitchEditor>())
])
{
    public ICommand SampleCommand { get; } = commands.Create(() => notificationPublisher.Publish(new MessageNotification(CardPageResources.ClickTitle, CardPageResources.ClickMessage, NotificationSeverity.Success)));

    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.CardOutline;
}
