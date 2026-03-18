// -----------------------------------------------------------------------
// <copyright file="ColorResourcesLocator.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using Avalonia.Styling;
using MyNet.Avalonia.Helpers;
using MyNet.Utilities;
using MyNet.Utilities.Localization;
using MyNet.Utilities.Logging;

namespace MyNet.Avalonia.Resources;

/// <summary>
/// Resources locator for color resources defined in the ColorResources resource manager. This static class provides a centralized location for accessing color resources based on their Color representation. It maintains a dictionary that maps Color objects to their corresponding resource keys, which is populated whenever the application's culture changes to ensure that the correct localized color resources are available. This allows for easy retrieval of color resources throughout the application, facilitating dynamic theming and localization.
/// </summary>
public static class ColorResourcesLocator
{
    private static bool _isInitialized;
    private static Dictionary<Color, string> _colorResourcesDictionary = [];

    /// <summary>
    /// Initializes application-wide translation and globalization services. This method ensures that required resources
    /// and event handlers are set up before localization features are used.
    /// </summary>
    /// <remarks>Call this method once during application startup to register translation resources,
    /// initialize the humanizer, and subscribe to culture change events. Subsequent calls have no effect.</remarks>
    public static void Initialize()
    {
        if (_isInitialized) return;

        // Common Resources
        TranslationService.RegisterResources(nameof(ColorResources), ColorResources.ResourceManager);

        GlobalizationService.Current.CultureChanged += OnCultureChanged;
        _isInitialized = true;
    }

    /// <summary>
    /// Returns the Color associated with the specified color name.
    /// </summary>
    /// <remarks>This method searches a predefined dictionary of color names and their corresponding Color
    /// values. If the provided name does not match any entry, the method returns the default Color value.</remarks>
    /// <param name="colorName">The name of the color to retrieve. This parameter is case-insensitive.</param>
    /// <returns>The Color corresponding to the specified name, or the default value if the name is not found.</returns>
    public static Color? FromName(string colorName) => _colorResourcesDictionary.FirstOrDefault(x => string.Equals(x.Value, colorName, StringComparison.OrdinalIgnoreCase)).Key;

    /// <summary>
    /// Returns the resource key associated with the specified Color.
    /// </summary>
    /// <param name="color">The Color to retrieve the resource key for.</param>
    /// <returns>The resource key associated with the specified Color, or null if not found.</returns>
    public static string? GetName(Color color) => _colorResourcesDictionary.GetValueOrDefault(color);

    /// <summary>
    /// Handles the event that occurs when the application's culture changes, updating color resources to reflect the
    /// new culture settings.
    /// </summary>
    /// <remarks>This method ensures that UI color resources are refreshed whenever the application's culture
    /// is modified, so that the user interface remains consistent with the selected culture.</remarks>
    /// <param name="sender">The source of the culture change event.</param>
    /// <param name="e">An object that contains the event data.</param>
    private static void OnCultureChanged(object? sender, EventArgs e) => DispatcherHelper.Post(FillColorResourcesDictionary);

    /// <summary>
    /// Fills the ColorResourcesDictionary with color keys and their corresponding resource keys from the ColorResources resource manager. This method is called whenever the culture changes to ensure that the dictionary is updated with the correct localized color resources. It iterates through all entries in the resource set, attempts to parse the key as a Color, and if successful, adds it to the dictionary with its associated resource key. If a key cannot be parsed as a Color, a warning is logged.
    /// </summary>
    private static void FillColorResourcesDictionary()
    {
        _colorResourcesDictionary = [];
        var resourceSet = ColorResources.ResourceManager.GetResourceSet(GlobalizationService.Current.Culture, true, true);

        if (resourceSet is null) return;

        foreach (var entry in resourceSet.OfType<DictionaryEntry>())
        {
            try
            {
                if (Color.TryParse(entry.Key.ToString().OrEmpty(), out var color))
                {
                    _colorResourcesDictionary.Add(color, entry.Value!.ToString()!);
                }
            }
            catch (Exception)
            {
                LogManager.Warning($"{entry.Key} is not a valid color key");
            }
        }
    }
}
