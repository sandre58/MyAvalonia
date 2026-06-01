// -----------------------------------------------------------------------
// <copyright file="PageHelper.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Avalonia.Controls;
using Avalonia.Media;
using MyNet.Avalonia.Colors;
using MyNet.Avalonia.Extensions;
using MyNet.Avalonia.Showcase.Views.Samples;
using MyNet.Generator.Facade;
using MyNet.Utilities.Generator;

namespace MyNet.Avalonia.Showcase.Helpers;

internal static class PageHelper
{
    /// <summary>
    /// Creates a new instance of the <see cref="ContentPage"/> class with the specified header and body content.
    /// </summary>
    /// <param name="header">The header text for the content page.</param>
    /// <param name="body">The body text for the content page.</param>
    /// <returns>A new instance of the <see cref="ContentPage"/> class.</returns>
    public static ContentPage MakeNavigationPage(string header, string body) =>
        new()
        {
            Header = header,
            Background = new SolidColorBrush(RandomGenerator.Color().ToColor().GetValueOrDefault(), 0.3),
            Content = new NavigationContent
            {
                Header = header,
                Body = body
            }
        };
}
