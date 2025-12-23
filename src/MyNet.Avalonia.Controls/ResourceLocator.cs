// -----------------------------------------------------------------------
// <copyright file="ResourceLocator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using MyNet.Avalonia.Controls.Primitives;
using MyNet.Avalonia.Controls.Resources;
using MyNet.Avalonia.Converters;
using MyNet.Utilities.Localization;

namespace MyNet.Avalonia.Controls;

/// <summary>
/// Provides initialization for resource localization in Avalonia applications.
/// </summary>
/// <remarks>
/// This class registers resource managers for translation services and ensures that resources
/// are initialized only once, even if called multiple times. It should be called early in the
/// application lifecycle, typically during startup.
/// </remarks>
public static class ResourceLocator
{
    private static bool _isInitialized;

    /// <summary>
    /// Initializes the resource locator and registers all resource managers for translation.
    /// </summary>
    /// <remarks>
    /// This method is idempotent and safe to call multiple times. Only the first call will perform initialization.
    /// It registers common resources such as ColorPickerResources and MessagesResources with the translation service,
    /// initializes the Avalonia resource locator, and registers custom type converters.
    /// </remarks>
    public static void Initialize()
    {
        if (_isInitialized) return;

        // Common Resources
        TranslationService.RegisterResources(nameof(ColorPickerResources), ColorPickerResources.ResourceManager);
        TranslationService.RegisterResources(nameof(MessagesResources), MessagesResources.ResourceManager);

        // Register custom type converters
        RegisterTypeConverters();

        Avalonia.ResourceLocator.Initialize();

        _isInitialized = true;
    }

    private static void RegisterTypeConverters() =>
        StringConverter.RegisterTypeConverter<DateContext>((dateContext, format, _, _, culture) => !string.IsNullOrEmpty(format)
                ? DateTimeConverter.Default.Convert(dateContext.ToDate(), format, culture)?.ToString()
                : dateContext.ToString());
}
