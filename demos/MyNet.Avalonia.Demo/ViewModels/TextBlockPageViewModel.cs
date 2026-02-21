// -----------------------------------------------------------------------
// <copyright file="TextBlockPageViewModel.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using MyNet.Avalonia.Demo.ViewModels.ControlCatalog;

namespace MyNet.Avalonia.Demo.ViewModels;

internal sealed class TextBlockPageViewModel() : ControlCatalogViewModel(nameof(TextBlock),
[
    new ControlThemeBuilder()
        .AddVariants("opacity-high", "opacity-medium", "opacity-low", "variant-underline", "variant-delete", "is-disablable")
        .AddDefaultRoles()
        .AddSizes("font-sub-caption", "font-caption", "h6", "h5", "h4", "h3", "h2", "h1")
]);
