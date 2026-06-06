// -----------------------------------------------------------------------
// <copyright file="FormPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Layout;
using Material.Icons;
using MyNet.Avalonia.Controls;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Extensions;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Samples;
using MyNet.UI.Commands;
using MyNet.UI.Notifications;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class FormPageViewModel(ICommandFactory commands, INotificationPublisher notificationPublisher) : ShowcaseViewModel(nameof(Form), commands, [
    new ControlThemeBuilder()
        .AddProperty(global::MyNet.Avalonia.Controls.Form.ColumnsProperty, 1, x => x.DisplayName(nameof(SettingsResources.Columns)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(1, 4)))
        .AddProperty(global::MyNet.Avalonia.Controls.Form.SpacingProperty, 16d, x => x.DisplayName(nameof(SettingsResources.Spacing)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 48)))
        .AddEnumProperty<Position, ListBoxEditor>(global::MyNet.Avalonia.Controls.Form.LabelPositionProperty, Position.Left, x => x.DisplayName(nameof(SettingsResources.Direction)))
        .AddEnumProperty<HorizontalAlignment, ListBoxEditor>(global::MyNet.Avalonia.Controls.Form.LabelAlignmentProperty, HorizontalAlignment.Left, x => x.DisplayName(nameof(SettingsResources.LabelAlignment)))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.FormatLineStyle;

    public FormViewModel Form { get; } = new(commands, notificationPublisher);
}
