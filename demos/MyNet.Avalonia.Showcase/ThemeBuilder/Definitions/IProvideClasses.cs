// -----------------------------------------------------------------------
// <copyright file="IProvideClasses.cs" company="Stéphane ANDRE">
// Copyright (c) Stéphane ANDRE. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MyNet.Avalonia.Showcase.ThemeBuilder.Definitions;

/// <summary>
/// Represents an interface for providing CSS classes. This interface defines a contract for classes that can supply an array of CSS class names, which can be used in theming and styling scenarios within the theme builder application. Implementing this interface allows a class to specify the CSS classes that should be applied to a control or element, facilitating dynamic styling based on the provided class names.
/// </summary>
internal interface IProvideClasses
{
    /// <summary>
    /// Retrieves an array of class names available in the current context.
    /// </summary>
    /// <param name="value">An optional value that can be used to determine the available classes. This parameter can be null.</param>
    /// <returns>An array of strings containing the names of all classes. The array will be empty if no classes are available.</returns>
    string[] ProvideClasses(object? value);
}
