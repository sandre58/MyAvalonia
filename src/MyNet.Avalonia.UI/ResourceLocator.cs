// -----------------------------------------------------------------------
// <copyright file="ResourceLocator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.UI.Resources;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.UI;

public static class ResourceLocator
{
    private static bool _isInitialized;

    public static void Initialize()
    {
        if (_isInitialized) return;

        // Common Resources
        TranslationService.RegisterResources(nameof(UiResources), UiResources.ResourceManager);
        TranslationService.RegisterResources(nameof(MessageResources), MessageResources.ResourceManager);
        TranslationService.RegisterResources(nameof(FormatResources), FormatResources.ResourceManager);

        Avalonia.ResourceLocator.Initialize();

        _isInitialized = true;
    }
}
