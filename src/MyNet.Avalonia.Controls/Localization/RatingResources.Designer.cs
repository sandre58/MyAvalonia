// -----------------------------------------------------------------------
// <copyright file="RatingResources.Designer.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Controls.Localization;

using System.Globalization;
using System.Resources;

/// <summary>
/// Localization resources for <see cref="Rating"/> accessibility strings.
/// </summary>
public static class RatingResources
{
    private static readonly ResourceManager ResourceManager =
        new("MyNet.Avalonia.Controls.Localization.RatingResources", typeof(RatingResources).Assembly);

    /// <summary>
    /// Rating {0} of {1}.
    /// </summary>
    public static string AutomationName =>
        ResourceManager.GetString(nameof(AutomationName), CultureInfo.CurrentUICulture) ?? "Rating {0} of {1}";

    /// <summary>
    /// Item {0}.
    /// </summary>
    public static string ItemAutomationName =>
        ResourceManager.GetString(nameof(ItemAutomationName), CultureInfo.CurrentUICulture) ?? "Item {0}";
}
