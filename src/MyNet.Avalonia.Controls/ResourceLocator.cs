// -----------------------------------------------------------------------
// <copyright file="ResourceLocator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls.Resources;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.Controls;

public static class ResourceLocator
{
    private static bool _isInitialized;

    public static void Initialize()
    {
        if (_isInitialized) return;

        // Common Resources
        TranslationService.RegisterResources(nameof(ColorPickerResources), ColorPickerResources.ResourceManager);

        Avalonia.ResourceLocator.Initialize();

        _isInitialized = true;
    }
}
