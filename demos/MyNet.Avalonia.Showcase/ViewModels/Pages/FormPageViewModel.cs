// -----------------------------------------------------------------------
// <copyright file="FormPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Layout;
using Material.Icons;
using MyNet.Avalonia.Controls.Enums;
using MyNet.Avalonia.Showcase.Resources;
using MyNet.Avalonia.Showcase.ThemeBuilder;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders;
using MyNet.Avalonia.Showcase.ThemeBuilder.Builders.Editors;
using MyNet.Avalonia.Showcase.ViewModels.Playground;
using MyNet.Avalonia.Showcase.ViewModels.Samples;
using MyNet.UI.Commands;

namespace MyNet.Avalonia.Showcase.ViewModels.Pages;

internal sealed class FormPageViewModel(ICommandFactory commands) : ShowcaseViewModel(nameof(Form), commands, [
    new ControlThemeBuilder()
        .AddProperty(Controls.Form.ColumnsProperty, 1, x => x.DisplayName(nameof(SettingsResources.Columns)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(1, 4)))
        .AddProperty(Controls.Form.SpacingProperty, 16d, x => x.DisplayName(nameof(SettingsResources.Spacing)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 48)))
        .AddValueAction((control, value) =>
            {
                var pixels = Convert.ToDouble(value, CultureInfo.CurrentCulture);
                control.SetValue(Controls.Form.LabelWidthProperty, pixels <= 0 ? GridLength.Auto : new(pixels));
            },
            120d,
            x => x.DisplayName(nameof(SettingsResources.LabelWidth)).Of<IntNumericUpDownEditor>(editor => editor.WithRange(0, 240)))
        .AddEnumProperty<Position, ListBoxEditor>(Controls.Form.LabelPositionProperty, Position.Left, x => x.DisplayName(nameof(SettingsResources.Direction)), configureChoice: (position, choice) => choice.WithIcon(position switch
        {
            Position.Left => MaterialIconKind.ArrowLeft,
            Position.Top => MaterialIconKind.ArrowUp,
            Position.Right => MaterialIconKind.ArrowRight,
            Position.Bottom => MaterialIconKind.ArrowDown,
            _ => MaterialIconKind.ArrowLeft
        }))
        .AddEnumProperty<HorizontalAlignment, ListBoxEditor>(Controls.Form.LabelAlignmentProperty, HorizontalAlignment.Left, x => x.DisplayName(nameof(SettingsResources.LabelAlignment)), configureChoice: (alignment, choice) => choice.WithIcon(alignment switch
        {
            HorizontalAlignment.Left => MaterialIconKind.FormatAlignLeft,
            HorizontalAlignment.Center => MaterialIconKind.FormatAlignCenter,
            HorizontalAlignment.Right => MaterialIconKind.FormatAlignRight,
            HorizontalAlignment.Stretch => MaterialIconKind.ArrowExpandHorizontal,
            _ => MaterialIconKind.FormatAlignLeft
        }))
])
{
    /// <inheritdoc/>
    public override MaterialIconKind Icon => MaterialIconKind.FormatLineStyle;

    public FormViewModel Form { get; } = new();
}
