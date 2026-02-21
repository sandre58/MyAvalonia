// -----------------------------------------------------------------------
// <copyright file="FieldsPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using DynamicData.Binding;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;
using MyNet.Utilities;
using MyNet.Utilities.DateTimes;
using PropertyChanged;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class FieldsPageViewModel : ControlCatalogViewModel
{
    public FieldsPageViewModel()
    : base("Fields", [
        new ControlThemeBuilder()
            .AddShapes("shape-circle")
            .AddVariants("variant-solid", "variant-outlined")
            .AddThemeRoles()
            .AddDefaultSizes(),
        new ControlThemeBuilder("Underline")
            .AddThemeRoles()
            .AddDefaultSizes()
    ]) => Disposables.AddRange(
        [
            this.WhenPropertyChanged(x => x.IsPassword).Subscribe(_ => PasswordChar = IsPassword ? '*' : '\0'),
            Playground.Appearance.WhenPropertyChanged(x => x.SelectedTheme).Subscribe(_ => Playground.AddOrRemoveCustomClass("is-underline", Playground.Appearance.GetActiveThemeDefinition()?.DisplayName == "Underline")),
            this.WhenPropertyChanged(x => x.ShowPlaceholderText).Subscribe(_ => Playground.AddOrRemoveCustomClass("has-watermark", ShowPlaceholderText))
        ]);

    public bool ShowPlaceholderText { get; set; } = true;

    public bool IsPassword { get; set; }

    public char PasswordChar { get; private set; }

    public string TimeMode => TimeFormat == Utilities.DateTimes.TimeFormat.TwentyFourHour ? "24HourClock" : "12HourClock";

    [AlsoNotifyFor(nameof(TimeMode))]
    public TimeFormat? TimeFormat { get; set; } = Utilities.DateTimes.TimeFormat.TwentyFourHour;
}
